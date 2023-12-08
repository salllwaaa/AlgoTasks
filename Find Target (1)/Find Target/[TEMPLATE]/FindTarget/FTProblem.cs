using Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace Problem
{

    public class Problem : ProblemBase, IProblem
    {
        #region ProblemBase Methods
        public override string ProblemName { get { return "FindTarget"; } }

        public override void TryMyCode()
        {            

            //Case1
            int N = 8;
            Location s = new Location(), d = new Location();
            s.X = 1; s.Y = 1;
            d.X = 5; d.Y = 1;
            int expected = 2;
            int output = FindTarget.Play(N, s, d);
            PrintCase(N, s, d, output, expected);

            //Case2
            N = 8;
            s.X = 1; s.Y = 1;
            d.X = 6; d.Y = 1;
            expected = -1;
            output = FindTarget.Play(N, s, d);
            PrintCase(N, s, d, output, expected);

            //Case3
            N = 4;
            s.X = 2; s.Y = 3;
            d.X = 4; d.Y = 3;
            expected = -1;
            output = FindTarget.Play(N, s, d);
            PrintCase(N, s, d, output, expected);

            //Case4
            N = 4;
            s.X = 4; s.Y = 4;
            d.X = 2; d.Y = 1;
            expected = 1;
            output = FindTarget.Play(N, s, d);
            PrintCase(N, s, d, output, expected);

            //Case5
            N = 8;
            s.X = 4; s.Y = 4;
            d.X = 8; d.Y = 4;
            expected = 2;
            output = FindTarget.Play(N, s, d);
            PrintCase(N, s, d, output, expected);
        }

        

        Thread tstCaseThr;
        bool caseTimedOut ;
        bool caseException;

        protected override void RunOnSpecificFile(string fileName, HardniessLevel level, int timeOutInMillisec)
        {
            int testCases;
            int actualResult = int.MinValue;
            int output = int.MinValue;

            FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read);

            StreamReader sr = new StreamReader(file);
            string line = sr.ReadLine();
            testCases = int.Parse(line);
   
            int totalCases = testCases;
            int correctCases = 0;
            int wrongCases = 0;
            int timeLimitCases = 0;
            bool readTimeFromFile = false;
            if (timeOutInMillisec == -1)
            {
                readTimeFromFile = true;
            }
            int i = 1;
            while (testCases-- > 0)
            {
                int N = int.Parse(sr.ReadLine());
                line = sr.ReadLine();
                string[] lineParts = line.Split(',');
                var s = new Location();
                s.X = int.Parse(lineParts[0]);
                s.Y = int.Parse(lineParts[1]);

                line = sr.ReadLine();
                lineParts = line.Split(',');
                var d = new Location();
                d.X = int.Parse(lineParts[0]);
                d.Y = int.Parse(lineParts[1]);
                
                line = sr.ReadLine();
                actualResult = int.Parse(line);
                caseTimedOut = true;
                caseException = false;
                {
                    tstCaseThr = new Thread(() =>
                    {
                        try
                        {
                            Stopwatch sw = Stopwatch.StartNew();
                            output = FindTarget.Play(N, s, d);
                            sw.Stop();
                            //PrintCase(vertices,edges, output, actualResult);
                            Console.WriteLine("N = {0}, time in ms = {1}", N, sw.ElapsedMilliseconds);
                            Console.WriteLine("{0}", output);
                        }
                        catch
                        {
                            caseException = true;
                            output = int.MinValue;
                        }
                        caseTimedOut = false;
                    });

                    //StartTimer(timeOutInMillisec);
                    if (readTimeFromFile)
                    {
                        timeOutInMillisec = int.Parse(sr.ReadLine().Split(':')[1]);
                    }
                    tstCaseThr.Start();
                    tstCaseThr.Join(timeOutInMillisec);
                }

                if (caseTimedOut)       //Timedout
                {
                    Console.WriteLine("Time Limit Exceeded in Case {0}.", i);
					tstCaseThr.Abort();
                    timeLimitCases++;
                }
                else if (caseException) //Exception 
                {
                    Console.WriteLine("Exception in Case {0}.", i);
                    wrongCases++;
                }
                else if (output == actualResult)    //Passed
                {
                    Console.WriteLine("Test Case {0} Passed!", i);
                    correctCases++;
                }
                else                    //WrongAnswer
                {
                    Console.WriteLine("Wrong Answer in Case {0}.", i);
                    Console.WriteLine(" your answer = {0}, correct answer = {1}", output, actualResult);
                    wrongCases++;
                }

                i++;
            }
            file.Close();
            sr.Close();
            Console.WriteLine();
            Console.WriteLine("# correct = {0}", correctCases);
            Console.WriteLine("# time limit = {0}", timeLimitCases);
            Console.WriteLine("# wrong = {0}", wrongCases);
            Console.WriteLine("\nFINAL EVALUATION (%) = {0}", Math.Round((float)correctCases / totalCases * 100, 0)); 
        }

        protected override void OnTimeOut(DateTime signalTime)
        {
        }

        public override void GenerateTestCases(HardniessLevel level, int numOfCases, bool includeTimeInFile = false, float timeFactor = 1)
        {
            throw new NotImplementedException();

        }

        #endregion

        #region Helper Methods
        private static void PrintCase(int N, Location s, Location d, int output, int expected)
        {
            Console.WriteLine("Board Size: {0} x {0}", N);
            Console.WriteLine("Player Location (X, Y): {0}, {1}", s.X, s.Y);
            Console.WriteLine("Target Location (X, Y): {0}, {1}", d.X, d.Y);

            Console.WriteLine("Output: {0}", output);
            Console.WriteLine("Expected: {0}", expected);
            if (output == expected)    //Passed
            {
                Console.WriteLine("CORRECT");
            }
            else                    //WrongAnswer
            {
                Console.WriteLine("WRONG");
            }
            Console.WriteLine();
        }
        
        #endregion
   
    }
}
