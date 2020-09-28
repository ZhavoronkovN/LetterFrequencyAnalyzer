using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cryptology.Second
{
    public class LogicSecond
    {
        public Dictionary<string, char[]> Substitutions = new Dictionary<string, char[]>();
        static Random _rnd = new Random();
        public LogicSecond()
        {
            foreach(var el in Common.Languages.Keys)
            {
                AddLanguage(el);
            }
        }

        public void AddLanguage(string name)
        {
            Substitutions.Add(name, Common.Languages[name].ToArray());
        }

        public void Random(string language)
        {
            string charsToChoose = new string(Common.Languages[language]);
            int length = Common.Languages[language].Length;
            string alreadyHave = "";
            for(int i = 0; i < length; i++)
            {
                var t = charsToChoose.Except(alreadyHave).ToArray();
                int ind = _rnd.Next(0, t.Length);
                char next = t[ind];
                alreadyHave += next;
                Substitutions[language][i] = next;
            }
        }

        public bool ReadRules(string file)
        {
            string res = ReadFile(file);
            if (String.IsNullOrEmpty(res))
            {
                return false;
            }
            foreach(string rule in res.Split('\n'))
            {
                if (rule.Contains(':'))
                {
                    var langSub = rule.Split(':');
                    if (Substitutions.ContainsKey(langSub[0]) && langSub[1].Length == Substitutions[langSub[0]].Length)
                    {
                        Substitutions[langSub[0]] = langSub[1].ToCharArray();
                    }
                }
            }
            return true;
        }

        public bool WriteRules(string file)
        {
            string res = "";
            foreach(var el in Substitutions)
            {
                res += el.Key + ":" + String.Join("",el.Value) + "\n";
            }
            return WriteFile(res, file);
        }

        public string Encode(string inp,string outp, string text)
        {
            string res = "";
            foreach(char ch in text)
            {
                if (inp.Contains(ch))
                {
                    res += outp.ElementAt(inp.IndexOf(ch));
                }
                else
                {
                    res += ch;
                }
            }
            return res;
        }

        public string ReadFile(string file)
        {
            try
            {
                using (FileStream fs = File.OpenRead(file))
                using (StreamReader sr = new StreamReader(fs))
                {
                    return sr.ReadToEnd();
                }
            }
            catch
            {
                return String.Empty;
            }
        }

        public bool WriteFile(string text,string file)
        {
            try
            {
                using (FileStream fs = File.OpenWrite(file))
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(text);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
