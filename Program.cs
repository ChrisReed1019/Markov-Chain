using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkovChain
{
    class Program
    {
        static void Main(string[] args)
        {
            string fileName = @"C:\Users\" + Environment.UserName + @"\Desktop\TheTexts.txt";
            string text = File.ReadAllText(fileName);
            Console.WriteLine("Contents of WriteText.txt = {0}", text);
            string[] textArray = text.Split(' ');
            List<string> followUp = new List<string>{ };
            for (int i = 0; i <= textArray.Length - 2; i++)
            {
                textArray[i] = textArray[i].ToLower();
                if (i != textArray.Length)
                    followUp.Add(textArray[i + 1]);

            }
            List<string> connections = new List<string>{ "" };
            for (int i = 0; i <= textArray.Length - 1; i++)
            {
                for (int j = -1; j <= connections.Count - 1 && connections.Count - 1 < 0; j++)
                {
                    if (j == -1)
                        j = 0;
                    Console.WriteLine(j);
                    Console.WriteLine("Connections: {0}, textArray: {1}, i: {2}, j: {3} ",connections[j],textArray[i],i,j);
                    if(connections[j].Contains("=>"))
                    {
                        string word = connections[j].Substring(0, connections[i].IndexOf("=>"));
                        Console.WriteLine(word);
                        if(textArray[i] == word)
                        {
                            connections[j] = connections[j] + "," + word;
                        } else
                        {
                            connections.Add(String.Format("{0}=>{1}", textArray[i], followUp[i]));
                            Console.WriteLine(String.Format("1.{0}=>{1}", textArray[i], followUp[i]));
                            break;
                        }
                    } else
                    {
                        connections.Add(String.Format("{0}=>{1}",textArray[i],followUp[i]));
                        Console.WriteLine(String.Format("2.{0}=>{1}", textArray[i], followUp[i]));
                        break;
                    }
                }
            }
            foreach (string str in connections)
                Console.WriteLine(str);
            Console.ReadKey();
        }
    }
}
