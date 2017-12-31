using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bot.Script.Kernel
{

    public class WordOfstring
    {


        public List<string> Words = new List<string>();
        public List<int> KeyWords = new List<int>();
        public int KeyWordsCount = 0;
        public int Emotions = 0;
        public bool ReqQuestion = false;

        public WordOfstring(string word)
        {
            var builder = new StringBuilder();
            foreach (var symbol in word)
            {
                if ((((((symbol > 64 && symbol < 123) || (symbol > 46 && symbol < 59)) || (symbol >= 1025 && symbol <= 1105)) || symbol == 124) || symbol == 16) || symbol == 961)
                {
                    builder.Append(symbol);
                }
                else
                {
                    if (builder.Length > 0)
                    {
                        Words.Add(builder.ToString());
                        KeyWords.Add(0);
                        builder.Clear();
                    }
                }
            }
            WordFilters();

        }

        private void WordFilters()
        {
            int index = 0;
            while (index < Words.Count)
            {
                var word = Words[index];
                if (index >= 0 && word.Contains("[key:"))
                {
                    KeyWords[index] = 1;
                    KeyWordsCount++;
                    word = word.Substring(5, (word.Length - 5));
                    if (word.Substring(word.Length - 1, 1) == "]")
                    {
                        word = word.Substring(0, word.Length - 1);
                    }
                }
                if (index >= 0 && word.Contains("[req:question]"))
                {
                    ReqQuestion = true;
                    word = "";
                }
                if (index >= 0 && word.Contains(":"))
                {
                    word = word.Substring(0, word.Length - 1);
                }
                if (index >= 0 && word == "")
                {
                    Words.RemoveRange(index, 1);
                    index--;
                }
                if (index >= 0 && word == "]")
                {
                    Words.RemoveRange(index, 1);
                    index--;
                }
                index++;
            }
        }

        
    }
}
