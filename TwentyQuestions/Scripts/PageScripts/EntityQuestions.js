/// <reference path="../_references.js" />

var EntityQuestions = function ()
{
	var WebPage = null;

	var Data = {};

	this.initializeControls = function ()
	{
		WebPage = new Mvc.WebPage();

		var googleImageSearch = new GoogleImageSearch($("#entityName").text(), function (imgUrl)
		{
			$("#entity-picture").attr("src", imgUrl);
		}).search();

		$("span.view-field").click(function ()
		{
			var index = $(this).data("index");
			$(this).hide();

			$('input[data-index=' + index + ']')
				.show()
				.focus();
		});

		$("a.change-link-lock").click(function ()
		{
			var ctx = this;

			var index = $(ctx).data("index");
			var setToLocked = $(ctx).text() == "Lock" ? true : false;

			WebPage.ajaxCall(
				"/EntityQuestions/SetLockedStatus",
				{
					"idEntityQuestion": index,
					"setToLocked": setToLocked
				},
				function (data)
				{
					if (data)
					{
						$(ctx)
							.text(setToLocked ? "Unlock" : "Lock")
							.css("color", setToLocked ? "blue" : "red");
					}
				},
				{
					"debugMode": false,
					"showAjaxRoller": true
				});
		});

		$("a.change-link-force").click(function () {
			var ctx = this;

			var index = $(ctx).data("index");
			var type = $(ctx).data("type");

			WebPage.ajaxCall(
				"/EntityQuestions/ForceMajorityAnswer",
				{
					"idEntityQuestion": index,
					"type": type
				},
				function (data) {
					if (data) {
						alert("Forced majority answer to be " + type + ". Consider locking this association next.");
					}
				},
				{
					"debugMode": false,
					"showAjaxRoller": true
				});
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
					"/EntityQuestions/UpdateEntityQuestionFitness",
					{
						"idEntityQuestion": index,
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