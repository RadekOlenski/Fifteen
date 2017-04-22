using System.Linq;

namespace Fifteen.Base
{
    public class Node
    {
        #region Fields

        public Puzzle PuzzleState;

        public int Depth;
        public string PreviousDirections;

        #endregion

        #region Constructor

        public Node(Puzzle puzzleState, int depth = 0, string previousDirections = "")
        {
            PuzzleState = puzzleState;
            Depth = depth;
            PreviousDirections = previousDirections;
        }

        #endregion

        //#region Overrides

        //public override bool Equals(object obj)
        //{
        //    Node node = obj as Node;
        //    return node != null && PuzzleState.PuzzleCurrentState.Cast<int>().SequenceEqual(PuzzleState.PuzzleDesiredState.Cast<int>());
        //}
        //#endregion
    }
}