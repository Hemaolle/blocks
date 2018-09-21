using System.Collections.Generic;
using UnityEngine;

public class Configuration {
    public const int MIN_DIMENSION = 5;
    public const int MAX_DIMENSION = 20;
    public const int MIN_COLORS = 3;
    public const int MAX_COLORS = 6;

    private int boardWidth;
    public int BoardWidth {
        get { return boardWidth; }
        set {
            ThrowIfDimensionInvalid(value);
            boardWidth = value;
        }
    }

    private int boardHeight;
    public int BoardHeight
    {
        get { return boardHeight; }
        set {
            ThrowIfDimensionInvalid(value);
            boardHeight = value;
        }
    }

    private int numColors;
    public int NumColors
    {
        get { return numColors; }
        set
        {
            ThrowIfNumColorsInvalid(value);
            numColors = value;
        }
    }
    public Dictionary<int, Material> PieceMaterials;

    // TODO: immutable?
    public Configuration(int boardWidth, int boardHeight, int numColors)
    {
        ThrowIfDimensionInvalid(boardWidth);
        ThrowIfDimensionInvalid(boardHeight);
        ThrowIfNumColorsInvalid(numColors);
        BoardWidth = boardWidth;
        BoardHeight = boardHeight;
        NumColors = numColors;
    }

    private void ThrowIfDimensionInvalid(int dimension)
    {
        if (dimension < MIN_DIMENSION || dimension > MAX_DIMENSION)
        {
            throw new System.ArgumentException("A board dimension outside limits");
        }
    }

    private void ThrowIfNumColorsInvalid(int numColors)
    {
        if (numColors < MIN_COLORS || numColors > MAX_COLORS)
        {
            throw new System.ArgumentException("The number of colors outside limits");
        }
    }    
}
