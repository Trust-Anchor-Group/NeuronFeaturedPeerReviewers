using Paiwise;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
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
				DateTime Now = DateTime.Now;

				foreach (FeaturedPeerReviewer Reviewer in this.peerReviewers.Values)
				{
					if (!Reviewer.ApprovedForPublication ||
						Reviewer.State != Waher.Networking.XMPP.Contracts.IdentityState.Approved ||
						Reviewer.From > Now ||
						Reviewer.To < Now)
					{
						continue;
					}

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

					if (!CaseInsensitiveString.IsNullOrEmpty(Reviewer.Address) &&
						!BeginsWith("ADDR", Reviewer.Address, Application))
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

			return AreSimilar(Value, ApplicationValue?.ToString() ?? string.Empty);
		}

		private static bool BeginsWith(CaseInsensitiveString Field, CaseInsensitiveString Value,
			Dictionary<CaseInsensitiveString, object> Application)
		{
			if (!Application.TryGetValue(Field, out object ApplicationValue))
				return false;

			string s1 = (ApplicationValue?.ToString() ?? string.Empty).ToLower();
			string s2 = Value.LowerCase;

			s1 = RemoveDiacritics(s1);
			s2 = RemoveDiacritics(s2);

			return s1.StartsWith(s2);
		}

		/// <summary>
		/// Checks if two strings are similar, by removing diacritics, and performing a
		/// case insensitive string comparison on the results.
		/// </summary>
		/// <param name="s1">String 1</param>
		/// <param name="s2">String 2</param>
		/// <returns>If strings are simlar.</returns>
		public static bool AreSimilar(string s1, string s2)
		{
			s1 = RemoveDiacritics(s1);
			s2 = RemoveDiacritics(s2);

			return string.Compare(s1, s2, true) == 0;
		}

		/// <summary>
		/// Removes diacritics from a string.
		/// </summary>
		/// <param name="s">String</param>
		/// <returns>String with diacritics removed</returns>
		public static string RemoveDiacritics(string s)
		{
			string FormD = s.Normalize(NormalizationForm.FormD);    // Diacritics become special characters
			StringBuilder sb = new StringBuilder();

			foreach (char ch in FormD)
			{
				UnicodeCategory Category = CharUnicodeInfo.GetUnicodeCategory(ch);
				if (Category != UnicodeCategory.NonSpacingMark)
				{
					switch (ch)
					{
						case 'Đ': sb.Append('D'); break;
						case 'đ': sb.Append('d'); break;
						default: sb.Append(ch); break;
					}
				}
			}

			return sb.ToString().Normalize(NormalizationForm.FormC);
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
