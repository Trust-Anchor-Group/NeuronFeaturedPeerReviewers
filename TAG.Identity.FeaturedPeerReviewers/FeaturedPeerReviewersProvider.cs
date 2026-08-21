using Paiwise;
using Paiwise.Functions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Waher.IoTGateway;
using Waher.Persistence;
using Waher.Runtime.Collections;
using Waher.Runtime.Inventory;
using Waher.Runtime.Threading;
using Waher.Script.Constants;
using Waher.Script.Graphs.Canvas2D.Operations;

namespace TAG.Identity.FeaturedPeerReviewers
{
	/// <summary>
	/// Service publishing featured peer reviewers.
	/// </summary>
	public class FeaturedPeerReviewersProvider : IPeerReviewServiceProvider, IConfigurableModule
	{
		private static readonly MultiReadSingleWriteObject synchObj = new MultiReadSingleWriteObject();
		private static readonly Dictionary<CaseInsensitiveString, FeaturedPeerReviewer> peerReviewers = new Dictionary<CaseInsensitiveString, FeaturedPeerReviewer>();
		private static bool loaded = false;

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
			await synchObj.BeginWrite();
			try
			{
				peerReviewers.Clear();
			}
			finally
			{
				await synchObj.EndWrite();
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
		/// Required properties for the identity authenticator service.
		/// </summary>
		public string[] RequiredProperties => GetPeerReviewProperties(true);

		/// <summary>
		/// Optional properties for the identity authenticator service.
		/// </summary>
		public string[] OptionalProperties => GetPeerReviewProperties(false);

		/// <summary>
		/// Required attachments for the identity authenticator service.
		/// </summary>
		public string[] RequiredAttachments => GetPeerReviewAttachments(true);

		/// <summary>
		/// Optional attachments for the identity authenticator service.
		/// </summary>
		public string[] OptionalAttachments => GetPeerReviewAttachments(false);

		private static bool TryGetPeerReviewConfiguration(out Type T, out object Instance)
		{
			T = Types.GetType("Waher.Service.IoTBroker.Setup.PeerReviewConfiguration");
			Instance = null;

			if (T is null)
				return false;

			PropertyInfo PI = T.GetProperty("Instance", BindingFlags.Static | BindingFlags.Public);
			if (PI is null)
				return false;

			Instance = PI.GetValue(null);

			PI = T.GetProperty("AllowPeerReview", BindingFlags.Instance | BindingFlags.Public);
			if (PI is null)
				return false;

			bool AllowPeerReview = (bool)PI.GetValue(Instance);
			if (!AllowPeerReview)
				return false;

			return true;
		}

		private static string[] GetPeerReviewProperties(bool Required)
		{
			if (!TryGetPeerReviewConfiguration(out Type T, out object Instance))
				return Array.Empty<string>();

			ChunkedList<string> Result = new ChunkedList<string>();

			foreach (KeyValuePair<string, string> P in PropertyNames)
			{
				PropertyInfo PI = T.GetProperty(P.Key, BindingFlags.Instance | BindingFlags.Public);
				if (PI is null)
					continue;

				bool Value = PI.GetValue(Instance) is bool b && b;

				switch (P.Value)
				{
					case PersonalInformation.AddressTag:
						if (Value)
						{
							if (Required)
								Result.Add(PersonalInformation.AddressTag);
							else
								Result.Add(PersonalInformation.Address2Tag);
						}
						else if (!Required)
						{
							Result.Add(PersonalInformation.AddressTag);
							Result.Add(PersonalInformation.Address2Tag);
						}
						break;

					case PersonalInformation.BirthDayTag:
						if (!(Value ^ Required))
						{
							Result.Add(PersonalInformation.BirthDayTag);
							Result.Add(PersonalInformation.BirthMonthTag);
							Result.Add(PersonalInformation.BirthYearTag);
						}
						break;

					default:
						if (!(Value ^ Required))
							Result.Add(P.Value);
						break;
				}
			}

			return Result.ToArray();
		}

		private static readonly Dictionary<string, string> PropertyNames = new Dictionary<string, string>()
		{
			{ "RequireFirstName", PersonalInformation.FirstNameTag },
			{ "RequireMiddleName", PersonalInformation.MiddleNamesTag },
			{ "RequireLastName", PersonalInformation.LastNamesTag },
			{ "RequirePersonalNumber", PersonalInformation.PersonalNumberTag },
			{ "RequireCountry", PersonalInformation.CountryTag },
			{ "RequireRegion", PersonalInformation.RegionTag },
			{ "RequireCity", PersonalInformation.CityTag },
			{ "RequireArea", PersonalInformation.AreaTag },
			{ "RequirePostalCode", PersonalInformation.PostalCodeTag },
			{ "RequireAddress", PersonalInformation.AddressTag },
			{ "RequireNationality", PersonalInformation.NationalityTag },
			{ "RequireGender", PersonalInformation.GenderTag },
			{ "RequireBirthDate", PersonalInformation.BirthDayTag }
		};

		private static string[] GetPeerReviewAttachments(bool Required)
		{
			if (!TryGetPeerReviewConfiguration(out Type T, out object Instance))
				return Array.Empty<string>();

			PropertyInfo PI = T.GetProperty("NrPhotosRequired", BindingFlags.Instance | BindingFlags.Public);

			int NrPhotosRequired = PI?.GetValue(Instance) is int i ? i : 0;

			if (NrPhotosRequired == 0)
			{
				if (Required)
					return Array.Empty<string>();
				else
				{
					return new string[]
					{
						"ProfilePhoto",
						"IdCardFront",
						"IdCardBack",
						"Passport",
						"DriverLicenseFront",
						"DriverLicenseBack"
					};
				}
			}
			else 
			{
				if (Required)
				{
					return new string[]
					{
						"ProfilePhoto"
					};
				}
				else
				{
					return new string[]
					{
						"IdCardFront",
						"IdCardBack",
						"Passport",
						"DriverLicenseFront",
						"DriverLicenseBack"
					};
				}
			}
		}

		/// <summary>
		/// Gets a service reference for peer reviewing an identity application.
		/// </summary>
		/// <param name="ServiceId">Service ID</param>
		/// <param name="Identity">Identity application</param>
		/// <returns>Service reference.</returns>
		public async Task<IPeerReviewService> GetServiceForPeerReview(string ServiceId, KeyValuePair<string, object>[] Identity)
		{
			await CheckLoaded();

			await synchObj.BeginRead();
			try
			{
				if (peerReviewers.TryGetValue(ServiceId, out FeaturedPeerReviewer Reviewer))
					return new FeaturedPeerReviewerService(Reviewer, this);
				else
					return null;
			}
			finally
			{
				await synchObj.EndRead();
			}
		}

		/// <summary>
		/// Gets available peer reviewers for a given identity application.
		/// </summary>
		/// <param name="Identity">Identity application.</param>
		/// <returns>Featured peer reviewers that can review the current application.</returns>
		public async Task<IPeerReviewService[]> GetServicesForPeerReview(KeyValuePair<string, object>[] Identity)
		{
			await CheckLoaded();

			Dictionary<CaseInsensitiveString, object> Application = new Dictionary<CaseInsensitiveString, object>();
			bool HasOrg = false;

			foreach (KeyValuePair<string, object> P in Identity)
			{
				Application[P.Key] = P.Value;

				switch (P.Key.ToUpper())
				{
					case "ORGNAME":
					case "ORGDEPT":
					case "ORGROLE":
					case "ORGNR":
					case "ORGADDR":
					case "ORGADDR2":
					case "ORGZIP":
					case "ORGAREA":
					case "ORGCITY":
					case "ORGREGION":
					case "ORGCOUNTRY":
						HasOrg = true;
						break;
				}
			}

			await synchObj.BeginRead();
			try
			{
				List<IPeerReviewService> Services = new List<IPeerReviewService>();
				DateTime Now = DateTime.Now;

				foreach (FeaturedPeerReviewer Reviewer in peerReviewers.Values)
				{
					if (!Reviewer.ApprovedForPublication ||
						Reviewer.State != Waher.Networking.XMPP.Contracts.IdentityState.Approved ||
						Reviewer.From > Now ||
						Reviewer.To < Now)
					{
						continue;
					}

					if (Reviewer.UseCountry && !IsMatch("COUNTRY", Reviewer.Country, Application))
						continue;

					if (Reviewer.UseRegion && !IsMatch("REGION", Reviewer.Region, Application))
						continue;

					if (Reviewer.UseCity && !IsMatch("CITY", Reviewer.City, Application))
						continue;

					if (Reviewer.UseArea && !IsMatch("AREA", Reviewer.Area, Application))
						continue;

					if (Reviewer.UseZip && !IsMatch("ZIP", Reviewer.Zip, Application))
						continue;

					if (Reviewer.UseAddress && !BeginsWith("ADDR", Reviewer.Address, Application))
						continue;

					if (HasOrg)
					{
						if (!IsMatch("ORGNAME", Reviewer.OrgName, Application))
							continue;

						if (!IsMatch("ORGNR", Reviewer.OrgNr, Application))
							continue;

						if (Reviewer.UseCountry && !IsMatch("ORGCOUNTRY", Reviewer.OrgCountry, Application))
							continue;

						if (Reviewer.UseRegion && !IsMatch("ORGREGION", Reviewer.OrgRegion, Application))
							continue;

						if (Reviewer.UseCity && !IsMatch("ORGCITY", Reviewer.OrgCity, Application))
							continue;

						if (Reviewer.UseArea && !IsMatch("ORGAREA", Reviewer.OrgArea, Application))
							continue;

						if (Reviewer.UseZip && !IsMatch("ORGZIP", Reviewer.OrgZip, Application))
							continue;

						if (Reviewer.UseAddress && !BeginsWith("ORGADDR", Reviewer.OrgAddress, Application))
							continue;
					}

					Services.Add(new FeaturedPeerReviewerService(Reviewer, this));
				}

				return Services.ToArray();
			}
			finally
			{
				await synchObj.EndRead();
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
			if (string.IsNullOrEmpty(s))
				return string.Empty;

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

		private static async Task CheckLoaded()
		{
			await synchObj.BeginRead();
			try
			{
				if (loaded)
					return;
			}
			finally
			{
				await synchObj.EndRead();
			}

			await synchObj.BeginWrite();
			try
			{
				if (loaded)
					return;

				peerReviewers.Clear();

				foreach (FeaturedPeerReviewer Reviewer in await Database.Find<FeaturedPeerReviewer>())
					peerReviewers[Reviewer.LegalId] = Reviewer;

				loaded = true;
			}
			finally
			{
				await synchObj.EndWrite();
			}
		}

		/// <summary>
		/// Method called when an application has been created or updated.
		/// </summary>
		/// <param name="Application">Application</param>
		public static async Task ApplicationUpdated(FeaturedPeerReviewer Application)
		{
			await synchObj.BeginWrite();
			try
			{
				if (loaded)
					peerReviewers[Application.LegalId] = Application;
			}
			finally
			{
				await synchObj.EndWrite();
			}
		}

		/// <summary>
		/// Method called when an application has been deleted.
		/// </summary>
		/// <param name="LegalId">Legal ID</param>
		public static async Task ApplicationDeleted(CaseInsensitiveString LegalId)
		{
			await synchObj.BeginWrite();
			try
			{
				if (loaded)
					peerReviewers.Remove(LegalId);
			}
			finally
			{
				await synchObj.EndWrite();
			}
		}
	}
}
