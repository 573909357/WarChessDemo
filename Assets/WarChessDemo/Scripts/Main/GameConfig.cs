using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameConfig : SerializedScriptableObject
{
    [SerializeField] public List<CharacterSetup> characterSetups;
    [SerializeField] public List<LevelSetup> levelSetups;
    [SerializeField] public TileBase defaultDisplayTile;
}
