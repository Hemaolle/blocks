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
    private bool clickable = true;

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
        if (clickable)
        {
            board.SelectPiece(boardPosition);
        }
     }

    public bool IsMoving()
    {
        return velocity != 0;
    }

    public void SetClickable(bool clickable)
    {
        this.clickable = clickable;
    }

    private void Update()
    {
        bool inMyPlace = transform.position == BoardToWorldCoordinates();
        if (inMyPlace)
        {
            return;
        }

        velocity += Time.deltaTime * world.PieceAcceleration;
        Vector3 newPosition = transform.position + (Vector3.down * velocity);
        bool newPositionIsPastMyPlace = newPosition.y < BoardToWorldCoordinates().y;
        if (newPositionIsPastMyPlace)
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
