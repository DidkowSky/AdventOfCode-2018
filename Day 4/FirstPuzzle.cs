using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Day_4
{
    public class FirstPuzzle
    {
        private string line;
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
            StreamReader file = new StreamReader("input.txt");
            int guardId = 0;
            SleepCycle sleepCycle = new SleepCycle();

            while ((line = file.ReadLine()) != null)
            {
                DateTime.TryParse(Regex.Match(line, @"\[([^)]*)\]").Groups[1].Value, out date);
                statement = line.Remove(0, 19);
                log.Add(date, statement);
            }

            foreach (DateTime key in log.Keys)
            {
                //Console.WriteLine(key + " " + log[key]);
                splitedLine = log[key].Split(' ');
                //Console.WriteLine(splitedLine[0]);
                if(splitedLine[0] == "Guard")
                {
                    int.TryParse(splitedLine[1].Remove(0, 1), out guardId);
                    if (!guardList.ContainsKey(guardId))
                    {
                        guardList.Add(guardId, new List<SleepCycle>());
                    }
                }
                else if (splitedLine[0] == "falls")
                {
                    sleepCycle.fellAsleep = key;
                }else if(splitedLine[0] == "wakes")
                {
                    sleepCycle.wokeUp = key;
                    guardList[guardId].Add(sleepCycle);
                }
            }

            StringBuilder stringBuilder = new StringBuilder();
            int[] scores = new int[60];
            int tmpKey = 0;
            int days = 0;
            DateTime day = new DateTime();
            float max = 0;
            int maxKey = 0;

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
                    stringBuilder.Clear();
                    stringBuilder.Append('.', 60);

                    //TODO: Merge same days to one line (day variable if the same still build stringBilder)

                    stringBuilder.Insert(((guardList[key][i].fellAsleep.Minute -1) < 0? 0 : guardList[key][i].fellAsleep.Minute -1), "#", (guardList[key][i].wokeUp.Minute - guardList[key][i].fellAsleep.Minute));
                    //Console.WriteLine("{0} | {1} - {2}", key, guardList[key][i].fellAsleep.Minute, guardList[key][i].wokeUp.Minute);
                    Console.WriteLine(key + " | " + stringBuilder.Remove(59, stringBuilder.Length - 60));

                    for (int j = 0; j < scores.Length; j++)
                    {
                        if ((j >= (guardList[key][i].fellAsleep.Minute - 1)) && (j < guardList[key][i].wokeUp.Minute))
                            scores[j] += 2;
                        else
                            scores[j]--;
                    }
                    days++;
                }

                stringBuilder.Clear();
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
                Console.WriteLine(stringBuilder);

                Console.WriteLine("{0}| {1} : {2}", maxKey, max, (days>0)? (float)(max/days) : 0);
            }

            file.Close();

            Console.ReadLine();

        }
    }
}
