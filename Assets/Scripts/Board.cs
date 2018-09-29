﻿using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Board {
    private int[,] pieces;
    private Configuration configuration;
	private event PieceEventEventHandler PieceRemoved;
	private event PieceEventEventHandler PieceAdded;

	public delegate void PieceEventEventHandler(Vector2Int position, int type);

	public int Width { get { return configuration.BoardWidth; } }
    public int Height { get { return configuration.BoardHeight; } }

    public Board(Configuration configuration)
    {
        this.configuration = configuration;
        pieces = new int[configuration.BoardWidth, configuration.BoardHeight];
        FillWithRandomPieces();
    }

    public Board(Configuration configuration, string s)
    {
        InitWith(configuration, s);
    }   

    public Board(Board b)
    {
        InitWith(b.configuration, b.ToString());
    }

    private void InitWith(Configuration configuration, string s)
    {
        this.configuration = configuration;
        pieces = new int[configuration.BoardWidth, configuration.BoardHeight];
        FromString(s);
    }

    private void FillWithRandomPieces()
    {
        for (int y = 0; y < configuration.BoardHeight; y++)
        {
            for (int x = 0; x < configuration.BoardWidth; x++)
            {
                pieces[x, y] = RandomizePiece();
            }
        }
    }

    private int RandomizePiece()
    {
        return UnityEngine.Random.Range(0, configuration.NumColors);
    }

    public int At(int x, int y)
    {
        return pieces[x, y];
    }

    /// <summary>
    /// Line y = 0 is the topmost one.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return Reduce(new StringBuilder(), (sb, x, y, type) =>
        {
            sb.Append(pieces[x, y]);
            bool lastColumn = x == Width - 1;
            bool lastRow = y == Height - 1;
            if (lastColumn && !lastRow)
            {
                sb.Append('\n');
            }
            return sb;
        }).ToString();
    }

    public void FromString(string s)
    {
        string[] lines = s.Split('\n');
        int numLines = lines.Length;
        if (numLines != Height)
        {
            throw new System.ArgumentException("The number of the lines doesn't match the height of the board.");
        }
        for (int y = 0; y < lines.Length; y++)
        {
            if (lines[y].Length != Width)
            {
                throw new System.ArgumentException("The length of a line doesn't match the width of the board.");
            }
            for (int x = 0; x < lines[y].Length; x++)
            {
                // Picked char - '0' as the conversion here just because it would work for more than 10 different
                // types of pieces too.
                pieces[x, y] = (int)(lines[y][x] - '0');
            }
        }
    }

    /// <summary>
    /// Return a list of coordinates for all the pieces that are four-way connected to the given coordinates
    /// </summary>
    public List<Vector2Int> ConnectedPiecesCoords(int x, int y)
    {
        var copy = new Board(this);
        copy.FloodFill(x, y, At(x,y), int.MinValue);
        return copy.FindAll(int.MinValue);
    }

    private List<Vector2Int> FindAll(int value)
    {
        return Reduce(new List<Vector2Int>(), (acc, x, y, type) =>
        {
            if (type == value)
            {
                acc.Add(new Vector2Int(x, y));
            }
            return acc;
        });
    } 

    private T Reduce<T>(T accumulator, Func<T, int, int, int, T> reducer) {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                accumulator = reducer(accumulator, x, y, At(x, y));
            }
        }
        return accumulator;
    }

    // From https://en.wikipedia.org/wiki/Flood_fill
    private void FloodFill(int x, int y, int targetType, int replacementType)
    {
        if (x < 0 || x >= Width)
        {
            return;
        }
        if (y < 0 || y >= Height)
        {
            return;
        }
        if (targetType == replacementType)
        {
            return;
        }
        if (At(x, y) != targetType)
        {
            return;
        }
        pieces[x, y] = replacementType;

        FloodFill(x, y + 1, targetType, replacementType);
        FloodFill(x, y - 1, targetType, replacementType);
        FloodFill(x - 1, y , targetType, replacementType);
        FloodFill(x + 1, y, targetType, replacementType);
        return;
    }

    /// <summary>
    /// Generate replacement pieces.
    /// </summary>
    /// <param name="removed">Coordinates of removed pieces for which to generate replacement pieces</param>
    /// <returns>A dictionary of coordinates and piece types for the new pieces. Keys are coordinates such that the
    /// new pieces are placed on top of the board for dropping on the board.</returns>
    public Dictionary<Vector2Int, int> GenerateReplacementPieces(List<Vector2Int> removed)
    {
        var result = new Dictionary<Vector2Int, int>();
        int[] removedPiecesPerColumn = RemovedPiecesPerColumn(removed);
        for (int column = 0; column < Width; column++)
        {
            for (int i = 0; i < removedPiecesPerColumn[column]; i++)
            {
                result.Add(new Vector2Int(column, -1 - i), RandomizePiece());
            }
        }
        return result;
    }

    private int[] RemovedPiecesPerColumn(List<Vector2Int> removed)
    {
        int[] removedPiecesPerColumn = new int[Width];
        foreach (var r in removed)
        {
            removedPiecesPerColumn[r.x]++;
        }
        return removedPiecesPerColumn;
    }

    public void ReplacePieces(List<Vector2Int> removed, Dictionary<Vector2Int, int> replacement)
    {
        foreach (var r in removed)
        {
            pieces[r.x, r.y] = int.MinValue;
        }

        outer:
        for (int y = 0; y < Height - 1; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                if (At(x, y) > int.MinValue && At(x, y + 1) == int.MinValue)
                {
                    pieces[x, y + 1] = At(x, y);
                    pieces[x, y] = int.MinValue;
                    goto outer;
                }
            }
        }

        int[] removedPiecesPerColumn = RemovedPiecesPerColumn(removed);
        for (int column = 0; column < Width; column++)
        {
            for (int i = 0; i < removedPiecesPerColumn[column]; i++)
            {
                pieces[column, i] = replacement[new Vector2Int(column, -1 - i)];
            }
        }
    }

	public void SubscribeToRemoves(PieceEventEventHandler removeHandler)
	{
		PieceRemoved += removeHandler;
	}

	public void SubscribeToAdds(PieceEventEventHandler addHandler)
	{
		PieceAdded += addHandler;
	}
}
