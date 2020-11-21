using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cryptology.Third
{
    public class LogicThird
    {
        static Random _rnd = new Random();

        public string Random(string language, int count)
        {
            string charsToChoose = new string(Common.Languages[language]);
            int length = Common.Languages[language].Length;
            string result = "";
            for (int i = 0; i < count; i++)
            { 
                int ind = _rnd.Next(0, length);
                result += Common.Languages[language][ind];
            }
            return result;
        }

        public string ReadKey(string file)
        {
            return ReadFile(file) ?? "";
        }

        public bool WriteKey(string file,string key)
        {
            return WriteFile(key, file);
        }

        public string Encode(string text, string key,string language)
        {
            if (text.Length > key.Length)
                return "Error! Key length less than text";
            string result = "";
            var alphabet = Common.Languages[language].ToList();
            for (int i = 0; i < text.Length; i++)
            {
                result += (alphabet.IndexOf(text[i]) ^ alphabet.IndexOf(key[i])).ToString() + ' ';
            }
            return result.Trim();
        }

        public string Decode(string text, string key, string language)
        {
            if (text.Split(' ').Length > key.Length)
                return "Error! Key length less than text";
            string result = "";
            var alphabet = Common.Languages[language].ToList();
            var numbers = text.Split(' ');
            for (int i = 0; i < numbers.Length; i++)
            {
                result += alphabet[int.Parse(numbers[i]) ^ alphabet.IndexOf(key[i])];
            }
            return result;
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

        public bool WriteFile(string text, string file)
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
