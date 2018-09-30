//-----------------------------------------------------------------------
// <copyright file="World.cs" company="Oskari Leppäaho">
//      Copyright (c) Oskari Leppäaho. All Rights Reserved.
// </copyright>
//-----------------------------------------------------------------------
using UnityEngine;

public class World
{
    public World(
        float pieceAcceleration,
        float pieceWidth,
        float pieceHeight,
        Vector3 boardPosition,
        Vector2 boardCenter)
    {
        PieceAcceleration = pieceAcceleration;
        PieceWidth = pieceWidth;
        PieceHeight = pieceHeight;
        BoardPosition = boardPosition;
        BoardCenter = boardCenter;
    }

    public float PieceAcceleration { get; }

    public float PieceWidth { get; }

    public float PieceHeight { get; }

    public Vector3 BoardPosition { get; }

    public Vector2 BoardCenter { get; }

    public Vector3 BoardToWorldCoordinates(Vector2Int boardCoordinates)
    {
        return BoardPosition + new Vector3(boardCoordinates.x * PieceWidth - BoardCenter.x,
            -boardCoordinates.y * PieceHeight + BoardCenter.y, 0);
    }
}
