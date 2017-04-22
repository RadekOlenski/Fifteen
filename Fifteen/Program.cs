using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fifteen.Base;
using Fifteen.Strategies;

namespace Fifteen
{
    class Program
    {
        #region Fields

        public static string ChosenStrategy;
        public static string[] SearchOrder;
        public static string ChosenHeuristic;

        public static string PuzzleEntryFileName;

        #endregion

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                //Ask in console for arguments
                ProcessConsoleInput();
            }
            else
            {
                //Read arguments from given args
                ProcessArgsInput();
            }
            PuzzleEntryFileName = "4x4_01_0001";
            ChosenHeuristic = "manh";
            List<object> entryFileArgs = ReadEntryFile();

            int height = Convert.ToInt32(entryFileArgs[0]);
            int width = Convert.ToInt32(entryFileArgs[1]);

            Puzzle entryPuzzle = new Puzzle(height, width, (List<int>) entryFileArgs[2]);
            
            Result result = ProcessPuzzle(entryPuzzle);
            SaveSolution(result);
            SaveStats(result);
            Console.ReadLine();
        }

        #region Methods

        public static void ProcessConsoleInput()
        {
            Console.WriteLine("Choose strategy [bfs, dfs, astr]: ");
            ChosenStrategy = Console.ReadLine();
            Console.WriteLine("Choose search order [R L U D]: ");
            string order = Console.ReadLine();
            if (order != null) SearchOrder = order.Split();
            if (ChosenStrategy == "astr")
            {
                Console.WriteLine("Choose heuristic [hamm, manh]: ");
                ChosenHeuristic = Console.ReadLine();
            }
            Console.WriteLine("Choose entry puzzle state file: ");
            PuzzleEntryFileName = Console.ReadLine();
        }

        public static void ProcessArgsInput()
        {
        }

        private static List<object> ReadEntryFile()
        {
            List<object> args = new List<object>();

            try
            {
                // Open the text file using a stream reader.
                using (StreamReader sr = new StreamReader($"Puzzles/{PuzzleEntryFileName}.txt"))
                {
                    // Read the stream to a string, and write the string to the console.
                    String line = sr.ReadLine();
                    if (line != null)
                    {
                        string[] sizes = line.Split();
                        args.AddRange(sizes);
                    }
                    String puzzleValues = sr.ReadToEnd();
                    string[] strings = puzzleValues.Split(new[] {"\r\n", "\n", " "}, StringSplitOptions.None);
                    List<int> values = strings.Select(int.Parse).ToList();
                    args.Add(values);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }

            return args;
        }

        private static Result ProcessPuzzle(Puzzle entryPuzzle)
        {
            switch (ChosenStrategy)
            {
                case "bfs":
                    return new Bfs(new Node(entryPuzzle), SearchOrder).ProcessPuzzle();
                case "dfs":
                    return new Dfs(new Node(entryPuzzle), SearchOrder).ProcessPuzzle();
                default:
                    return new Astr(new Node(entryPuzzle), ChosenHeuristic, SearchOrder).ProcessPuzzle();
            }
        }

        private static void SaveSolution(Result result)
        {
            using (StreamWriter file = new StreamWriter($"Solutions/{PuzzleEntryFileName}_sol.txt"))
            {
                file.WriteLine($"{result.SolutionSteps}");
                file.WriteLine($"{result.Directions}");
            }
        }

        private static void SaveStats(Result result)
        {
            using (StreamWriter file = new StreamWriter($"Stats/{PuzzleEntryFileName}_stats.txt"))
            {
                file.WriteLine($"{result.SolutionSteps}");
                file.WriteLine($"{result.VisitedNodes}");
                file.WriteLine($"{result.ProcessedNodes}");
                file.WriteLine($"{result.MaxDepth}");
                file.WriteLine($"{result.Duration}");
            }
        }

        #endregion
    }
}