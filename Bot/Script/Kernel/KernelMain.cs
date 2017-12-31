using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;
using Bot.Script.Base;

namespace Bot.Script.Kernel {

    public class KernelMain {

        public bool StopDialog;
        private bool greeting;
        private const bool ShareRequest = false;
        public DataBase Base;
        public string LocType;
        public string Question;
        private string answer;
        private string questionPattern;
        public string UserName;
        private string context;
        private List<int> patternIndex;
        private List<int> patternIndexKey;
        private List<int> patternIndexTemp;
        private List<int> patternIndexSource;
        private List<int> patternIndexCount;
        private readonly List<List<WordOfstring>> patternWordIndex;
        private readonly List<WordOfstring> patternContIndex;
        private int patternIndexMax;
        private int suspenseCount;
        private int repeatCount;
        public int ReaskCycle;
        private int mood;
        public int Emotion;
        private int interestingIndex;
        private int jokeIndex;
        private int sexFemaleIndex;

        public KernelMain(){
            Base = new DataBase();
            LocType = "";
            Question = "";
            answer = "";
            questionPattern = "";
            UserName = "";
            context = "";
            patternIndex = new List<int>();
            patternIndexKey = new List<int>();
            patternIndexTemp = new List<int>();
            patternIndexSource = new List<int>();
            patternIndexCount = new List<int>();
            patternWordIndex = new List<List<WordOfstring>>();
            patternContIndex = new List<WordOfstring>();
            patternIndexMax = new int();
        }

