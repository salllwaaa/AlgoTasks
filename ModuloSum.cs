using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Problem
{
    // ***************
    // DON'T CHANGE CLASS OR FUNCTION NAME
    // YOU CAN ADD FUNCTIONS IF YOU NEED TO
    // ***************
    public static class ModuloSum
    {
        #region YOUR CODE IS HERE    

        #region FUNCTION#1: Calculate the Value
        //Your Code is Here:
        //==================
        /// <summary>
        /// Fill this function to check whether there's a subsequence numbers in the given array that their sum is divisible by M
        /// </summary>
        /// <param name="items">array of integers</param>
        /// <param name="N">array size </param>
        /// <param name="M">value to check against it</param>
        /// <returns>true if there's subsequence with sum divisible by M... false otherwise</returns>

        static bool[,] memomize;
        static char[,] sumToNumberChar;
        static List<int> sumToNumberList;
   

        public static bool SolveValue(int[] items, int N, int M)
        {
            memomize = new bool[N + 1, M + 1];
            sumToNumberChar=new char[N + 1, M + 1];
            bool SolveValueHelper(int Ns, int SM)
            {
                if (SM == 0)
                {
                    sumToNumberChar[Ns, SM] = 'C';
                    return true;
                }
                if (Ns == 0)
                {
                    return false;
                }

                if (memomize[Ns, SM] == true)
                {
                    return memomize[Ns, SM];
                }

                bool result;
                if (items[Ns - 1] % SM == 0)
                {
                    sumToNumberChar[Ns, SM] = 'C';
                    result = true;
                }
                else
                {
                    sumToNumberChar[Ns, SM] = 'U';
                    
                    result = SolveValueHelper(Ns - 1, SM) || SolveValueHelper(Ns - 1, (SM - items[N - 1] + SM) % SM);
                   
                }

                return memomize[Ns, SM] = result;
            }

            return SolveValueHelper(N, M);
        }



        static public int[] ConstructSolution(int[] items, int N, int M)
        {
            sumToNumberList = new List<int>();
            if (memomize[N, M] == false)
            {
                return null;
            }
            else
            {
                backtrack(items, N, M);
            }
            return sumToNumberList.ToArray();
        }


        static public void backtrack(int[] items, int N, int M)
        {
            
            if (M == 0 || N == 0)
            {
              
                return;
            }
            if (sumToNumberChar[N, M] == 'U')
            {
                
                backtrack(items, N - 1, M);
            }
            else
            {
               
                backtrack(items, N - 1, (M - items[N - 1] + M) % M);
                sumToNumberList.Add(items[N - 1]);
            }
        }




        #endregion
        #endregion

    }
}