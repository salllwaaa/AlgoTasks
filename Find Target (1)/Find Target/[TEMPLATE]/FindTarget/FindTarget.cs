using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Problem
{
    public struct Location
    {
        int x, y;

        public int Y
        {
            get { return y; }
            set { y = value; }
        }

        public int X
        {
            get { return x; }
            set { x = value; }
        }
    }
    public static class FindTarget
    {
        #region YOUR CODE IS HERE
        //Your Code is Here:
        //==================
        /// <summary>
        /// Given the dimention of the board and the current location of the player, calculate the min number of moves to reach the given target or return -1 if can't be reached
        /// </summary>
        /// <param name="N">board dimension</param>
        /// <param name="src">current location of the player</param>
        /// <param name="target">target location</param>
        /// <returns>min number of moves to reach the target OR -1 if can't reach the target</returns>
        public static int Play(int N, Location src, Location target)
        {
            // initialize board as 2D array
            int[,] board = new int[N, N];
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    board[i, j] = 1;
                }
            }
            // initialization for BFS
            Queue<Location> BFSqueue = new Queue<Location>();
            BFSqueue.Enqueue(src);
            bool[,] visited = new bool[N,N];
            visited[src.X - 1,src.Y - 1] = true;

            // BFS
            int noOfMoves = 0;
            int qlen;
            Location current= new Location();
            Location neighbor = new Location();
            int neighborX, neighborY;
            int[] Xmoves = { 2, 2, -2, -2 };
            int[] Ymoves = { -3, 3, -3, 3 };

            while (BFSqueue.Count > 0)
            {
                qlen = BFSqueue.Count;
                for (int i = 0; i < qlen; i++)
                {
                    current = BFSqueue.Dequeue();
                    //target found
                    if (current.X == target.X && current.Y == target.Y)
                    {
                        return noOfMoves;
                    }

                    for (int j = 0; j < 4; j++)
                    {
                        neighborX = current.X + Xmoves[j];
                        neighborY = current.Y + Ymoves[j];
                        //neighbor location is available
                        if (neighborX >= 1 && neighborX <= N && neighborY >= 1 && neighborY <= N)
                        {
                            // check neighbor if not visited
                            if (!visited[neighborX - 1, neighborY - 1])
                            {
                                visited[neighborX - 1, neighborY - 1] = true;
                                neighbor.X = neighborX;
                                neighbor.Y = neighborY;
                                BFSqueue.Enqueue(neighbor);
                            }
                        }
                    }
                }
                noOfMoves++;
            }

            // target not reachable
            return -1;
        }



    }
    #endregion
}
