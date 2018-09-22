using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BoardTests {

    [Test]
    public void ToString_result_remains_unchanged_after_ToString_FromString_chain() {
        var b = new Board(FiveBySix());
        var s = b.ToString();
        b.FromString(s);
        Assert.That(s == b.ToString());
    }

    [TestCase(0, 0, ExpectedResult = 1)]
    [TestCase(1, 0, ExpectedResult = 2)]
    [TestCase(2, 0, ExpectedResult = 3)]
    [TestCase(3, 0, ExpectedResult = 4)]
    [TestCase(4, 0, ExpectedResult = 5)]
    public int At_test_top_row(int x, int y)
    {
        return GenerateBoardAndTakePieceAt(
            "12345\n" +
            "00000\n" +
            "00000\n" +
            "00000\n" +
            "00000",
            x, y);
    }

    [TestCase(0, 4, ExpectedResult = 1)]
    [TestCase(1, 4, ExpectedResult = 2)]
    [TestCase(2, 4, ExpectedResult = 3)]
    [TestCase(3, 4, ExpectedResult = 4)]
    [TestCase(4, 4, ExpectedResult = 5)]
    public int At_test_bottom_row(int x, int y)
    {
        return GenerateBoardAndTakePieceAt(
            "00000\n" +
            "00000\n" +
            "00000\n" +
            "00000\n" +
            "12345",
            x, y);
    }

    [TestCase(0, 0, ExpectedResult = 1)]
    [TestCase(0, 1, ExpectedResult = 2)]
    [TestCase(0, 2, ExpectedResult = 3)]
    [TestCase(0, 3, ExpectedResult = 4)]
    [TestCase(0, 4, ExpectedResult = 5)]
    public int At_test_left_column(int x, int y)
    {
        return GenerateBoardAndTakePieceAt(
            "10000\n" +
            "20000\n" +
            "30000\n" +
            "40000\n" +
            "50000",
            x, y);
    }

    [TestCase(4, 0, ExpectedResult = 1)]
    [TestCase(4, 1, ExpectedResult = 2)]
    [TestCase(4, 2, ExpectedResult = 3)]
    [TestCase(4, 3, ExpectedResult = 4)]
    [TestCase(4, 4, ExpectedResult = 5)]
    public int At_test_right_column(int x, int y)
    {
        return GenerateBoardAndTakePieceAt(
            "00001\n" +
            "00002\n" +
            "00003\n" +
            "00004\n" +
            "00005",
            x, y);
    }

    [TestCase(
        "10000\n" +
        "10000\n" +
        "10000\n" +
        "10000\n" +
        "10000",
        0, 0,

        "X....\n" +
        "X....\n" +
        "X....\n" +
        "X....\n" +
        "X....")]
    [TestCase(
        "11111\n" +
        "00100\n" +
        "00010\n" +
        "00110\n" +
        "00000",
        0, 0,

        "XXXXX\n" +
        "..X..\n" +
        ".....\n" +
        ".....\n" +
        ".....")]
    [TestCase(
        "22222\n" +
        "20102\n" +
        "20212\n" +
        "20212\n" +
        "22222",
        0, 0,

        "XXXXX\n" +
        "X...X\n" +
        "X.X.X\n" +
        "X.X.X\n" +
        "XXXXX")]
    [TestCase(
        "22222\n" +
        "20102\n" +
        "20212\n" +
        "20212\n" +
        "22222",
        2, 1,

        ".....\n" +
        "..X..\n" +
        ".....\n" +
        ".....\n" +
        ".....")]
    public void ConnectedPieces_test(string layout, int x, int y, string expectedS)
    {
        var b = new Board(FiveByFive(), layout);
        List<Vector2Int> actual = b.ConnectedPiecesCoords(x, y);
        List<Vector2Int> expected = GetXCoorinates(expectedS);
        Assert.That(actual, Is.EquivalentTo(expected));
    }

    [TestCase(
        "X....\n" +
        "X....\n" +
        "X....\n" +
        "X....\n" +
        "X....",

        "X....\n" +
        "X....\n" +
        "X....\n" +
        "X....\n" +
        "X....")]
    [TestCase(
        "XXXXX\n" +
        "..X..\n" +
        ".....\n" +
        ".....\n" +
        ".....",

        ".....\n" +
        ".....\n" +
        ".....\n" +
        "..X..\n" +
        "XXXXX")]
    [TestCase(
        "XXXXX\n" +
        "X...X\n" +
        "X.X.X\n" +
        "X.X.X\n" +
        "XXXXX",

        "X...X\n" +
        "X.X.X\n" +
        "X.X.X\n" +
        "XXXXX\n" +
        "XXXXX")]
    [TestCase(
        ".....\n" +
        "..X..\n" +
        ".....\n" +
        ".....\n" +
        ".....",

        ".....\n" +
        ".....\n" +
        ".....\n" +
        ".....\n" +
        "..X..")]
    public void GenerateReplacementPieces(string removedS, string expectedS)
    {
        var b = new Board(FiveByFive());
        var removed = GetXCoorinates(removedS);
        Dictionary<Vector2Int, int> replacement = b.GenerateReplacementPieces(removed);

        // Adjust expected coordinates to match the replacement coordinates that are on top of the board
        IEnumerable<Vector2Int> expectedKeys = GetXCoorinates(expectedS).Select(x =>
        {
            x.y -= 5;
            return x;
        });
        Assert.That(replacement.Keys, Is.EquivalentTo(expectedKeys));
    }

    private List<Vector2Int> GetXCoorinates(string s)
    {
        var result = new List<Vector2Int>();
        string[] lines = s.Split('\n');             
        for (int y = 0; y < lines.Length; y++)
        {            
            for (int x = 0; x < lines[y].Length; x++)
            {
                if (lines[y][x] == 'X')
                {
                    result.Add(new Vector2Int(x, y));
                }
            }
        }
        return result;
    }

    private static int GenerateBoardAndTakePieceAt(string layout, int x, int y)
    {        
        var b = new Board(FiveByFive(), layout);
        return b.At(x, y);
    }

    private static Configuration FiveBySix()
    {
        return new Configuration(5, 6, 3);        
    }

    private static Configuration FiveByFive()
    {
        return new Configuration(5, 5, 3);
    }
}