        private void PatternSpecialCommand(){
            if (questionPattern.Contains("[set:username]"))
            {
                PatternSpecialCommand_FindName();
            }
        }
        private void PatternSpecialCommand_FindName()
        {
            bool local1 = false;
            var local5 = new WordOfstring(questionPattern);
            var local6 = new WordOfstring(Question);

            int local2 = -1;
            do  {
                local2++;
            } while (((local5.Words[local2] != "[set:username]") && ((local2 < (local5.Words.Count - 1)))));
            if (local5.Words[local2] != "[set:username]"){
                local2 = -1;
            }
            if (local2 != -1){
                if (local5.Words.Count == local6.Words.Count){
                    bool local7 = true;
                    int local3 = 0;
                    while (local3 < local5.Words.Count) {
                        if (local6.Words[local2].ToUpper() == local5.Words[local3].ToUpper()){
                            local7 = false;
                        }
                        local3++;
                    }
                    if (local7){
                        UserName = local6.Words[local2];
                        local1 = true;
                    } else {
                        local3 = 0;
                        while (local3 < local6.Words.Count) {
                            bool local8 = true;
                            int local4 = 0;
                            while (local4 < local5.Words.Count) {
                                if (local6.Words[local3].ToUpper() == local5.Words[local4].ToUpper()){
                                    local8 = false;
                                }
                                local4++;
                            }
                            if (local8){
                                UserName = local6.Words[local3];
                                local1 = true;
                            }
                            local3++;
                        }
                    }
                }
            }
            if (!local1){
                answer = "Извини, твое имя не ясно.";
            }
        }
        private void AnswerSpecialCommand()
        {
            if (answer.Contains("[username]"))
            {
                answer = answer.Replace("[username]", UserName);
            }

            if (answer.Contains("[botname]"))
            {
                answer = answer.Replace("[botname]", Base.BotName);
            }

            if (answer.Contains("[time]"))
            {
                answer = answer.Replace("[time]", new DateTime().ToShortTimeString());
            }

            if (answer.Contains("[botage]"))
            {
                answer = answer.Replace("[botage]", Base.BotAge.ToString(CultureInfo.InvariantCulture));
            }

            if (answer.Contains("[ask]"))
            {
                answer = answer.Replace("[ask]", AskQuestion());
            }

            if (answer.Contains("[weekday]"))
            {
                answer = answer.Replace("[weekday]", GetWeekDay());
            }

            if (answer.Contains("[stop]"))
            {
                answer = answer.Replace("[stop]", ExtenstionsUniversal("[stop]"));
                StopDialog = true;
            }

            if (answer.Contains("[tell:joke]"))
            {
                answer = answer.Replace("[tell:joke]", ExtenstionsJoke("joke"));
            }

            if (answer.Contains("[tell:interesting]"))
            {
                answer = answer.Replace("[tell:interesting]", ExtenstionsInteresting("interesting"));
            }

            if (answer.Contains("[tell:sex_female]"))
            {
                answer = answer.Replace("[tell:sex_female]", ExtenstionsSexFemale("sex_female"));
            }

            if (answer.Contains("[greeting]"))
            {
                answer = answer.Replace("[greeting]", "");
                if (!greeting)
                {
                    greeting = true;
                }
                else
                {
                    answer = ExtenstionsUniversal("greeting_repeat");
                }
            }

            if (answer.Contains("[reask]"))
            {
                if (ReaskCycle < 1)
                {
                    Question = (Question + " [x?]");
                    ReaskCycle++;
                    answer = GetAnswer();
                }
                else
                {
                    answer = ExtenstionsUniversal("suspense");
                }
            }
        }
        private string GetWeekDay()
        {
            int local1 = new DateTime().Day;
            string local2 = ("weekday_" + local1);
            return (ExtenstionsUniversal(local2));
        }
        private string AskQuestion()
        {
            var local1 = new List<string>();
            
            int local5 = 0;
            while (local5 < Base.PatternBase.Count) {
                if ((((Base.PatternBase[local5].Questions[0][(Base.PatternBase[local5].Questions[0].Length - 1)] == '?')) && ((Base.PatternBase[local5].IsNew == false)))){
                    bool local3 = true;
                    bool local4 = false;
                    if (char.ToUpper(Base.PatternBase[local5].Questions[0][0]) != Base.PatternBase[local5].Questions[0][0]){
                        local3 = false;
                    }
                    int local6 = 0;
                    while (local6 < Base.PatternBase[local5].Questions[0].Length) {
                        if (Base.PatternBase[local5].Questions[0][local6] == '.'){
                            local3 = false;
                        }
                        if (Base.PatternBase[local5].Questions[0][local6] == ','){
                            local3 = false;
                        }
                        if (Base.PatternBase[local5].Questions[0][local6] == '!'){
                            local3 = false;
                        }
                        if (Base.PatternBase[local5].Questions[0][local6] == ':'){
                            local3 = false;
                        }
                        if (Base.PatternBase[local5].Questions[0][local6] == '*'){
                            local3 = false;
                        }
                        int local7 = Base.PatternBase[local5].Questions[0][local6];
                        if ((((((LocType == "en")) && ((local7 >= 1025)))) && ((local7 <= 1105)))){
                            local3 = false;
                        }
                        if ((((local7 >= 1025)) && ((local7 <= 1105)))){
                            local4 = true;
                        }
                        local6++;
                    }
                    if ((((LocType == "ru")) && (!(local4)))){
                        local3 = false;
                    }
                    if (local3){
                        var local2 = new WordOfstring(Base.PatternBase[local5].Questions[0]);
                        if (local2.Words.Count > 4){
                            local1.Add(Base.PatternBase[local5].Questions[0]);
                        }
                    }
                }
                local5++;
            }
            var local8 = (int)Math.Floor((((double)new Random().Next(0, 100) / 100) * local1.Count));
            return (local1[local8]);
        }
        private string ExtenstionsSuspenseX2()
        {
            var local4 = FillExtension("suspense_x2");
            if (local4.Count == 0){
                return ("?");
            }
            int local5 = (int)Math.Floor(((double)new Random().Next(0, 100) / 100) * local4.Count);
            return (((local4[local5] + " ") + AskQuestion()));
        }
        private bool CheckHumanRepeat(string _arg1)
        {
            return false;
        }
        private bool CheckHumanRepeatBot(string _arg1)
        {
            return false;
        }
        private bool CheckBotRepeat(string _arg1)
        {
            return false;
        }
        private bool CheckSuspense()
        {
            var local1 = new WordOfstring(questionPattern);
            var local2 = new WordOfstring(Question);
            bool local3 = false;
            int local4 = 0;
            while (local4 < local2.Words.Count) {
                int local5 = 0;
                while (local5 < local1.Words.Count) {
                    if (local2.Words[local4].ToUpper() == local1.Words[local5].ToUpper()){
                        local3 = true;
                    }
                    local5++;
                }
                local4++;
            }
            return (!(local3));
        }

