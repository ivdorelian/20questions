(function ($)
{
	$.fn.extend({
		preventSubmit: function (callbackOnEnter)
		{
			var options = jQuery.noop();
			if ((callbackOnEnter) && jQuery.isFunction(callbackOnEnter))
			{
				options = callbackOnEnter;
			}

			//Iterate over the current set of matched elements
			return this.each(function ()
			{
				$(this).keydown(function (ev)
				{
					if (ev.which == 13)
					{
						var oCallback = options;
						ev.stopPropagation();
						ev.preventDefault();

						if ((oCallback) && jQuery.isFunction(oCallback))
						{
							oCallback.call();
						}
						return false;
					}
				});

				$(this).keypress(function (ev)
				{
					if (ev.which == 13)
					{
						ev.stopPropagation();
						ev.preventDefault();

						return false;
					}
				});
			});
		}
	});
})(jQuery);