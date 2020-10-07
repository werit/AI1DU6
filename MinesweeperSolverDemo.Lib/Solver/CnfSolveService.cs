using System;
using System.Diagnostics;
using System.IO;

namespace MinesweeperSolverDemo.Lib.Solver
{
    public static class CnfSolveService
    {
        public static bool IsCnfSatisfiable()
        {
            var programOutput = RunSat4J();
            return VerifySatisfiability(programOutput);
        }

        private static StreamReader RunSat4J()
        {
            using (var process = new Process())
            {
                process.StartInfo.FileName = @"java";
                process.StartInfo.Arguments = "-jar ../../../sat4J/org.sat4j.core.jar inputForSolver.cnf"; //argument
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process.StartInfo.CreateNoWindow = false; //not diplay a windows
                process.Start();

                process.WaitForExit();
                return process.StandardOutput;
            }
        }

        private static bool VerifySatisfiability(StreamReader input)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            var line = input.ReadLine();
            while (line != null)
            {
                if (line.StartsWith("s"))
                {
                    var lineSplit = line.Split(' ');
                    {
                        return lineSplit[1] == "SATISFIABLE";
                    }
                }

                line = input.ReadLine();
            }

            throw new Exception("Solver could not work on input provided.");
        }
    }
}