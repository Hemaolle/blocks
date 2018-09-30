//-----------------------------------------------------------------------
// <copyright file="PieceController.cs" company="Oskari Leppäaho">
//      Copyright (c) Oskari Leppäaho. All Rights Reserved.
// </copyright>
//-----------------------------------------------------------------------
using UnityEngine;

public class PieceController : MonoBehaviour
{
    private Board board;
    private Vector2Int boardPosition;
    private World world;
    private float velocity;


    public void SetBoard(Board b)
    {
        board = b;
    }

    public void SetBoardPosition(Vector2Int c)
    {
        boardPosition = c;
    }

    public void SetWorld(World w)
    {
        world = w;
    }

    public void Select()
    {
        board.SelectPiece(boardPosition);
    }

    private void Update()
    {
        if (transform.position == BoardToWorldCoordinates())
        {
            return;
        }

        velocity += Time.deltaTime * world.PieceAcceleration;
        Vector3 newPosition = transform.position + Vector3.down * velocity;
        if (newPosition.y < BoardToWorldCoordinates().y)
        {
            transform.position = BoardToWorldCoordinates();
            velocity = 0;
        }
        else
        {
            transform.position = newPosition;
        }
    }

    private Vector3 BoardToWorldCoordinates()
    {
        return world.BoardToWorldCoordinates(boardPosition);
    }
}
