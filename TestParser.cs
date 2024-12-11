using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AhhaMoment
{
    class QuizEntry
    {
        public string Question { get; set; }
        public string Hint { get; set; }
        public string Answer { get; set; }
    }
}

namespace TestParser
{
    class Program
    {
        static void Main(String[] args)
        {
            string filePath = "\"C:\\BrainLife_Codespace\\aha_dataset.txt\"";

            // Check if the file exists
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"File not found: {filePath}");
                return;
            }

            // Define the regex pattern
            string pattern = @"\{\[(?<Question>.*?)\],\[(?<Hint>.*?)\],\[(?<Answer>.*?)\]\}";

            // Create a compiled Regex object with case-insensitive option
            Regex regex = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);

            // Initialize a list to store quiz entries
            List<QuizEntry> quizEntries = new List<QuizEntry>();

            // Read the file line by line
            int lineNumber = 0;
            foreach (string line in File.ReadLines(filePath))
            {
                lineNumber++;
                // Find all matches in the current line
                MatchCollection matches = regex.Matches(line);

                if (matches.Count > 0)
                {
                    foreach (Match match in matches)
                    {
                        string question = match.Groups["Question"].Value.Trim();
                        string hint = match.Groups["Hint"].Value.Trim();
                        string answer = match.Groups["Answer"].Value.Trim();

                        // Validate extracted data
                        if (string.IsNullOrEmpty(question) || string.IsNullOrEmpty(answer))
                        {
                            Console.WriteLine($"Incomplete data on line {lineNumber}:");
                            Console.WriteLine(line);
                            Console.WriteLine(new string('-', 40));
                            continue; // Skip this entry
                        }

                        // Create a new QuizEntry object and add it to the list
                        quizEntries.Add(new QuizEntry
                        {
                            Question = question,
                            Hint = hint,
                            Answer = answer
                        });
                    }
                }
                else
                {
                    Console.WriteLine($"No match found on line {lineNumber}:");
                    Console.WriteLine(line);
                    Console.WriteLine(new string('-', 40));
                }


                // Display all parsed entries
                Console.WriteLine("\nAll Parsed Quiz Entries:");
                Console.WriteLine(new string('=', 40));
                int entryNumber = 0;
                foreach (var entry in quizEntries)
                {
                    entryNumber++;
                    Console.WriteLine($"Entry {entryNumber}:");
                    Console.WriteLine($"Question: {entry.Question}");
                    Console.WriteLine($"Hint: {entry.Hint}");
                    Console.WriteLine($"Answer: {entry.Answer}");
                    Console.WriteLine(new string('-', 40));
                }

                Console.WriteLine("Parsing complete.");
            }
        }
    }
}