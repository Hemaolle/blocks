//-----------------------------------------------------------------------
// <copyright file="BoardSelectTests.cs" company="Oskari Leppäaho">
//      Copyright (c) Oskari Leppäaho. All Rights Reserved.
// </copyright>
//-----------------------------------------------------------------------
using UnityEngine;
using NUnit.Framework;
using System.Collections.Generic;
using System;
using System.Linq;

[TestFixture]
public class BoardSelectTests
{
    [Test, TestCaseSource(typeof(BoardSelectTestCaseData), "TestCases")]
    public void SelectPiece_test_add_positions(BoardSelectTestCase c)
    {
        var cfg = BoardTests.FiveByFive();
        var b = new Board(cfg, c.Layout);

        var addEventArgsReceived = new List<PieceAddedEventArgs>();
        b.SubscribeToAdds((o, e) => addEventArgsReceived.Add(e));

        var xsInSelected = GetCharCoorinates(c.Selected, 'X');
        Assert.That(xsInSelected.Count, Is.EqualTo(1));
        var selected = xsInSelected.First();
        var adds = GetCharCoorinates(c.ExpectedAdds, 'X');

        b.SelectPiece(selected);

        Assert.That(
            addEventArgsReceived.Select(x => x.Coordinates),
            Is.EquivalentTo(adds),
            "Received add events not matching expected");
    }

    [Test, TestCaseSource(typeof(BoardSelectTestCaseData), "TestCases")]
    public void SelectPiece_test_add_piece_types(BoardSelectTestCase c)
    {
        var cfg = BoardTests.FiveByFive();
        var b = new Board(cfg, c.Layout);

        var addEventArgsReceived = new List<PieceAddedEventArgs>();
        b.SubscribeToAdds((o, e) => addEventArgsReceived.Add(e));

        var xsInSelected = GetCharCoorinates(c.Selected, 'X');
        Assert.That(xsInSelected.Count, Is.EqualTo(1));
        var selected = xsInSelected.First();
        var adds = GetCharCoorinates(c.ExpectedAdds, 'X');

        b.SelectPiece(selected);

        Assert.That(
            addEventArgsReceived.All(x => x.PieceType >= 0 && x.PieceType < cfg.NumColors),
            "Added piece types not within configured range");
    }

    [Test, TestCaseSource(typeof(BoardSelectTestCaseData), "TestCases")]
    public void SelectPiece_test_removes(BoardSelectTestCase c)
    {
        var cfg = BoardTests.FiveByFive();
        var b = new Board(cfg, c.Layout);

        var removeEventArgsReceived = new List<PieceRemovedEventArgs>();
        b.SubscribeToRemoves((o, e) => removeEventArgsReceived.Add(e));

        var xsInSelected = GetCharCoorinates(c.Selected, 'X');
        Assert.That(xsInSelected.Count, Is.EqualTo(1));
        var selected = xsInSelected.First();
        var removed = GetCharCoorinates(c.ExpectedRemovals, 'X');

        b.SelectPiece(selected);

        Assert.That(
            removeEventArgsReceived.Select(x => x.Coordinates),
            Is.EquivalentTo(removed),
            "Received remove events not matching expected");
    }

    [Test, TestCaseSource(typeof(BoardSelectTestCaseData), "TestCases")]
    public void SelectPiece_test_moves(BoardSelectTestCase c)
    {
        var cfg = BoardTests.FiveByFive();
        var b = new Board(cfg, c.Layout);

        var moveEventArgsReceived = new List<PieceMovedEventArgs>();
        b.SubscibeToMoves((o, e) => moveEventArgsReceived.Add(e));

        var xsInSelected = GetCharCoorinates(c.Selected, 'X');
        Assert.That(xsInSelected.Count, Is.EqualTo(1));
        var selected = xsInSelected.First();
        var moves = GetNumberCoordinates(c.ExpectedMovesFrom, c.ExpectedMovesTo);

        b.SelectPiece(selected);

        // Skip asserting for moves if arguments empty. In some cases there are some many pieces moved that the single
        // digit encoding of the test cases doesn't work.
        if (!string.IsNullOrEmpty(c.ExpectedMovesFrom) && !string.IsNullOrEmpty(c.ExpectedMovesTo))
        {
            Assert.That(moveEventArgsReceived
                .Select(x => new Tuple<Vector2Int, Vector2Int>(x.OldCoordinates, x.NewCoordinates)),
                Is.EquivalentTo(moves),
                "Received move events not matching expected");
        }
    }

    private List<Tuple<Vector2Int, Vector2Int>> GetNumberCoordinates(string s1, string s2)
    {
        var result = new List<Tuple<Vector2Int, Vector2Int>>();
        for (int i = 0; i < 10; i++)
        {
            if (!s1.Contains(i.ToString()[0]))
            {
                break;
            }

            result.Add(
                new Tuple<Vector2Int, Vector2Int>(
                    GetCharCoorinates(s1, i.ToString()[0]).First(),
                    GetCharCoorinates(s2, i.ToString()[0]).First()));
        }

        return result;
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

    private static int GenerateBoardAndReturnPieceAt(string layout, int x, int y)
    {
        var b = new Board(BoardTests.FiveByFive(), layout);
        return b.At(x, y);
    }
}
