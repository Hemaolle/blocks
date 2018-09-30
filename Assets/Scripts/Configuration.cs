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

    public Configuration(int boardWidth, int boardHeight, int numColors)
    {
        ThrowIfDimensionInvalid(boardWidth);
        ThrowIfDimensionInvalid(boardHeight);
        ThrowIfNumColorsInvalid(numColors);
        BoardWidth = boardWidth;
        BoardHeight = boardHeight;
        NumColors = numColors;
    }

    public int BoardWidth { get; }
    public int BoardHeight { get; }
    public int NumColors { get; }

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
