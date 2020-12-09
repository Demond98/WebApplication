using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;

namespace WebApplication1
{
	public class Encoder
	{
		/*static void Main(string[] args)
		{
			//var textFromUser = GetTextFromUser();
			var textFromUser = ReadFile("encryptedText");
			//var keyFromUser = GetKeyFromUser();
			var keyFromUser = "скорпион";
			var adjustedKey = AdjustKeyToInputText(textFromUser.ToArray(), keyFromUser.ToArray());
			var vigenereTable = MakeVigenereTable();
			var decryptedText = DecryptText(textFromUser, new string(adjustedKey), vigenereTable);
			WriteToFile(decryptedText, "decryptedText");
		}*/

		public static char[] AdjustKeyToInputText(char[] textFromUser, char[] keyFromUser)
		{
			//string key = "скорпион";

			var onlyRussianLettersArray = textFromUser.Where(ch => IsRussianLetter(ch)).ToArray();
			var keyAdjustedToInputString = new char[onlyRussianLettersArray.Length];
			var keyIndexer = 0;

			for (var i = 0; i < onlyRussianLettersArray.Length; i++)
			{
				if (keyIndexer > keyFromUser.Length - 1)
				{
					keyIndexer = 0;
				}

				keyAdjustedToInputString[i] = keyFromUser[keyIndexer];
				keyIndexer++;
			}

			return keyAdjustedToInputString;
		}

		public static char[][] MakeVigenereTable()
		{
			var alphabet = Enumerable.Range('а', 'я' - 'а' + 1).Select(a => (char)a).ToArray();
				//new char[] { 'а', 'б', 'в', 'г', 'д', 'е', 'ё', 'ж', 'з', 'и', 'й', 'к', 'л', 'м', 'н', 'о',
				//			 'п', 'р', 'с', 'т', 'у', 'ф', 'х', 'ц', 'ч', 'ш', 'щ', 'ъ', 'ы', 'ь', 'э', 'ю', 'я' };

			var vigenereTable = new char[alphabet.Length][];
			vigenereTable[0] = alphabet;

			for (int i = 1; i < alphabet.Length; i++)
			{
				var previousRow = vigenereTable[i - 1];
				var firstLetterFromPreviousRow = vigenereTable[i - 1][0];
				var currentRow = new char[alphabet.Length];
				Array.Copy(previousRow, 1, currentRow, 0, previousRow.Length - 1);
				currentRow[currentRow.Length - 1] = firstLetterFromPreviousRow;
				vigenereTable[i] = currentRow;
			}

			return vigenereTable;
		}

		public static string EncryptText(string textFromUser, string adjustedKey, char[][] vigenereTable)
		{
			var encryptedText = new char[textFromUser.Length];
			var alphabet = vigenereTable[0]; //только первая строка таблицы где весь алфавит
			int keyIndexer = 0;

			for (int i = 0; i < textFromUser.Length; i++)
			{
				if (!char.IsLetter(textFromUser[i]) || !IsRussianLetter(textFromUser[i]))
				{
					encryptedText[i] = textFromUser[i];
				}
				else
				{
					var rowIndex = Array.IndexOf(alphabet, char.ToLower(adjustedKey[keyIndexer]));
					var columnIndex = Array.IndexOf(alphabet, char.ToLower(textFromUser[i]));
					encryptedText[i] = vigenereTable[rowIndex][columnIndex];
					keyIndexer++;
				}
			}

			return new string(encryptedText);
		}

		public static string DecryptText(string textFromUser, string adjustedKey, char[][] vigenereTable)
		{
			var decryptedText = new char[textFromUser.Length];
			var alphabet = vigenereTable[0];
			var keyIndexer = 0;

			for (int i = 0; i < textFromUser.Length; i++)
			{
				if (!char.IsLetter(textFromUser[i]) || !IsRussianLetter(textFromUser[i]))
				{
					decryptedText[i] = textFromUser[i];
				}
				else
				{
					int rowIndexer = Array.IndexOf(alphabet, char.ToLower(adjustedKey[keyIndexer]));
					var rowWhereEncryptedLetterExist = vigenereTable[rowIndexer]; // строка, соответсвтующая букве ключа, в которой нужно найти по зашифрованной букве столбец, в котором она(зашифрованная буква) распологается и заголовок этого столбца будет являться исходной буквой
					int columnIndexer = Array.IndexOf(rowWhereEncryptedLetterExist, char.ToLower(textFromUser[i]));
					decryptedText[i] = alphabet[columnIndexer];
					keyIndexer++;
				}
			}

			return new string(decryptedText);
		}

		public static string ReadFile(string fileName)
		{
			//string path = @"C:\Users\Михаил\Education\Course2\Lection8\l8t4\" + fileName + ".txt";
			string path = fileName + ".txt";
			try
			{
				return File.ReadAllText(path, System.Text.Encoding.Default);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return null;
			}
		}

		public static void WriteToFile(string textToWrite, string fileName)
		{
			//string path = @"C:\Users\Михаил\Education\Course2\Lection8\l8t4\" + fileName + ".txt";
			string path = fileName + ".txt";

			try
			{
				File.WriteAllText(path, textToWrite, System.Text.Encoding.Default);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

		public static bool IsRussianLetter(char c)
			=> (c >= 'А' && c <= 'я') || c == 'ё' || c == 'Ё';
	}
}