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

        public Dictionary<int, int> PuzzleCurrentState;

        #endregion

        #region Constructors

        public Puzzle(int width, int height, List<int> values)
        {
            PuzzleCurrentState = new Dictionary<int, int>();

            for (int i = 1; i <= width * height; i++)
            {
                PuzzleCurrentState.Add(i, values[i - 1]);
            }
        }

        #endregion


    }
}
