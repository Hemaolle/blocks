using UnityEngine;
using System;

public class PieceAddedEventArgs : EventArgs
{
    public PieceAddedEventArgs(Vector2Int coordinates, int additionsInSameColumn, int pieceType)
    {
        Coordinates = coordinates;
        AdditionsInSameColumn = additionsInSameColumn;
        PieceType = pieceType;
    }

    public Vector2Int Coordinates { get; }
    public int AdditionsInSameColumn { get; }
    public int PieceType { get; }
}
