using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Replacer
{
    using System.IO;
    using System.Xml;

    using Newtonsoft.Json;

    using Formatting = Newtonsoft.Json.Formatting;

    class Program
    {
        public static void Main(string[] args)
        {
            Dirs(@"E:\Data\Public\Bot\Replacer\bin\Debug\Script\");
        }

        public static void Dirs(string dir)
        {
            foreach (var fileName in Directory.GetFiles(dir).Where(f => f.Contains(".as")))
            {
                Repl(fileName);
            }
            foreach (var dd in Directory.GetDirectories(dir))
            {
                Dirs(dd);
            }
        }

        public static void Files(string file)
        {
            var lines = File.ReadAllLines(file);
            var ls2 = new List<string>();
            foreach (var line in lines)
            {
                var l2 = line;
                var re = new System.Text.RegularExpressions.Regex("public var *.*.*;");
                if (re.IsMatch(line))
                {
                    var i1 = line.IndexOf(":");
                    var i2 = line.IndexOf(";");
                    var sbs = line.Substring(i1);
                    var l1 = line.Replace("var", sbs.Replace(":", "").Replace(";", ""));
                    l2 = l1.Replace(sbs, ";");
                    Console.WriteLine("Match : {0}", line);
                }
                ls2.Add(l2);
            }
            File.Delete(file);
            File.WriteAllLines(file, ls2);
        }

        public static void Repl(string path)
        {

            String f = File.ReadAllText(path);
            // === constructors and functions ===
            f = Regex.Replace(f, "override", "");
            // constructors
            f = Regex.Replace(f, @"([a-z]*)\\s+function\\s+(\\w*)\\s*\\Q(\\E\\s*([^)]*)\\s*\\Q)\\E(\\s*\\{)", "$1 $2(%#$3%#)$4");
            // functions
            f = Regex.Replace(f, @"([a-z]*)\\s+function\\s+(\\w*)\\s*\\Q(\\E\\s*([^)]*)\\s*\\Q)\\E\\s*:\\s*(\\w*)", "$1 $4 $2(%#$3%#)");
            // remove zero parameter marks
            f = Regex.Replace(f, "%#\\s*%#", "");
            // Now, replace unprocessed parameters
            for (int i = 0; i < 9; i++)
            {
                // w/o default values
                f = Regex.Replace(f, "%#\\s*(\\w*)\\s*:\\s*(\\w*)\\s*,", "$2 $1,%#");
                f = Regex.Replace(f, "%#\\s*(\\w*)\\s*:\\s*(\\w*)\\s*%#", "$2 $1");
                // w/ default values
                f = Regex.Replace(f, "%#\\s*(\\w*)\\s*:\\s*(\\w*)\\s*=\\s*([^):,]*)\\s*,", "$2 $1=$3,%#");
                f = Regex.Replace(f, "%#\\s*(\\w*)\\s*:\\s*(\\w*)\\s*=\\s*([^):,]*)\\s*%#", "$2 $1=$3");
            }

            // === typecasts ===
            f = Regex.Replace(f, "(\\w+)\\s+as\\s+(\\w+)", "($2)$1");
            f = Regex.Replace(f, @"int\\s*\\Q(\\E([^)]+)\\Q)\\E", "((int)$1)");

            f = Regex.Replace(f, @"public var (.*?)\:(.*?)( \= )??(.*?)??\;", "public $4 $1 $3$2;");

            // === variable declarations ===
            f = f.Replace(":*", "");
            f = f.Replace("*;", ";");
            // XXX multiple comma separated declarations not supported yet!
            // === type translation ===
            f = f.Replace("Number", "double");
            f = f.Replace("Boolean", "bool");

            f = f.Replace("extends", ":");
            f = f.Replace("function", "void");
            f = f.Replace("super()", "base()");
            f = f.Replace(":void", "");
            f = f.Replace("package", "namespace Bot");
            f = f.Replace("import", "using");
            f = f.Replace("var", "dynamic");
            f = f.Replace("length", "Length");
            f = f.Replace("substr", "Substring");
            f = f.Replace("indexOf", "IndexOf");
            f = f.Replace("String", "string");
            f = f.Replace("this.", "");
            f = f.Replace("toUpperCase", "ToUpper");
            f = f.Replace("slice", "Slice");
            f = f.Replace("new Date", "new DateTime");

            File.Delete(path);
            File.WriteAllText(path.Replace(".as", ".cs"), f);
        }
    }
}
