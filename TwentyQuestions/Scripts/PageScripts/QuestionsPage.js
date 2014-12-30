/// <reference path="../_references.js" />

var QuestionsPage = function ()
{
	/// <summary>Represents the QuestionsPage object.</summary>
	/// <returns type="QuestionsPage">Initializes a new instance of the QuestionsPage object.</returns>


	var WebPage = null;

	var Controls = {};
	Controls.txtSubmitQuestion = null;

	var Data = {};
	Data.ClickedID = null;
	Data.ClickedBody = '';

	this.initializeControls = function ()
	{
		/// <summary>Initializes the controls of the QuestionsPage object.</summary>

		WebPage = new Mvc.WebPage();

		Controls.txtSubmitQuestion = $("#txtSubmitQuestion");
		if (Controls.txtSubmitQuestion.length > 0)
		{
			Controls.txtSubmitQuestion.preventSubmit();
			Controls.txtSubmitQuestion.autocomplete({
				source: function (request, response)
				{
					Controls.txtSubmitQuestion.data({ "IDQuestion": -1 });
					WebPage.ajaxCall(
						"/Questions/GetQuestionsForAutocomplete",
						{
							"needle": Controls.txtSubmitQuestion.val()
						},
						function (data)
						{
							response($.map(data, function (item)
							{
								return { ID: item.IDQuestion, label: item.QuestionBody }
							}))
						});
				},
				minLength: 10,
				select: function (event, ui)
				{
					if (ui && ui.item)
					{
						Controls.txtSubmitQuestion.data({ "IDQuestion": ui.item.ID });
					}
				},
				close: function ()
				{
					if (Controls.txtSubmitQuestion.data().ObjectId == -1)
					{
						//Controls.txtObjectName.val("");
					}
				}
			});
		}

		$("a.edit-link-qbody").click(function () {
			var index = $(this).data("index");

			$('a#' + index).hide();

			$('span.control-links[data-index=' + index + ']')
				.hide();

			$('input.edit-field-qbody[data-index=' + index + ']')
				.show()
				.focus();
		});

		// keyup apparently works better across browsers
		$("input.edit-field-qbody").keyup(function (e) {
			var ctx = this;

			var p = e.which;
			var index = $(this).data("index");
			var value = $(this).val();

			if (p == 13) // enter
			{
				WebPage.ajaxCall(
					"/Questions/UpdateQuestionBody",
					{
						"idQuestion": index,
						"newBody": value
					},
					function (data) {
						if (data) {
							$(ctx)
								.val(value)
								.hide();

							$('a#' + index)
								.text(value)
								.show();

							$('span.control-links[data-index=' + index + ']')
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

				$('span.control-links[data-index=' + index + ']')
					.show();

				$('a#' + index).show();
			}
		});

		$("#delDialog .btn.btn-primary").on("click", function (e) {
			WebPage.ajaxCall(
				"/Questions/DeleteQuestion",
				{
					"idQuestion": Data.ClickedID
				},
				function (data) {
					if (data) {
						$("a.del-link-question[data-index=" + Data.ClickedID + "]").closest("tr").remove();
						$("#delDialog").modal("hide");
					}
				},
				{
					"debugMode": false,
					"showAjaxRoller": true
				});
		});

		$("a.del-link-question").on("click", function (e) {
			e.preventDefault();
			Data.ClickedID = $(this).data("index");

			$("#delDialog b").text($('a#' + Data.ClickedID).text());

			$("#delDialog").modal("show");
		});

	}
}