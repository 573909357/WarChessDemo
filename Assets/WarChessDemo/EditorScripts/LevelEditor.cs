using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelEditor : MonoBehaviour
{
    [SerializeField] private Tilemap land;
    [SerializeField] private Tilemap canSpawn;
    [SerializeField] private Tilemap canMove;
    [SerializeField] private TileBase displayTile;
    
    [Title("Config")]
    [SerializeField, ShowInInspector] private List<LevelSetup.EnemyConfig> enemy;
    
    [Button]
    private void Save(LevelSetup setup)
    {
        var bounds = land.cellBounds;
        setup.cells = new Dictionary<Vector3Int, LevelSetup.CellSetup>();
        for (int x = bounds.xMin; x <= bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y <= bounds.yMax; y++)
            {
                var position = new Vector3Int(x, y);
                setup.cells[new Vector3Int(x, y)] = new LevelSetup.CellSetup
                {
                    tile_Land = land.GetTile(position),
                    canSpawn = canSpawn.GetTile(position) != null,
                    canMove = canMove.GetTile(position) != null,
                };
            }
        }
        
        if (enemy != null)
            setup.enemy = new List<LevelSetup.EnemyConfig>(enemy);
    }

    [Button]
    private void Load(LevelSetup setup)
    {
        land.ClearAllTiles();
        canSpawn.ClearAllTiles();
        canMove.ClearAllTiles();

        foreach (var (position, cellSetup) in setup.cells)
        {
            land.SetTile(position, cellSetup.tile_Land);
            if (cellSetup.canSpawn)
                canSpawn.SetTile(position, displayTile);
            if (cellSetup.canMove)
                canMove.SetTile(position, displayTile);
        }
    }
}
