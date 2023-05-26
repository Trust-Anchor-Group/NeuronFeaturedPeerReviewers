AuthenticateSession(Request,"QuickLoginUser");

Application:=select top 1 * from TAG.Identity.FeaturedPeerReviewers.FeaturedPeerReviewer where LegalId=QuickLoginUser.Id;
if !exists(Application) then BadRequest("No application to delete.");

DeleteObject(Application);
TAG.Identity.FeaturedPeerReviewers.FeaturedPeerReviewersProvider.ApplicationDeleted(Application.LegalId);

if System.IO.File.Exists(Application.PhotoFileName) then
	System.IO.File.Delete(Application.PhotoFileName);

LogInformation("Application for featured peer reviewer deleted by applicant.",
{
	"Object":Application.LegalId,
	"Actor":QuickLoginUser.UserName,
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

QuickLoginUser:=null;

TabIDs:=GetTabIDs("/FeaturedPeerReviewers/Settings.md");
foreach TabID in TabIDs do
(
	TabInfo:=GetTabInformation(TabID);
	if exists(TabInfo.Session.User) and TabInfo.Session.User.HasPrivilege("Admin.Identity.FeaturedPeerReviewers") then
		PushEvent(TabID,"RemoveApplication",Application.LegalId)
);

ApplicationUrl:=Waher.IoTGateway.Gateway.GetUrl("/FeaturedPeerReviewers/Apply.md");
Message:="Your application to become featured peer reviewer on *"+
	Waher.IoTGateway.Gateway.Domain+"* has been deleted. If you want, you can [apply]("+ApplicationUrl+") again.";
if !empty(Application.Jid) then SendFormattedMessage(Application.Jid,Message);
if !empty(Application.EMail) then SendMail(Application.EMail,"Application deleted.",Message);

true