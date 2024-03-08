using UnityEngine;
using UnityEngine.Tilemaps;

public enum Tetrimino
{
    I,
    O,
    T,
    J,
    L,
    S,
    Z
}

[System.Serializable]
public struct TetriminoData
{
    public Tetrimino tetrimino;
    public Tile tile;
    public Vector2Int[] cells { get; private set; }
    public Vector2Int[,] wallKicks { get; private set; }

    public void Initialize()
    {
        this.cells = Data.Cells[this.tetrimino];
        wallKicks = Data.WallKicks[tetrimino];
    }
}