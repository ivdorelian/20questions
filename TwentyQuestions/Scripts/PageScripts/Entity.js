/// <reference path="../_references.js" />

var Entity = function ()
{
	var WebPage = null;

	var Data = {};
	Data.ClickedID = null;

	this.initializeControls = function ()
	{
		WebPage = new Mvc.WebPage();

		$("a.edit-link-name").click(function ()
		{
			var index = $(this).data("index");

			$('a#' + index).hide();

			$('span.control-links[data-index=' + index + ']')
				.hide();

			$('input.edit-field-name[data-index=' + index + ']')
				.show()
				.focus();
		});

		$("span.view-field-description").click(function ()
		{
			var index = $(this).data("index");
			$(this).hide();

			$('input.edit-field-description[data-index=' + index + ']')
				.show()
				.focus();
		});

		// keyup apparently works better across browsers
		$("input.edit-field-name").keyup(function (e)
		{
			var ctx = this;

			var p = e.which;
			var index = $(this).data("index");
			var value = $(this).val();

			if (p == 13) // enter
			{
				WebPage.ajaxCall(
					"/Entities/UpdateEntityName",
					{
						"idEntity": index,
						"newName": value
					},
					function (data)
					{
						if (data)
						{
							$(ctx)
								.val(value)
								.hide();

							$('a#' + index)
								.text(value)
								.show();

							$('span.control-links[data-index=' + index + ']').show();
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

				$('span[data-index=' + index + ']').show();

				$('a#' + index).show();
			}
		});

		$("input.edit-field-description").keyup(function (e) {
			var ctx = this;

			var p = e.which;
			var index = $(this).data("index");
			var value = $(this).val();

			if (p == 13) // enter
			{
				WebPage.ajaxCall(
					"/Entities/UpdateEntityDescription",
					{
						"idEntity": index,
						"newDescription": value
					},
					function (data) {
						if (data) {
							$(ctx)
								.val(value)
								.hide();

							$('span.view-field-description[data-index=' + index + ']')
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

				$('span.view-field-description[data-index=' + index + ']')
					.show();
			}
		});

		$("#delDialog .btn.btn-primary").on("click", function (e) {
			WebPage.ajaxCall(
				"/Entities/DeleteEntity",
				{
					"idEntity": Data.ClickedID
				},
				function (data) {
					if (data) {
						$("a.del-link-entity[data-index=" + Data.ClickedID + "]").closest("tr").remove();
						$("#delDialog").modal("hide");
					}
				},
				{
					"debugMode": false,
					"showAjaxRoller": true
				});
		});

		$("a.del-link-entity").on("click", function (e) {
			e.preventDefault();
			Data.ClickedID = $(this).data("index");

			$("#delDialog b").text($('a#' + Data.ClickedID).text());

			$("#delDialog").modal("show");
		});
	};
};