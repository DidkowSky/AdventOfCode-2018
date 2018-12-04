using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Day_4
{
    public class FirstPuzzle
    {
        private float max = 0;
        private int maxKey = 0;
        private int tmpKey = 0;
        private string statement;
        private string[] splitedLine;
        private DateTime date;
        private SortedDictionary<DateTime, string> log = new SortedDictionary<DateTime, string>();
        private SortedDictionary<int, List<SleepCycle>> guardList = new SortedDictionary<int, List<SleepCycle>>();

        private struct SleepCycle
        {
            public DateTime fellAsleep;
            public DateTime wokeUp;
        }

        public void SolvePuzzle()
        {
            StreamReader inputFile = new StreamReader("input.txt");
            StreamWriter outputFile = new StreamWriter("output.txt");

            FillLog(inputFile);
            FillGuardList();

            StringBuilder stringBuilder = new StringBuilder();
            int[] scores = new int[60];
            int days = 0;
            int day = -1;

            foreach (int key in guardList.Keys)
            {
                if(tmpKey != key)
                {
                    tmpKey = key;
                    days = 0;
                    for(int i = 0; i < scores.Length; i++)
                    {
                        scores[i] = 0;
                    }
                }

                for (int i = 0; i < guardList[key].Count; i++)
                {
                    if(day != guardList[key][i].fellAsleep.Day)
                    {
                        if (day >= 0)
                        {
                            //Console.WriteLine(key + " | " + stringBuilder.Remove(59, stringBuilder.Length - 60));
                            outputFile.WriteLine(key + " | " + stringBuilder.Remove(59, stringBuilder.Length - 60) + " | " + day + " | " + days + " | " + guardList[key][i].fellAsleep.Minute + " | " + guardList[key][i].wokeUp.Minute);
                        }

                        stringBuilder.Clear();
                        stringBuilder.Append('.', 60);

                        day = guardList[key][i].fellAsleep.Day;

                        if (i > 0)
                        {
                            for (int j = 0; j < scores.Length; j++)
                            {
                                if ((j >= (guardList[key][i].fellAsleep.Minute - 1)) && (j < guardList[key][i].wokeUp.Minute))
                                    scores[j] ++;
                            }
                        }
                        days++;
                    }

                    //TODO: Merge same days to one line (day variable if the same still build stringBilder)

                    stringBuilder.Insert(guardList[key][i].fellAsleep.Minute, "#", (guardList[key][i].wokeUp.Minute - guardList[key][i].fellAsleep.Minute));
                    //Console.WriteLine("{0} | {1} - {2}", key, guardList[key][i].fellAsleep.Minute, guardList[key][i].wokeUp.Minute);
                }
                //Console.WriteLine(key + " | " + stringBuilder.Remove(59, stringBuilder.Length - 60));
                outputFile.WriteLine(key + " | " + stringBuilder.Remove(59, stringBuilder.Length - 60) + " | " + day + " | " + days);

                //stringBuilder.Clear();
                stringBuilder = BuildScoreString(scores);

                //Console.WriteLine(stringBuilder);
                outputFile.WriteLine(stringBuilder);

                int sum = 0;
                for(int i = 0; i < scores.Length; i++)
                {
                    sum += scores[i];
                }

                //Console.WriteLine("{0}| {1} : {2}", maxKey, max, (days>0)? (float)(max/days) : 0);
                outputFile.WriteLine("{0}| {1} : {2}", maxKey, max, (float)sum);//(days > 0) ? (float)(max / days) : 0);
            }

            inputFile.Close();
            outputFile.Close();

            Console.ReadLine();
        }

        private void FillLog(StreamReader file)
        {
            string line;

            while ((line = file.ReadLine()) != null)
            {
                DateTime.TryParse(Regex.Match(line, @"\[([^)]*)\]").Groups[1].Value, out date);
                statement = line.Remove(0, 19);
                log.Add(date, statement);
            }
        }

        private void FillGuardList()
        {
            int guardId = 0;
            SleepCycle sleepCycle = new SleepCycle();

            foreach (DateTime key in log.Keys)
            {
                //Console.WriteLine(key + " " + log[key]);
                splitedLine = log[key].Split(' ');
                //Console.WriteLine(splitedLine[0]);
                if (splitedLine[0] == "Guard")
                {
                    Console.WriteLine(int.TryParse(splitedLine[1].Remove(0, 1), out guardId));
                    if (!guardList.ContainsKey(guardId))
                    {
                        guardList.Add(guardId, new List<SleepCycle>());
                    }
                }
                else if (splitedLine[0] == "falls")
                {
                    sleepCycle.fellAsleep = key;
                }
                else if (splitedLine[0] == "wakes")
                {
                    sleepCycle.wokeUp = key;
                    guardList[guardId].Add(sleepCycle);
                }
            }
        }

        private StringBuilder BuildScoreString(int[] scores)
        {
            StringBuilder stringBuilder = new StringBuilder();
            max = int.MinValue;

            for (int j = 0; j < scores.Length; j++)
            {
                if (scores[j] > max)
                {
                    max = scores[j];
                    maxKey = tmpKey;
                }
                stringBuilder.Append(scores[j] + ",");
            }
            return stringBuilder;
        }
    }
}
