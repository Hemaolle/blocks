//-----------------------------------------------------------------------
// <copyright file="Board.cs" company="Oskari Leppäaho">
//      Copyright (c) Oskari Leppäaho. All Rights Reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Board
{
    private const int EmptyPiece = int.MinValue;

    // TODO: Nullable ints could be nicer to represent empty spaces.
    private int[,] pieces;
    private Configuration configuration;

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

    private event EventHandler<PieceRemovedEventArgs> PieceRemoved;

    private event EventHandler<PieceAddedEventArgs> PieceAdded;

    private event EventHandler<PieceMovedEventArgs> PieceMoved;

    public int Width
    {
        get { return configuration.BoardWidth; }
    }

    public int Height
    {
        get { return configuration.BoardHeight; }
    }

    public void SelectPiece(Vector2Int coordinates)
    {
        List<Vector2Int> piecesToRemove = ConnectedPiecesCoords(coordinates.x, coordinates.y);
        Dictionary<Vector2Int, int> replacementPieces = GenerateReplacementPieces(piecesToRemove);
        ReplacePieces(piecesToRemove, replacementPieces);
    }

    public int At(int x, int y)
    {
        return pieces[x, y];
    }

    /// <summary>
    /// Line y = 0 is the topmost one in the string representation.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return Reduce(
            new StringBuilder(),
            (sb, x, y, type) =>
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

    public void SubscribeToRemoves(EventHandler<PieceRemovedEventArgs> removeHandler)
    {
        PieceRemoved += removeHandler;
    }

    public void SubscribeToAdds(EventHandler<PieceAddedEventArgs> addHandler)
    {
        PieceAdded += addHandler;
    }

    public void SubscibeToMoves(EventHandler<PieceMovedEventArgs> moveHandler)
    {
        PieceMoved += moveHandler;
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

    /// <summary>
    /// Return a list of coordinates for all the pieces that are four-way connected to the given coordinates
    /// </summary>
    private List<Vector2Int> ConnectedPiecesCoords(int x, int y)
    {
        var copy = new Board(this);
        copy.FloodFill(x, y, At(x, y), EmptyPiece);
        return copy.FindAll(EmptyPiece);
    }

    private List<Vector2Int> FindAll(int value)
    {
        return Reduce(
            new List<Vector2Int>(),
            (acc, x, y, type) =>
            {
                if (type == value)
                {
                    acc.Add(new Vector2Int(x, y));
                }

                return acc;
            });
    } 

    private T Reduce<T>(T accumulator, Func<T, int, int, int, T> reducer)
    {
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
        FloodFill(x - 1, y, targetType, replacementType);
        FloodFill(x + 1, y, targetType, replacementType);
        return;
    }

    /// <summary>
    /// Generate replacement pieces.
    /// </summary>
    /// <param name="removed">Coordinates of removed pieces for which to generate replacement pieces</param>
    /// <returns>A dictionary of coordinates and piece types for the new pieces. The new pieces are
    /// generated for the top locations in the column (placing them higher for a drop or similar is left
    /// for the user of this class).</returns>
    private Dictionary<Vector2Int, int> GenerateReplacementPieces(List<Vector2Int> removed)
    {
        var result = new Dictionary<Vector2Int, int>();
        int[] removedPiecesPerColumn = Utilities.CountInColumns(removed, Width);
        for (int x = 0; x < Width; x++)
        {
            for (int i = 0; i < removedPiecesPerColumn[x]; i++)
            {
                result.Add(new Vector2Int(x, ReplacementYCoordinate(i)), RandomizePiece());
            }
        }

        return result;
    }

    /// <summary>
    /// Remove pieces in the given coordinates on the board and place the given new pieces on the
    /// board. When pieces are removed, the existing pieces move down to fill the empty spaces
    /// and the new pieces will be placed on top of the columns. Triggers events for the removal
    /// of the old pieces, addition of new pieces and movement of existing pieces.
    /// </summary>
    /// <param name="removed">Coordinates of removed pieces</param>
    /// <param name="replacement">Coordinates and types of the new pieces</param>
    private void ReplacePieces(List<Vector2Int> removed, Dictionary<Vector2Int, int> replacement)
    {
        foreach (var r in removed)
        {
            OnPieceRemoved(new PieceRemovedEventArgs(new Vector2Int(r.x, r.y)));
            pieces[r.x, r.y] = EmptyPiece;
        }

        for (int x = 0; x < Width; x++)
        {
            int? y;
            do
            {
                y = LowestPieceWithEmptyBelow(x);
                if (y.HasValue)
                {
                    int? newY = LowestEmptyPiece(x);
                    OnPieceMoved(new PieceMovedEventArgs(new Vector2Int(x, y.Value), new Vector2Int(x, newY.Value)));
                    pieces[x, newY.Value] = pieces[x, y.Value];
                    pieces[x, y.Value] = EmptyPiece;
                }
            }
            while (y.HasValue);
        }

        int[] removedPiecesPerColumn = Utilities.CountInColumns(removed, Width);
        for (int x = 0; x < Width; x++)
        {
            for (int i = 0; i < removedPiecesPerColumn[x]; i++)
            {
                int replacementPieceType = replacement[new Vector2Int(x, ReplacementYCoordinate(i))];
                pieces[x, i] = replacementPieceType;
                OnPieceAdded(new PieceAddedEventArgs(new Vector2Int(x, i), removedPiecesPerColumn[x], replacementPieceType));
            }
        }
    }

    private int? LowestPieceWithEmptyBelow(int x)
    {
        bool emptyFound = false;
        for (int y = Height - 1; y > -1; y--)
        {            
            if (IsEmpty(x, y))
            {
                emptyFound = true;
                continue;
            }
            
            // The current piece is not empty since we didn't continue above.
            if (emptyFound)
            {
                return y;
            }
        }

        return null;
    }

    private int? LowestEmptyPiece(int x)
    {
        for (int y = Height - 1; y > -1; y--)
        {
            if (IsEmpty(x, y))
            {
                return y;
            }
        }

        return null;
    }

    private bool IsEmpty(int x, int y)
    {
        return pieces[x, y] == EmptyPiece;
    }

    /// <summary>
    /// The Y coordinate for the i:th replacement piece in a column. The replacement pieces will
    /// appear on top of the column it is added to.
    /// 
    /// This method may look a bit silly as it is only identity, but we're sort of converting
    /// from index in the list of added pieces to a Y coordinate.
    /// </summary>
    /// <param name="i">The index of the replacement piece.</param>
    /// <returns>The y coordinate for the replacement piece.</returns>
    private int ReplacementYCoordinate(int i)
    {
        return i;
    }

    private void OnPieceRemoved(PieceRemovedEventArgs e)
    {
        PieceRemoved?.Invoke(this, e);
    }

    private void OnPieceAdded(PieceAddedEventArgs e)
    {
        PieceAdded?.Invoke(this, e);
    }

    private void OnPieceMoved(PieceMovedEventArgs e)
    {
        PieceMoved?.Invoke(this, e);
    }
}
