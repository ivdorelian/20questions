using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TwentyQuestions.Models;

namespace TwentyQuestions.ViewModels.Home
{
	/// <summary>
	/// Represents the home view model.
	/// </summary>
	public class HomeViewModel : LayoutViewModel
	{
		/// <summary>
		/// Gets or sets the collection of all statistics.
		/// </summary>
		public IEnumerable<Statistic> StatisticCollection
		{
			get;
			set;
		}

		public TwentyQuestions.Models.Game[] RecentGames
		{
			get;
			set;
		}

		/// <summary>
		/// Initializes a new instance of the TwentyQuestions.ViewModels.Home.HomeViewModel class.
		/// </summary>
		public HomeViewModel()
		{
			
		}
	}
}