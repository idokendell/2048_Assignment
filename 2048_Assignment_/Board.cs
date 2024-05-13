using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2048_Assignment_
{
    public class Board
    {
        public int[,] Data { get; protected set; }
        private static int[] _first_values = { 2, 4 };
        public GameStatus Status { get; protected set; }
        public int Points { get; protected set; }
        private const int GAME_SIZE = 4;
        public Board()
        {
            Start();
        }

        public void Start()
        {
            Data = new int[GAME_SIZE, GAME_SIZE];
            Points = 0;
            Status = GameStatus.Idle;
            this.Place();
            this.Place();
        }
        private List<int> EmptyPlaces(int row)
        {
            List<int> emptyIndex = new List<int>();
            for (int i = 0; i < Data.GetLength(1); i++)
            {
                if (Data[row, i] == 0)
                {
                    emptyIndex.Add(i);
                }
            }
            return emptyIndex;
        }
        private void Place()
        {
            List<int> empty_rows = new List<int>() { 0, 1, 2, 3 };
            List<int> empty = new List<int>();
            Random rnd = new Random(); int row = 0;
            while (empty.Count() ==0)
            {
                row = empty_rows[new Random().Next(0, empty_rows.Count())];
                empty = EmptyPlaces(row);
                if (empty.Count==0)
                    empty_rows.Remove(row);
            }
            int index = empty[rnd.Next(0, empty.Count())];
            Data[row, index] = _first_values[rnd.Next(0, _first_values.Length)];
        }

        public void Move(Direction move)
        {
            if (Status!= GameStatus.Lose)
            {
                switch (move)
                {
                    case Direction.UP:
                        Points+=Move_Up();
                        break;
                    case Direction.DOWN:
                        Points+=Move_Down();
                        break;
                    case Direction.RIGHT:
                        Points+=Move_Right();
                        break;
                    case Direction.LEFT:
                        Points+=Move_Left();
                        break;
                }
                if (Status!= GameStatus.Lose)
                {
                    Place();
                }
                CheckStatus();
            }
        }
        //move single row right but every move uses this method
        public static int MoveSingleLine(int[] arr)
        {
            int extraPoint = 0;
            int boundary = -1;
            for (int i = arr.Length-1; i>-1; i--)
            {
                if (arr[i]!=0)
                {
                    for (int j = arr.Length-1; j>i; j--)
                    {
                        if (arr[j] != 0) continue;
                        arr[j]=arr[i];
                        if (j!=arr.Length-1)
                        {
                            if (arr[j]==arr[j+1]&&boundary!=j+1)//prevent dup
                            {
                                extraPoint+=MergeEqualsNums(arr, j);
                                boundary=j+1;
                            }
                        }
                        arr[i]=0;
                        break;
                    }
                    //if there wasnt moves check for merge
                    if (arr[i]!=0 && i!=arr.Length-1)
                    {
                        if (arr[i]==arr[i+1]&& boundary!=i+1)
                        {
                            extraPoint+=MergeEqualsNums(arr, i);
                            boundary=i+1;

                        }
                    }

                }
            }
            return extraPoint;
        }
        public static int MergeEqualsNums(int[] arr, int index)
        {
            arr[index+1] *=2;
            arr[index]=0;
            return arr[index+1];
        }
        private int Move_Right()
        {
            return Move_Horizontal(false);
        }
        private int Move_Left()
        {
            return Move_Horizontal(true);
        }
        private int Move_Up()
        {
            return Move_Vertical(true);
        }
        private int Move_Down()
        {
            return Move_Vertical(false);
        }
        private int Move_Vertical(bool reversed)
        {
            int[] tempCol = new int[4];
            int extraPoints = 0;
            for (int i = 0; i<Data.GetLength(1); i++)
            {
                tempCol=GetVerticalSlice(i);
                if (reversed)
                {
                    Array.Reverse(tempCol);
                    extraPoints+=MoveSingleLine(tempCol);
                    Array.Reverse(tempCol);
                    SetVerticalSlice(i, tempCol);
                }
                else
                {
                    extraPoints+=MoveSingleLine(tempCol);
                    SetVerticalSlice(i, tempCol);
                }
                //for (int k = 0; k<Data.Count(); k++)
                //{
                //    Data[k][i]=tempCol[k];
                //}
            }
            return extraPoints;
        }

        private int Move_Horizontal(bool reversed)
        {
            int extraPoints = 0;
            int[] tempCol = new int[4];
            for (int row = 0; row<Data.GetLength(0); row++)
            {
                tempCol=GetHorizontalSlice(row);
                if (reversed)
                {
                    Array.Reverse(tempCol);
                    extraPoints+=MoveSingleLine(tempCol);
                    Array.Reverse(tempCol);
                    SetHorizontalSlice(row, tempCol);
                }
                else
                {
                    extraPoints+=MoveSingleLine(tempCol);
                    SetHorizontalSlice(row, tempCol);
                }
            }
            return extraPoints;
        }

        private bool Win()
        {
            for (int i = 0; i<Data.GetLength(0); i++)
            {
                for (int j = 0; j<Data.GetLength(1); j++)
                {
                    if (Data[i, j] == 2048)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        private bool Lost()
        {
            for (int i = 0; i<Data.GetLength(0); i++)
            {
                for (int j = 0; j<Data.GetLength(1); j++)
                {
                    if (Data[i, j] == 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        private void CheckStatus()
        {
            if (Win())
            {
                Status=GameStatus.Win;
            }
            if (Lost())
            {
                Status=GameStatus.Lose;
            }

        }
        private int[] GetHorizontalSlice(int x)
        {
            int[] result = new int[GAME_SIZE];
            for (int y = 0; y < GAME_SIZE; y++)
                result[y] = Data[x, y];
            return result;
        }

        private void SetHorizontalSlice(int row, int[] values)
        {
            for (uint y = 0; y < values.Length; y++)
                Data[row, y] = values[y];
        }

        private int[] GetVerticalSlice(int y)
        {
            int[] result = new int[GAME_SIZE];
            for (uint x = 0; x < GAME_SIZE; x++)
                result[x] = Data[x, y];
            return result;
        }

        private void SetVerticalSlice(int y, int[] values)
        {
            for (int row = 0; row < GAME_SIZE; row++)
                Data[row, y] = values[row];
        }

    }
}
