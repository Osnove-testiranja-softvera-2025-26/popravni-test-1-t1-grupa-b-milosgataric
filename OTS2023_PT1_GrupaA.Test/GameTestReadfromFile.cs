using NUnit.Framework;
using OTS2026_PT1_GrupaA.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTS2026_PT1_GrupaA.Test
{
    public class GameTestDataFromFile
    {
        // Format linije: X Y yes|no
        public static IEnumerable Get_ValidatePosition_OKInput_SuccesfulValidation_TestData(string filename)
        {
            string path = $@"{AppDomain.CurrentDomain.BaseDirectory}\{filename}";
            string[] lines = File.ReadAllLines(path);

            List<TestCaseData> testCasesData = new List<TestCaseData>();
            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;
                string[] values = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                int x = Int32.Parse(values[0]);
                int y = Int32.Parse(values[1]);
                bool expectedResult = values[2] == "yes";
                testCasesData.Add(new TestCaseData(x, y, expectedResult));
            }
            return testCasesData;
        }

        // Format linije: fish bait hasBoat(yes|no) score(bad|average|good)
        public static IEnumerable Get_CalculateIncome_OKInput_SuccesfulCalculation_TestData(string filename)
        {
            string path = $@"{AppDomain.CurrentDomain.BaseDirectory}\{filename}";
            string[] lines = File.ReadAllLines(path);

            List<TestCaseData> testCasesData = new List<TestCaseData>();
            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;
                string[] values = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                int fish = Int32.Parse(values[0]);
                int bait = Int32.Parse(values[1]);
                bool hasBoat = values[2] == "yes";
                Game.Score expectedScore = GetScoreFromString(values[3]);
                testCasesData.Add(new TestCaseData(fish, bait, hasBoat, expectedScore));
            }
            return testCasesData;
        }

        private static Game.Score GetScoreFromString(string score)
        {
            if (score.ToLower().Equals("bad"))
                return Game.Score.Bad;
            if (score.ToLower().Equals("average"))
                return Game.Score.Average;
            return Game.Score.Good;
        }
    }
}