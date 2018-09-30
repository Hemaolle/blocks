//-----------------------------------------------------------------------
// <copyright file="Configuration.cs" company="Oskari Leppäaho">
//      Copyright (c) Oskari Leppäaho. All Rights Reserved.
// </copyright>
//-----------------------------------------------------------------------
public class Configuration
{
    public const int MinDimension = 5;
    public const int MaxDimension = 20;
    public const int MinColors = 3;
    public const int MaxColors = 6;

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

    private int boardWidth;
    public int BoardWidth
    {
        get
        {
            return boardWidth;
        }

        set
        {
            ThrowIfDimensionInvalid(value);
            boardWidth = value;
        }
    }

    private int boardHeight;
    public int BoardHeight
    {
        get
        {
            return boardHeight;
        }

        set
        {
            ThrowIfDimensionInvalid(value);
            boardHeight = value;
        }
    }

    private int numColors;
    public int NumColors
    {
        get
        {
            return numColors;
        }

        set
        {
            ThrowIfNumColorsInvalid(value);
            numColors = value;
        }
    }

    private void ThrowIfDimensionInvalid(int dimension)
    {
        if (dimension < MinDimension || dimension > MaxDimension)
        {
            throw new System.ArgumentException("A board dimension outside limits");
        }
    }

    private void ThrowIfNumColorsInvalid(int numColors)
    {
        if (numColors < MinColors || numColors > MaxColors)
        {
            throw new System.ArgumentException("The number of colors outside limits");
        }
    }    
}
