using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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

        //Return values
        private int processedNodes;
        private int visitedNodesNumber;

        private int depth;
        private Stopwatch stopwatch;

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

        public Result ProcessPuzzle()
        {
            stopwatch = Stopwatch.StartNew();

            //We enqueue our first given node
            nodesQueue.Enqueue(entryNode);

            //Check if current Puzzle State is desired puzzle state
            if (entryNode.PuzzleState.CheckIfInDesiredState())
            {
                stopwatch.Stop();
                return ProcessResult(entryNode);
            }

            //We process nodes till no left in queue
            while (nodesQueue.Count != 0)
            {
                //We dequeue last node in queue
                entryNode = nodesQueue.Dequeue();
                processedNodes++;

                //We iterate node with all directions from given search order
                for (var i = 0; i < searchOrder.Length; i++)
                {
                    string direction = searchOrder[i];
                    //Check if we can move zero in desired direction 
                    if (!entryNode.PuzzleState.CanMoveInDirection(direction)) continue;

                    //If we can move we create new node
                    var tempNode = new Node(entryNode.PuzzleState.MoveInDirection(direction), entryNode.Depth + 1,
                        entryNode.PreviousDirections + direction);

                    if (tempNode.Depth > depth)
                    {
                        depth = tempNode.Depth;
                    }

                    //Check if node is already visited
                    if (visitedNodes.Contains(tempNode)) continue;

                    //Check if this is a node we are looking for
                    if (tempNode.PuzzleState.CheckIfInDesiredState())
                    {
                        stopwatch.Stop();
                        return ProcessResult(tempNode);
                    }
                    visitedNodesNumber++;

                    //If new node is not in queue and is not visited we add it to new queue
                    if (!nodesQueue.Contains(tempNode))
                    {
                        nodesQueue.Enqueue(tempNode);
                        visitedNodes.Add(tempNode);
                    }
                }

                //We add current node to visited nodes
                if (!visitedNodes.Contains(entryNode))
                {
                    visitedNodes.Add(entryNode);
                }
            }

            stopwatch.Stop();
            return ProcessFailedResult();
        }

        private Result ProcessResult(Node endNode)
        {
            Result result = new Result
            {
                SolutionSteps = endNode.Depth,
                Directions = endNode.PreviousDirections,
                ProcessedNodes = processedNodes,
                VisitedNodes = visitedNodesNumber,
                MaxDepth = depth,
                Duration = stopwatch.ElapsedMilliseconds
            };

            return result;
        }

        private Result ProcessFailedResult()
        {
            Result result = new Result
            {
                SolutionSteps = -1,
                ProcessedNodes = processedNodes,
                VisitedNodes = visitedNodesNumber,
                MaxDepth = depth,
                Duration = stopwatch.ElapsedMilliseconds
            };

            return result;
        }

        #endregion
    }
}