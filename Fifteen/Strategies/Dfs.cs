using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Fifteen.Base;

namespace Fifteen.Strategies
{
    public class Dfs
    {
        #region Fields

        private const int MaxRecursionDepth = 11;

        private Node entryNode;
        private List<Node> visitedNodes;
        private string[] searchOrder;
        private Stack<Node> stack;

        //Return values
        private int processedNodes;

        private int visitedNodesNumber;

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

            while (stack.Count > 0)
            {
                entryNode = stack.Pop();
                processedNodes++;

                if (visitedNodes.Contains(entryNode))
                    continue;

                visitedNodes.Add(entryNode);

                if (entryNode.Depth >= MaxRecursionDepth) continue;

                for (var i = 0; i < searchOrder.Length; i++)
                {
                    string direction = searchOrder[i];
                    //Check if we can move zero in desired direction 
                    if (!entryNode.PuzzleState.CanMoveInDirection(direction)) continue;

                    Node tempNode = new Node(entryNode.PuzzleState.MoveInDirection(direction), entryNode.Depth + 1,
                        entryNode.PreviousDirections + direction);

                    if (tempNode.Depth > depth)
                    {
                        depth = tempNode.Depth;
                    }

                    //if (visitedNodes.Contains(tempNode)) continue;
                   
                    //Check if this is a node we are looking for
                    if (tempNode.PuzzleState.CheckIfInDesiredState())
                    {
                        stopwatch.Stop();
                        return ProcessResult(tempNode);
                    }

                    if(stack.Contains(tempNode)) continue;

                    stack.Push(tempNode);
                    visitedNodesNumber++;
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