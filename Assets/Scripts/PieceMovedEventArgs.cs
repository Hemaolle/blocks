using System;
using UnityEngine;

public class PieceMovedEventArgs : EventArgs
{
    public PieceMovedEventArgs(Vector2Int oldCoordinates, Vector2Int newCoordinates)
    {
        OldCoordinates = oldCoordinates;
        NewCoordinates = newCoordinates;
    }

    public Vector2Int OldCoordinates { get; }
    public Vector2Int NewCoordinates { get; }
}
