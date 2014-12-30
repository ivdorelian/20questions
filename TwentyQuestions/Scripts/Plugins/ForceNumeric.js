//jQuery custom plugins
(function ($)
{
	$.fn.forceNumeric = function (options)
	{
		var settings = $.extend({
			'allowNegative': false,
			'allowDecimal': false,
			'decimalPlaces': 0
		}, options);

		return this.each(function ()
		{
			$(this).keypress(function (e)
			{
				var key;
				var isCtrl = false;
				var keychar;
				var reg;

				if (window.event)
				{
					key = e.keyCode;
					isCtrl = window.event.ctrlKey;
				}
				else if (e.which)
				{
					key = e.which;
					isCtrl = e.ctrlKey;
				}

				if (isNaN(key)) return true;

				keychar = String.fromCharCode(key);

				// check for backspace or delete, or if Ctrl was pressed
				if (key == 8 || isCtrl)
				{
					return true;
				}

				reg = /\d/;
				var isFirstNegative = settings.allowNegative ? keychar == '-' && this.value.indexOf('-') == -1 : false;
				var isFirstDecimal = settings.allowDecimal ? keychar == '.' && this.value.indexOf('.') == -1 : false;

				var cursorPosition = $(this).getCursorPosition();
				var currentValue = $(this).val();
				var currentValueLength = currentValue.length;

				//only allow negative character in the beginning
				if (settings.allowNegative && isFirstNegative)
				{
					return cursorPosition == 0;
				}

				//if first decimal, make sure it is in the right decimal position, else move it automatically to the right decimal place
				if (settings.allowDecimal && isFirstDecimal && settings.decimalPlaces > 0)
				{
					//first decimal and at the end of the value, it's ok
					if (cursorPosition == currentValueLength || currentValueLength - cursorPosition <= settings.decimalPlaces)
					{
						return true;
					}

					//else we have to move it automatically to the right decimal place
					if (currentValueLength - cursorPosition != settings.decimalPlaces)
					{
						var integerPart = currentValue.substring(0, currentValueLength - settings.decimalPlaces);
						var fractionalPart = currentValue.substring(currentValueLength - settings.decimalPlaces, currentValueLength);
						$(this).val(integerPart + "." + fractionalPart);
						//return false to avoid system to print the . at the end
						return false;
					}
				}

				//if there's a decimal in place, make sure no more numeric allowed after specified decimal place
				if (settings.allowDecimal && currentValue.indexOf('.') > 0)
				{
					//if decimal places was reached, cancel the input
					if (cursorPosition > currentValue.indexOf('.') && currentValue.length - currentValue.indexOf('.') > settings.decimalPlaces)
					{
						return false;
					}
				}

				return isFirstNegative || isFirstDecimal || reg.test(keychar);
			});
		});
	};

	$.fn.getCursorPosition = function ()
	{
		var pos = 0;
		var input = $(this).get(0);
		// IE Support
		if (document.selection)
		{
			input.focus();
			var sel = document.selection.createRange();
			var selLen = document.selection.createRange().text.length;
			sel.moveStart('character', -input.value.length);
			pos = sel.text.length - selLen;
		}
			// Firefox support
		else if (input.selectionStart || input.selectionStart == '0')
			pos = input.selectionStart;

		return pos;
	};
})(jQuery);