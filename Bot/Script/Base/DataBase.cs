using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Bot.Script.Base {

    public class DataBase {
        public string BotName;
        public int BotAge;
        public List<PatternType> PatternBase;
        public int NewPatternsStart;
        private readonly XmlDocument db;
        private readonly XmlDocument nonModerate;
        public XmlDocument Index;
        public XmlDocument Extensions;
        private readonly XmlDocument newPatterns;

        public DataBase(){
            BotName = "pBot";
            BotAge = new int();
            PatternBase = new List<PatternType>();
            NewPatternsStart = 0;
            db = new XmlDocument();
            nonModerate = new XmlDocument();
            Index = new XmlDocument();
            Extensions = new XmlDocument();
            newPatterns = new XmlDocument();
        }

        public void Load(){
            Extensions.Load("extensions.xml");
            db.Load("kengine.xml");
            nonModerate.Load("non_moderated.xml");
            newPatterns.Load("newpatterns.xml");
            Index.Load("base_index.xml");
            FillPatternLines(db.FirstChild.ChildNodes[1], false, true);
            FillPatternLines(nonModerate.FirstChild, false, false);
            FillPatternLines(newPatterns.FirstChild, true, false);
        }

        private void FillPatternLines(XmlNode xml, bool isNew, bool isModerate)
        {
            foreach (XmlNode node in xml.ChildNodes)
            {
                var pattern = new PatternType {IsNew = isNew, IsModerate = isModerate};
                foreach (XmlNode childNode in node.ChildNodes)
                {
                    foreach (var xnode in childNode.ChildNodes.Cast<XmlNode>().Where(x => x.FirstChild != null))
                    {
                        switch (childNode.Name)
                        {
                            case "q":
                            case "question":
                                if (xnode.Name == "variant" || xnode.Name == "v")
                                {
                                    pattern.Questions.Add(xnode.FirstChild.Value);
                                }
                                break;
                            case "a":
                            case "answer":
                                if (xnode.Name == "variant" || xnode.Name == "v")
                                {
                                    pattern.Answers.Add(xnode.FirstChild.Value);
                                }
                                break;
                            case "th":
                            case "theme":
                                if (xnode.Name == "variant" || xnode.Name == "v")
                                {
                                    pattern.Themes.Add(xnode.FirstChild.Value);
                                }
                                break;
                            case "c":
                            case "context":
                                pattern.Context = childNode.FirstChild.Value;
                                break;
                        }
                    }
                }
                foreach (XmlAttribute attribute in node.Attributes)
                {
                    if (attribute.Name == "exactmatch" || attribute.Name == "e")
                    {
                        pattern.Emotion = Convert.ToInt32(node.Attributes[attribute.Name].Value);
                    }
                    if (attribute.Name == "author" || attribute.Name == "a")
                    {
                        pattern.Author = node.Attributes[attribute.Name].Value;
                    }
                    if (attribute.Name == "mood" || attribute.Name == "m")
                    {
                        pattern.Mood = Convert.ToInt32(node.Attributes[attribute.Name].Value);
                    }
                    if (attribute.Name == "dialog" || attribute.Name == "d")
                    {
                        pattern.Dialog = Convert.ToInt32(node.Attributes[attribute.Name].Value);
                    }
                }
                PatternBase.Add(pattern);
            }
        }
    }
}
