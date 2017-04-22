using System.Collections.Generic;
using Fifteen.Base;

namespace Fifteen.Strategies
{
    public class Bfs
    {
        #region Fields

        private Node entryNode;
        private Queue<Node> nodesQueue;
        private List<Node> visitedNodes;
        private string[] searchOrder;

        #endregion

        #region Constructor

        public Bfs(Node entryNode, string[] searchOrder)
        {
            this.entryNode = entryNode;
            this.searchOrder = searchOrder;
            nodesQueue = new Queue<Node>();
            visitedNodes = new List<Node>();
        }

        #endregion

        #region Methods

        public void ProcessPuzzle()
        {
            Node tempNode;

            //We enqueue our first given node
            nodesQueue.Enqueue(entryNode);

            //We process nodes till no left in queue
            while (nodesQueue.Count != 0)
            {
                //We dequeue lest node in queue
                entryNode = nodesQueue.Dequeue();

                //We iterate node with all directions from given search order
                foreach (string direction in searchOrder)
                {
                    //Check if we cane move zero in desired direction 
                    if (entryNode.PuzzleState.CanMoveInDirection(direction))
                    {
                        //If we can move we create new node
                        tempNode = new Node(entryNode.PuzzleState.MoveInDirection(direction));
                    }
                    else
                    {
                        
                    }
                }
            }
        }

        #endregion

    }
}
