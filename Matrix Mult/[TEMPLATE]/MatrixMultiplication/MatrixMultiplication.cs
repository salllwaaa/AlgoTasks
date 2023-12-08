using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Problem
{
    // *****************************************
    // DON'T CHANGE CLASS OR FUNCTION NAME
    // YOU CAN ADD FUNCTIONS IF YOU NEED TO
    // *****************************************
    public static class MatrixMultiplication
    {
        #region YOUR CODE IS HERE

        //Your Code is Here:
        //==================
        /// <summary>
        /// Multiply 2 square matrices in an efficient way [Strassen's Method]
        /// </summary>
        /// <param name="M1">First square matrix</param>
        /// <param name="M2">Second square matrix</param>
        /// <param name="N">Dimension (power of 2)</param>
        /// <returns>Resulting square matrix</returns>
        static public int[,] MatrixMultiply(int[,] M1, int[,] M2, int N)
        {
            int[,] Resultmatrix = new int[N, N];
            if (N <= 64)
            {
                Resultmatrix = NormalMatrixMutiplication(M1, M2, N);
            }
            else
            {
                int midSize = N / 2;

                int[,] p1 = new int[midSize, midSize];
                int[,] p2 = new int[midSize, midSize];
                int[,] p3 = new int[midSize, midSize];
                int[,] p4 = new int[midSize, midSize];
                int[,] p5 = new int[midSize, midSize];
                int[,] p6 = new int[midSize, midSize];
                int[,] p7 = new int[midSize, midSize];
                int[,] A00 = new int[midSize, midSize];
                int[,] A01 = new int[midSize, midSize];
                int[,] A10 = new int[midSize, midSize];
                int[,] A11 = new int[midSize, midSize];
                int[,] B00 = new int[midSize, midSize];
                int[,] B01 = new int[midSize, midSize];
                int[,] B10 = new int[midSize, midSize];
                int[,] B11 = new int[midSize, midSize];
                int[,] C00 = new int[midSize, midSize];
                int[,] C01 = new int[midSize, midSize];
                int[,] C10 = new int[midSize, midSize];
                int[,] C11 = new int[midSize, midSize];


            

                A00 = DivideMatrixInto(M1, 0, 0, midSize);
                A01 = DivideMatrixInto(M1, 0, midSize, midSize);
                A10 = DivideMatrixInto(M1, midSize, 0, midSize);
                A11 = DivideMatrixInto(M1, midSize, midSize, midSize);

                B00 = DivideMatrixInto(M2, 0, 0, midSize);
                B01 = DivideMatrixInto(M2, 0, midSize, midSize);
                B10 = DivideMatrixInto(M2, midSize, 0, midSize);
                B11 = DivideMatrixInto(M2, midSize, midSize, midSize);
                /* -------------------------------------------------------------
                 | c00   c01 |    | a00    a01 |      | b00    b01 |
                 |           |  = |            | *    |            |
                 | c10   c11 |    | a10    a11 |      | b10    b11 |
                 -------------------------------------------------------------
                */

                Parallel.Invoke(
                    // p1 = a00 . ( b01 - b11)
                    () => { p1 = MatrixMultiply(A00, MatrixSubtraction(B01, B11, midSize), midSize); },
                    // p2 = (a00 + a01) . b11
                    () => { p2 = MatrixMultiply(MatrixAddition(A00, A01, midSize), B11, midSize); },
                    // p3 = (a10 + a11) . b00
                    () => { p3 = MatrixMultiply(MatrixAddition(A10, A11, midSize), B00, midSize); },
                    // p4 = a11 . (b10 - b00)
                    () => { p4 = MatrixMultiply(A11, MatrixSubtraction(B10, B00, midSize), midSize); },
                    //p5 = (a00 + a11) . (b00 + b11)
                    () => { p5 = MatrixMultiply(MatrixAddition(A00, A11, midSize), MatrixAddition(B00, B11, midSize), midSize); },
                    //p6 = (a01 - a11) . (b10 + b11)
                    () => { p6 = MatrixMultiply(MatrixSubtraction(A01, A11, midSize), MatrixAddition(B10, B11, midSize), midSize); },
                    //p7 = (a00 - a10) . (b00 + b01)
                    () => { p7 = MatrixMultiply(MatrixSubtraction(A00, A10, midSize), MatrixAddition(B00, B01, midSize), midSize); }

                    );

               
                /*
               -------------------------------------------------------------
               | c00   c01  |            |(p5+p4 - p2+p6)     p1+p2       |  
               |            |    =       |                                |
               | c10   c11  |            | p3+p4             (p5+p1-p3-p7)|    
               -------------------------------------------------------------
                */



                Task t1 = Task.Factory.StartNew(() => C00 = MatrixAddition(MatrixSubtraction(MatrixAddition(p5, p4, midSize), p2, midSize), p6, midSize));
                Task t2 = Task.Factory.StartNew(() => C01 = MatrixAddition(p1, p2, midSize));
                Task t3 = Task.Factory.StartNew(() => C10 = MatrixAddition(p3, p4, midSize));
                Task t4 = Task.Factory.StartNew(() => C11 = MatrixSubtraction(MatrixSubtraction(MatrixAddition(p5, p1, midSize), p3, midSize), p7, midSize));


                Task.WaitAll(t1, t2, t3, t4);

                Resultmatrix = CombineSubMatricesInto(C00,  C01,  C10,  C11,midSize);


            }
            



            return Resultmatrix;
        }

        static public int[,] MatrixAddition(int[,] M1, int[,] M2, int N)
        {
            int[,] AdditionMatrix = new int[N, N];


            for (int row = 0; row < N; row++)
            {
                for (int col = 0; col < N; col++)
                {
                    AdditionMatrix[row, col] = M1[row, col] + M2[row, col];
                }
            }

            return AdditionMatrix;
        }
        static public int[,] MatrixSubtraction(int[,] M1, int[,] M2, int N)
        {
            int[,] SubtractionMatrix = new int[N, N];

            for (int row = 0; row < N; row++)
            {
                for (int col = 0; col < N; col++)
                {
                    SubtractionMatrix[row, col] = M1[row, col] - M2[row, col];
                }
            }


            return SubtractionMatrix;
        }
        static public int[,] NormalMatrixMutiplication( int[,] M1, int[,] M2,  int N)
        {
            int[,] Resultmatrix = new int[N, N];

            for (int row = 0; row < N; row++)
            {
              
                for (int col = 0; col < N; col++)
                {
                  
                    Resultmatrix[row, col] = 0;

                  
                    for (int k = 0; k < N; k++)
                    {
                        
                        Resultmatrix[row, col] += M1[row, k] * M2[k, col];
                    }
                }
            }
            return Resultmatrix;

        }


       

        
        static public int[,] CombineSubMatricesInto(int[,] C00, int[,] C01, int[,] C10, int[,] C11, int midSize)
        {
            int[,] ResultMatrix = new int[midSize*2, midSize*2];

            for (int row = 0; row < midSize; row++)
            {
                for (int col = 0; col < midSize; col++)
                {
                    ResultMatrix[row, col] = C00[row, col];
                    ResultMatrix[row, col + midSize] = C01[row, col];
                    ResultMatrix[row + midSize, col] = C10[row, col];
                    ResultMatrix[row + midSize, col + midSize] = C11[row, col];
                }

            }
            return ResultMatrix;
        }



        static public int[,] DivideMatrixInto(int[,]M,int from,int to,int midSize)
        {
            int[,] subMatrix = new int[midSize, midSize];
            for (int row = 0; row < midSize; row++)
            {
                for (int col = 0; col < midSize; col++)
                {
                    subMatrix[row, col] = M[row+from,col+to];
                }
            }
            return subMatrix;
        }

    }

    #endregion
}


