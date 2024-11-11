using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameplayManager : SingletonFromPrefab<GameplayManager>
{
    [SerializeField] Transform tilePrefab;
    [SerializeField] GameObject boardPlacement;

    public bool isFirstClick { get; private set; }

    private int width;
    private int height;
    private int numMines;
    private int activeTiles;
    private List<Tile> tiles = new();
    GameObject board;

    private void Start()
    {
        if (!board)
        {
            board = new GameObject();
            board.name = "Board";
            board.transform.parent = boardPlacement.transform;
            board.transform.localPosition = new Vector3(0, 0, 0);
        }
        CreateGameBoard(9, 9, 10);
        ResetGameState();
    }

    public void CheckCreateGameBoard(int width, int height, int numMines)
    {
        if (this.width != width || this.height != height || this.numMines != numMines)
        {
            CreateGameBoard(width, height, numMines);
        }
        ResetGameState();
    }

    public void CreateGameBoard(int width, int height, int numMines)
    {
        ClearBoard();

        this.width = width;
        this.height = height;
        this.numMines = numMines;

        for (int row = 0; row < height; row++)
        {
            for (int col = 0; col < width; col++)
            {
                Transform tileTransform = Instantiate(tilePrefab);
                tileTransform.parent = board.transform;
                float xIndex = col - ((width - 1) / 2.0f);
                float yIndex = row - ((height - 1) / 2.0f);
                tileTransform.localPosition = new Vector2(xIndex * 0.5f, yIndex * 0.5f);
                Tile tile = tileTransform.GetComponent<Tile>();
                tiles.Add(tile);
            }
        }
    }

    public void ResetGameState()
    {
        isFirstClick = true;
        activeTiles = width * height;

        foreach (var tile in tiles)
        {
            tile.ResetTile();
        }

        //Shuffle Tiles for mine positions.
        int[] minePositions = Enumerable.Range(0, tiles.Count).OrderBy(x => Random.Range(0.0f, 1.0f)).ToArray();

        // Set mines to first numMines positions and inform neighbours
        for (int i = 0; i < numMines; i++)
        {
            int pos = minePositions[i];
            tiles[pos].SetMine();
            IncrementNeighboursMineCount(pos);
        }

        GameStateManager.ChangeGameState(GameStateManager.GameState.Gameplay);
        UIManager.Instance.gameplayUI.HideGameOverScreen();
    }

    public void ClearBoard()
    {
        if (tiles.Count == 0) return;

        tiles.Clear();

        Destroy(board);

        board = new GameObject();
        board.name = "Board";
        board.transform.parent = boardPlacement.transform;
        board.transform.localPosition = new Vector3(0, 0, 0);
    }

    private void IncrementNeighboursMineCount(int location)
    {
        foreach (int post in GetNeighbours(location))
        {
            if (!tiles[post].isMine)
                tiles[post].IncrementMineCount();
        }
    }

    private void DecrementNeighboursMineCount(int location)
    {
        foreach (int post in GetNeighbours(location))
        {
            if (!tiles[post].isMine)
                tiles[post].DecrementMineCount();
        }
    }

    public void ClickNeighbours(Tile tile)
    {
        int location = tiles.IndexOf(tile);
        foreach (int pos in GetNeighbours(location))
        {
            tiles[pos].ClickedTile();
        }
    }

    public void ClickNeighbours(List<int> neighbours)
    {
        foreach (int pos in neighbours)
        {
            tiles[pos].ClickedTile();
        }
    }

    private List<int> GetNeighbours(int pos)
    {
        List<int> neighbours = new();
        int row = pos / width;
        int col = pos % width;
        //position checks
        //current row check
        //left check
        if (col > 0)
            neighbours.Add(pos - 1);
        //right check
        if (col < width - 1)
            neighbours.Add(pos + 1);

        //upper row check
        if (row < height - 1)
        {
            neighbours.Add(pos + width);
            //left check
            if (col > 0)
                neighbours.Add(pos + width - 1);
            //right check
            if (col < width - 1)
                neighbours.Add(pos + width + 1);
        }
        //lower row check
        if (row > 0)
        {
            neighbours.Add(pos - width);
            //left check
            if (col > 0)
                neighbours.Add(pos - width - 1);
            //right check
            if (col < width - 1)
                neighbours.Add(pos - width + 1);
        }
        return neighbours;
    }

    public void FirstClick(Tile tile)
    {
        int pos = tiles.IndexOf(tile);
        isFirstClick = false;
        if (!tiles[pos].isMine) return;
        //move mine if first click is mine
        int i = 0;
        while (tiles[i].isMine)
            i++;
        tiles[i].SetMine();
        IncrementNeighboursMineCount(i);

        tiles[pos].ResetMine();
        DecrementNeighboursMineCount(pos);
    }

    //click sorrounding if number of sorrrounding flags equals number of mines around tile
    public void ExpandIfFlagged(Tile tile)
    {
        int location = tiles.IndexOf(tile);
        int flag_count = 0;
        List<int> neighbours = GetNeighbours(location);

        foreach (int pos in neighbours)
        {
            if (tiles[pos].flagged)
                flag_count++;
        }
        if (flag_count == tile.mineCount)
        {
            ClickNeighbours(neighbours);
        }
    }

    public void CheckGameOver()
    {
        activeTiles--;
        if (activeTiles == numMines)
        {
            foreach (Tile tile in tiles)
            {
                tile.SetFlaggedIfMine();
            }

            GameStateManager.ChangeGameState(GameStateManager.GameState.GameOver);
            UIManager.Instance.gameplayUI.ShowVictoryScreen();
        }
    }

    public void GameOver()
    {
        foreach (Tile tile in tiles)
        {
            tile.ShowGameOverState();
        }
        GameStateManager.ChangeGameState(GameStateManager.GameState.GameOver);
        UIManager.Instance.gameplayUI.ShowDefeatScreen();
    }
}
