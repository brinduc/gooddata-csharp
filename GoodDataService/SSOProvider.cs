﻿using System;
using System.Web;

namespace GoodDataService
{
	public class SsoProvider
	{
		public GoodDataConfigurationSection Config { get; set; }

		public SsoProvider()
		{
			Config = GoodDataConfigurationSection.GetConfig();
		}

		public string GenerateToken(string email, int validaityOffsetInMinutes = 10)
		{
			var userData = CreateUserData(email);
			var gpg = new GnuPgpProcessor();
			var signedData = gpg.Sign(Config.Passphrase, userData);
			var encryptedData = gpg.Encrypt(Config.Recipient, signedData);
			return EncodeUserData(encryptedData);
		}


		private static string CreateUserData(string email, int validaityOffsetInMinutes = 10)
		{
			return "{\"email\":\"" + email + "\",\"validity\":" +
			       DateTime.UtcNow.AddMinutes(validaityOffsetInMinutes).ToUnixTime() + "}";
		}

		private static string EncodeUserData(string input)
		{
			return HttpUtility.UrlEncode(input);
		}
	}
}