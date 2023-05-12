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
				window.alert(xhttp.responseText);

			delete xhttp;
		}
	};

	xhttp.open("POST", "Accept.ws", true);
	xhttp.setRequestHeader("Content-Type", "text/plain");
	xhttp.setRequestHeader("Accept", "application/json");
	xhttp.send(LegalId);
}

function Reject(LegalId)
{
	var xhttp = new XMLHttpRequest();
	xhttp.onreadystatechange = function ()
	{
		if (xhttp.readyState === 4)
		{
			if (xhttp.status === 200)
				RemoveApplication(LegalId);
			else
				window.alert(xhttp.responseText);

			delete xhttp;
		}
	};

	xhttp.open("POST", "Reject.ws", true);
	xhttp.setRequestHeader("Content-Type", "text/plain");
	xhttp.setRequestHeader("Accept", "application/json");
	xhttp.send(LegalId);
}

function RemoveApplication(LegalId)
{
	var Tr = document.getElementById(LegalId + "_1");
	Tr.parentElement.removeChild(Tr);

	Tr = document.getElementById(LegalId + "_2");
	Tr.parentElement.removeChild(Tr);
}

function AddFeaturedReviewer(Application)
{
}