using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BotEngine.Domain;
using Replacer;

namespace BotEngine
{
    class Program
    {

   
        static void Main(string[] args)
        {
            //while (true)
            //{
            //    var input = Console.ReadLine();

            //    var matchs = _db.FindMatch(input).ToList();
            //    if (!matchs.Any())
            //    {
            //        Console.WriteLine("Not found responce");
            //        continue;
            //    }
            //    var rnd = new Random();
            //    var response = matchs[rnd.Next(0, matchs.Count - 1)];
            //    Console.WriteLine(response);
            //}


        }

        //static List<string> FindMatch(string input)
        //{
        //    var list = _db.Patterns.Where(f => f.Questions.Any(n => n.Variants.Value.ToLower().Contains(input.ToLower())))
        //        .SelectMany(f => f.Answers).Select(f => f.Variants.Value).ToList();
        //    if (!list.Any())
        //    {
        //        var words = SentanceToWords.Convert(input);
        //        foreach (var list2 in words.Select(word => _db.Patterns
        //            .Where(f => f.Questions.Any(n => n.Variants.Value.ToLower().Contains(word.ToLower())))
        //            .SelectMany(f => f.Answers).Select(f => f.Variants.Value).ToList()))
        //        {
        //            list.AddRange(list2);
        //        }
        //    }
        //    return list;
        //}
    }
}
