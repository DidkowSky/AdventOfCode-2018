using System;
using System.Collections.Generic;
using System.IO;

namespace First_puzzle
{
    public class Calculator
    {
        private List<int> list = new List<int>();

        public void AddToList(int value)
        {
            if (!list.Contains(value))
            {
                list.Add(value);
            }
            else
            {
                Console.WriteLine("And the winner is: " + value);
                Console.ReadKey();
                return;
            }
        }

        public int ParseLine(string line)
        {
            if (int.TryParse(line, out int value))
            {
                return value;
            }
            else
            {
                Console.WriteLine("Nie udalo się przekonwertować na int.");
                Console.ReadKey();
                Environment.Exit(0);
                return int.MinValue;
            }
        }
    }

    public class Program
    {
        static void Main(string[] args)
        {
            Calculator calc = new Calculator();
            try
            {
                using (StreamReader sr = new StreamReader("stream.txt"))
                {
                    int sum = 0;
                    string line;
                        
                    line = sr.ReadLine();

                    sum = calc.ParseLine(line);
                    Console.WriteLine(sum);
                    
                    while (true)
                    {
                        if ((line = sr.ReadLine()) != null)
                        {
                            //line = sr.ReadLine();
                            //Console.WriteLine(line);
                            sum += calc.ParseLine(line);
                            calc.AddToList(sum);
                            Console.WriteLine(sum);
                        }
                        else
                        {
                            sr.DiscardBufferedData();
                            sr.BaseStream.Seek(0, SeekOrigin.Begin);
                        }
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
            Console.ReadKey();
        }

        private static void ParseLine()
        {
            throw new NotImplementedException();
        }
    }
}
