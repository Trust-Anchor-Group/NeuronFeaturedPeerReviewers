Title: Featured Peer Reviewers
Description: Allows the operator to administer featured peer reviewers.
Author: Peter Waher
Date: 2023-05-10
Master: /Master.md
Cache-Control: max-age=0, no-cache, no-store
UserVariable: User
Privilege: Admin.Identity.FeaturedPeerReviewers
Login: /Login.md
Javascript: Settings.js

========================================================================

Tools
=========

<button type="button" onclick="OpenPage('Apply.md')" class="posButton">Application Page</button>

========================================================================

Pending applications
--------------------------

<table>
<thread>
<tr>
<th rowspan="2"/>
<th colspan="5">Legal ID</th>
<th colspan="2">FullName</th>
<th colspan="2">Description</th>
<th></th>
</tr>
<tr>
<th>State</th>
<th>From</th>
<th>To</th>
<th>Country</th>
<th>Region</th>
<th>City</th>
<th>Area</th>
<th>Postal Code</th>
<th>Address</th>
<th></th>
</tr>
</thead>
<tbody>
{{
foreach Reviewer in select * from FeaturedPeerReviewer where ApprovedForPublication=false do
(
	]]<tr id='((Reviewer.LegalId))_1'>
<td rowspan="2"><img src='/FeaturedPeerReviewers/Images/((Reviewer.LegalId)).webp' alt='Photo' width='((Reviewer.PhotoWidth))' height='((Reviewer.PhotoHeight))' /></td>
<td colspan="5"><a href="/ValidateLegalId.md?ID=((Reviewer.LegalId))&Purpose=Reviewing%20application" target="_blank">`((Reviewer.LegalId))`</a></td>
<td colspan="2">((Reviewer.FullName))</td>
<td colspan="2">((Reviewer.Description))</td>
<td><button type="button" class="posButton" onclick="Accept('((Reviewer.LegalId))')">Accept</button></td>
</tr>
<tr id='((Reviewer.LegalId))_2'>
<td>`((Reviewer.State))`</td>
<td>((Reviewer.From.ToShortDateString();))</td>
<td>((Reviewer.To.ToShortDateString();))</td>
<td>((Reviewer.UseCountry?Reviewer.Country))</td>
<td>((Reviewer.UseRegion?Reviewer.Region))</td>
<td>((Reviewer.UseCity?Reviewer.City))</td>
<td>((Reviewer.UseArea?Reviewer.Area))</td>
<td>((Reviewer.UseZip?Reviewer.Zip))</td>
<td>((Reviewer.UseAddress?Reviewer.Address))</td>
<td><button type="button" class="negButton" onclick="Reject('((Reviewer.LegalId))')">Reject</button></td>
</tr>
[[
)
}}
</tbody>
</table>

========================================================================

Featured Peer Reviewers
--------------------------

| Legal ID | State | From | To | Full Name | Description | Country | Region | City | Area | Postal Code | Address |
|:---------|:------|:-----|:---|:----------|:------------|:--------|:-------|:-----|:-----|:------------|:--------|
{{
foreach Reviewer in select * from FeaturedPeerReviewer where ApprovedForPublication=true do
(
	]]| `((Reviewer.LegalId))` | `((Reviewer.State))` | ((Reviewer.From.ToShortDateString();)) | ((Reviewer.To.ToShortDateString();)) | ((Reviewer.FullName)) | ((Reviewer.Description)) | ((Reviewer.UseCountry?Reviewer.Country)) | ((Reviewer.UseRegion?Reviewer.Region)) | ((Reviewer.UseCity?Reviewer.City)) | ((Reviewer.UseArea?Reviewer.Area)) | ((Reviewer.UseZip?Reviewer.Zip)) | ((Reviewer.UseAddress?Reviewer.Address)) |
[[
)
}}