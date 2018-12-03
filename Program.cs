using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MarkovChain
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string fileName = @"C:\Users\" + Environment.UserName + @"\Desktop\TheTexts.txt";
            string text = File.ReadAllText(fileName);
            text = text.Replace("\n", " ").Replace("\t", "").Replace("\r", "").Replace("  ", " ").Replace("--", "- ");
            Console.WriteLine("Contents of WriteText.txt = {0}", text);
            string[] textArray = text.Split(' ');
            List<string> followUp = new List<string> { };
            List<string> textList = textArray.ToList();
            //put periods alone in array and get starting words
            List<string> startingWords = new List<string> { };
            List<string> endingWords = new List<string> { };
            for (int i = 0; i <= textList.Count - 1; i++)
            {
                if (textList[i].Contains("."))
                {
                    if (i != textList.Count - 1)
                        startingWords.Add(textList[i + 1]);
                    if (textList[i].Length > 1)
                    {
                        //Console.Write(textList[i]);
                        textList[i] = textList[i].Substring(0, textList[i].Length - 1);
                        endingWords.Add(textList[i]);
                        textList.Insert(i + 1, ".");
                        //textList.RemoveAt(i);
                    }
                }
            }
            //combine things like "hello and world" and tolower all
            for (int i = 0; i <= textList.Count - 1; i++)
            {
                textList[i] = textList[i].ToLower();
                string replacement = textList[i].Replace("\n", string.Empty).Replace("\t", string.Empty).Replace("\r", string.Empty);
                textList[i] = replacement;
                if (i != textList.Count)
                {
                    if (textList[i].StartsWith("\"") && textList[i + 1].EndsWith("\""))
                    {
                        textList[i] = textList[i] + "_" + textList[i + 1];
                        textList.RemoveAt(i + 1);
                    }
                }
                //remove blank
                if (string.IsNullOrEmpty(textList[i]) || string.IsNullOrWhiteSpace(textList[i]))
                {
                    textList.RemoveAt(i);
                }
            }
            for (int i = 0; i <= textList.Count - 2; i++)
            {
                textList[i] = textList[i].ToLower();
                if (i != textList.Count)
                    followUp.Add(textList[i + 1]);

            }
            for (int i = 0; i <= followUp.Count - 1; i++)
            {
                string replacement = followUp[i].Replace("\n", " ").Replace("\t", " ").Replace("\r", " ");
                followUp[i] = replacement;
            }
            List<string> connections = new List<string> { "" };
            int j = 0;
            for (int i = 0; i <= textList.Count - 1; i++)
            {
                do
                {
                    if (j == -1)
                        j = 0;
                    if (i < followUp.Count && connections[j].Contains("=>"))
                    {
                        connections.Add(string.Format("{0}=>{1}", textList[i], followUp[i]));
                    }
                    else
                    {
                        if (i < followUp.Count)
                        {
                            connections.Add(string.Format("{0}=>{1}", textList[i], followUp[i]));
                        }
                    }
                    j++;
                    break;
                } while (j <= connections.Count - 1 && connections.Count - 1 < 0);
            }
            /*
            string word = connections[j].Substring(0, connections[i].IndexOf("=>"));
                        if (textList[i] == word)
                        {
                            Console.WriteLine(word + " " + textList[i]);
                            connections[j] = String.Concat(connections[j],",",word);
                        }
             */
            //join words
            for (int i = 1; i <= connections.Count - 1; i++)
            {
                string word = connections[i].Substring(0, connections[i].IndexOf("=>"));
                bool done = false;
                while (!done && i != connections.Count - 1)
                {
                    for (int k = i + 1; k <= connections.Count - 1; k++)
                    {
                        string w = connections[k].Substring(0, connections[k].IndexOf("=>"));
                        string w2 = connections[k].Substring(connections[k].IndexOf("=>") + 2); ;
                        if (w == word)
                        {
                            connections[i] = connections[i] + "|" + w2;
                            connections.RemoveAt(k);
                            done = true;
                        }
                    }
                    done = true;
                }
                Console.WriteLine(i + "/" + (connections.Count - 1));
            }
            Console.WriteLine("Done!");
            Console.ReadKey();
            Console.Write("Enter amount length: ");
            int len = int.Parse(Console.ReadLine());
            string output = "";
            Random rand = new Random();
            for (int i = 0; i <= len; i++)
            {
                string[] spl = output.Split(' ');
                if (i == 0)
                {
                    output = output + startingWords[rand.Next(startingWords.Count)];
                }
                else if(spl[spl.Length - 1].EndsWith("."))
                {
                    output = output + " " + startingWords[rand.Next(startingWords.Count)];
                }
                else if(i == len)
                {
                    output = output + endingWords[rand.Next(endingWords.Count)] + ".";
                }
                else
                {
                    string prevWord;
                    if (spl.Length - 2 > 0)
                        prevWord = spl[spl.Length - 2];
                    else
                        prevWord = spl[0];
                    for (int f = 1; f <= connections.Count - 1; f++)
                    {
                        string word = connections[f].Substring(0, connections[f].IndexOf("=>"));
                        string w2 = connections[f].Substring(connections[f].IndexOf("=>") + 2);
                        List<string> splw2 = w2.Split('|').ToList();
                        if (prevWord.ToLower() == word)
                        {
                            string nextWord = splw2[rand.Next(splw2.Count)];
                            if (nextWord != ".")
                                output = output + " " + nextWord;
                            else
                                output = output + nextWord;
                        }
                    }
                }
            }
            Console.WriteLine(output);
            Console.ReadKey();

        }
        private static string ToLiteral(string input)
        {
            using (StringWriter writer = new StringWriter())
            {
                using (CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp"))
                {
                    provider.GenerateCodeFromExpression(new CodePrimitiveExpression(input), writer, null);
                    return writer.ToString();
                }
            }
        }

    }
}
