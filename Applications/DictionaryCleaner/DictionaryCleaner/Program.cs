using System;
using System.Collections.Generic;
using System.IO;

/*
 * Quick Application for cleaning a given dictionary file.
 * This determines what words will be allowed in the dictionary 
 * file for the Word Application.
 */

namespace DictionaryCleaner
{
    internal class Program
    {
        private static HashSet<string> _dictionaryList;

        private static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("The program must be run with the dictionary file and the desired output file .i.e.");
                Console.WriteLine("./DictionaryCleaner dictionary.dic en-dic.dic");
                return;
            }

            if (!File.Exists(args[0]))
            {
                Console.WriteLine("{0} does not exist.", args[0]);
                return;
            }

            string inFile = args[0];
            string outFile = args[1];
            _dictionaryList = new HashSet<string>();

            using (var sr = new StreamReader(inFile))
            {
                string line;
                // ADD YOUR RULES HERE
                while ((line = sr.ReadLine()) != null)
                {
                    // RULE: Don't add words with apostrophe's
                    bool rule_1 = !line.Contains("'");
                    // RULE: No words longer than 9 characters
                    bool rule_2 = line.Length < 10;
                    // RULE: Only single-letter words are a or i
                    bool rule_3 = (line.Length > 1) ||
                                  (line.Contains("a")) ||
                                  (line.Contains("i"));
                    // RULE: All words must contain vowels unless it is "why"
                    bool rule_4 = (line.Contains("a")) ||
                                  (line.Contains("e")) ||
                                  (line.Contains("i")) ||
                                  (line.Contains("o")) ||
                                  (line.Contains("u")) ||
                                  (line.Equals("why"));

                    if (rule_1 && rule_2 && rule_3 && rule_4)
                        _dictionaryList.Add(line.ToLower());
                }
            }

            using (var sw = new StreamWriter(outFile))
            {
                foreach (string word in _dictionaryList)
                {
                    sw.WriteLine(word);
                }
            }

            Console.WriteLine("Done!");
        }
    }
}