using Paiwise;
using System.Collections.Generic;
using System.Threading.Tasks;
using Waher.IoTGateway;
using Waher.Persistence;
using Waher.Runtime.Threading;

namespace TAG.Identity.FeaturedPeerReviewers
{
	/// <summary>
	/// Service publishing featured peer reviewers.
	/// </summary>
	public class FeaturedPeerReviewersProvider : IPeerReviewServiceProvider, IConfigurableModule
	{
		private readonly MultiReadSingleWriteObject synchObj = new MultiReadSingleWriteObject();
		private readonly Dictionary<CaseInsensitiveString, FeaturedPeerReviewer> peerReviewers = new Dictionary<CaseInsensitiveString, FeaturedPeerReviewer>();
		private bool loaded = false;

		/// <summary>
		/// Service publishing featured peer reviewers.
		/// </summary>
		public FeaturedPeerReviewersProvider()
		{
		}

		/// <summary>
		/// ID of service provider
		/// </summary>
		public string Id => typeof(FeaturedPeerReviewersProvider).FullName;

		/// <summary>
		/// Name of service provider
		/// </summary>
		public string Name => "Featured Peer Reviewers";

		/// <summary>
		/// Icon for service provider.
		/// </summary>
		public string IconUrl => string.Empty;

		/// <summary>
		/// Icon width for service provider.
		/// </summary>
		public int IconWidth => 0;

		/// <summary>
		/// Icon height for service provider.
		/// </summary>
		public int IconHeight => 0;

		/// <summary>
		/// Called when module is started.
		/// </summary>
		public Task Start()
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Called when module is stopped.
		/// </summary>
		public async Task Stop()
		{
			await this.synchObj.BeginWrite();
			try
			{
				this.peerReviewers.Clear();
			}
			finally
			{
				await this.synchObj.EndWrite();
			}
		}

		/// <summary>
		/// Lists available configuration pages.
		/// </summary>
		/// <returns>Configuration pages.</returns>
		public Task<IConfigurablePage[]> GetConfigurablePages()
		{
			return Task.FromResult(new IConfigurablePage[]
			{
				new ConfigurablePage("Featured Peer Reviewers", "/FeaturedPeerReviewers/Settings.md", "Admin.Identity.FeaturedPeerReviewers")
			});
		}

		/// <summary>
		/// Gets a service reference for peer reviewing an identity application.
		/// </summary>
		/// <param name="ServiceId">Service ID</param>
		/// <param name="Identity">Identity application</param>
		/// <returns>Service reference.</returns>
		public async Task<IPeerReviewService> GetServiceForPeerReview(string ServiceId, KeyValuePair<string, object>[] Identity)
		{
			await this.CheckLoaded();

			await this.synchObj.BeginRead();
			try
			{
				if (this.peerReviewers.TryGetValue(ServiceId, out FeaturedPeerReviewer Reviewer))
					return new FeaturedPeerReviewerService(Reviewer, this);
				else
					return null;
			}
			finally
			{
				await this.synchObj.EndRead();
			}
		}

		/// <summary>
		/// Gets available peer reviewers for a given identity application.
		/// </summary>
		/// <param name="Identity">Identity application.</param>
		/// <returns>Featured peer reviewers that can review the current application.</returns>
		public async Task<IPeerReviewService[]> GetServicesForPeerReview(KeyValuePair<string, object>[] Identity)
		{
			await this.CheckLoaded();

			Dictionary<CaseInsensitiveString, object> Application = new Dictionary<CaseInsensitiveString, object>();

			foreach (KeyValuePair<string, object> P in Identity)
				Application[P.Key] = P.Value;

			await this.synchObj.BeginRead();
			try
			{
				List<IPeerReviewService> Services = new List<IPeerReviewService>();

				foreach (FeaturedPeerReviewer Reviewer in this.peerReviewers.Values)
				{
					if (!Reviewer.ApprovedForPublication)
						continue;

					if (!CaseInsensitiveString.IsNullOrEmpty(Reviewer.Country) &&
						!IsMatch("COUNTRY", Reviewer.Country, Application))
					{
						continue;
					}

					if (!CaseInsensitiveString.IsNullOrEmpty(Reviewer.Region) &&
						!IsMatch("REGION", Reviewer.Region, Application))
					{
						continue;
					}

					if (!CaseInsensitiveString.IsNullOrEmpty(Reviewer.City) &&
						!IsMatch("CITY", Reviewer.City, Application))
					{
						continue;
					}

					if (!CaseInsensitiveString.IsNullOrEmpty(Reviewer.Area) &&
						!IsMatch("AREA", Reviewer.Area, Application))
					{
						continue;
					}

					if (!CaseInsensitiveString.IsNullOrEmpty(Reviewer.Zip) &&
						!IsMatch("ZIP", Reviewer.Zip, Application))
					{
						continue;
					}

					Services.Add(new FeaturedPeerReviewerService(Reviewer, this));
				}

				return Services.ToArray();
			}
			finally
			{
				await this.synchObj.EndRead();
			}
		}

		private static bool IsMatch(CaseInsensitiveString Field, CaseInsensitiveString Value, 
			Dictionary<CaseInsensitiveString, object> Application)
		{
			if (!Application.TryGetValue(Field, out object ApplicationValue))
				return false;

			return Value == new CaseInsensitiveString(ApplicationValue?.ToString() ?? string.Empty);
		}

		private async Task CheckLoaded()
		{
			await this.synchObj.BeginRead();
			try
			{
				if (this.loaded)
					return;
			}
			finally
			{
				await this.synchObj.EndRead();
			}

			await this.synchObj.BeginWrite();
			try
			{
				if (this.loaded)
					return;

				this.peerReviewers.Clear();

				foreach (FeaturedPeerReviewer Reviewer in await Database.Find<FeaturedPeerReviewer>())
					this.peerReviewers[Reviewer.LegalId] = Reviewer;

				this.loaded = true;
			}
			finally
			{
				await this.synchObj.BeginWrite();
			}
		}
	}
}
