using System;
using Waher.Persistence;
using Waher.Persistence.Attributes;

namespace TAG.Identity.FeaturedPeerReviewers
{
	/// <summary>
	/// Contains a record of a featured peer reviewer.
	/// </summary>
	[CollectionName("FeaturedPeerReviewers")]
	[TypeName(TypeNameSerialization.None)]
	[Index("LegalId")]
	public class FeaturedPeerReviewer
	{
		/// <summary>
		/// Contains a record of a featured peer reviewer.
		/// </summary>
		public FeaturedPeerReviewer() 
		{
		}

		/// <summary>
		/// Object ID
		/// </summary>
		[ObjectId]
		public string ObjectId { get; set; }

		/// <summary>
		/// Legal ID of peer reviewer
		/// </summary>
		public CaseInsensitiveString LegalId { get; set; }

		/// <summary>
		/// Trust Provider of Legal ID of peer reviewer
		/// </summary>
		public CaseInsensitiveString Provider { get; set; }

		/// <summary>
		/// State of identity, at time of application.
		/// </summary>
		public Waher.Networking.XMPP.Contracts.IdentityState State { get; set; }

		/// <summary>
		/// When ID was created
		/// </summary>
		public DateTime Created { get; set; }

		/// <summary>
		/// When ID was updated
		/// </summary>
		public DateTime Updated { get; set; }

		/// <summary>
		/// From when ID is approved
		/// </summary>
		public DateTime From { get; set; }

		/// <summary>
		/// To when ID is approved
		/// </summary>
		public DateTime To { get; set; }

		/// <summary>
		/// If peer reviewer has been approved for publication.
		/// </summary>
		public bool ApprovedForPublication { get; set; }

		/// <summary>
		/// Full name of reviewer
		/// </summary>
		public string FullName { get; set; }

		/// <summary>
		/// Content-Type of photo
		/// </summary>
		public string PhotoContentType { get; set; }

		/// <summary>
		/// File name of photo
		/// </summary>
		public string PhotoFileName { get; set; }

		/// <summary>
		/// Width of photo
		/// </summary>
		public int PhotoWidth { get; set; }

		/// <summary>
		/// Height of photo
		/// </summary>
		public int PhotoHeight { get; set; }

		/// <summary>
		/// Country limitation of reviewer
		/// </summary>
		public CaseInsensitiveString Country { get; set; }

		/// <summary>
		/// Region limitation of reviewer
		/// </summary>
		public CaseInsensitiveString Region { get; set; }

		/// <summary>
		/// City limitation of reviewer
		/// </summary>
		public CaseInsensitiveString City { get; set; }

		/// <summary>
		/// Area limitation of reviewer
		/// </summary>
		public CaseInsensitiveString Area { get; set; }

		/// <summary>
		/// Zip limitation of reviewer
		/// </summary>
		public CaseInsensitiveString Zip { get; set; }

		/// <summary>
		/// Address limitation of reviewer
		/// </summary>
		public CaseInsensitiveString Address { get; set; }

		/// <summary>
		/// E-mail to reviewer
		/// </summary>
		public CaseInsensitiveString EMail { get; set; }

		/// <summary>
		/// Phone Number to reviewer
		/// </summary>
		public CaseInsensitiveString PhoneNumber { get; set; }

		/// <summary>
		/// XMPP Address to reviewer
		/// </summary>
		public CaseInsensitiveString Jid { get; set; }
	}
}
