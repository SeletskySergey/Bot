using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BotEngine.Domain;
using Newtonsoft.Json;

namespace Bot
{
    public class Pattern
    {
        [JsonIgnore]
        public int Index = 0;
        public List<string> Questions = new List<string>();
        public List<string> Answers = new List<string>();
    }

    public class Program
    {
        public static void Main()
        {
            var json = File.ReadAllText("base.json");
            var data = JsonConvert.DeserializeObject<List<Pattern>>(json);

            var history = new Queue<string>();
            while (true)
            {
                if (history.Count > 10)
                {
                    history.Dequeue();
                }
                var input = Console.ReadLine();
                var matchs = FindMatch(data, input).ToList();
                var rnd = new Random();
                matchs.RemoveAll(history.Contains);
                if (matchs.Count == 0)
                {
                    matchs.Add("do not know");
                }
                var response = matchs[rnd.Next(0, matchs.Count)];
                history.Enqueue(response);
                Console.WriteLine(response);
            }
        }

        static List<string> FindMatch(List<Pattern> db, string input)
        {
            var words = SentanceToWords.Convert(input);

            var patterns = new List<Pattern>();
            foreach (var word in words)
            {
                patterns.AddRange(db.Where(f => f.Questions.Any(n => n.ToLower().Contains(word))));
            }

            foreach (var p in patterns)
            {
                p.Index = 0;
                foreach (var q in p.Questions)
                {
                    p.Index += SentanceToWords.CalcLevensteinDistance(q.ToLower(), input.ToLower());
                }
            }

            patterns = patterns.Where(f => f.Index > 50).OrderByDescending(f => f.Index).Take(3).ToList();

            if (patterns.Count == 0)
            {
                return new List<string>{"не знаю что сказать"};
            }
            return patterns.SelectMany(f => f.Answers).ToList();
        }
    }
}
