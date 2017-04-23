using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Fifteen.Base;

namespace Fifteen.Strategies
{
    public class Dfs
    {
        #region Fields

        private const int MaxRecursionDepth = 15;

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

            //-----------------------------------------RECURSIVE METHOD-------------------------


            visitedNodes.Add(entryNode);

            Node returnedNode = RunDfs(entryNode);

            if (returnedNode != null)
            {
                return ProcessResult(returnedNode);
            }
            else
            {
                return ProcessFailedResult();
            }

            //-----------------------------------------STACK METHOD-------------------------

            ////We enqueue our first given node
            //stack.Push(entryNode);


            //while (stack.Count > 0)
            //{
            //    entryNode = stack.Pop();
            //    processedNodes++;

            //    if (visitedNodes.Contains(entryNode))
            //        continue;

            //    visitedNodes.Add(entryNode);

            //    if (entryNode.Depth >= MaxRecursionDepth) continue;

            //    for (var i = 0; i < searchOrder.Length; i++)
            //    {
            //        string direction = searchOrder[i];
            //        //Check if we can move zero in desired direction 
            //        if (!entryNode.PuzzleState.CanMoveInDirection(direction)) continue;

            //        Node tempNode = new Node(entryNode.PuzzleState.MoveInDirection(direction), entryNode.Depth + 1,
            //            entryNode.PreviousDirections + direction);

            //        if (tempNode.Depth > depth)
            //        {
            //            depth = tempNode.Depth;
            //        }

            //        //if (visitedNodes.Contains(tempNode)) continue;

            //        //Check if this is a node we are looking for
            //        if (tempNode.PuzzleState.CheckIfInDesiredState())
            //        {
            //            stopwatch.Stop();
            //            return ProcessResult(tempNode);
            //        }

            //        if (stack.Contains(tempNode)) continue;

            //        stack.Push(tempNode);
            //        visitedNodesNumber++;
            //    }
            //    visitedNodesNumber++;

            //}

            //stopwatch.Stop();
            //return ProcessFailedResult();
        }

        private Node RunDfs(Node node)
        {
            //if (visitedNodes.Contains(entryNode)) return null;

            //Check max recursion depth
            if (node.Depth >= MaxRecursionDepth) return null;

            //Iterate over all search directions
            for (var index = 0; index < searchOrder.Length; index++)
            {
                string direction = searchOrder[index];

                if (!node.PuzzleState.CanMoveInDirection(direction)) continue;

                Node childNode = new Node(node.PuzzleState.MoveInDirection(direction), node.Depth + 1,
                    node.PreviousDirections + direction);

                if (childNode.Depth > depth)
                {
                    depth = childNode.Depth;
                }

                //Check if node is already visited
                if (!visitedNodes.Contains(childNode))
                {
                    //Check if this is a node we are looking for
                    if (childNode.PuzzleState.CheckIfInDesiredState())
                    {
                        stopwatch.Stop();
                        return childNode;
                    }

                    //Add node to visited
                    visitedNodes.Add(childNode);

                    //Run recurrency on node
                    Node returnedNode = RunDfs(childNode);


                    //If this is node we are looking for return it
                    if (returnedNode != null)
                    {
                        return returnedNode;
                    }
                }
            }

            processedNodes++;

            //Return null if node hasn't been found
            return null;
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