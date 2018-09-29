using UnityEngine;
using System;

public class BoardEventArgs : EventArgs
{
    public BoardEventArgs(Vector2Int coordinates, int pieceType)
    {
        Coordinates = coordinates;
        PieceType = pieceType;
    }

    public Vector2Int Coordinates { get; }
    public int PieceType { get; }
}
