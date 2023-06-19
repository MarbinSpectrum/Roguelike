using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "Tile", menuName = "ScriptableObjects/Tile", order = 1)]
public class TileData : ScriptableObject
{
    public Tile tile;
    [PreviewField]
    public Sprite tileSprite;
    public TileType tileType;
}
