using System.Text;
using UnityEngine;

public class Board {
    Piece[,] pieces;
    Configuration configuration;
    
    public int Width { get { return configuration.BoardWidth; } }
    public int Height { get { return configuration.BoardHeight; } }

    public Board(Configuration configuration)
    {
        this.configuration = configuration;
        pieces = new Piece[configuration.BoardWidth, configuration.BoardHeight];
        Populate();
    }

    private void Populate()
    {
        for (int y = 0; y < configuration.BoardHeight; y++)
        {
            for(int x = 0; x < configuration.BoardWidth; x++)
            {
                pieces[x, y] = new Piece(Random.Range(0, configuration.NumColors));
            }
        }
    }

    public Piece At(int x, int y)
    {
        return pieces[x, y];
    }

    /// <summary>
    /// Line y = 0 is the topmost one.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        bool first = true;
        StringBuilder sb = new StringBuilder();
        for (int y = 0; y < configuration.BoardHeight; y++)
        {
            if (!first)
            {
                sb.Append('\n');
            }
            for (int x = 0; x < configuration.BoardWidth; x++)
            {
                sb.Append(pieces[x, y]);
            }
            first = false;
        }
        return sb.ToString();
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
                pieces[x, y] = new Piece((int)(lines[y][x] - '0'));
            }
        }
    }
}
