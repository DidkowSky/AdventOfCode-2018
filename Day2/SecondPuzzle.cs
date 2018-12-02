using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day2
{
    class SecondPuzzle
    {
        private  struct Lines
        {
            public int line1;
            public int line2;
        }

        private int counter = 0;
        private string line;
        private Dictionary<Lines, int> differenceNumbers = new Dictionary<Lines, int>();

        public void CalculateSolution()
        {
            counter = 0;
            StreamReader file = new StreamReader("stream.txt");

            while ((line = file.ReadLine()) != null)
            {
                CheckDiff(line, counter);

                counter++;
            }
            
            file.Close();
            
            Console.WriteLine(differenceNumbers.Values.Min());

            var value = differenceNumbers.FirstOrDefault(x => x.Value == differenceNumbers.Values.Min()).Key;

            Console.WriteLine(value.line1 + " | " + value.line2);

            Console.WriteLine(File.ReadLines("stream.txt").Skip(value.line1).Take(1).First());
            Console.WriteLine(File.ReadLines("stream.txt").Skip(value.line2).Take(1).First());

            Console.ReadLine();
        }

        private void CheckDiff(string line, int number)
        {
            string secondLine;
            int differentChars = 0;
            int counter = number;
            Lines lines = new Lines();

            while (true)
            {
                counter++;

                if (counter < File.ReadLines("stream.txt").Count())
                {
                    differentChars = 0;
                    secondLine = File.ReadLines("stream.txt").Skip(counter).Take(1).First();

                    for (int i = 0; i < line.Length; i++)
                    {
                        if (!line[i].Equals(secondLine[i]))
                        {
                            differentChars++;
                        }
                    }

                    lines.line1 = number;
                    lines.line2 = counter;
                    differenceNumbers.Add(lines, differentChars);
                }
                else
                {
                    return;
                }
            }
        }
    }
}
