using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "LevelSetup")]
public class LevelSetup : SerializedScriptableObject
{
    [SerializeField] public Dictionary<Vector3Int, CellSetup> cells;
    [SerializeField] public List<EnemyConfig> enemy;

    public class CellSetup
    {
        public TileBase tile_Land;
        public bool canSpawn;
        public bool canMove;
    }

    public class EnemyConfig
    {
        public Vector3Int position;
        public CharacterSetup setup;
    }
}
