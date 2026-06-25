using NUnit.Framework;
using OTS2026_PT1_GrupaA.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTS2026_PT1_GrupaB.Test
{
    public class GameTestCaseData
    {
        // (startX, startY, smer, ocekivaniX, ocekivaniY)
        public static IEnumerable MovePlayer_SuccessfulMove_PositionChanged_TestData
        {
            get
            {
                yield return new TestCaseData(5, 5, Move.Up, 5, 4);
                yield return new TestCaseData(5, 5, Move.Down, 5, 6);
                yield return new TestCaseData(5, 5, Move.Left, 4, 5);
                yield return new TestCaseData(5, 5, Move.Right, 6, 5);
                yield return new TestCaseData(15, 15, Move.Up, 15, 14);
                yield return new TestCaseData(15, 15, Move.Down, 15, 16);
                yield return new TestCaseData(15, 15, Move.Left, 14, 15);
                yield return new TestCaseData(15, 15, Move.Right, 16, 15);
            }
        }
    }
}