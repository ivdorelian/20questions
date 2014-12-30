using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TwentyQuestions.Models;

namespace TwentyQuestions.ViewModels.Game
{
	public class SubmitNewEntityViewModel : LayoutViewModel
	{
		public string AccessID { get; set; }
		public Entity SubmittedEntity { get; set; }
	}
}