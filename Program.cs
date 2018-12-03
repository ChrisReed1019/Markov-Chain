using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace MarkovChain
{
    class Program
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
                        textList.Insert(i + 1, ".");
                        Console.WriteLine("Removing {0} ({1}) and inserting . at {2}",i,textList[i],i+1);
                        //textList.RemoveAt(i);
                    }
                }
            }
            //combine things like "hello and world" and tolower all
            for (int i = 0; i <= textList.Count - 1; i++)
            {
                textList[i] = textList[i].ToLower();
                string replacement = textList[i].Replace("\n", String.Empty).Replace("\t", String.Empty).Replace("\r", String.Empty); 
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
                            Console.WriteLine(ToLiteral(textList[i]) + " + " + ToLiteral(followUp[i]));
                            Console.WriteLine("Connecting {0} and {1}", textList[i], followUp[i]);
                            connections.Add(string.Format("{0}=>{1}", textList[i], followUp[i]));
                        }
                        else
                        {
                            if (i < followUp.Count)
                            {
                                Console.WriteLine(ToLiteral(textList[i]) + " + " + ToLiteral(followUp[i]));
                                Console.WriteLine("Connecting {0} and {1}", textList[i], followUp[i]);
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
            for (int i = 0; i <= textList.Count - 1; i++)
            {
                for (j = 0; j <= connections.Count - 1; j++)
                {

                }
            }
            Console.WriteLine("----");
            Console.ReadKey();
            for (int i = 0; i <= connections.Count - 1; i++)
            {
                Console.WriteLine("{0}: {1}", i, connections[i]);
            }
            Console.WriteLine("Done!");
            Console.ReadKey();
            Console.Write("Enter word length: ");

            Console.ReadKey();

        }
        private static string ToLiteral(string input)
        {
            using (var writer = new StringWriter())
            {
                using (var provider = CodeDomProvider.CreateProvider("CSharp"))
                {
                    provider.GenerateCodeFromExpression(new CodePrimitiveExpression(input), writer, null);
                    return writer.ToString();
                }
            }
        }

    }
}
