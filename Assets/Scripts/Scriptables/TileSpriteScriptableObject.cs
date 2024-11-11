using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TileSpriteData", menuName = "ScriptableObjects/TileSpriteScriptableObject", order = 1)]
public class TileSpriteScriptableObject : ScriptableObject
{
    [Header("Tile Sprites")]
    [SerializeField] Sprite _unclickedTile;
    [SerializeField] Sprite _flaggedTile;
    [SerializeField] Sprite _mineTile;
    [SerializeField] Sprite _mineWrongTile;
    [SerializeField] Sprite _mineHitTile;
    [SerializeField] List<Sprite> _clickedTiles;

    // public readonly access
    public Sprite unclickedTile => _unclickedTile;
    public Sprite flaggedTile => _flaggedTile;
    public Sprite mineTile => _mineTile;
    public Sprite mineWrongTile => _mineWrongTile;
    public Sprite mineHitTile => _mineHitTile;
    public List<Sprite> clickedTiles => _clickedTiles;
}
