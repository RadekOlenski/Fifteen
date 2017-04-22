using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fifteen.Base
{
    public class Puzzle
    {
        #region Fields

        public int[,] PuzzleCurrentState;
        public int[,] PuzzleDesiredState;

        public int Width;
        public int Height;

        public int Size;

        public List<string> AvailableDirections;

        private int zeroPositionI;
        private int zeroPositionJ;

        #endregion

        #region Constructors

        public Puzzle(int height, int width, List<int> values)
        {
            Height = height;
            Width = width;
            Size = Width * Height;
            PuzzleCurrentState = new int[Height, Width];

            int counter = 0;
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    PuzzleCurrentState[i, j] = values[counter];
                    if (values[counter] == 0)
                    {
                        zeroPositionI = i;
                        zeroPositionJ = j;
                    }
                    counter++;
                }
            }

            int value = 1;
            PuzzleDesiredState = new int[Height, Width];
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    if (i == Height - 1 && j == Width - 1)
                    {
                        PuzzleDesiredState[i, j] = 0;
                    }
                    else
                    {
                        PuzzleDesiredState[i, j] = value;
                        value++;
                    }
                }
            }

            AvailableDirections = new List<string>();
            CheckAvailableDirections();
        }

        public Puzzle(int height, int width, int[,] values, int[,] desiredState, int zeroPositionI, int zeroPositionJ)
        {
            Height = height;
            Width = width;
            Size = Width * Height;
            PuzzleCurrentState = values;
            PuzzleDesiredState = desiredState;

            this.zeroPositionI = zeroPositionI;
            this.zeroPositionJ = zeroPositionJ;

            AvailableDirections = new List<string>();
            CheckAvailableDirections();
        }

        #endregion

        #region Methods

        private void CheckAvailableDirections()
        {
            int maxHeight = Height - 1;
            int maxWidth = Width - 1;

            if (zeroPositionI != 0 && zeroPositionI != maxHeight)
            {
                AvailableDirections.Add("U");
                AvailableDirections.Add("D");
            }
            else if (zeroPositionI == 0)
            {
                AvailableDirections.Add("D");
            }
            else if (zeroPositionI == maxHeight)
            {
                AvailableDirections.Add("U");
            }

            if (zeroPositionJ != 0 && zeroPositionJ != maxWidth)
            {
                AvailableDirections.Add("L");
                AvailableDirections.Add("R");
            }
            else if (zeroPositionJ == 0)
            {
                AvailableDirections.Add("R");
            }
            else if (zeroPositionJ == maxWidth)
            {
                AvailableDirections.Add("L");
            }
        }

        public bool CanMoveInDirection(string direction)
        {
            return AvailableDirections.Contains(direction);
        }

        public Puzzle MoveInDirection(string direction)
        {
            int newZeroPositionI = zeroPositionI;
            int newZeroPositionJ = zeroPositionJ;

            switch (direction)
            {
                case "U":
                {
                    newZeroPositionI -= 1;
                    break;
                }
                case "D":
                {
                    newZeroPositionI += 1;
                    break;
                }
                case "L":
                {
                    newZeroPositionJ -= 1;
                    break;
                }
                case "R":
                {
                    newZeroPositionJ += 1;
                    break;
                }
            }

            int currentNewPositionValue = PuzzleCurrentState[newZeroPositionI, newZeroPositionJ];

            int[,] newPuzzleState = new int[Height, Width];

            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    newPuzzleState[i, j] = PuzzleCurrentState[i, j];
                }
            }

            newPuzzleState[newZeroPositionI, newZeroPositionJ] = 0;
            newPuzzleState[zeroPositionI, zeroPositionJ] = currentNewPositionValue;

            return new Puzzle(Height, Width, newPuzzleState, PuzzleDesiredState, newZeroPositionI, newZeroPositionJ);
        }

        public bool CheckIfInDesiredState()
        {
            return PuzzleCurrentState.Cast<int>().SequenceEqual(PuzzleDesiredState.Cast<int>());
        }

        #endregion
    }
}