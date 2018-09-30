//-----------------------------------------------------------------------
// <copyright file="BoardSelectTestCaseData.cs" company="Oskari Leppäaho">
//      Copyright (c) Oskari Leppäaho. All Rights Reserved.
// </copyright>
//-----------------------------------------------------------------------
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BoardSelectTestCaseData
{
    public static IEnumerable TestCases
    {
        get
        {
            return cases.Select(x => new TestCaseData(x).SetName(x.Name));
        }
    }

    private static List<BoardSelectTestCase> cases = new List<BoardSelectTestCase>
    {
        new BoardSelectTestCase(
            name:
                "Removing the leftmost column",

            layout:
                "10000\n" +
                "10000\n" +
                "10000\n" +
                "10000\n" +
                "10000",

            selected:
                ".....\n" +
                "X....\n" +
                ".....\n" +
                ".....\n" +
                ".....",

            expectedRemovals:
                "X....\n" +
                "X....\n" +
                "X....\n" +
                "X....\n" +
                "X....",

            expectedMovesFrom:
                ".....\n" +
                ".....\n" +
                ".....\n" +
                ".....\n" +
                ".....",

            expectedMovesTo:
                ".....\n" +
                ".....\n" +
                ".....\n" +
                ".....\n" +
                ".....",

            expectedAdds:
                "X....\n" +
                "X....\n" +
                "X....\n" +
                "X....\n" +
                "X...."),

        new BoardSelectTestCase(
            name:
                "Removing the rightmost column",

            layout:
                "00001\n" +
                "00001\n" +
                "00001\n" +
                "00001\n" +
                "00001",

            selected:
                ".....\n" +
                ".....\n" +
                "....X\n" +
                ".....\n" +
                ".....",

            expectedRemovals:
                "....X\n" +
                "....X\n" +
                "....X\n" +
                "....X\n" +
                "....X",

            expectedMovesFrom:
                ".....\n" +
                ".....\n" +
                ".....\n" +
                ".....\n" +
                ".....",

            expectedMovesTo:
                ".....\n" +
                ".....\n" +
                ".....\n" +
                ".....\n" +
                ".....",

            expectedAdds:
                "....X\n" +
                "....X\n" +
                "....X\n" +
                "....X\n" +
                "....X"),

        new BoardSelectTestCase(
            name:
                "Removing the top row",

            layout:
                "11111\n" +
                "00000\n" +
                "00000\n" +
                "00000\n" +
                "00000",

            selected:
                "..X..\n" +
                ".....\n" +
                ".....\n" +
                ".....\n" +
                ".....",

            expectedRemovals:
                "XXXXX\n" +
                ".....\n" +
                ".....\n" +
                ".....\n" +
                ".....",

            expectedMovesFrom:
                ".....\n" +
                ".....\n" +
                ".....\n" +
                ".....\n" +
                ".....",

            expectedMovesTo:
                ".....\n" +
                ".....\n" +
                ".....\n" +
                ".....\n" +
                ".....",

            expectedAdds:
                "XXXXX\n" +
                ".....\n" +
                ".....\n" +
                ".....\n" +
                "....."),

        new BoardSelectTestCase(
            name:
                "Removing the bottom row",

            layout:
                "00000\n" +
                "00000\n" +
                "00000\n" +
                "00000\n" +
                "11111",

            selected:
                ".....\n" +
                ".....\n" +
                ".....\n" +
                ".....\n" +
                "X....",

            expectedRemovals:
                ".....\n" +
                ".....\n" +
                ".....\n" +
                ".....\n" +
                "XXXXX",

            // Skip, not enough single digit numbers
            expectedMovesFrom:
                "",

            expectedMovesTo:
                "",

            expectedAdds:
                "XXXXX\n" +
                ".....\n" +
                ".....\n" +
                ".....\n" +
                "....."),

        new BoardSelectTestCase(
            name: "An existing piece getting moved",

            layout:
                "22222\n" +
                "20102\n" +
                "20212\n" +
                "20212\n" +
                "22222",

            selected:
                ".....\n" +
                "..X..\n" +
                ".....\n" +
                ".....\n" +
                ".....",

            expectedRemovals:
                ".....\n" +
                "..X..\n" +
                ".....\n" +
                ".....\n" +
                ".....",

            expectedMovesFrom:
                "..0..\n" +
                ".....\n" +
                ".....\n" +
                ".....\n" +
                ".....",

            expectedMovesTo:
                ".....\n" +
                "..0..\n" +
                ".....\n" +
                ".....\n" +
                ".....",

            expectedAdds:
                "..X..\n" +
                ".....\n" +
                ".....\n" +
                ".....\n" +
                "....."),

            new BoardSelectTestCase(
            name: "An existing piece getting moved multiple spaces",

            layout:
                "22222\n" +
                "20102\n" +
                "20102\n" +
                "20102\n" +
                "22222",

            selected:
                ".....\n" +
                ".....\n" +
                "..X..\n" +
                ".....\n" +
                ".....",

            expectedRemovals:
                ".....\n" +
                "..X..\n" +
                "..X..\n" +
                "..X..\n" +
                ".....",

            expectedMovesFrom:
                "..0..\n" +
                ".....\n" +
                ".....\n" +
                ".....\n" +
                ".....",

            expectedMovesTo:
                ".....\n" +
                ".....\n" +
                ".....\n" +
                "..0..\n" +
                ".....",

            expectedAdds:
                "..X..\n" +
                "..X..\n" +
                "..X..\n" +
                ".....\n" +
                "....."),

            new BoardSelectTestCase(
            name: "Removed pieces reaching to mutiple rows and columns",

            layout:
                "00000\n" +
                "00000\n" +
                "00200\n" +
                "02200\n" +
                "22000",

            selected:
                ".....\n" +
                ".....\n" +
                ".....\n" +
                "..X..\n" +
                ".....",

            expectedRemovals:
                ".....\n" +
                ".....\n" +
                "..X..\n" +
                ".XX..\n" +
                "XX...",

            expectedMovesFrom:
                "047..\n" +
                "158..\n" +
                "26...\n" +
                "3....\n" +
                ".....",

            expectedMovesTo:
                ".....\n" +
                "0....\n" +
                "147..\n" +
                "258..\n" +
                "36...",

            expectedAdds:
                "XXX..\n" +
                ".XX..\n" +
                ".....\n" +
                ".....\n" +
                "....."),

            new BoardSelectTestCase(
            name: "Remaining pieces between removed pieces horizontally",

            layout:
                "00000\n" +
                "02200\n" +
                "00200\n" +
                "02200\n" +
                "00000",

            selected:
                ".....\n" +
                ".....\n" +
                ".....\n" +
                "..X..\n" +
                ".....",

            expectedRemovals:
                ".....\n" +
                ".XX..\n" +
                "..X..\n" +
                ".XX..\n" +
                ".....",

            expectedMovesFrom:
                ".02..\n" +
                ".....\n" +
                ".1...\n" +
                ".....\n" +
                ".....",

            expectedMovesTo:
                ".....\n" +
                ".....\n" +
                ".0...\n" +
                ".12..\n" +
                ".....",

            expectedAdds:
                ".XX..\n" +
                ".XX..\n" +
                "..X..\n" +
                ".....\n" +
                ".....")
    };
}
