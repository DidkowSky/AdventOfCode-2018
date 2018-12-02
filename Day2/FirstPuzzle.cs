using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Day2
{
    public class FirstPuzzle
    {
        private int twoTimesCounter = 0;
        private int threeTimesCounter = 0;
        private string line;
        private List<char> charList = new List<char>();

        public void CalculateSolution()
        {
            StreamReader file = new StreamReader("stream.txt");
            
            while ((line = file.ReadLine()) != null)
            {
                charList.Clear();

                Console.WriteLine(line.Length);
                for(int j = 0; j < line.Length; j++)
                {
                    charList.Add(line[j]);
                    Console.Write(line[j]);
                }
                Console.WriteLine();

                Console.WriteLine(line);

                CheckString(charList);
            }

            file.Close();

            Console.WriteLine(twoTimesCounter + " * " + threeTimesCounter + " = " + (twoTimesCounter * threeTimesCounter));

            Console.ReadLine();
        }

        private void CheckString(List<char> list)
        {
            bool two = false;
            bool three = false;

            var g = list.GroupBy(k => k);
            foreach (var grp in g)
            {
                if (grp.Count() == 2)
                {
                    Console.WriteLine("{0} {1}", grp.Key, grp.Count());
                    two = true;
                }
                if (grp.Count() == 3)
                {
                    Console.WriteLine("{0} {1}", grp.Key, grp.Count());
                    three = true;
                }
            }

            if (two)
            {
                twoTimesCounter++;
            }
            if (three)
            {
                threeTimesCounter++;
            }
        }
    }
}
