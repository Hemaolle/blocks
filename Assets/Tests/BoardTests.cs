//-----------------------------------------------------------------------
// <copyright file="BoardTests.cs" company="Oskari Leppäaho">
//      Copyright (c) Oskari Leppäaho. All Rights Reserved.
// </copyright>
//-----------------------------------------------------------------------
using NUnit.Framework;

[TestFixture]
public class BoardTests
{
    public static Configuration FiveBySix()
    {
        return new Configuration(5, 6, 6);
    }

    public static Configuration FiveByFive()
    {
        return new Configuration(5, 5, 6);
    }
    [Test]
    public static void ToString_result_remains_unchanged_after_ToString_FromString_chain()
    {
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
    public static int At_test_top_row(int x, int y)
    {
        return GenerateBoardAndReturnPieceAt(
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
    public static int At_test_bottom_row(int x, int y)
    {
        return GenerateBoardAndReturnPieceAt(
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
    public static int At_test_left_column(int x, int y)
    {
        return GenerateBoardAndReturnPieceAt(
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
    public static int At_test_right_column(int x, int y)
    {
        return GenerateBoardAndReturnPieceAt(
            "00001\n" +
            "00002\n" +
            "00003\n" +
            "00004\n" +
            "00005",
            x, y);
    }

    private static int GenerateBoardAndReturnPieceAt(string layout, int x, int y)
    {        
        var b = new Board(FiveByFive(), layout);
        return b.At(x, y);
    }
}

