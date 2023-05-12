Title: Apply to become a Featured Peer Reviewer
Description: Allows a peer reviewer to apply to become a featured peer reviewer.
Author: Peter Waher
Date: 2023-05-10
Master: /Master.md
Cache-Control: max-age=0, no-cache, no-store
Javascript: /Events.js
Javascript: QuickLogin.js
CSS: /QuickLogin.css
Neuron: {{Domain:=Waher.IoTGateway.Gateway.Domain}}

========================================================================

Apply to become a Featured Peer Reviewer
--------------------------------------------

On this page you can apply to become a featured peer reviewer. This means that you will be presented as a peer reviewer for new
users in your area, to help them review their identity applications and approve them. You can also edit or remove your application
from this page.

To become a featured peer reviewer, or edit your existing application, you need to scan the QR-code below using your TAG ID app, 
or variant. Once you have done that, and approved the request to log in, your information will be presented on the page. There you 
can provide the local details you wish to provide, in what country, region, city, area or postal code you wish your identity will 
be presented as a peer reviewer. When you are done, you can submit your application. The operator will receive a notification. If the
operator approves you, your information will be presented to new users in the location you've selected. If a new user requests a peer
review from you, you will receive a push notification in your app.

**Note**: Your photo will become publicly available to new users, albeit in a miniature format, together with your name and the 
information about the location you provide peer review services. You can at any time, remove your identity from the list of featured 
peer reviewers.

<form action="Apply.md" method="POST">

{{
if exists(QuickLoginUser.Id) then
(
    Application:=select top 1 * from TAG.Identity.FeaturedPeerReviewers.FeaturedPeerReviewer where LegalId=QuickLoginUser.Id;
    if !exists(Application) then
    (
        QuickLoginUser:=null;
        SeeOther("Apply.md")
    );

    if exists(Posted) and !Application.ApprovedForPublication then
    (
        Application.UseCountry:=Boolean(Posted.UseCountry ?? false);
        Application.UseRegion:=Boolean(Posted.UseRegion ?? false);
        Application.UseCity:=Boolean(Posted.UseCity ?? false);
        Application.UseArea:=Boolean(Posted.UseArea ?? false);
        Application.UseZip:=Boolean(Posted.UseZip ?? false);
        Application.UseAddress:=Boolean(Posted.UseAddress ?? false);
        Application.Description:=Posted.Description;

        UpdateObject(Application)
    );

    ]]
| Featured Peer Review application                                                   ||
|:-------------|:---------------------------------------------------------------------|
| ID           | `((Application.LegalId))`                                            |
| ID State     | `((Application.State))`                                              |
| Provider     | `((Application.Provider))`                                           |
| Application  | ((Application.ApprovedForPublication ? "Featured" : "Not Featured")) |
| Created      | ((Application.Created))                                              |
| Updated      | ((Application.Updated))                                              |
| From         | ((Application.From))                                                 |
| To           | ((Application.To))                                                   |
| Full Name    | ((Application.FullName))                                             |
| Country      | <input type='checkbox' id='UseCountry' name='UseCountry'((Application.UseCountry?" checked":""))((Disabled:=Application.ApprovedForPublication ? " disabled='disabled'" : ""))><label for='UseCountry'>Only review applications for ((Application.Country))</label> |
| Region       | <input type='checkbox' id='UseRegion' name='UseRegion'((Application.UseRegion?" checked":""))((Disabled))><label for='UseRegion'>Only review applications for ((Application.Region))</label> |
| City         | <input type='checkbox' id='UseCity' name='UseCity'((Application.UseCity?" checked":""))((Disabled))><label for='UseCity'>Only review applications for ((Application.City))</label> |
| Area         | <input type='checkbox' id='UseArea' name='UseArea'((Application.UseArea?" checked":""))((Disabled))><label for='UseArea'>Only review applications for ((Application.Area))</label> |
| Postal Code  | <input type='checkbox' id='UseZip' name='UseZip'((Application.UseZip?" checked":""))((Disabled))><label for='UseZip'>Only review applications for ((Application.Zip))</label> |
| Address      | <input type='checkbox' id='UseAddress' name='UseAddress'((Application.UseAddress?" checked":""))((Disabled))><label for='UseAddress'>Only review applications for ((Application.Address))</label> |
| e-Mail       | ((Application.EMail))                                                |
| Phone Number | ((Application.PhoneNumber))                                          |
| JID          | ((Application.Jid))                                                  |
| Description  | <input type='text' id='Description' name='Description' value='((HtmlValueEncode(Application.Description) ))'/> |
| <div style='text-align:center'><img src='/FeaturedPeerReviewers/Images/((Application.LegalId)).webp' alt='Photo' width='((Application.PhotoWidth))' height='((Application.PhotoHeight))' /></div> ||
| <div style='text-align:center'><button type='submit' class='posButton'((Disabled))>Update</button> <button type='button' class='negButton' onclick='DeleteApplication()'>Delete</button></div> ||
[[
)
else
(
    ]]<div id="quickLoginCode" data-mode="image" data-serviceId="((QuickLoginServiceId(Request);))"
     data-purpose="To apply to become a featured peer reviewer on ((Domain))."></div>
[[
)
}}

</form>