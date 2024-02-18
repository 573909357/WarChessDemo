using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileController : MonoBehaviour
{
    [SerializeField] private TileBase tile;
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private TilemapRenderer tilemapRenderer;

    [Button]
    private void SetTile(Vector3Int positon)
    {
        tilemap.SetTile(positon, tile);
    }
    
    [Button]
    private void GetTile(Vector3Int positon)
    {
        tile = tilemap.GetTile(positon);
    }
    
    [Button]
    private Color GetColor(Vector3Int positon)
    {
        return tilemap.GetColor(positon);
    }
    
    [Button]
    private void SetColor(Vector3Int positon, Color color)
    {
        tilemap.SetColor(positon, color);
    }
}
