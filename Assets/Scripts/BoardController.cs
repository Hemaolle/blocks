using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour {
    public GameObject[] piecePrefabs;
    public Transform boardParent;
    public Transform bottom;
    public float pieceWidth;
    public float pieceHeight;
    public int boardWidth;
    public int boardHeight;
    public int colors;    

    private Board board;
    private Dictionary<Vector2Int, GameObject> pieces = new Dictionary<Vector2Int, GameObject>();
    Vector2 boardCenter;

	// Use this for initialization
	void Start () {
        board = new Board(new Configuration(boardWidth, boardHeight, colors));
        board.SubscribeToAdds(PieceAdded);
        board.SubscribeToRemoves(PieceRemoved);

        boardCenter = new Vector2(boardWidth / 2 * pieceWidth, boardHeight / 2 * pieceHeight);
        bottom.position = transform.position + Vector3.down * (boardCenter.y + pieceHeight / 2);
        for (int y = 0; y < board.Height; y++)
        {
            for (int x = 0; x < board.Width; x++)
            {
                InstantiatePiece(new Vector2Int(x, y), NewPiecePosition(x, y), board.At(x, y));
            }
        }
	}

    void PieceRemoved (object sender, PieceRemovedEventArgs e) {
        Destroy(pieces[e.Coordinates]);
        pieces.Remove(e.Coordinates);
	}

    void PieceAdded(object sender, PieceAddedEventArgs e)
    {
        InstantiatePiece(e.Coordinates, NewPiecePosition(e.Coordinates.x, e.Coordinates.y - e.AdditionsInSameColumn), e.PieceType);
    }

    private void InstantiatePiece(Vector2Int boardCoordinates, Vector3 worldPosition, int pieceType)
    {
        GameObject newPiece = Instantiate(piecePrefabs[board.At(boardCoordinates.x, boardCoordinates.y)],
            worldPosition,
            Quaternion.identity, boardParent);
        var pc = newPiece.GetComponent<PieceController>();
        pc.SetBoard(board);
        pc.SetCoordinates(boardCoordinates);
        pieces.Add(boardCoordinates, newPiece);
    }

    private Vector3 NewPiecePosition(int x, int y)
    {
        return transform.position + new Vector3(x * pieceWidth - boardCenter.x, -y * pieceHeight + boardCenter.y, 0);
    }
}
