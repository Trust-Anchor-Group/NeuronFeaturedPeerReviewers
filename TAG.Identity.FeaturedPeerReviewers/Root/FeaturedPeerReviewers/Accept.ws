AuthenticateSession(Request,"User");
Authorize(User,"Admin.Identity.FeaturedPeerReviewers");

Application:=select top 1 * from TAG.Identity.FeaturedPeerReviewers.FeaturedPeerReviewer where LegalId=Posted;
if !exists(Application) then NotFound("Application not found.");

Application.ApprovedForPublication:=true;
UpdateObject(Application);

{
	"legalId": Application.LegalId,
	"provider": Application.Provider,
	"state": Application.State,
	"created": Application.Created,
	"updated": Application.Updated,
	"from": Application.From,
	"to": Application.To,
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