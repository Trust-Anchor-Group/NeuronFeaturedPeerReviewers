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
	}
}
