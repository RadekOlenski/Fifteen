using System;
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

            //-----------------------------------------RECURSIVE METHOD-------------------------


            //visitedNodes.Add(entryNode);

            //Node returnedNode = RunDfs(entryNode);

            //if (returnedNode != null)
            //{
            //    return ProcessResult(returnedNode);
            //}
            //else
            //{
            //    return ProcessFailedResult();
            //}

            //-----------------------------------------STACK METHOD-------------------------

            //We enqueue our first given node
            stack.Push(entryNode);

            visitedNodesNumber++;


            while (stack.Count > 0)
            {
                entryNode = stack.Pop();

                //if (visitedNodes.Contains(entryNode))
                //    continue;


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
                        processedNodes++;
                        return ProcessResult(tempNode);
                    }

                    if (stack.Contains(tempNode)) continue;

                    stack.Push(tempNode);
                    visitedNodes.Add(entryNode);
                    visitedNodesNumber++;
                }
                processedNodes++;

            }

            stopwatch.Stop();
            return ProcessFailedResult();
        }

        private Node RunDfs(Node node)
        {
            if (visitedNodes.Contains(node)) return null;

            visitedNodes.Add(node);

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

                    ////Add node to visited
                    //visitedNodes.Add(childNode);

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
                VisitedNodes = visitedNodesNumber,
                MaxDepth = depth,
                Duration = Math.Round(stopwatch.Elapsed.TotalMilliseconds, 3)
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
                Duration = Math.Round(stopwatch.Elapsed.TotalMilliseconds, 3)
            };

            return result;
        }

        #endregion
    }
}