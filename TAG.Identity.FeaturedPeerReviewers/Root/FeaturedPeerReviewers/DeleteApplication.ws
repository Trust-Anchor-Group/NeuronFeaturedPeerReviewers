﻿AuthenticateSession(Request,"QuickLoginUser");

Application:=select top 1 * from TAG.Identity.FeaturedPeerReviewers.FeaturedPeerReviewer where LegalId=QuickLoginUser.Id;
if !exists(Application) then BadRequest("No application to delete.");

DeleteObject(Application);

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
	"EMail": Application.EMail,
	"PhoneNumber": Application.PhoneNumber,
	"Jid": Application.Jid,
	"Description": Application.Description
});

QuickLoginUser:=null;

true