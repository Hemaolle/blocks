//-----------------------------------------------------------------------
// <copyright file="PieceRemovedEventArgs.cs" company="Oskari Leppäaho">
//      Copyright (c) Oskari Leppäaho. All Rights Reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using UnityEngine;

public class PieceRemovedEventArgs : EventArgs
{
    public PieceRemovedEventArgs(Vector2Int coordinates)
    {
        Coordinates = coordinates;
    }

    public Vector2Int Coordinates { get; }
}
