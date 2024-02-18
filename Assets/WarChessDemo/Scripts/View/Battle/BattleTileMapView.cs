using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class BattleTileMapView : MonoBehaviour
{
    [Title("TileMaps")]
    [SerializeField] public Tilemap land;
    [SerializeField] public Tilemap controller;
    
    [Space]
    [SerializeField] public TileMapInputEvents controllerInputEvents;


    
    public void Init()
    {
        controllerInputEvents.onPointerMove += _OnPointerMove;
        controllerInputEvents.onPointerClick += _OnPointerClick;
    }

    public void SetupMap(LevelSetup levelSetup)
    {
        land.ClearAllTiles();

        foreach (var (position, cellSetup) in levelSetup.cells)
        {
            land.SetTile(position, cellSetup.tile_Land);
        }
    }
    
    private void _OnPointerMove(PointerEventData data, TileMapInputEvents _)
    {
    }
    
    private void _OnPointerClick(PointerEventData data, TileMapInputEvents _)
    {
        var battleManager = GameInstance.gameInstance.battleManager;
    }
}
