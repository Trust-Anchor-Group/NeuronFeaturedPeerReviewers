AuthenticateSession(Request,"User");
Authorize(User,"Admin.Identity.FeaturedPeerReviewers");

Application:=select top 1 * from TAG.Identity.FeaturedPeerReviewers.FeaturedPeerReviewer where LegalId=Posted;
if !exists(Application) then NotFound("Application not found.");

Application.ApprovedForPublication:=true;
UpdateObject(Application);
TAG.Identity.FeaturedPeerReviewers.FeaturedPeerReviewersProvider.ApplicationUpdated(Application);

LogInformation("Application for featured peer reviewer accepted.",
{
	"Object":Application.LegalId,
	"Actor":User.UserName,
	"LegalId": Application.LegalId,
	"Provider": Application.Provider,
	"State": Application.State,
	"FullName": Application.FullName,
	"Country": Application.Country,
	"Region": Application.Region,
	"City": Application.City,
	"Area": Application.Area,
	"Zip": Application.Zip,
	"Address": Application.Address,
	"OrgName": Application.OrgName,
	"OrgNr": Application.OrgNr,
	"OrgCountry": Application.OrgCountry,
	"OrgRegion": Application.OrgRegion,
	"OrgCity": Application.OrgCity,
	"OrgArea": Application.OrgArea,
	"OrgZip": Application.OrgZip,
	"OrgAddress": Application.OrgAddress,
	"EMail": Application.EMail,
	"PhoneNumber": Application.PhoneNumber,
	"Jid": Application.Jid,
	"Description": Application.Description
});

PushEvent("/FeaturedPeerReviewers/Apply.md","ApplicationUpdated",Application.LegalId);

ApplicationUrl:=Waher.IoTGateway.Gateway.GetUrl("/FeaturedPeerReviewers/Apply.md");
Message:="Your [application]("+ApplicationUrl+") to become featured peer reviewer on *"+
	Waher.IoTGateway.Gateway.Domain+"* has been accepted.";
if !empty(Application.Jid) then SendFormattedMessage(Application.Jid,Message);
if !empty(Application.EMail) then SendMail(Application.EMail,"Application accepted.",Message);

{
	"legalId": Application.LegalId,
	"provider": Application.Provider,
	"state": Application.State,
	"created": Application.Created.ToShortDateString(),
	"updated": Application.Updated.ToShortDateString(),
	"from": Application.From.ToShortDateString(),
	"to": Application.To.ToShortDateString(),
	"approvedForPublication": Application.ApprovedForPublication,
	"fullName": Application.FullName,
	"useCountry": Application.UseCountry,
	"useRegion": Application.UseRegion,
	"useCity": Application.UseCity,
	"useArea": Application.UseArea,
	"useZip": Application.UseZip,
	"useAddress": Application.UseAddress,
	"country": Application.Country,
	"region": Application.Region,
	"city": Application.City,
	"area": Application.Area,
	"zip": Application.Zip,
	"address": Application.Address,
	"orgName": Application.OrgName,
	"orgNr": Application.OrgNr,
	"orgCountry": Application.OrgCountry,
	"orgRegion": Application.OrgRegion,
	"orgCity": Application.OrgCity,
	"orgArea": Application.OrgArea,
	"orgZip": Application.OrgZip,
	"orgAddress": Application.OrgAddress,
	"eMail": Application.EMail,
	"phoneNumber": Application.PhoneNumber,
	"jid": Application.Jid,
	"photo": "/FeaturedPeerReviewers/Images/"+Application.LegalId+".webp",
	"photoContentType": Application.PhotoContentType,
	"photoWidth": Application.PhotoWidth,
	"photoHeight": Application.PhotoHeight,
	"description": Application.Description
}