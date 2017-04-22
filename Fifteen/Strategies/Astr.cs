using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fifteen.Base;
using Priority_Queue;

namespace Fifteen
{
    public class Astr
    {
        private string chosenHeuristic;
        private Node entryNode;
        private SimplePriorityQueue<Node> nodesQueue;
        private List<Node> visitedNodes;
        private string[] searchOrder;

        //Return values
        private int processedNodes;

        private int depth;
        private Stopwatch stopwatch;

        public Astr(Node node, string chosenHeuristic, string[] searchOrder)
        {
            this.entryNode = node;
            this.chosenHeuristic = chosenHeuristic;
            this.searchOrder = searchOrder;
            nodesQueue = new SimplePriorityQueue<Node>();
            visitedNodes = new List<Node>();
        }

        public Result ProcessPuzzle()
        {
           return chosenHeuristic.Equals("hamm") ? ProcessHamming() : ProcessManhattan();
        }

        public Result ProcessManhattan()
        {
            stopwatch = Stopwatch.StartNew();

            //We enqueue our first given node
            nodesQueue.Enqueue(entryNode, GetManhattanCost(entryNode));

            //Check if current Puzzle State is desired puzzle state
            if (GetManhattanCost(entryNode) == 0)
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
                foreach (string direction in searchOrder)
                {
                    //Check if we can move zero in desired direction 
                    if (!entryNode.PuzzleState.CanMoveInDirection(direction)) continue;

                    //If we can move we create new node and add it to queue
                    var tempNode = new Node(entryNode.PuzzleState.MoveInDirection(direction), entryNode.Depth + 1,
                        entryNode.PreviousDirections + direction);

                    if (tempNode.Depth > depth)
                    {
                        depth = tempNode.Depth;
                    }

                    if (visitedNodes.Contains(tempNode)) continue;

                    if (GetManhattanCost(tempNode) == 0)
                    {
                        stopwatch.Stop();
                        return ProcessResult(tempNode);
                    }

                    //If new node is not in queue and is not visited we add it to new queue
                    if (!nodesQueue.Contains(tempNode))
                    {
                        nodesQueue.Enqueue(tempNode, GetManhattanCost(tempNode));
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
        public Result ProcessHamming()
        {
            stopwatch = Stopwatch.StartNew();

            //We enqueue our first given node
            nodesQueue.Enqueue(entryNode, GetHammingCost(entryNode));

            //Check if current Puzzle State is desired puzzle state
            if (GetHammingCost(entryNode) == 0)
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
                foreach (string direction in searchOrder)
                {
                    //Check if we can move zero in desired direction 
                    if (!entryNode.PuzzleState.CanMoveInDirection(direction)) continue;

                    //If we can move we create new node and add it to queue
                    var tempNode = new Node(entryNode.PuzzleState.MoveInDirection(direction), entryNode.Depth + 1,
                        entryNode.PreviousDirections + direction);

                    if (tempNode.Depth > depth)
                    {
                        depth = tempNode.Depth;
                    }

                    if (visitedNodes.Contains(tempNode)) continue;

                    if (GetHammingCost(tempNode) == 0)
                    {
                        stopwatch.Stop();
                        return ProcessResult(tempNode);
                    }

                    //If new node is not in queue and is not visited we add it to new queue
                    if (!nodesQueue.Contains(tempNode))
                    {
                        nodesQueue.Enqueue(tempNode, GetHammingCost(tempNode));
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

        public int GetHammingCost(Node node)
        {
            int cost = 0;
            var tab = node.PuzzleState.PuzzleCurrentState;
            for (int i = 0; i < tab.GetLength(0); i++)
            {
                for (int j = 0; j < tab.GetLength(1); j++)
                {
                    if (tab[i, j] != (i * tab.GetLength(1)) + (j + 1))
                    {
                        cost++;
                    }
                }
            }
            //checking last element
            //subtracting last added cost from loop which is always false
            cost--;
            if (tab[tab.GetLength(0) - 1, tab.GetLength(1) - 1] != 0)
            {
                cost++;
            }
            return cost;
        }

        public int GetManhattanCost(Node node)
        {
            int cost = 0, tempI = 0, tempJ = 0;
            var tab = node.PuzzleState.PuzzleCurrentState;
            for (int i = 0; i < tab.GetLength(0); i++)
            {
                for (int j = 0; j < tab.GetLength(1); j++)
                {
                    if (tab[i, j] == 0)
                    {
                        cost += Math.Abs(i - 3) + Math.Abs(j - 3);
                        continue;
                    }
                    tempI = (tab[i, j] - 1) / tab.GetLength(1);
                    tempJ = (tab[i, j] - 1) % tab.GetLength(1);
                    cost += Math.Abs(i - tempI) + Math.Abs(j - tempJ);
                }
            }
            return cost;
        }

    }
}
