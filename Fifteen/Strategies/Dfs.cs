using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Fifteen.Base;

namespace Fifteen.Strategies
{
    public class Dfs
    {
        #region Fields

        private const int MaxRecursionDepth = 20;

        private Node entryNode;
        private HashSet<Node> visitedNodes;
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
            visitedNodes = new HashSet<Node>();
        }

        #endregion

        #region Methods

        public Result ProcessPuzzle()
        {
            stopwatch = Stopwatch.StartNew();

            //We enqueue our first given node
            stack.Push(entryNode);

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