using Paiwise;
using System.Threading.Tasks;
using Waher.Events;
using Waher.IoTGateway;
using Waher.Networking.XMPP.Contracts.EventArguments;

namespace TAG.Identity.FeaturedPeerReviewers
{
	/// <summary>
	/// Peer reviewer service.
	/// </summary>
	public class FeaturedPeerReviewerService : IPeerReviewService
	{
		private readonly FeaturedPeerReviewer reviewer;
		private readonly IPeerReviewServiceProvider provider;
		private readonly string name;

		/// <summary>
		/// Peer reviewer service.
		/// </summary>
		/// <param name="Reviewer">Reviewer</param>
		/// <param name="Provider">Service provider reference</param>
		public FeaturedPeerReviewerService(FeaturedPeerReviewer Reviewer, IPeerReviewServiceProvider Provider)
		{
			this.reviewer = Reviewer;
			this.provider = Provider;

			this.name = this.reviewer.FullName;
			if (!string.IsNullOrEmpty(this.reviewer.Description))
				this.name += ", " + this.reviewer.Description;
		}

		/// <summary>
		/// ID of service
		/// </summary>
		public string Id => this.reviewer.LegalId;

		/// <summary>
		/// Name of service
		/// </summary>
		public string Name => this.name;

		/// <summary>
		/// Icon for service.
		/// </summary>
		public string IconUrl => Gateway.GetUrl("/FeaturedPeerReviewers/Images/" + this.reviewer.LegalId + ".webp");

		/// <summary>
		/// Icon width for service.
		/// </summary>
		public int IconWidth => this.reviewer.PhotoWidth;

		/// <summary>
		/// Icon height for service.
		/// </summary>
		public int IconHeight => this.reviewer.PhotoHeight;

		/// <summary>
		/// Legal ID of peer reviewer
		/// </summary>
		public string PeerReviewerLegalId => this.reviewer.LegalId;

		/// <summary>
		/// if the reviewer is external (from the Neuron point-of-view).
		/// </summary>
		public bool External => true;

		/// <summary>
		/// Service provider reference.
		/// </summary>
		public IPeerReviewServiceProvider PeerReviewServiceProvider => this.provider;

		/// <summary>
		/// Checks the veracity of identity claims.
		/// </summary>
		/// <param name="Application">Identity application.</param>
		/// <param name="ClientUrlCallback">Client-side URL that needs to be opened to complete the validation process.</param>
		/// <param name="State">State object passed on in calls to the callback method.</param>
		public Task Validate(IIdentityReviewApplication Application,
			EventHandlerAsync<ClientUrlEventArgs> ClientUrlCallback, object State)
		{
			Application.ReportError("Peer reviewer is external. Peer-review request should be sent directly to reviewer.",
				"en", "ReviewerExternal", ValidationErrorType.Client, this);

			return Task.CompletedTask;
		}
	}
}
