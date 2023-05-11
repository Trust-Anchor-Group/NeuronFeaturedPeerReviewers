AuthenticateSession(Request,"QuickLoginUser");

Application:=select top 1 * from TAG.Identity.FeaturedPeerReviewers.FeaturedPeerReviewer where LegalId=QuickLoginUser.Id;
if !exists(Application) then BadRequest("No application to delete.");

DeleteObject(Application);
QuickLoginUser:=null;

true