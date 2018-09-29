using UnityEngine;

public class PieceController : MonoBehaviour {
    private Board board;
    private Vector2Int coordinates;

    public void SetBoard(Board b)
    {
        this.board = b;
    }

    public void SetCoordinates(Vector2Int c)
    {
        this.coordinates = c;
    }

    public void Select()
    {
        board.SelectPiece(coordinates);
    }
}
