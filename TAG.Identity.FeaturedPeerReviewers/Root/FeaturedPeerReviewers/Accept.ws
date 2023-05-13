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
	"EMail": Application.EMail,
	"PhoneNumber": Application.PhoneNumber,
	"Jid": Application.Jid,
	"Description": Application.Description
});

PushEvent("/FeaturedPeerReviewers/Apply.md","ApplicationUpdated",Application.LegalId);

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
	"eMail": Application.EMail,
	"phoneNumber": Application.PhoneNumber,
	"jid": Application.Jid,
	"photo": PhotoUrl,
	"photoContentType": Application.PhotoContentType,
	"photoWidth": Application.PhotoWidth,
	"photoHeight": Application.PhotoHeight,
	"description": Application.Description
}