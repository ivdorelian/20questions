/// <reference path="../_references.js" />


var Mvc = Mvc || {};

var AjaxActionResultType = {
	"Undefined": 0,
	"Success": 1,
	"Error": 2
};

Mvc.WebPage = function ()
{
	/// <summary>Creates a page instance</summary>

};

Mvc.WebPage.prototype.ajaxCall = function (url, arguments, successCallback, oConfiguration)
{
	/// <summary>Performs an ajax call</summary>

	//alert(url);

	var DefaultConfiguration = {
		"showAjaxRoller": false,
		"debugMode": false
	};

	var Configuration = $.extend(DefaultConfiguration, oConfiguration);

	if (Configuration.showAjaxRoller)
	{
		var ajaxRoller = $("#progress_bar");
		ajaxRoller.show();
	}

	var ajaxDataType = 'json';
	if (Configuration.debugMode)
	{
		ajaxDataType = 'html';
	}


	$.ajax({
		type: "POST",
		url: url,
		data: arguments,
		dataType: ajaxDataType,
		success: function (reply)
		{
			if (Configuration.showAjaxRoller)
			{
				ajaxRoller.hide();
			}

			if (!Configuration.debugMode)
			{
				if (reply.ResultType == AjaxActionResultType.Success)
				{
					if ((successCallback) && (typeof successCallback == "function"))
					{
						successCallback.call(this, reply.Data);
					}
				}
				else
				{
					alert(reply.ErrorText);
				}
			}
			else
			{
				// debug mode is on -> we show the data as html in a pop-up
				alert(reply);
			}
		},
		error: function (XMLHttpRequest, textStatus, errorThrown)
		{
			if (Configuration.showAjaxRoller)
			{
				ajaxRoller.hide();
			}

			alert('Unexpected error. Please contact administrator! Error: ' + textStatus + '-' + XMLHttpRequest.responseText);
		}
	});
};

Mvc.WebPage.prototype.deleteOldGames = function ()
{
	this.ajaxCall(
		"/Game/DeleteOldGames",
		{},
		function (data) { },
		null);
}