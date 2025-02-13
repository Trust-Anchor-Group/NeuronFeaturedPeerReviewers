function OpenPage(Url)
{
	window.open(Url, "_blank");
}

function Accept(LegalId)
{
	var xhttp = new XMLHttpRequest();
	xhttp.onreadystatechange = function ()
	{
		if (xhttp.readyState === 4)
		{
			if (xhttp.status === 200)
			{
				RemoveApplication(LegalId);
				AddFeaturedReviewer(JSON.parse(xhttp.responseText));
			}
			else
				Popup.Alert(xhttp.responseText); // await is not needed

			delete xhttp;
		}
	};

	xhttp.open("POST", "Accept.ws", true);
	xhttp.setRequestHeader("Content-Type", "text/plain");
	xhttp.setRequestHeader("Accept", "application/json");
	xhttp.send(LegalId);
}

async function Delete(LegalId)
{
	if (!(await Popup.Confirm("Are you sure you want to delete the featured reviewer?")))
		return;

	RejectOrDelete(LegalId,false);
}

function Reject(LegalId)
{
	RejectOrDelete(LegalId, true);
}

function RejectOrDelete(LegalId,IsReject)
{
	var xhttp = new XMLHttpRequest();
	xhttp.onreadystatechange = function ()
	{
		if (xhttp.readyState === 4)
		{
			if (xhttp.status === 200)
				RemoveApplication(LegalId);
			else
				Popup.Alert(xhttp.responseText); // await is not needed

			delete xhttp;
		}
	};

	xhttp.open("POST", "Reject.ws", true);
	xhttp.setRequestHeader("Content-Type", "application/json");
	xhttp.setRequestHeader("Accept", "application/json");
	xhttp.send(JSON.stringify(
		{
			"legalId": LegalId,
			"isReject": IsReject
		}));
}

function RemoveApplication(LegalId)
{
	var Tr = document.getElementById(LegalId + "_1");
	Tr.parentElement.removeChild(Tr);

	Tr = document.getElementById(LegalId + "_2");
	Tr.parentElement.removeChild(Tr);
}

function NewApplication(Reviewer)
{
	var Button1 = document.createElement("BUTTON");
	Button1.className = "posButton";
	Button1.innerText = "Accept";
	Button1.setAttribute("type", "button");
	Button1.setAttribute("onclick", "Accept('" + Reviewer.legalId + "')");

	var Button2 = document.createElement("BUTTON");
	Button2.className = "negButton";
	Button2.innerText = "Reject";
	Button2.setAttribute("type", "button");
	Button2.setAttribute("onclick", "Reject('" + Reviewer.legalId + "')");

	var TBody = document.getElementById("Applications");
	AddRecord(Reviewer, TBody, Button1, Button2);
}

function AddFeaturedReviewer(Reviewer)
{
	var Button = document.createElement("BUTTON");
	Button.className = "negButton";
	Button.innerText = "Delete";
	Button.setAttribute("type", "button");
	Button.setAttribute("onclick", "Delete('" + Reviewer.legalId + "')");

	var TBody = document.getElementById("FeaturedReviewers");
	AddRecord(Reviewer, TBody, null, Button);
}

function AddRecord(Reviewer, TBody, Button1, Button2)
{
	var Tr = document.createElement("TR");
	Tr.setAttribute("id", Reviewer.legalId + "_1");
	TBody.appendChild(Tr);

	var Td = document.createElement("TD");
	Td.setAttribute("rowspan", "2");
	Td.innerHTML = "<img src='/FeaturedPeerReviewers/Images/" + Reviewer.legalId + ".webp' alt='Photo' width='" +
		Reviewer.photoWidth + "' height='" + Reviewer.photoHeight + "' />";
	Tr.appendChild(Td);

	Td = document.createElement("TD");
	Td.setAttribute("colspan", "5");
	Td.innerHTML = "<a href='/ValidateLegalId.md?ID=" + Reviewer.legalId + "&Purpose=Reviewing%20application' target='_blank'><code>" +
		Reviewer.legalId + "</code></a>";
	Tr.appendChild(Td);

	Td = document.createElement("TD");
	Td.setAttribute("colspan", "2");
	Td.innerText = Reviewer.fullName;
	Tr.appendChild(Td);

	Td = document.createElement("TD");
	Td.setAttribute("colspan", "2");
	Td.innerText = Reviewer.description;
	Tr.appendChild(Td);

	Td = document.createElement("TD");
	if (Button1)
		Td.appendChild(Button1);
	Tr.appendChild(Td);

	Tr = document.createElement("TR");
	Tr.setAttribute("id", Reviewer.legalId + "_2");
	TBody.appendChild(Tr);

	Td = document.createElement("TD");
	Td.innerText = Reviewer.state;
	Tr.appendChild(Td);

	Td = document.createElement("TD");
	Td.innerText = Reviewer.from;
	Tr.appendChild(Td);

	Td = document.createElement("TD");
	Td.innerText = Reviewer.to;
	Tr.appendChild(Td);

	Td = document.createElement("TD");
	Td.innerText = Reviewer.useCountry ? Reviewer.country : "*";
	Tr.appendChild(Td);

	Td = document.createElement("TD");
	Td.innerText = Reviewer.useRegion ? Reviewer.region : "*";
	Tr.appendChild(Td);

	Td = document.createElement("TD");
	Td.innerText = Reviewer.useCity ? Reviewer.city : "*";
	Tr.appendChild(Td);

	Td = document.createElement("TD");
	Td.innerText = Reviewer.useArea ? Reviewer.area : "*";
	Tr.appendChild(Td);

	Td = document.createElement("TD");
	Td.innerText = Reviewer.useZip ? Reviewer.zip : "*";
	Tr.appendChild(Td);

	Td = document.createElement("TD");
	Td.innerText = Reviewer.useAddress ? Reviewer.address : "*";
	Tr.appendChild(Td);

	Td = document.createElement("TD");
	if (Button2)
		Td.appendChild(Button2);
	Tr.appendChild(Td);
}

function ApplicationUpdated(Reviewer)
{
	RemoveApplication(Reviewer.legalId);
	NewApplication(Reviewer);
}