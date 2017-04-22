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
            AvailableDirections = new List<string>();

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

            CheckAvailableDirections();
        }

        #endregion

        #region Methods

        private void CheckAvailableDirections()
        {
            if (zeroPositionI != 0 && zeroPositionI != 3)
            {
                AvailableDirections.Add("U");
                AvailableDirections.Add("D");
            }
            else
                switch (zeroPositionI)
                {
                    case 0:
                        AvailableDirections.Add("D");
                        break;
                    case 3:
                        AvailableDirections.Add("U");
                        break;
                }

            if (zeroPositionJ != 0 && zeroPositionJ != 3)
            {
                AvailableDirections.Add("L");
                AvailableDirections.Add("R");
            }
            else
                switch (zeroPositionJ)
                {
                    case 0:
                        AvailableDirections.Add("R");
                        break;
                    case 3:
                        AvailableDirections.Add("L");
                        break;
                }
        }

        public bool CanMoveToDirection(string direction)
        {
            return AvailableDirections.Contains(direction);
        }

        #endregion
    }
}