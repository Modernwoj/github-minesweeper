using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Tile : MonoBehaviour
{
    [SerializeField] TileSpriteScriptableObject sprites;

    public bool isMine { get; private set; }
    public bool flagged { get; private set; }
    public int mineCount { get; private set; }

    private SpriteRenderer spriteRenderer;
    private bool active = true;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnMouseOver()
    {
        if (GameStateManager.gamestate != GameStateManager.GameState.Gameplay) return;
        if (active)
        {
            if (Input.GetMouseButtonDown(0))
            {
                ClickedTile();
                return;
            }
            if (Input.GetMouseButtonDown(1))
            {
                FlaggedTile();
                return;
            }
        }
        else
        {
            if (Input.GetMouseButton(0) && Input.GetMouseButton(1))
            {
                GameplayManager.Instance.ExpandIfFlagged(this);
            }
        }
    }
    public void ClickedTile()
    {
        if (active && !flagged)
        {
            active = false;
            if (GameplayManager.Instance.isFirstClick)
                GameplayManager.Instance.FirstClick(this);
            if (isMine)
            {
                spriteRenderer.sprite = sprites.mineHitTile;
                GameplayManager.Instance.GameOver();
                Debug.Log("GameOver");
                return;
            }
            else
            {
                spriteRenderer.sprite = sprites.clickedTiles[mineCount];
                if (mineCount == 0) {
                    GameplayManager.Instance.ClickNeighbours(this);
                }
            }
            GameplayManager.Instance.CheckGameOver();
        }
    }
    private void FlaggedTile()
    {
        flagged = !flagged;
        spriteRenderer.sprite = flagged ? sprites.flaggedTile : sprites.unclickedTile;
    }

    public void ShowGameOverState()
    {
        if (!active) return;
        active = false;
        if (!flagged && isMine)
        {
            spriteRenderer.sprite = sprites.mineTile;
            return;
        }
        if (flagged && !isMine)
        {
            spriteRenderer.sprite = sprites.mineWrongTile;
            return;
        }
    }

    public void SetFlaggedIfMine()
    {
        if(!active) return;
        active = false;
        if (isMine)
        {
            flagged = true;
            spriteRenderer.sprite = sprites.flaggedTile;
        }
    }

    public void SetMine()
    {
        isMine = true;
    }

    public void ResetMine()
    {
        isMine = false;
    }

    public void IncrementMineCount()
    {
        mineCount+=1;
    }
    public void DecrementMineCount()
    {
        mineCount -=1;
        if(mineCount < 0) mineCount = 0;
    }

    public void ResetTile()
    {
        active = true;
        isMine = false;
        flagged = false;
        mineCount = 0;
        spriteRenderer.sprite = sprites.unclickedTile;
    }
}
