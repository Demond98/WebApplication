using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication1.Pages
{
	[IgnoreAntiforgeryToken]
	public class IndexModel : PageModel
	{
		public string Text { get; set; }
		public string Key { get; set; }
		public string Result { get; set; }

		public void OnPost(string text, string key, string action)
		{
			Text = text;
			Key = key;

			var adjustedKey = Encoder.AdjustKeyToInputText(text.ToArray(), key.ToArray());
			var vigenereTable = Encoder.MakeVigenereTable();

			switch (action)
			{
				case "encode":					
					Result = Encoder.DecryptText(text, new string(adjustedKey), vigenereTable);
					break;
				case "decode":
					Result = Encoder.EncryptText(text, new string(adjustedKey), vigenereTable);
					break;
			}			
		}
	}
}