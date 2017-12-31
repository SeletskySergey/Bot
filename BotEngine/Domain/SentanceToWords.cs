using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BotEngine.Domain
{
    public class SentanceToWords
    {
        public static List<string> Convert(string sentance)
        {
            return sentance.ToLowerInvariant().Split(new char[]{',', '.', ' ', '\t', '\r', '?', '!'}, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        public static int CalcLevensteinDistance(string left, string right)
        {
            var matrix = new int[left.Length + 1, right.Length + 1];

            // Step 1
            if (left.Length == 0 || right.Length == 0)
            {
                return 0;
            }

            if (left.Length == right.Length)
            {
                if (left == right)
                {
                    return 100;
                }
            }

            // Step 2
            for (var i = 0; i <= left.Length; matrix[i, 0] = i++)
            {
            }

            for (var j = 0; j <= right.Length; matrix[0, j] = j++)
            {
            }

            // Step 3
            for (var i = 1; i <= left.Length; i++)
            {
                //Step 4
                for (var j = 1; j <= right.Length; j++)
                {
                    // Step 5
                    var cost = (right[j - 1] == left[i - 1]) ? 0 : 1;

                    // Step 6
                    matrix[i, j] = Math.Min(Math.Min(matrix[i - 1, j] + 1, matrix[i, j - 1] + 1), matrix[i - 1, j - 1] + cost);
                }
            }
            // Step 7
            var distance = matrix[left.Length, right.Length];
            double longestStringSize = Math.Max(left.Length, right.Length);
            return (int)((distance / longestStringSize) * 100.0);
        }
    }
}
