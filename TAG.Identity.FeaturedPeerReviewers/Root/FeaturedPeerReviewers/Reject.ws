AuthenticateSession(Request,"User");
Authorize(User,"Admin.Identity.FeaturedPeerReviewers");

Application:=select top 1 * from TAG.Identity.FeaturedPeerReviewers.FeaturedPeerReviewer where LegalId=Posted;
if !exists(Application) then NotFound("Application not found.");

DeleteObject(Application);

if System.IO.File.Exists(Application.PhotoFileName) then
	System.IO.File.Delete(Application.PhotoFileName);

LogInformation("Application for featured peer reviewer rejected.",
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

true;