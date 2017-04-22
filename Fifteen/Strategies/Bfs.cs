using System.Collections;
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
            //We enqueue our first given node
            nodesQueue.Enqueue(entryNode);

            //Check if current Puzzle State is desired puzzle state
            if (entryNode.PuzzleState.CheckIfInDesiredState())
            {
                //return values
                return;
            }

            //We process nodes till no left in queue
            while (nodesQueue.Count != 0)
            {
                //We dequeue last node in queue
                entryNode = nodesQueue.Dequeue();

                //We iterate node with all directions from given search order
                foreach (string direction in searchOrder)
                {
                    //Check if we can move zero in desired direction 
                    if (entryNode.PuzzleState.CanMoveInDirection(direction))
                    {
                        
                        //If we can move we create new node and add it to queue
                        var tempNode = new Node(entryNode.PuzzleState.MoveInDirection(direction));

                        if(visitedNodes.Contains(tempNode)) continue;

                        if (tempNode.PuzzleState.CheckIfInDesiredState())
                        {
                            //return values
                            return;
                        }

                        //If new node is not in queue and is not visited we add it to new queue
                        if (!nodesQueue.Contains(tempNode) && !visitedNodes.Contains(tempNode))
                        {
                            nodesQueue.Enqueue(tempNode);
                            visitedNodes.Add(tempNode);
                        }
                    }
                }

                //We add current node to visited nodes
                if (!visitedNodes.Contains(entryNode))
                {
                    visitedNodes.Add(entryNode);
                }
            }
        }

        #endregion
    }
}