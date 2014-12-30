var GoogleImageSearch = function (seachKeyword, callbackFunction)
{
	var internalSearchKeyword = seachKeyword;
	var internalCallbackFunction = callbackFunction;

	this.search = function ()
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
				internalCallbackFunction(searchCtx.results[0].tbUrl);
			}
		},
		null);

		// Find me a beautiful car.
		imageSearch.execute(internalSearchKeyword);
	}
};