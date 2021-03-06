﻿//-----------------------------------------------------------------------
// <copyright file="PieceAddedEventArgs.cs" company="Oskari Leppäaho">
//      Copyright (c) Oskari Leppäaho. All Rights Reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using UnityEngine;

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
