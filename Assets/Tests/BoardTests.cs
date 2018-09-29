using UnityEngine;
using NUnit.Framework;
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
        List<Vector2Int> expected = GetCharCoorinates(expectedS, 'X');
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

        "XXXXX\n" +
        "..X..\n" +
        ".....\n" +
        ".....\n" +
        ".....")]
    [TestCase(
        "XXXXX\n" +
        "X...X\n" +
        "X.X.X\n" +
        "X.X.X\n" +
        "XXXXX",

        "XXXXX\n" +
        "XXXXX\n" +
        "X.X.X\n" +
        "X.X.X\n" +
        "X...X")]
    [TestCase(
        ".....\n" +
        "..X..\n" +
        ".....\n" +
        ".....\n" +
        ".....",

        "..X..\n" +
        ".....\n" +
        ".....\n" +
        ".....\n" +
        ".....")]
    public void GenerateReplacementPieces(string removedS, string expectedS)
    {
        var b = new Board(FiveByFive());
        var removed = GetCharCoorinates(removedS, 'X');
        Dictionary<Vector2Int, int> replacement = b.GenerateReplacementPieces(removed);
        IEnumerable<Vector2Int> expectedKeys = GetCharCoorinates(expectedS, 'X');
        Assert.That(replacement.Keys, Is.EquivalentTo(expectedKeys));
    }

    [TestCase(
        "10000\n" +
        "10000\n" +
        "10000\n" +
        "10000\n" +
        "10000",

        "X....\n" +
        "X....\n" +
        "X....\n" +
        "X....\n" +
        "X....",

        "3....\n" +
        "3....\n" +
        "3....\n" +
        "3....\n" +
        "3....",

        "30000\n" +
        "30000\n" +
        "30000\n" +
        "30000\n" +
        "30000")]
    [TestCase(
        "11111\n" +
        "00100\n" +
        "00010\n" +
        "00110\n" +
        "00000",

        "XXXXX\n" +
        "..X..\n" +
        ".....\n" +
        ".....\n" +
        ".....",

        "33333\n" +
        "..3..\n" +
        ".....\n" +
        ".....\n" +
        ".....",

        "33333\n" +
        "00300\n" +
        "00010\n" +
        "00110\n" +
        "00000")]
    [TestCase(
        "22222\n" +
        "20102\n" +
        "20212\n" +
        "20212\n" +
        "22222",

        "XXXXX\n" +
        "X...X\n" +
        "X.X.X\n" +
        "X.X.X\n" +
        "XXXXX",

        "33333\n" +
        "33333\n" +
        "3.3.3\n" +
        "3.3.3\n" +
        "3...3",

        "33333\n" +
        "33333\n" +
        "30303\n" +
        "30313\n" +
        "30113")]
    [TestCase(
        "22222\n" +
        "20102\n" +
        "20212\n" +
        "20212\n" +
        "22222",

        ".....\n" +
        "..X..\n" +
        ".....\n" +
        ".....\n" +
        ".....",

        "..3..\n" +
        ".....\n" +
        ".....\n" +
        ".....\n" +
        ".....",

        "22322\n" +
        "20202\n" +
        "20212\n" +
        "20212\n" +
        "22222")]
    public void ReplacePieces(string layout, string removedS, string replacementS, string expected)
    {
        var b = new Board(FiveByFive(), layout);
        // TODO: A separate test for events using the same test cases.
        var addEventArgsReceived = new List<BoardEventArgs>();
        b.SubscribeToAdds((o, e) => addEventArgsReceived.Add(e));
        var removeEventArgsReceived = new List<BoardEventArgs>();
        b.SubscribeToRemoves((o, e) => removeEventArgsReceived.Add(e));
        var removed = GetCharCoorinates(removedS, 'X');
        var replacementAllThrees = GetCharCoorinates(replacementS, '3')
            .ToDictionary(x => x, x => 3);
        b.ReplacePieces(removed, replacementAllThrees);
        Assert.That(b.ToString(), Is.EqualTo(expected));

        // TODO: flip expected and actual
        Assert.That(removed, Is.EquivalentTo(removeEventArgsReceived.Select(x => x.Coordinates)));
        Assert.That(replacementAllThrees.Keys, Is.EquivalentTo(addEventArgsReceived.Select(x => x.Coordinates)));
        Assert.That(addEventArgsReceived.All(x => x.PieceType == 3));
    }

    private List<Vector2Int> GetCharCoorinates(string s, char c)
    {
        var result = new List<Vector2Int>();
        string[] lines = s.Split('\n');             
        for (int y = 0; y < lines.Length; y++)
        {            
            for (int x = 0; x < lines[y].Length; x++)
            {
                if (lines[y][x] == c)
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
        return new Configuration(5, 6, 6);        
    }

    private static Configuration FiveByFive()
    {
        return new Configuration(5, 5, 6);
    }
}
