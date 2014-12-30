using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace TwentyQuestions.Utils
{
	class RandomString
	{
		public string GetRandomString(int length)
		{
			string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

			StringBuilder s = new StringBuilder();

			using (RNGCryptoServiceProvider rand = new RNGCryptoServiceProvider())
			{
				byte[] bytes = new byte[length];
				rand.GetBytes(bytes);
				for (int i = 0; i < length; i++)
				{
					s.Append(chars[bytes[i] % chars.Length]);
				}
			}

			return s.ToString();
		}
	}
}