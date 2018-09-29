using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Board {
    // TODO these could be nullabale ints.
    private int[,] pieces;
    private Configuration configuration;
	private event EventHandler<PieceRemovedEventArgs> PieceRemoved;
	private event EventHandler<PieceAddedEventArgs> PieceAdded;	

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

    public int Width { get { return configuration.BoardWidth; } }
    public int Height { get { return configuration.BoardHeight; } }

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
    /// Remove pieces in the given coordinates on the board an place the given new pieces on the
    /// board. When pieces are removed, the existing pieces "fall down" to fill the empty spaces
    /// and the new pieces will be placed on top of the columns. Triggers events for the removal
    /// of the old pieces and the addition of new pieces. Note that the coordinates for the add
    /// events are on top of the board 
    /// </summary>
    /// <param name="removed"></param>
    /// <param name="replacement"></param>
    public void ReplacePieces(List<Vector2Int> removed, Dictionary<Vector2Int, int> replacement)
    {
        foreach (var r in removed)
        {
            OnPieceRemoved(new PieceRemovedEventArgs(new Vector2Int(r.x, r.y)));
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

    /// <summary>
    /// Calculates the Y coordinate for the i:th replacement piece in a column. The replacement pieces will
    /// appear on top of the column it is added to.
    /// </summary>
    /// <param name="i">The index of the replacement piece.</param>
    /// <returns>The y coordinate for the replacement piece.</returns>
    private int ReplacementYCoordinate(int i)
    {
        return i;
    }

	public void SubscribeToRemoves(EventHandler<PieceRemovedEventArgs> removeHandler)
	{
		PieceRemoved += removeHandler;
	}

    public void SubscribeToAdds(EventHandler<PieceAddedEventArgs> addHandler)
	{
		PieceAdded += addHandler;
	}

    private void OnPieceRemoved(PieceRemovedEventArgs e)
    {
        PieceRemoved?.Invoke(this, e);
    }

    private void OnPieceAdded(PieceAddedEventArgs e)
    {
        PieceAdded?.Invoke(this, e);
    }
}
