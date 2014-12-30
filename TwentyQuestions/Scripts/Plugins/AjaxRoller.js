(function ($)
{
	$.ajaxRoller = {};
	$.ajaxRoller.show = function (imagePath)
	{
		if (!imagePath)
		{
			imagePath = 'images/ajax-loader.gif';
		}

		var targetElement = $("body");

		if ((targetElement) && (targetElement.length > 0))
		{
			var ajaxDiv = $(".ajaxRoller");
			if ((ajaxDiv) && (ajaxDiv.length > 0))
			{
				ajaxDiv.remove();
			}

			ajaxDiv = $("<div class='ajaxRoller'></div>")
				.css("position", "absolute")
				.css("margin", "auto")
				.css("top", "0px")
				.css("left", "0px")
				.css("width", "100%")
				.css("height", "100%")
				.css("text-align", "center")
				.css("vertical-align", "middle")
				.css("background-color", "Transparent")
				.css("z-index", "100000")
				.append(
					$("<img src='" + imagePath + "' alt='Loading...' />")
						.css("margin-top", "20%")
				);
			targetElement.append(ajaxDiv);
		}
	};

	$.ajaxRoller.hide = function ()
	{
		var ajaxDiv = $(".ajaxRoller");
		if ((ajaxDiv) && (ajaxDiv.length > 0))
		{
			ajaxDiv.remove();
		}
	};
})(jQuery);