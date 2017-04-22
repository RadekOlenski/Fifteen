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
        public static string SolutionFileName;
        public static string AdditionalDataFileName;

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

            List<object> entryFileArgs = ReadEntryFile();

            int height = Convert.ToInt32(entryFileArgs[0]);
            int width = Convert.ToInt32(entryFileArgs[1]);

            Puzzle entryPuzzle = new Puzzle(height, width, (List<int>) entryFileArgs[2]);

            //Bfs bfs = new Bfs(new Node(entryPuzzle), SearchOrder);
            //Result result = bfs.ProcessPuzzle();

            //Dfs dfs = new Dfs(new Node(entryPuzzle), SearchOrder);
            //Result result = dfs.ProcessPuzzle();

            ChosenHeuristic = "manh";
            Astr astr = new Astr(new Node(entryPuzzle),ChosenHeuristic, new []{"R", "L", "U", "D"});
            Result astrResult = astr.ProcessPuzzle();
            Console.ReadLine();
        }

        #region Methods

        public static void ProcessConsoleInput()
        {
            Console.WriteLine("Choose strategy [bfs, dfs, astr]: ");
            ChosenStrategy = Console.ReadLine();
            if (ChosenStrategy == "dfs" || ChosenStrategy == "bfs")
            {
                Console.WriteLine("Choose search order [R L U D]: ");
                string order = Console.ReadLine();
                if (order != null) SearchOrder = order.Split();
            }
            else
            {
                Console.WriteLine("Choose heuristic [hamm, manh]: ");
                ChosenHeuristic = Console.ReadLine();
            }
            Console.WriteLine("Choose entry puzzle state file: ");
            PuzzleEntryFileName = Console.ReadLine();
            Console.WriteLine("Choose solution file: ");
            SolutionFileName = Console.ReadLine();
            Console.WriteLine("Choose additional data file: ");
            AdditionalDataFileName = Console.ReadLine();
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
                //Hardocded entry file
                //using (StreamReader sr = new StreamReader($"Puzzles/{PuzzleEntryFileName}"))
                using (StreamReader sr = new StreamReader("Puzzles/4x4_01_0001.txt"))
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

        #endregion
    }
}