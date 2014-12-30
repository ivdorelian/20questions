/// <reference path="../_references.js" />

var QuestionEntities = function ()
{
	var WebPage = null;

	var Data = {};

	this.initializeControls = function ()
	{
		WebPage = new Mvc.WebPage();

		$("span.view-field").click(function ()
		{
			var index = $(this).data("index");
			$(this).hide();

			$('input[data-index=' + index + ']')
				.show()
				.focus();
		});

		// force the input field to decimal only and containing 3 decimal places
		$("input.edit-field").forceNumeric(
		{
			'allowNegative': true,
			'allowDecimal': true,
			'decimalPlaces': 3
		});

		// keyup apparently works better across browsers
		$("input.edit-field").keyup(function (e)
		{
			var ctx = this;

			var p = e.which;
			var index = $(this).data("index");
			var value = $(this).val();

			if (p == 13) // enter
			{
				WebPage.ajaxCall(
					"/QuestionEntities/UpdateQuestionEntityFitness",
					{
						"idQuestionEntity": index,
						"newFitness": value
					},
					function (data)
					{
						if (data)
						{
							$(ctx)
								.val(value)
								.hide();

							$('span[data-index=' + index + ']')
								.text(value)
								.show();
						}
					},
					{
						"debugMode": false,
						"showAjaxRoller": true
					});
			}
			else if (p == 27) // esc
			{
				$(ctx)
					.val(value)
					.hide();

				$('span[data-index=' + index + ']')
					.show();
			}
		});
	};
};