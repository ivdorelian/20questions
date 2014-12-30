using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwentyQuestions.Utils
{
	public static class NumericUtils
	{
		public static int Max(params int[] list)
		{
			if (list == null || list.Length < 1)
			{
				return int.MinValue;
			}

			int m = list[0];
			for (int i = 1; i < list.Length; ++i)
			{
				if (list[i] > m)
				{
					m = list[i];
				}
			}

			return m;
		}
	}
}