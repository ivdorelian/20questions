/// <reference path="../_references.js" />

var GamePage = function ()
{
	/// <summary>Represents the GamePage object.</summary>
	/// <returns type="GamePage">Initializes a new instance of the GamePage object.</returns>

	var WebPage = null;

	var GivenAnswer = {
		/// <field name='Undefined' type='int'>Undefined.</field>
		Undefined: 0,

		/// <field name='Yes' type='int'>Yes. (If the player thinks that the asked question is correct)</field>
		Yes: 1,

		/// <field name='No' type='int'>No. (If the player thinks that the asked question is not correct)</field>
		No: 2,

		/// <field name='Unknown' type='int'>Unknown. (If the player doesn't know if the question is correct or not)</field>
		Unknown: 3,

		/// <field name='ProbablyYes' type='int'>ProbablyYes. (If the player thinks that the asked question is probably correct)</field>
		ProbablyYes: 4,

		/// <field name='ProbablyNo' type='int'>ProbablyNo. (If the player thinks that the asked question is probably not correct)</field>
		ProbablyNo: 5
	};

	var ResponseType = {
		"Question": 1,
		"Result": 2
	}

	var Data = {};
	Data.GameId = null;
	Data.IDEntity = -1;
	Data.QuestionIndex = 0;
	Data.LastGivenAnswer = GivenAnswer.Undefined;
	Data.SelectedObjectName = "";
	Data.SelectedObjectDescription = "";
	Data.CanAnswerQuestion = false;
	Data.IsLastQuestion = false;

	Data.AnswerCameFromTopGuesses = false;
	Data.SelectedTopGuessRank = 1;

	Data.TimesPlayed = 0;
	Data.FirstPlayed = '';
	Data.LastPlayed = '';
	Data.Description = '';

	var Controls = {
		btnAnswerYes: null,
		btnAnswerNo: null,
		btnAnswerDontKnow: null,
		btnAnswerProbablyYes: null,
		btnAnswerProbablyNo: null
	};

	Controls.gridQuestionList = null;
	Controls.txtObjectName = null; // used for autocomplete - in case the system does not guess and the user wants to upload the object he thought of
	Controls.txtObjectDescription = null;
	Controls.txtObjectPicture = null;

	Controls.tmplResponse = null;
	Controls.tmplTopGuesses = null;

	this.initializeControls = function ()
	{
		/// <summary>Initializes the controls of the GamePage object.</summary>

		WebPage = new Mvc.WebPage();
		WebPage.deleteOldGames();

		Data.GameId = $("#idGame").val(); // will be taken later from a hidden field.
		Data.QuestionIndex = $("#current_questions_number").text(); // gets the current question index
		Data.CanAnswerQuestion = true; // lets the player answer questions.

		//getNextQuestion(GivenAnswer.Undefined);

		initializeAnswerButtons();

		Controls.gridQuestionList = $("#gridQuestionList");
		Controls.wrapperResponse = $("#wrapper-response");
		Controls.wrapperTopGuesses = $("#wrapper-top-guesses");

		Controls.tmplResponse = $("#tmplResponse");
		Controls.tmplTopGuesses = $("#tmplTopGuesses");

		if (Controls.gridQuestionList.length <= 0)
		{
			throw "gridQuestionList.notFoundException";
		}

		Controls.txtObjectName = $("#txtObjectName");
		Controls.txtObjectDescription = $("#txtObjectDescription");
		Controls.txtObjectPicture = $("#txtObjectPicture");

		if (Controls.txtObjectName.length > 0)
		{
			Controls.txtObjectName.preventSubmit();
			Controls.txtObjectName.autocomplete({
				source: function (request, response)
				{
					Controls.txtObjectName.data({ "ObjectId": -1 });
					WebPage.ajaxCall(
						"/Game/EntitiesNamedLike",
						{
							"needle": Controls.txtObjectName.val()
						},
						function (data)
						{
							Controls.txtObjectDescription.prop('readonly', false);
							response($.map(data, function (item)
							{
								return { ID: item.IDEntity, label: item.Name, TimesPlayed: item.TimesPlayed, FirstPlayed: item.FirstPlayedString, LastPlayed: item.LastPlayedString, Description: item.Description }
							}))
						});
				},
				minLength: 3,
				focus: function (event, ui)
				{
					if (ui && ui.item)
					{
						Controls.txtObjectDescription.val(ui.item.Description);
					}
				},
				select: function (event, ui)
				{
					if (ui && ui.item)
					{
						Controls.txtObjectDescription.prop('readonly', true);
						Controls.txtObjectName.data({ "ObjectId": ui.item.ID });

						Controls.txtObjectDescription.val(ui.item.Description);

						Data.TimesPlayed = ui.item.TimesPlayed;
						Data.FirstPlayed = ui.item.FirstPlayed;
						Data.LastPlayed = ui.item.LastPlayed;
						Data.Description = ui.item.Description;
					}
				},
				close: function ()
				{
					if (Controls.txtObjectName.data().ObjectId == -1)
					{
						//Controls.txtObjectName.val("");
					}
				}
			});
		}
	};

	var initializeAnswerButtons = function ()
	{
		/// <summary>Initializes the answer buttons of the GamePage object.</summary>
		/// <returns type="void"></returns>

		Controls.btnAnswerYes = $("#btnAnswerYes");
		if (Controls.btnAnswerYes.length > 0)
		{
			Controls.btnAnswerYes.click(function ()
			{
				answerQuestion(GivenAnswer.Yes);
			});
		}
		else
		{
			throw "btnAnswerYes.notFoundException";
		}

		Controls.btnAnswerNo = $("#btnAnswerNo");
		if (Controls.btnAnswerNo.length > 0)
		{
			Controls.btnAnswerNo.click(function ()
			{
				answerQuestion(GivenAnswer.No);
			});
		}
		else
		{
			throw "btnAnswerNo.notFoundException";
		}

		Controls.btnAnswerDontKnow = $("#btnAnswerDontKnow");
		if (Controls.btnAnswerDontKnow.length > 0)
		{
			Controls.btnAnswerDontKnow.click(function ()
			{
				answerQuestion(GivenAnswer.Unknown);
			});
		}
		else
		{
			throw "btnAnswerDontKnow.notFoundException";
		}

		Controls.btnAnswerProbablyYes = $("#btnAnswerProbablyYes");
		if (Controls.btnAnswerProbablyYes.length > 0)
		{
			Controls.btnAnswerProbablyYes.click(function ()
			{
				answerQuestion(GivenAnswer.ProbablyYes);
			});
		}
		else
		{
			throw "btnAnswerProbablyYes.notFoundException";
		}

		Controls.btnAnswerProbablyNo = $("#btnAnswerProbablyNo");
		if (Controls.btnAnswerProbablyNo.length > 0)
		{
			Controls.btnAnswerProbablyNo.click(function ()
			{
				answerQuestion(GivenAnswer.ProbablyNo);
			});
		}
		else
		{
			throw "btnAnswerProbablyNo.notFoundException";
		}
	};

	var getNextQuestion = function ()
	{
		/// <summary>Gets the next question of the current game.</summary>
		/// <returns type="json">The JSON object representing the next question of the game.</returns>

		WebPage.ajaxCall(
			'/Game/NextQuestion',
			{
				idGame: Data.GameId
			},
			function (data)
			{
				if (data)
				{
					Data.IsLastQuestion = data[2].IsLastQuestion;

					Controls.gridQuestionList.html(data[0]);
					Data.QuestionIndex = data[2].CurrentQuestionIndex;

					$(".current_question_content").text(data[1].QuestionBody);
					$("#current_questions_number").text(Data.QuestionIndex);

					Data.CanAnswerQuestion = true; // enables the possibility of answering questions.
				}
			},
			{
				"debugMode": false,
				"showAjaxRoller": true
			});
	};

	var answerQuestion = function (answer)
	{
		/// <summary>Answers the current question of the game.</summary>
		/// <param name="answer" type="int">The user given answer.</param>
		/// <returns type="bool">True if the question has been answered successfully, false if an error occurred</returns>

		if (Data.CanAnswerQuestion)
		{
			Data.LastGivenAnswer = answer;

			if (!Data.IsLastQuestion)
			{
				Data.CanAnswerQuestion = false; // disables the possibility of answering questions.

				WebPage.ajaxCall(
					'/Game/AnswerQuestion',
					{
						idGame: Data.GameId,
						questionIndex: Data.QuestionIndex,
						answer: answer
					},
					function (data)
					{
						if (data)
						{
							getNextQuestion();
						}
					},
					{
						"debugMode": false,
						"showAjaxRoller": true
					});
			}
			else
			{
				WebPage.ajaxCall(
					'/Game/GetGameResult',
					{
						idGame: Data.GameId,
						questionIndex: Data.QuestionIndex,
						answer: answer
					},
					onSuccessGetGameResult,
					{
						"debugMode": false,
						"showAjaxRoller": true
					});
			}
		}
	};

	var onSuccessGetGameResult = function (data)
	{
		if (data)
		{
			Controls.gridQuestionList.html(data[0]);

			var response = {};
			response.ObjectName = data[1].GuessedObject.Name;
			response.ObjectDescription = data[1].GuessedObject.Description;
			response.CertaintyPercentage = data[1].CertaintyPercentage.toFixed(2);

			Data.SelectedObjectName = data[1].GuessedObject.Name;
			Data.SelectedObjectDescription = data[1].GuessedObject.Description;
			Data.IDEntity = data[1].GuessedObject.IDEntity;

			Data.TimesPlayed = data[1].GuessedObject.TimesPlayed;
			Data.FirstPlayed = data[1].GuessedObject.FirstPlayedString;
			Data.LastPlayed = data[1].GuessedObject.LastPlayedString;

			Controls.tmplResponse
				.tmpl(response)
				.appendTo(Controls.wrapperResponse); // load the response

			Controls.wrapperResponse.removeClass("hide"); // show the response

			$("#btnCorrectAnswer").click(function ()
			{
				ajaxSetCorrectGuess(1);
			});

			$("#btnIncorrectAnswer").click(function ()
			{
				ajaxSetIncorrectGuess();
			});


			// hide unused controls
			$("#wrapper-possible-answers").hide(); // we hide the answer buttons -> game has finished, user can't send any answers
			$(".current_question_number").hide();
			$(".current_question_content").hide();

			var googleImageSearch = new GoogleImageSearch(data[1].GuessedObject.Name, function (imgUrl)
			{
				$("#wrapper-response-entity-picture").attr("src", imgUrl);
				$("#wrapper-response-entity-picture").show();

				$("#wrapper-response-entity-picture-progress-bar").hide();
			}).search();
		}
	};

	var ajaxSetCorrectGuess = function (attempt)
	{
		/// <summary>Marks the current game result as the correct guess.</summary>

		WebPage.ajaxCall(
			"/Game/SetCorrectGuess",
			{
				"idGame": Data.GameId,
				"idGuessedEntity": Data.IDEntity,
				"attempt": attempt
			},
			function (data)
			{
				$("#wrapper-response-entity-picture").hide();

				$("#feedback_buttons").hide(); // asnwer was correct - we hide the feedback buttons

				if (!Data.AnswerCameFromTopGuesses)
				{
					$("#feedback_accepted_answer_machine_message").removeClass("hide"); // asnwer was correct - we show the system message
				}
				else
				{
					var positionSuffix = '';

					if (Data.SelectedTopGuessRank % 10 == 1)
					{
						positionSuffix = Data.SelectedTopGuessRank + '-st';
					}
					else if (Data.SelectedTopGuessRank % 10 == 2)
					{
						positionSuffix = Data.SelectedTopGuessRank + '-nd';
					}
					else if (Data.SelectedTopGuessRank % 10 == 3)
					{
						positionSuffix = Data.SelectedTopGuessRank + '-rd';
					}
					else
					{
						positionSuffix = Data.SelectedTopGuessRank + '-th';
					}

					Controls.wrapperTopGuesses.hide();

					var selectTopGuessControl = $("#selected-top-guess");

					selectTopGuessControl.removeClass("hide"); // wrapper for the selected top guess
					selectTopGuessControl.find(".guessed_object").text(Data.SelectedObjectName);
					selectTopGuessControl.find(".guessedTop_desc").text(Data.SelectedObjectDescription);
					selectTopGuessControl.find(".selected_top_guess_rank").text(positionSuffix);
				}

				Controls.gridQuestionList.html(data);


				showEntityPlayedInfo();
				$(".play_again").removeClass("hide");
			},
			{
				"debugMode": false,
				"showAjaxRoller": true
			});
	};

	var ajaxSetIncorrectGuess = function ()
	{
		/// <summary>Marks the current game result as the incorrect guess.</summary>

		WebPage.ajaxCall(
			"/Game/SetIncorrectGuess",
			{
				"idGame": Data.GameId,
				"idGuessedEntity": Data.IDEntity,
				"attempt": 1
			},
			function (data)
			{
				if (data)
				{
					ajaxGetTopGuesses();
				}
			},
			{
				"debugMode": false,
				"showAjaxRoller": true
			});
	};

	var ajaxGetTopGuesses = function ()
	{
		WebPage.ajaxCall(
			"/Game/GetTopGuesses",
			{
				"idGame": Data.GameId
			},
			function (data)
			{
				if (data)
				{
					Controls.wrapperResponse.hide();

					//var k = 0;
					//for (k = 0; k < data.length; k++)
					//{
					//	var guess = {};
					//	guess.Index = (k + 1);
					//	guess.ObjectId = data[k].IDEntity;
					//	guess.Name = data[k].Name;
					//	guess.Dot = '. ';
					//	guess.Class = 'top_guesses_link';

					//	Controls.tmplTopGuesses.tmpl(guess).appendTo(Controls.wrapperTopGuesses);
					//}

					//var additionalGuess = {};
					//additionalGuess.ObjectId = -1;
					//additionalGuess.Name = "No, none of these.";
					//additionalGuess.Class = 'none_of_top_guesses_link';

					//Controls.tmplTopGuesses.tmpl(additionalGuess).appendTo(Controls.wrapperTopGuesses);

					Controls.wrapperTopGuesses.append(data);

					Controls.wrapperTopGuesses.removeClass("hide");

					$("#feedback_buttons").hide(); // we hide the feedback buttons
					$("#feedback-sorry-message").show(); // we show a sorry feedback message to the user

					Controls.wrapperTopGuesses.find(".top_guesses_link").click(function ()
					{
						Data.IDEntity = $(this).data("id");
						Data.SelectedObjectName = $(this).data("name");
						Data.SelectedObjectDescription = $(this).data("description");
						Data.SelectedTopGuessRank = $(this).data("index") + 1; // we add '1' because the first guess has the rank 1


						Data.TimesPlayed = $(this).data("timesplayed");
						Data.FirstPlayed = $(this).data("firstplayed");
						Data.LastPlayed = $(this).data("lastplayed");

						Data.AnswerCameFromTopGuesses = true;

						ajaxSetCorrectGuess(2);
					});

					Controls.wrapperTopGuesses.find(".none_of_top_guesses_link").click(function ()
					{
						Controls.wrapperTopGuesses.hide(); // we hide the top guesses list because the user has selected the right answer.

						// reset entity played info
						Data.TimesPlayed = null;
						Data.FirstPlayed = '';
						Data.LastPlayed = '';

						// we show a grid where the user can set the correct answer
						// the user will be provided with an autocomplete which will load object names from database
						// if none of the object names loaded from the database match the user`s thought object, he can submit his as well.
						$("#divUserInputAnswer").show();

						$("#btnSubmitAddNew").click(function ()
						{
							ajaxSubmitNewEntity();
						});
					});
				}
			},
			{
				"debugMode": false,
				"showAjaxRoller": true
			});
	};

	var ajaxSubmitNewEntity = function ()
	{
		var selectedObjectID = Controls.txtObjectName.data().ObjectId;
		if (!selectedObjectID || selectedObjectID <= 0)
		{
			selectedObjectID = -1;
		}

		WebPage.ajaxCall(
			"/Game/SubmitNewEntity",
			{
				"idGame": Data.GameId,
				"idEntity": selectedObjectID,
				"entityName": Controls.txtObjectName.val(),
				"entityDescription": Controls.txtObjectDescription.val()
			},
			function (data)
			{
				Data.SelectedObjectName = Controls.txtObjectName.val();
				Data.Description = Controls.txtObjectDescription.val();

				// hide the submit button
				// unbind the click event from the submit button so the user cannot use it again
				$("#btnSubmitAddNew").hide().unbind("click");

				// we hide the textboxes after we finished the insertion
				Controls.txtObjectName.hide();
				Controls.txtObjectDescription.hide();
				Controls.txtObjectPicture.hide();

				if (selectedObjectID > 0 || data[1]) {
					Controls.gridQuestionList.html(data[0]);

					Data.Description = data[1].Description;
					Data.TimesPlayed = data[1].TimesPlayed;
					Data.FirstPlayed = data[1].FirstPlayedString;
					Data.LastPlayed = data[1].LastPlayedString;
				}

				$("#divUserInputAnswer")
					.empty()
					.append(
						$('<span class="guessed_object"></span>')
							.text(Controls.txtObjectName.val())
					)
					.append(' (' + Data.Description + ')')
					.append(
						$("<p></p>")
							.text("Object successfully added.")
					);

				showEntityPlayedInfo();
				$(".play_again").removeClass("hide");
			},
			{
				"debugMode": false,
				"showAjaxRoller": true
			});
	};

	var showEntityPlayedInfo = function ()
	{
		if (Data.TimesPlayed && Data.TimesPlayed > 0)
		{
			var timesPlayedText = Data.TimesPlayed == 1 ? "time" : "times";
			text = $('<span><small>' + 'Played <strong>' + Data.TimesPlayed + '</strong> ' + timesPlayedText + ' since <strong>' + Data.FirstPlayed + '</strong>. ' + 'Last played on <strong>' + Data.LastPlayed + '</strong>' + '</small></span>');
		}
		else
		{
			text = $('<span><small>never played before</small></span>');
		}

		$("#entity_played_info")
			.removeClass("hide")
			.append(text);

		var googleImageSearch = new GoogleImageSearch(Data.SelectedObjectName, function (imgUrl)
		{
			$("#entity_played_info-picture").attr("src", imgUrl);
			$("#entity_played_info-picture").show();

			$("#entity_played_info-picture-progress-bar").hide();
		}).search();
	};

	var loadEntityPicture = function (entityName, callbackFunction)
	{
		// Our ImageSearch instance.
		var imageSearch = new google.search.ImageSearch();

		// Restrict to extra large images only
		imageSearch.setRestriction(
			google.search.ImageSearch.RESTRICT_IMAGESIZE,
			google.search.ImageSearch.IMAGESIZE_LARGE);

		var searchCtx = imageSearch;

		imageSearch.setSearchCompleteCallback(
		this,
		function ()
		{
			// Check that we got results
			if (searchCtx.results && searchCtx.results.length > 0)
			{
				callbackFunction(searchCtx.results[0].tbUrl);
			}
		},
		null);

		// Find me a beautiful car.
		imageSearch.execute(entityName);
	};
};