        private bool CheckSymbolRepeat(string s)
        {
            if (s.Length < 3) return true;
            return s.Where((c, i) => i >= 2 && s[i - 1] == c && s[i - 2] == c).Any();
        }

        private List<string> FillExtension(string name)
        {
            var list = new List<string>();
            foreach (XmlNode node in Base.Extensions.FirstChild.ChildNodes)
            {
                if (node.Name == name)
                {
                    foreach (XmlNode childNode in node.ChildNodes)
                    {
                        if (LocType == childNode.Name)
                        {
                            list.Add(childNode.FirstChild.Value);
                        }
                    }
                }
            }
            return list;
        }

        private string ExtenstionsAskQuestion()
        {
            int local5;
            int local6;
            var local4 = FillExtension("ask_question");
            if (local4.Count == 0){
                return "?";
            }
            local5 = (int)Math.Floor((((double)new Random().Next(0, 100) / 100) * local4.Count));
            local6 = (int)Math.Floor((((double)new Random().Next(0, 100) / 100) * 3));
            if (local6 == 1){
                return (AskQuestion());
            }
            return (((local4[local5] + " ") + AskQuestion()));
        }
        private string ExtenstionsUniversal(string name)
        {
            var list = FillExtension(name);
            if (list.Count == 0)
            {
                return "?";
            }
            var index = (int)Math.Floor((((double)new Random().Next(0, 100) / 100) * list.Count));
            return (list[index]);
        }
        private string ExtenstionsInteresting(string _arg1)
        {
            int local5;
            var local4 = FillExtension(_arg1);
            if (local4.Count == 0){
                return ("?");
            }
            local5 = interestingIndex;
            interestingIndex++;
            if (interestingIndex > (local4.Count - 1)){
                interestingIndex = 0;
            }
            return (local4[local5]);
        }
        private string ExtenstionsJoke(string _arg1)
        {
            int local5;
            var local4 = FillExtension(_arg1);
            if (local4.Count == 0){
                return ("?");
            }
            local5 = jokeIndex;
            jokeIndex++;
            if (jokeIndex > (local4.Count - 1)){
                jokeIndex = 0;
            }
            return (local4[local5]);
        }
        private string ExtenstionsSexFemale(string _arg1)
        {
            int local5;
            var local4 = FillExtension(_arg1);
            if (local4.Count == 0){
                return ("?");
            }
            local5 = sexFemaleIndex;
            sexFemaleIndex++;
            if (sexFemaleIndex > (local4.Count - 1)){
                sexFemaleIndex = 0;
            }
            return (local4[local5]);
        }
        public string GetAnswer()
        {
            int local2;
            int startIndex;
            int local30;
            int local8 = 0;
            var local9 = new List<List<char>>();
            var local10 = new List<int>();
            var question = new WordOfstring(Question);
            var local13 = new WordOfstring(context);
            bool local17 = false;
            int local1 = patternWordIndex.Count;
            while (local1 < Base.PatternBase.Count) {
                patternWordIndex.Add(new List<WordOfstring>());
                local2 = 0;
                while (local2 < Base.PatternBase[local1].Questions.Count) {
                    patternWordIndex[local1].Add(new WordOfstring(""));
                    local2++;
                }
                patternContIndex.Add(new WordOfstring(""));
                local1++;
            }
            var local22 = Base.Index.FirstChild.ChildNodes[0];

            patternIndex = new List<int>();
            patternIndexKey = new List<int>();
            patternIndexTemp = new List<int>();
            patternIndexSource = new List<int>();
            patternIndexCount = new List<int>();
            patternIndexMax = 0;

            var arr = Base.Index.FirstChild.ChildNodes[1].ChildNodes.Cast<XmlNode>().ToList();

            foreach (var word in question.Words)
            {
                string local31 = char.ToUpper(word[0]).ToString();
                foreach (XmlNode cn in local22.ChildNodes)
                {
                    string local32 = cn.ChildNodes[1].FirstChild.Value;
                    if (local32 == local31)
                    {
                        startIndex = Convert.ToInt32(cn.ChildNodes[2].FirstChild.Value);
                        int endIndex = Convert.ToInt32(cn.ChildNodes[3].FirstChild.Value);

                        foreach (var item in arr.Skip(startIndex).Take(endIndex - startIndex))
                        {
                            string local35 = item.ChildNodes[0].FirstChild.Value;
                            if (local35 == word.ToUpper())
                            {
                                foreach (XmlNode local4 in item.ChildNodes)
                                {
                                    if (local4.Name == "index")
                                    {
                                        int local36 = Convert.ToInt32(local4.FirstChild.Value);
                                        if (patternIndex.IndexOf(local36) == -1)
                                        {
                                            patternIndex.Add(local36);
                                            patternIndexCount.Add(1);
                                            if (1 > patternIndexMax)
                                            {
                                                patternIndexMax = 1;
                                            }
                                            foreach (XmlAttribute local377 in local4.Attributes)
                                            {
                                                if (local377.Name == "key")
                                                {
                                                    patternIndexKey.Add(Convert.ToInt32(local377.Value));
                                                }
                                            }
                                        }
                                        else
                                        {
                                            int local46 = patternIndex.IndexOf(local36);
                                            patternIndexCount[local46] = (patternIndexCount[local46] + 1);
                                            foreach (XmlAttribute local378 in local4.Attributes)
                                            {
                                                if (local378.Name == "key")
                                                {
                                                    patternIndexKey[patternIndex.IndexOf(local36)] =
                                                        Convert.ToInt32(local4.Attributes[local378.Name].Value);
                                                }
                                            }
                                            if (patternIndexCount[patternIndex.IndexOf(local36)] > patternIndexMax)
                                            {
                                                patternIndexMax = patternIndexCount[patternIndex.IndexOf(local36)];
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            patternIndexSource = patternIndex;
            int local24 = 0;
            while (local24 < patternIndex.Count)
            {
                if ((((patternIndexCount[local24] == patternIndexMax)) || ((patternIndexKey[local24] == 1)))){
                    patternIndexTemp.Add(patternIndex[local24]);
                }
                local24++;
            }
            patternIndex = patternIndexTemp;
            if (Base.NewPatternsStart != -1){
                local1 = Base.NewPatternsStart;
                while (local1 < Base.PatternBase.Count) {
                    patternIndex.Add(local1);
                    local1++;
                }
            }
            patternIndexTemp = new List<int>();
            local24 = 0;
            while (local24 < patternIndex.Count)
            {
                if (patternIndex[local24] < Base.PatternBase.Count){
                    patternIndexTemp.Add(patternIndex[local24]);
                }
                local24++;
            }
            patternIndex = patternIndexTemp;
            if ((((patternIndex.Count == 0)) && ((patternIndexMax > 1))))
            {
                patternIndex = patternIndexSource;
                if (Base.NewPatternsStart != -1){
                    local1 = Base.NewPatternsStart;
                    while (local1 < Base.PatternBase.Count) {
                        patternIndex.Add(local1);
                        local1++;
                    }
                }
                patternIndexTemp = new List<int>();
                local24 = 0;
                while (local24 < patternIndex.Count)
                {
                    if (patternIndex[local24] < Base.PatternBase.Count){
                        patternIndexTemp.Add(patternIndex[local24]);
                    }
                    local24++;
                }
                patternIndex = patternIndexTemp;
            }
            if (question.Words.Count > 0){
                local9 = new List<List<char>>();
                local24 = 0;
                while (local24 < patternIndex.Count)
                {
                    local1 = patternIndex[local24];
                    local10 = new List<int>();
                    bool local18 = false;
                    local2 = 0;
                    int local5;
                    int local14;
                    int local15;
                    while (local2 < Base.PatternBase[local1].Questions.Count) {
                        local5 = 0;
                        local14 = 0;
                        int local16 = 0;
                        if (patternWordIndex[local1][local2].Words.Count == 0){
                            patternWordIndex[local1][local2] = new WordOfstring(Base.PatternBase[local1].Questions[local2]);
                        }
                        var local12 = new WordOfstring("")
                        {
                            Emotions = patternWordIndex[local1][local2].Emotions,
                            KeyWordsCount = patternWordIndex[local1][local2].KeyWordsCount,
                            ReqQuestion = patternWordIndex[local1][local2].ReqQuestion,
                            KeyWords = patternWordIndex[local1][local2].KeyWords,
                            Words = patternWordIndex[local1][local2].Words
                        };
                        startIndex = 0;
                        while (startIndex < local12.Words.Count) {
                            local12.Words[startIndex] = patternWordIndex[local1][local2].Words[startIndex];
                            startIndex++;
                        }
                        startIndex = 0;
                        while (startIndex < local12.KeyWords.Count)
                        {
                            local12.KeyWords[startIndex] = patternWordIndex[local1][local2].KeyWords[startIndex];
                            startIndex++;
                        }
                        startIndex = 0;
                        while (startIndex < question.Words.Count) {
                            local15 = 0;
                            foreach (var word in local12.Words)
                            {
                                if (CompareWords(word, question.Words[startIndex]) > 70)
                                {
                                    if (local15 == 0)
                                    {
                                        local14++;
                                    }
                                    local15++;
                                    local18 = true;
                                    if (CompareWords(word, question.Words[startIndex]) >= 90)
                                    {
                                        if (local12.KeyWords[local12.Words.IndexOf(word)] == 1)
                                        {
                                            if (local15 == 1)
                                            {
                                                local16++;
                                            }
                                        }
                                        else
                                        {
                                            local5 = (local5 + (100 / (local15 * local15)));
                                        }
                                    }
                                    else
                                    {
                                        local5 = (local5 + (CompareWords(word, question.Words[startIndex]) / (local15 * local15)));
                                    }
                                }
                            }
                            startIndex++;
                        }
                        if ((((Base.PatternBase[local1].IsModerate == true)) && ((local14 > 0)))){
                            local5 = (local5 + 50);
                        }
                        bool local39 = false;
                        bool local40 = false;
                        if (Base.PatternBase[local1].Questions[local2].Contains("?"))
                        {
                            local39 = true;
                        }
                        if (Question.Contains("?"))
                        {
                            local40 = true;
                        }
                        if ((((local39 == true)) && ((local40 == true)))){
                            local5 = (local5 + 5);
                        }
                        if (local39 != local40){
                            local5 = (local5 - 25);
                        }
                        if (((local12.ReqQuestion) && (!(local40)))){
                            local5 = (local5 - 500);
                        }
                        if (local14 > 0){
                            if ((((((local12.Words.Count > local14)) && ((question.Words.Count > local14)))) && (((local14 / local12.Words.Count) >= 0.5)))){
                                foreach (var word in local12.Words)
                                {
                                    if (word == "[set:username]")
                                    {
                                        local5 = (local5 + 80);
                                        local14++;
                                    }
                                    if ((((((word == "[word:any]")) && ((local12.Words.Count > local14)))) && ((question.Words.Count > local14))))
                                    {
                                        local5 = (local5 + 10);
                                        local14++;
                                    }
                                }
                            }
                        }
                        int local21;
                        if (local12.KeyWordsCount > 0){
                            if (local16 == local12.KeyWordsCount){
                                local21 = (local16 * 300);
                            } else {
                                local21 = (-100 * local12.KeyWordsCount);
                            }
                            if (local14 > 0){
                            }
                        } else {
                            local21 = 0;
                        }
                        int local19 = (-10 * Math.Abs((local12.Words.Count - local14)));
                        if (local12.Words.Count == local14){
                            local19 = 10;
                        }
                        int local20 = 0;
                        if ((((local12.Words.Count == question.Words.Count)) && ((local14 > 0)))){
                            local20 = 25;
                        }
                        int local41 = 0;
                        int local42 = 0;
                        startIndex = 0;
                        if (Base.PatternBase[local1].Questions[local2].Contains("+"))
                        {
                            local41++;
                        }
                        startIndex = 0;
                        while (startIndex < Question.Length) {
                            if (Question.Contains("+")){
                                local42++;
                            }
                            startIndex++;
                        }
                        if (local41 > 0){
                            if (local41 == local42){
                                local5 = (local5 + 20);
                            } else {
                                local5 = (local5 - 20);
                            }
                        }
                        local5 = (((local5 + local20) + local19) + local21);
                        local10.Add(local5);
                        local2++;
                    }
                    local9.Add(local10.Select(f => (char)f).ToList());
                    if (local18){
                        if (patternContIndex[local1].Words.Count == 0){
                            patternContIndex[local1] = new WordOfstring(Base.PatternBase[local1].Context);
                        }
                        WordOfstring local43 = patternContIndex[local1];
                        local5 = 0;
                        local14 = 0;
                        startIndex = 0;
                        while (startIndex < local43.Words.Count) {
                            local15 = 0;
                            foreach (var word in local13.Words)
                            {
                                if (local43.Words[startIndex].ToUpper() == word.ToUpper())
                                {
                                    local14++;
                                    local15++;
                                    local5 = (local5 + (25 / ((local15 * (local13.Words.IndexOf(word) + 1)) * (startIndex + 1))));
                                }
                            }
                            startIndex++;
                        }
                        if (local14 > 0){
                        }
                        local2 = 0;
                        while (local2 < local9[local24].Count)
                        {
                            if (local9[local24][local2] <= 75){
                                local5 = (char)Math.Floor(((decimal)local5 / 10));
                            }

                            local9[local24][local2] = (char)(local9[local24][local2] + local5);
                            local2++;
                        }
                    }
                    local5 = 0;
                    if ((((Base.PatternBase[local1].Mood <= -5)) && ((mood < -10)))){
                        local5 = (local5 + 100);
                    }
                    if ((((Base.PatternBase[local1].Mood <= -5)) && ((mood < -15)))){
                        local5 = (local5 + 300);
                    }
                    local2 = 0;
                    while (local2 < local9[local24].Count)
                    {
                        local9[local24][local2] = (char)(local9[local24][local2] + local5);
                        local2++;
                    }
                    local2 = 0;
                    while (local2 < local9[local24].Count)
                    {
                        if (local9[local24][local2] > 0){
                        }
                        local2++;
                    }
                    local24++;
                }
                local8 = -10000;
                local1 = 0;
                while (local1 < local9.Count)
                {
                    local2 = 0;
                    while (local2 < local9[local1].Count) {
                        if (local9[local1][local2] > local8){
                            local8 = local9[local1][local2];
                        }
                        local2++;
                    }
                    local1++;
                }
                var local38 = new List<int[]>();
                local1 = 0;
                while (local1 < local9.Count)
                {
                    local2 = 0;
                    while (local2 < local9[local1].Count) {
                        if (local9[local1][local2] == local8){

                            local38.Add(new int[]{local1, local2});
                        }
                        local2++;
                    }
                    local1++;
                }
                int local6;
                int local7;
                if (local9.Count > 0)
                {
                    local30 = (int)Math.Floor((((double)new Random().Next(0, 100) / 100) * local38.Count));
                    local6 = patternIndex[local38[local30][0]];
                    local7 = local38[local30][1];
                } else {
                    local6 = -1;
                    local7 = -1;
                }
                if (local6 != -1 && !(local7 == -1)){
                    int local44 = (int)Math.Floor((((double)new Random().Next(0, 100) / 100) * Base.PatternBase[local6].Answers.Count));
                    answer = Base.PatternBase[local6].Answers[local44];
                    questionPattern = Base.PatternBase[local6].Questions[local7];
                    mood = (mood + Base.PatternBase[local6].Mood);
                    Emotion = Base.PatternBase[local6].Emotion;
                } else {
                    local8 = 0;
                    answer = "";
                }
            }
            bool local25 = CheckExactMatch();
            if (CheckSuspense() && local8 < 100)
            {
                if (suspenseCount < 2){
                    answer = ExtenstionsUniversal("symbol_repeat");
                } else {
                    answer = ExtenstionsSuspenseX2();
                    local17 = true;
                }
                suspenseCount++;
            } else {
                suspenseCount = 0;
            }
            if (CheckSuspense()){
                local17 = false;
                answer = ExtenstionsUniversal("suspense_after_question");
            }
            if (CheckBotRepeat(answer))
            {
                answer = ((answer + " ") + ExtenstionsUniversal("bot_repeat"));
                local17 = false;
                mood = (mood - 2);
            }
            bool local28 = CheckHumanRepeat(Question);
            if (local28){
                answer = ExtenstionsUniversal("human_repeat");
                local17 = false;
                repeatCount++;
                mood = (mood - 2);
            }
            bool local29 = CheckHumanRepeatBot(Question);
            if (local29){
                answer = ExtenstionsUniversal("human_repeat_bot");
                local17 = false;
                repeatCount++;
                mood = (mood - 2);
            }
            if (((!(local29)) && (!(local28)))){
                repeatCount = 0;
            }
            if (((CheckSymbolRepeat(Question)) && ((local8 < 100)))){
                answer = ExtenstionsUniversal("symbol_repeat");
                local17 = false;
            }
            local30 = (int)Math.Floor((((double)new Random().Next(0, 100) / 100) * 3));
            if ((((((((local30 == 1)) && (answer[(answer.Length - 1)] != '?'))))) && ((mood > -5)))){
                local17 = true;
                answer = ((answer + " ") + ExtenstionsAskQuestion());
            }
            if (question.Emotions != 0){
                if (question.Words.Count == 0){
                    answer = "";
                }
                if (question.Emotions > 0){
                    answer = (":) " + answer);
                    mood = (mood + 1);
                    Emotion = 1;
                }
                if (question.Emotions < 0){
                    answer = (":( " + answer);
                    mood = (mood - 1);
                    Emotion = -1;
                }
            }
            if ((((suspenseCount > 4)) || ((repeatCount > 4)))){
                answer = "[stop]";
            }
            if (mood < -30){
                answer = "[stop]";
            }
            if (!ShareRequest){
                local30 = (int)Math.Floor((((double)new Random().Next(0, 100) / 100) * 3));
                if ((((((((local30 == 1)) && ((answer.Length < 100)))))) && (!(local17)))){
                }
            }
            PatternSpecialCommand();
            AnswerSpecialCommand();
            return answer;
        }
        private int CompareWords(string left, string right)
        {
            return 100;
        }

        private bool CheckExactMatch()
        {
            var questionPattern = new WordOfstring(this.questionPattern);
            var question = new WordOfstring(Question);
            if (questionPattern.Words.Count == question.Words.Count){
                var match = true;
                foreach (var word in question.Words)
                {
                    var not = true;
                    foreach (var w in questionPattern.Words)
                    {
                        if (word.ToUpper() == w.ToUpper())
                        {
                            not = false;
                        }
                    }
                    if (not)
                    {
                        match = false;
                    }
                }
                return match;
            }
            return false;
        }

    }
}
