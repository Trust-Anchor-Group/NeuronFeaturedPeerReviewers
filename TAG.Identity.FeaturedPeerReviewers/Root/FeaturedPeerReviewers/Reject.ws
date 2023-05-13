﻿AuthenticateSession(Request,"User");
Authorize(User,"Admin.Identity.FeaturedPeerReviewers");

{
	"legalId": Required(Str(PLegalId)),
	"isReject": Required(Bool(PIsReject))
}:=Posted;

Application:=select top 1 * from TAG.Identity.FeaturedPeerReviewers.FeaturedPeerReviewer where LegalId=PLegalId;
if !exists(Application) then NotFound("Application not found.");

DeleteObject(Application);
TAG.Identity.FeaturedPeerReviewers.FeaturedPeerReviewersProvider.ApplicationDeleted(Application.LegalId);

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

ApplicationUrl:=Waher.IoTGateway.Gateway.GetUrl("/FeaturedPeerReviewers/Apply.md");

if PIsReject then
(
	Message:="Your application to become featured peer reviewer on *"+
		Waher.IoTGateway.Gateway.Domain+"* has been rejected. If you want, you can [apply]("+ApplicationUrl+") again."
)
else
(
	Message:="Your application to become featured peer reviewer on *"+
		Waher.IoTGateway.Gateway.Domain+"* has been deleted. If you want, you can [apply]("+ApplicationUrl+") again."
);

if !empty(Application.Jid) then SendFormattedMessage(Application.Jid,Message);
if !empty(Application.EMail) then SendMail(Application.EMail,"Application rejected.",Message);

true;