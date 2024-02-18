using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameInstance : MonoBehaviour
{
    public static GameInstance gameInstance;
    
    [SerializeField] public GameConfig gameConfig;
    [SerializeField] public Camera mainCamera;
    [SerializeField] public BattleManager battleManager;
    [SerializeField] public LevelSelectPanel levelSelectPanel;
    [SerializeField] public BattleTileMapView battleTileMapView;
    [SerializeField] public CharacterSelectPanel characterSelectPanel;
    [SerializeField] public BattlePanel battlePanel;
    
    [NonSerialized] public Input_ControlManager input_ControlManager;
    
    
    private void Awake()
    {
        gameInstance = this;
        input_ControlManager = new Input_ControlManager();
        battleManager.Init();
        battleTileMapView.Init();
        levelSelectPanel.Init();
        levelSelectPanel.gameObject.SetActive(false);
        characterSelectPanel.Init();
        characterSelectPanel.gameObject.SetActive(false);
        battlePanel.Init();
        battlePanel.gameObject.SetActive(false);
        StartGame();
    }

    private void Update()
    {
        input_ControlManager.Update();
    }

    private void StartGame()
    {
        levelSelectPanel.Setup();
    }
}
