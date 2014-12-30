/**
 * Created with JetBrains PhpStorm.
 * User: Alex
 * Date: 9/30/13
 * Time: 7:54 PM
 * To change this template use File | Settings | File Templates.
 */



var PHP = PHP || {};

PHP.WebPage = function()
{
	/// <summary>Creates a page instance</summary>

};

PHP.WebPage.prototype.ajaxCall = function (url, arguments, successCallback, oConfiguration)
{
	/// <summary>Performs an ajax call</summary>

	//alert(url);

	var DefaultConfiguration = {
		"showAjaxRoller": false,
		"debugMode": false
	};

	var Configuration = $.extend(DefaultConfiguration, oConfiguration);

	if(Configuration.showAjaxRoller)
	{
		var ajaxRoller =  $("#loading-indicator");
		ajaxRoller.show();
	}

	var ajaxDataType = 'json';
	if(Configuration.debugMode)
	{
		ajaxDataType = 'html';
	}


	$.ajax({
		type: "POST",
		url: url,
		data:arguments,
		dataType: ajaxDataType,
		success: function (reply)
		{
			if(Configuration.showAjaxRoller)
			{
				ajaxRoller.hide();
			}

			if(!Configuration.debugMode)
			{
				if(reply.success)
				{
					if ((successCallback) && (typeof successCallback == "function"))
					{
						successCallback.call(this, reply.data);
					}
				}
				else
				{
					alert(reply.errorText);
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
			if(Configuration.showAjaxRoller)
			{
				ajaxRoller.hide();
			}

			//alert("Error!");
			/*
			var errorText = 'Unexpected error. Please contact administrator! Error: ' + textStatus + ' - ';
			var jsonObject = null;

			try
			{
				jsonObject = $.parseJSON(XMLHttpRequest.responseText);
				if (jsonObject)
				{
					errorText += jsonObject.Message;
					errorText += "Exception type: " + jsonObject.ExceptionType;
					errorText += "Stack trace: " + jsonObject.StackTrace;
				}
			}
			catch (ex)
			{
				errorText += XMLHttpRequest.responseText;
			}

			alert(errorText);
			*/
			alert('Unexpected error. Please contact administrator! Error: ' + textStatus + '-' + XMLHttpRequest.responseText);
		}
	});
};
