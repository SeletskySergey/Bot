using System.Collections.Generic;

namespace Bot.Script.Base {
    public class PatternType
    {
        public int Index = 0;
        public List<string> Questions;
        public List<string> Answers;
        public List<string> Themes;
        public string Context;
        public string Author;
        public double Dialog;
        public int Emotion;
        public int Mood;
        public bool IsNew;
        public bool IsModerate;

        public PatternType(){
            Questions = new List<string>();
            Answers = new List<string>();
            Themes = new List<string>();
            Context = "";
            Author = "";
            Dialog = new double();
            Mood = 0;
            IsNew = false;
            IsModerate = false;
            Emotion = 0;
        }
    }
}
