using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fifteen.Base;

namespace Fifteen
{
    public class Node
    {
        #region Fields

        public Node Left;
        public Node Right;
        public Node Up;
        public Node Down;

        public Puzzle PuzzleState;

        #endregion

        #region Constructor

        public Node(Puzzle puzzleState, Node left = null, Node right = null, Node up = null, Node down = null)
        {
            PuzzleState = puzzleState;
            Left = left;
            Right = right;
            Up = up;
            Down = down;
        }

        #endregion



    }
}
