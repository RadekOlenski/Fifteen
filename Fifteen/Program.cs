﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fifteen.Base;
using Fifteen.Puzzles;
using Fifteen.Strategies;

namespace Fifteen
{
    class Program
    {
        #region Fields

        public static string ChosenStrategy = "none";
        public static string[] SearchOrder;
        public static string ChosenHeuristic;

        public static string PuzzleEntryFileName;
        public static string PuzzleOutputFileName;
        public static string PuzzleOutputFileNameSol;
        public static string PuzzleOutputFileNameStats;

        public static DataTable ResultTable;
        public static bool isFileNameAuto = true;


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
                ProcessArgsInput(args);
            }
            //Process all puzzles in program's folder using auto names and specific settings
            //and save data to excel file with given name
            if (isFileNameAuto)
            {
                string[] filePaths = Directory.GetFiles(@"Puzzles/", "*.txt", SearchOption.TopDirectoryOnly);
                InitializeDataTable();
                foreach (var filePath in filePaths)
                {
                    HandleSpecificPuzzle(filePath);
                }
                CreateFile(ResultTable, $"{ChosenStrategy}_{TransformSearchOrderToOutput()}_{ChosenHeuristic}_results.xlsx", "Template.xlsx");
            }
            //Process only one specific puzzle with three given filenames
            else
            {
                HandleSpecificPuzzle(PuzzleEntryFileName);
            }
            Console.WriteLine($"Done!");
            Console.ReadLine();
        }

        #region Methods
        private static void HandleSpecificPuzzle(string filePath)
        {
            PuzzleEntryFileName = filePath;
            Console.WriteLine($"Starting {PuzzleEntryFileName}...");
            List<object> entryFileArgs = ReadEntryFile();
            int height = Convert.ToInt32(entryFileArgs[0]);
            int width = Convert.ToInt32(entryFileArgs[1]);
            Puzzle entryPuzzle = new Puzzle(height, width, (List<int>)entryFileArgs[2]);
            Result result = ProcessPuzzle(entryPuzzle);
            PreparePuzzleOutputFileName();
            SaveSolution(result);
            SaveStats(result);
            if (isFileNameAuto)
            {
                AddResultToDataTable(result);
            }
            Console.WriteLine($"{PuzzleEntryFileName} done!");
        }

        private static void ProcessArgsInput(string[] args)
        {
            int txtCounter = 0;
            foreach (string arg in args)
            {
                if (arg == "-astr" || arg == "-bfs" || arg == "-dfs")
                {
                    ChosenStrategy = arg;
                }
                else if (arg == "-hamm" || arg == "-manh")
                {
                    ChosenHeuristic = arg;
                }
                else if (arg.Contains("-") && arg.Contains("R") && arg.Contains("D") && arg.Contains("U") && arg.Contains("L"))
                {
                    SearchOrder = new string[] { arg.Substring(1, 1), arg.Substring(2, 1), arg.Substring(3, 1), arg.Substring(4, 1) };
                }
                else if (arg.Contains(".txt") && txtCounter == 0)
                {
                    isFileNameAuto = false;
                    PuzzleEntryFileName = $"Puzzles/{arg}";
                    txtCounter++;
                }
                else if (arg.Contains(".txt") && txtCounter == 1)
                {
                    isFileNameAuto = false;
                    PuzzleOutputFileNameSol = arg;
                    txtCounter++;
                }
                else if (arg.Contains(".txt") && txtCounter == 2)
                {
                    isFileNameAuto = false;
                    PuzzleOutputFileNameStats = arg;
                    txtCounter++;
                }
            }
            if (txtCounter > 0 && txtCounter < 3)
            {
                throw new Exception("Some output filenames are missing!");
            }
        }

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
            Console.WriteLine("Manual filenames? [Y/N] (required 3 filenames with extensions)");
            if (Console.ReadLine().ToLower() == "y")
            {
                isFileNameAuto = false;
                Console.WriteLine("Choose entry puzzle state file: ");
                PuzzleEntryFileName = $"Puzzles/{Console.ReadLine()}";
                Console.WriteLine("Choose output puzzle solution file: ");
                PuzzleOutputFileNameSol = Console.ReadLine();
                Console.WriteLine("Choose output puzzle stats file: ");
                PuzzleOutputFileNameStats = Console.ReadLine();
            }
        }

        private static List<object> ReadEntryFile()
        {
            List<object> args = new List<object>();

            try
            {
                // Open the text file using a stream reader.
                using (StreamReader sr = new StreamReader($"{PuzzleEntryFileName}"))
                {
                    // Read the stream to a string, and write the string to the console.
                    String line = sr.ReadLine();
                    if (line != null)
                    {
                        string[] sizes = line.Split();
                        args.AddRange(sizes);
                    }
                    String puzzleValues = sr.ReadToEnd();
                    List<string> strings = puzzleValues.Split(new[] { "\r\n", "\n", " " }, StringSplitOptions.None).ToList();
                    strings.Remove(strings.Last());
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
            if (isFileNameAuto)
            {
                PuzzleOutputFileNameSol = $"{PuzzleOutputFileName}_sol.txt";
            }
            using (StreamWriter file = new StreamWriter($"Solutions/{PuzzleOutputFileNameSol}"))
            {
                file.WriteLine($"{result.SolutionSteps}");
                file.WriteLine($"{result.Directions}");
            }
        }

        private static void SaveStats(Result result)
        {
            if (isFileNameAuto)
            {
                PuzzleOutputFileNameStats = $"{PuzzleOutputFileName}_stats.txt";
            }
            using (StreamWriter file = new StreamWriter($"Stats/{PuzzleOutputFileNameStats}"))
            {
                file.WriteLine($"{result.SolutionSteps}");
                file.WriteLine($"{result.VisitedNodes}");
                file.WriteLine($"{result.ProcessedNodes}");
                file.WriteLine($"{result.MaxDepth}");
                file.WriteLine($"{result.Duration}");
            }
        }

        private static void PreparePuzzleOutputFileName()
        {
            if (!isFileNameAuto) return;
            PuzzleOutputFileName = PuzzleEntryFileName.Substring(7, 13);
            PuzzleOutputFileName += $"_{ChosenStrategy}";
            if (ChosenHeuristic == "astr")
            {
                PuzzleOutputFileName += $"_{ChosenHeuristic}";
            }
            else
            {
                PuzzleOutputFileName += $"_{TransformSearchOrderToOutput()}";
            }
        }

        public static void CreateFile(DataTable reportdata, string fileName, string excelTemplate)
        {
            var data = new ExcelData
            {
                ReportData = reportdata
            };

            new ExcelWriter(excelTemplate).Export(data, fileName);
        }

        private static void InitializeDataTable()
        {
            ResultTable = new DataTable();
            ResultTable.Columns.Add("PuzzleDepth");
            ResultTable.Columns.Add("ID");
            ResultTable.Columns.Add("Strategy");
            ResultTable.Columns.Add("Order");
            ResultTable.Columns.Add("Heuristic");
            ResultTable.Columns.Add("Steps");
            ResultTable.Columns.Add("Directions");
            ResultTable.Columns.Add("VisitedNodes");
            ResultTable.Columns.Add("ProcessedNodes");
            ResultTable.Columns.Add("MaxDepth");
            ResultTable.Columns.Add("Duration");
        }

        private static void AddResultToDataTable(Result result)
        {
            DataRow row = ResultTable.NewRow();
            row["PuzzleDepth"] = PuzzleOutputFileName.Substring(5, 2);
            row["ID"] = PuzzleOutputFileName.Substring(8, 5);
            row["Strategy"] = ChosenStrategy;
            row["Order"] = TransformSearchOrderToOutput();
            row["Heuristic"] = ChosenHeuristic;
            row["Steps"] = result.SolutionSteps;
            row["Directions"] = result.Directions;
            row["VisitedNodes"] = result.VisitedNodes;
            row["ProcessedNodes"] = result.ProcessedNodes;
            row["MaxDepth"] = result.MaxDepth;
            row["Duration"] = result.Duration;
            ResultTable.Rows.Add(row);
        }

        private static string TransformSearchOrderToOutput()
        {
            string word = String.Empty;
            foreach (var s in SearchOrder)
            {
                word += s;
            }
            return word.ToLower();
        }
        #endregion
    }
}