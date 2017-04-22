using System.Collections.Generic;
using System.Diagnostics;
using Fifteen.Base;

namespace Fifteen.Strategies
{
    public class Dfs
    {
        #region Fields

        private const int MaxRecursionDepth = 20;

        private Node entryNode;
        private List<Node> visitedNodes;
        private string[] searchOrder;
        private Stack<Node> stack;

        //Return values
        private int processedNodes;

        private int depth;
        private Stopwatch stopwatch;

        #endregion

        #region Constructor

        public Dfs(Node entryNode, string[] searchOrder)
        {
            this.entryNode = entryNode;
            this.searchOrder = searchOrder;
            stack = new Stack<Node>();
            visitedNodes = new List<Node>();
        }

        #endregion

        #region Methods

        public Result ProcessPuzzle()
        {
            stopwatch = Stopwatch.StartNew();

            //We enqueue our first given node
            stack.Push(entryNode);

            //Check if current Puzzle State is desired puzzle state
            if (entryNode.PuzzleState.CheckIfInDesiredState())
            {
                stopwatch.Stop();
                return ProcessResult(entryNode);
            }

            //We process nodes till no left in queue
            while (stack.Count != 0)
            {
                //We dequeue last node in queue
                entryNode = stack.Pop();
                processedNodes++;

                //We add current node to visited nodes
                visitedNodes.Add(entryNode);

                //Check if this is a node we are looking for
                if (entryNode.PuzzleState.CheckIfInDesiredState())
                {
                    stopwatch.Stop();
                    return ProcessResult(entryNode);
                }

                if (entryNode.Depth >= MaxRecursionDepth) continue;
                //We iterate node with all directions from given search order
                for (var index = 0; index < searchOrder.Length; index++)
                {
                    string direction = searchOrder[index];
                    //Check if we can move zero in desired direction 
                    if (!entryNode.PuzzleState.CanMoveInDirection(direction)) continue;

                    //If we can move we create new node
                    var tempNode = new Node(entryNode.PuzzleState.MoveInDirection(direction), entryNode.Depth + 1,
                        entryNode.PreviousDirections + direction);

                    if (tempNode.Depth > depth)
                    {
                        depth = tempNode.Depth;
                    }

                    bool isAlreadyVisited = false;
                    //Check if node is already visited
                    for (var i = 0; i < visitedNodes.Count; i++)
                    {
                        Node visitedNode = visitedNodes[i];
                        if (!visitedNode.PuzzleState.CheckIfEquals(tempNode.PuzzleState.PuzzleCurrentState))
                            continue;
                        isAlreadyVisited = true;
                        break;
                    }
                    if (isAlreadyVisited) continue;

                    stack.Push(tempNode);
                    visitedNodes.Add(tempNode);
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
                VisitedNodes = visitedNodes.Count,
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
                VisitedNodes = visitedNodes.Count,
                MaxDepth = depth,
                Duration = stopwatch.ElapsedMilliseconds
            };

            return result;
        }

        #endregion
    }
}