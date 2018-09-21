using UnityEngine;

public class Piece {
    public int Type { get; }

    public Piece(int type)
    {
        Type = type;
    }

    public override string ToString()
    {
        return Type.ToString();
    }
}
