AuthenticateSession(Request,"QuickLoginUser");

if QuickLoginUser.State != Waher.Networking.XMPP.Contracts.IdentityState.Approved then BadRequest("Identity not approved.");
if QuickLoginUser.From>Now then BadRequest("Identity will only be valid for use after "+Str(QuickLoginUser.From));
if QuickLoginUser.To<Now then BadRequest("Identity is not valid for use after "+Str(QuickLoginUser.To));

Application:=select top 1 * from TAG.Identity.FeaturedPeerReviewers.FeaturedPeerReviewer where LegalId=QuickLoginUser.Id;
if exists(Application) then
	SeeOther("Apply.md");

Application:=Create(TAG.Identity.FeaturedPeerReviewers.FeaturedPeerReviewer);
Application.LegalId:=QuickLoginUser.Id;
Application.Provider:=QuickLoginUser.Provider;
Application.State:=QuickLoginUser.State;
Application.Created:=QuickLoginUser.Created;
Application.Updated:=QuickLoginUser.Updated;
Application.From:=QuickLoginUser.From;
Application.To:=QuickLoginUser.To;
Application.ApprovedForPublication:=false;
Application.FullName:=QuickLoginUser.UserName;
Application.Country:=(QuickLoginUser.Properties.COUNTRY ??? "");
Application.Region:=(QuickLoginUser.Properties.REGION ??? "");
Application.City:=(QuickLoginUser.Properties.CITY ??? "");
Application.Area:=(QuickLoginUser.Properties.AREA ??? "");
Application.Zip:=(QuickLoginUser.Properties.ZIP ??? "");
Application.Address:=(QuickLoginUser.Properties.ADDR ??? "");
Application.UseCountry:=!empty(Application.Country);
Application.UseRegion:=!empty(Application.Region);
Application.UseCity:=!empty(Application.City);
Application.UseArea:=!empty(Application.Area);
Application.UseZip:=!empty(Application.Zip);
Application.UseAddress:=!empty(Application.Address);
Application.EMail:=(QuickLoginUser.Properties.EMAIL ??? "");
Application.PhoneNumber:=(QuickLoginUser.Properties.PHONE ??? "");
Application.Jid:=(QuickLoginUser.Properties.JID ??? "");
Application.Description:="";

AvatarUrl:=Waher.IoTGateway.Gateway.GetUrl(QuickLoginUser.AvatarUrl)+"?Width=128&Height=128";
Avatar:=Get(AvatarUrl);

FileName:="";
PhotoUrl:="/FeaturedPeerReviewers/Images/"+QuickLoginUser.Id+".webp";
if !Gateway.HttpServer.TryGetFileName(PhotoUrl,false,FileName) then
	ServiceUnavailable("Unable to save photo.");

Folder:=System.IO.Path.GetDirectoryName(FileName);
if !System.IO.Directory.Exists(Folder) then
	System.IO.Directory.CreateDirectory(Folder);

Application.PhotoFileName:=FileName;
Application.PhotoContentType:=SaveFile(Avatar,FileName);
Application.PhotoWidth:=Avatar.Width;
Application.PhotoHeight:=Avatar.Height;

SaveNewObject(Application);
TAG.Identity.FeaturedPeerReviewers.ApplicationUpdated(Application);

Waher.IoTGateway.Gateway.SendNotification("New [application for featured peer reviewer]("+
	 Waher.IoTGateway.Gateway.GetUrl("/FeaturedPeerReviewers/Settings.md")+") received from **"+
	 Application.FullName+"**, **"+Application.Description+"**.");

LogInformation("Application for featured peer reviewer received.",
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

Result:=
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
};

TabIDs:=GetTabIDs("/FeaturedPeerReviewers/Settings.md");
foreach TabID in TabIDs do
(
	TabInfo:=GetTabInformation(TabID);
	if exists(TabInfo.Session.User) and TabInfo.Session.User.HasPrivilege("Admin.Identity.FeaturedPeerReviewers") then
		PushEvent(TabID,"NewApplication",Result)
);

Result
