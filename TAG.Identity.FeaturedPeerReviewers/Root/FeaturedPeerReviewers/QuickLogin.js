function DisplayQuickLogin()
{
	var Div = document.getElementById("quickLoginCode");
	if (!Div)
		return;

	if (!Div.hasAttribute("data-done"))
	{
		Div.className = "QuickLogin";
		Div.setAttribute("data-done", "0");
	}
	else if (Div.getAttribute("data-done") == "1")
		return;

	var Mode = Div.getAttribute("data-mode");
	var Purpose = Div.getAttribute("data-purpose");
	var ServiceId = Div.hasAttribute("data-serviceId") ? Div.getAttribute("data-serviceId") : "";

	var xhttp = new XMLHttpRequest();
	xhttp.onreadystatechange = function ()
	{
		if (xhttp.readyState === 4)
		{
			if (xhttp.status === 200)
			{
				try
				{
					var Data = JSON.parse(xhttp.responseText);
					var A = document.getElementById("quickLoginA");

					if (!A)
					{
						A = document.createElement("A");
						A.setAttribute("id", "quickLoginA");
						Div.appendChild(A);
					}

					A.setAttribute("href", Data.signUrl);

					if (Data.text)
					{
						var Pre = document.getElementById("quickLoginPre");

						if (!Pre)
						{
							Pre = document.createElement("PRE");
							Pre.setAttribute("id", "quickLoginPre");
							A.appendChild(Pre);
						}

						Pre.innerText = Data.text;

						var Img = document.getElementById("quickLoginImg");
						if (Img)
							Img.parentNode.removeChild(Img);
					}
					else
					{
						var Img = document.getElementById("quickLoginImg");

						if (!Img)
						{
							Img = document.createElement("IMG");
							Img.setAttribute("id", "quickLoginImg");
							A.appendChild(Img);
						}

						if (Data.base64)
							Img.setAttribute("src", "data:" + Data.contentType + ";base64," + Data.base64);
						else if (Data.src)
							Img.setAttribute("src", Data.src);

						Img.setAttribute("width", Data.width);
						Img.setAttribute("height", Data.height);

						var Pre = document.getElementById("quickLoginPre");
						if (Pre)
							Pre.parentNode.removeChild(Pre);
					}

					LoginTimer = window.setTimeout(function () { DisplayQuickLogin(); }, 2000);
				}
				catch (e)
				{
					console.log(e);
					console.log(xhttp.responseText);
				}
			}
			else
				ShowError(xhttp);
		};
	}

	var Uri = window.location.protocol + "//" + FindNeuronDomain() + "/QuickLogin";

	xhttp.open("POST", Uri, true);
	xhttp.setRequestHeader("Content-Type", "application/json");
	xhttp.send(JSON.stringify(
		{
			"serviceId": ServiceId,
			"tab": TabID,
			"mode": Mode,
			"purpose": Purpose
		}));
}

function SignatureReceivedBE(Empty)
{
	window.clearTimeout(LoginTimer);

	var Div = document.getElementById("quickLoginCode");
	if (!Div)
		return;

	Div.setAttribute("data-done", "1");
	Div.innerHTML = "<p>Preparing application...</p>";

	var xhttp = new XMLHttpRequest();
	xhttp.onreadystatechange = function ()
	{
		if (xhttp.readyState === 4)
		{
			if (xhttp.status === 200)
			{
				Div.innerHTML = "";
				ApplicationPrepared(Div,JSON.parse(xhttp.responseText));
			}
			else if (xhttp.status === 406)
				window.location.href = "Remove.md";
			else
				window.alert(xhttp.responseText + " (" + xhttp.status + ")");

			delete xhttp;
		}
	};

	xhttp.open("POST", "PrepareApplication.ws", true);
	xhttp.setRequestHeader("Content-Type", "text/plain");
	xhttp.setRequestHeader("Accept", "application/json");
	xhttp.send("");
}

function ApplicationPrepared(Div,Application)
{
	var H2 = document.createElement("H2");
	Div.appendChild(H2);
	H2.innerText = "Application created.";

	var TBody = AddTable(Div, "Identity of reviewer");

	AddRow(TBody, "Id", Application.legalId, true);
	AddRow(TBody, "Provider", Application.provider, true);
	AddRow(TBody, "State", Application.state, true);
	AddRow(TBody, "Created", new Date(1000 * Application.created), false);

	if (Application.updated)
		AddRow(TBody, "Updated", new Date(1000 * Application.updated), false);

	AddRow(TBody, "From", new Date(1000 * Application.from), false);
	AddRow(TBody, "To", new Date(1000 * Application.to), false);
	AddRow(TBody, "Full Name", Application.fullName, false);
	AddRow(TBody, "Country", Application.country, false);
	AddRow(TBody, "Region", Application.region, false);
	AddRow(TBody, "City", Application.city, false);
	AddRow(TBody, "Area", Application.area, false);
	AddRow(TBody, "Postal Code", Application.zip, false);
	AddRow(TBody, "Address", Application.address, false);
	AddRow(TBody, "e-Mail", Application.eMail, false);
	AddRow(TBody, "Phone Number", Application.phoneNumber, false);
	AddRow(TBody, "JID", Application.jid, false);

	var Tr = document.createElement("TR");
	TBody.appendChild(Tr);

	var Td = document.createElement("TD");
	Td.setAttribute("colspan", "2");
	Td.setAttribute("style", "text-align:center");
	Tr.appendChild(Td);

	var Img = document.createElement("IMG");
	Img.setAttribute("src", Application.photo);
	Img.setAttribute("alt", "Photo");
	Img.setAttribute("width", Application.photoWidth);
	Img.setAttribute("height", Application.photoHeight);
	Td.appendChild(Img);
}

function AddTable(Div, Title)
{
	var Table = document.createElement("TABLE");
	Div.appendChild(Table);

	var THead = document.createElement("THEAD");
	Table.appendChild(THead);

	var Tr = document.createElement("TR");
	THead.appendChild(Tr);

	var Th = document.createElement("TH");
	Th.setAttribute("colspan", "2");
	Th.innerText = Title;
	Tr.appendChild(Th);

	var TBody = document.createElement("TBODY");
	Table.appendChild(TBody);

	return TBody;
}

function AddRow(TBody, Name, Value, Code)
{
	var Tr = document.createElement("TR");
	TBody.appendChild(Tr);

	var Td = document.createElement("TD");
	Td.innerText = Name;
	Tr.appendChild(Td);

	Td = document.createElement("TD");
	Tr.appendChild(Td);

	if (Code)
	{
		var Code = document.createElement("CODE");
		Code.innerText = Value;
		Td.appendChild(Code);
	}
	else
		Td.innerText = Value;
}

var LoginTimer = window.setTimeout(function () { DisplayQuickLogin(); }, 100);