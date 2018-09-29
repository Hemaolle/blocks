using UnityEngine;
using System;

public class PieceRemovedEventArgs : EventArgs
{
    public PieceRemovedEventArgs(Vector2Int coordinates)
    {
        Coordinates = coordinates;
    }

    public Vector2Int Coordinates { get; }
}
