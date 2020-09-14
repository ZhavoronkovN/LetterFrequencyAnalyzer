using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Xml.Serialization;

namespace LFA_WPF
{
    public class DataElement: INotifyPropertyChanged //Interface to make table know when data chanched
    {
        //all this for interface
        public event PropertyChangedEventHandler PropertyChanged;
        public char Char { get { return _char; } set { _char = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Char")); } }
        public uint Count { get { return _count; } set { _count = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Count")); } }
        public double Frequency { get { return _frequency; } set { _frequency = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Frequency")); } }

        char _char;
        uint _count;
        double _frequency;

        public DataElement(char key)
        {
            Char = key;
            Count = 0;
            Frequency = 0;
        }

        public DataElement(char key,uint count, double freq)
        {
            Char = key;
            Count = count;
            Frequency = freq;
        }
    }

    public class Logic
    {
        //alphabets
        public static Dictionary<string,char[]> Languages = new Dictionary<string, char[]> { 
            { "English", new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' } },
            { "Ukrainian", new char[] { 'а', 'б', 'в', 'г', 'ґ', 'д', 'е', 'є', 'ж', 'з', 'и', 'і', 'ї', 'й', 'к', 'л', 'м', 'н', 'о', 'п', 'р', 'с', 'т', 'у', 'ф', 'х', 'ц', 'ч', 'ш', 'щ', 'ь', 'ю', 'я' } }
        };
        //statistics
        public Dictionary<string, List<DataElement>> CalculatedLanguages;

        public Logic()
        {
            CalculatedLanguages = new Dictionary<string, List<DataElement>>();
            //fill statistic with zeros
            foreach(var language in Languages.Keys)
            {
                AddLanguage(language);
            }
        }

        public void AddLanguage(string language)
        {
            if (!CalculatedLanguages.ContainsKey(language))
            {
                List<DataElement> dataElements = new List<DataElement>();
                Array.ForEach(Languages[language], (char ch) => dataElements.Add(new DataElement(ch)));
                CalculatedLanguages.Add(language, dataElements);
            }
        }

        public bool UpdateWithFile(string language,string file)
        {
            if (!File.Exists(file))
            {
                return false;
            }
            try
            {
                Dictionary<char, uint> result = new Dictionary<char, uint>();
                Array.ForEach(Languages[language], (char ch) => result.Add(Char.ToLower(ch), 0));

                using (FileStream fs = File.OpenRead(file)) //these two lines would be a couple of times in this file, so it is important to know, why we need them
                using (StreamReader sr = new StreamReader(fs)) //FileStream allows us to work with big files (>1GB). StreamReader (or Writer) let us simple way to write (read) to stream, without buffer and arrays
                {
                    while (!sr.EndOfStream)
                    {
                        ProcessLine(sr.ReadLine(), ref result);
                    }
                }

                foreach (var element in CalculatedLanguages[language])
                {
                    element.Count += result[element.Char];
                }

                UpdateFrequency(language);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void ProcessLine(string line,ref Dictionary<char,uint> result)
        {
            foreach (char ch in line)
            {
                char lowerChar = Char.ToLower(ch);
                if (result.ContainsKey(lowerChar)) //else : we dont need it (special character, number, etc)
                {
                    result[lowerChar]++;
                }
            }
        }

        private void UpdateFrequency(string language)
        {
            ulong sum = 0;
            CalculatedLanguages[language].ForEach((DataElement element) => sum += element.Count); //sum of all letters
            CalculatedLanguages[language].ForEach((DataElement element) => element.Frequency = (double)element.Count/sum); //frequency
        }

        public void ClearCalculated(string language)
        {
            CalculatedLanguages.Remove(language);
            AddLanguage(language);
        }

        public bool ImportStat(string language,string file)
        {
            try
            {
                List<DataElement> dataElements = new List<DataElement>();
                using (FileStream fs = File.OpenRead(file))
                using (StreamReader sr = new StreamReader(fs))
                {
                    string saveString = $"Language : [Name : {language}, Alphabet : {String.Join(",",Languages[language])}]";
                    if (sr.ReadLine() != saveString)
                    {
                        //trying to import different language
                        return false;
                    }
                    while (!sr.EndOfStream)
                    {
                        var splittedLine = sr.ReadLine().Split(';');
                        dataElements.Add(new DataElement(splittedLine[0].First(), uint.Parse(splittedLine[1]), Double.Parse(splittedLine[2])));
                    }
                }
                CalculatedLanguages[language] = dataElements;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool ExportStat(string language, string file)
        {
            try
            {
                using (FileStream fs = File.OpenWrite(file))
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    string saveString = $"Language : [Name : {language}, Alphabet : {String.Join(",", Languages[language])}]";
                    sw.WriteLine(saveString); //to preserve importing to different language
                    foreach (var element in CalculatedLanguages[language])
                    {
                        sw.WriteLine($"{element.Char};{element.Count};{element.Frequency}");
                    }
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
