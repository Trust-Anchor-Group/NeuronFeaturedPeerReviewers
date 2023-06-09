﻿using System;
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
	[Index("ApprovedForPublication", "FullName")]
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
		/// If Country limitation of reviewer should be used.
		/// </summary>
		public bool UseCountry { get; set; }

		/// <summary>
		/// If Region limitation of reviewer should be used.
		/// </summary>
		public bool UseRegion { get; set; }

		/// <summary>
		/// If City limitation of reviewer should be used.
		/// </summary>
		public bool UseCity { get; set; }

		/// <summary>
		/// If Area limitation of reviewer should be used.
		/// </summary>
		public bool UseArea { get; set; }

		/// <summary>
		/// If Zip limitation of reviewer should be used.
		/// </summary>
		public bool UseZip { get; set; }

		/// <summary>
		/// If Address limitation of reviewer should be used.
		/// </summary>
		public bool UseAddress { get; set; }

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
		/// Organization Name
		/// </summary>
		[DefaultValueStringEmpty]
		public CaseInsensitiveString OrgName { get; set; }

		/// <summary>
		/// Organization Number
		/// </summary>
		[DefaultValueStringEmpty]
		public CaseInsensitiveString OrgNr { get; set; }

		/// <summary>
		/// Organization Country limitation of reviewer
		/// </summary>
		[DefaultValueStringEmpty]
		public CaseInsensitiveString OrgCountry { get; set; }

		/// <summary>
		/// Organization Region limitation of reviewer
		/// </summary>
		[DefaultValueStringEmpty]
		public CaseInsensitiveString OrgRegion { get; set; }

		/// <summary>
		/// Organization City limitation of reviewer
		/// </summary>
		[DefaultValueStringEmpty]
		public CaseInsensitiveString OrgCity { get; set; }

		/// <summary>
		/// Organization Area limitation of reviewer
		/// </summary>
		[DefaultValueStringEmpty]
		public CaseInsensitiveString OrgArea { get; set; }

		/// <summary>
		/// Organization Zip limitation of reviewer
		/// </summary>
		[DefaultValueStringEmpty]
		public CaseInsensitiveString OrgZip { get; set; }

		/// <summary>
		/// Organization Address limitation of reviewer
		/// </summary>
		[DefaultValueStringEmpty]
		public CaseInsensitiveString OrgAddress { get; set; }

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

		/// <summary>
		/// Textual description presented to users.
		/// </summary>
		public string Description { get; set; }
	}
}
