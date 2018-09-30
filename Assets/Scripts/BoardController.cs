//-----------------------------------------------------------------------
// <copyright file="BoardController.cs" company="Oskari Leppäaho">
//      Copyright (c) Oskari Leppäaho. All Rights Reserved.
// </copyright>
//-----------------------------------------------------------------------
using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    public GameObject[] piecePrefabs;
    public Transform boardParent;
    public float pieceWidth;
    public float pieceHeight;
    public int boardWidth;
    public int boardHeight;
    public int colors;
    public float pieceAcceleration;

    private World world;
    private Board board;
    private Dictionary<Vector2Int, GameObject> pieces = new Dictionary<Vector2Int, GameObject>();

	// Use this for initialization
	void Start ()
    {
        board = new Board(new Configuration(boardWidth, boardHeight, colors));
        board.SubscribeToAdds(PieceAdded);
        board.SubscribeToRemoves(PieceRemoved);
        board.SubscibeToMoves(PieceMoved);

        Vector2 boardCenter = new Vector2(boardWidth / 2 * pieceWidth, boardHeight / 2 * pieceHeight);
        world = new World(pieceAcceleration, pieceWidth, pieceHeight, transform.position, boardCenter);

        for (int y = 0; y < board.Height; y++)
        {
            for (int x = 0; x < board.Width; x++)
            {
                InstantiatePiece(new Vector2Int(x, y), world.BoardToWorldCoordinates(new Vector2Int(x, y)), board.At(x, y));
            }
        }
	}

    void PieceRemoved (object sender, PieceRemovedEventArgs e)
    {
        Destroy(pieces[e.Coordinates]);
        pieces.Remove(e.Coordinates);
	}

    void PieceAdded(object sender, PieceAddedEventArgs e)
    {
        InstantiatePiece(e.Coordinates, world.BoardToWorldCoordinates(new Vector2Int(e.Coordinates.x, e.Coordinates.y - e.AdditionsInSameColumn)), e.PieceType);
    }

    void PieceMoved(object sender, PieceMovedEventArgs e)
    {
        pieces[e.OldCoordinates].GetComponent<PieceController>().SetBoardPosition(e.NewCoordinates);
        pieces.Add(e.NewCoordinates, pieces[e.OldCoordinates]);
        pieces.Remove(e.OldCoordinates);        
    }

    private GameObject InstantiatePiece(Vector2Int boardCoordinates, Vector3 worldPosition, int pieceType)
    {
        GameObject newPiece = Instantiate(piecePrefabs[board.At(boardCoordinates.x, boardCoordinates.y)],
            worldPosition,
            Quaternion.identity, boardParent);
        var pc = newPiece.GetComponent<PieceController>();
        pc.SetBoard(board);
        pc.SetBoardPosition(boardCoordinates);
        pc.SetWorld(world);        
        pieces.Add(boardCoordinates, newPiece);
        return newPiece;
    }
}
