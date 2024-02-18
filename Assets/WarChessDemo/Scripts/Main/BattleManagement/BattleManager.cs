using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public BattleState battleState;
    public MoveState moveState;
    public BattleController battleController;
    public LevelSetup levelSetup;
    public List<BattleCharacter> enemies;
    public List<BattleCharacter> characters;
    
    
    public void Init()
    {
        battleState = BattleState.None;
        battleController = new BattleController();
        battleController.Init();
    }

    public void StartBattle(LevelSetup levelSetup)
    {
        this.levelSetup = levelSetup;
        StartCoroutine(BattleProcess());
        StartCoroutine(BattleMoveProcess());
    }

    public BattleCharacter GetEnemy(Vector3Int position)
    {
        return enemies.Find(e => e.position == position);
    }

    public BattleCharacter GetPlayerCharacter(Vector3Int position)
    {
        return characters.Find(c => c.position == position);
    }
    
    public BattleCharacter GetCharacter(Vector3Int position)
    {
        var result = GetPlayerCharacter(position);
        if (result == null)
            return GetEnemy(position);
        return result;
    }

    public void AddCharacter(CharacterSetup selectedCharacterSetup, Vector3Int position)
    {
        var character = new BattleCharacter(selectedCharacterSetup, false);
        character.position = position;
        characters.Add(character);
        GameInstance.gameInstance.battlePanel.SpawnCharacter(character);
    }
    
    public void Kill(BattleCharacter character)
    {
        if (character.isEnemy)
            enemies.Remove(character);
        else
            characters.Remove(character);
        GameInstance.gameInstance.battlePanel.DispawnCharacter(character);
    }
    
    private IEnumerator BattleProcess()
    {
        battleState = BattleState.Prepare;
        GameInstance.gameInstance.input_ControlManager.currentModule = new BattlePrepareControlModule();
        GameInstance.gameInstance.characterSelectPanel.Setup();
        GameInstance.gameInstance.battleTileMapView.SetupMap(levelSetup);
        enemies = levelSetup.enemy.ConvertAll(enemyConfig => new BattleCharacter(enemyConfig.setup, true){position = enemyConfig.position});
        characters = new List<BattleCharacter>();
        GameInstance.gameInstance.battlePanel.Setup();
        
        yield return WaitState(BattleState.BattleStart);
        GameInstance.gameInstance.characterSelectPanel.Unset();
        GameInstance.gameInstance.input_ControlManager.currentModule = new BattleControlModule();
        GameInstance.gameInstance.battlePanel.OnStartBattle();
        
        while (battleState != BattleState.BattleEnd)
        {
            
            if (enemies.Count == 0 || characters.Count == 0)
            {
                battleState = BattleState.BattleEnd;
            }
            yield return null;
        }
        
        GameInstance.gameInstance.battlePanel.Unset();
        GameInstance.gameInstance.levelSelectPanel.Setup();
    }

    private IEnumerator BattleMoveProcess()
    {
        yield return WaitState(BattleState.BattleStart);
        moveState = MoveState.PlayerTurn;
        while (battleState == BattleState.BattleStart)
        {
            yield return new WaitUntil(() => moveState == MoveState.EnemyTurn);
            foreach (var enemy in enemies)
            {
                enemy.EnemyMove();
                yield return new WaitForSeconds(0.6f);
                if (enemy.EnemyAttack())
                    yield return new WaitForSeconds(0.6f);
            }

            moveState = MoveState.PlayerTurn;
            foreach (var character in characters)
            {
                character.moved = false;
            }
        }
    }

    private CustomYieldInstruction WaitState(BattleState state)
    {
        return new WaitUntil(() => battleState == state);
    }

    
    
    public enum BattleState
    {
        None,
        Prepare,
        BattleStart,
        BattleEnd,
    }

    public enum MoveState
    {
        PlayerTurn,
        EnemyTurn,
    }
}

public class BattlePrepareControlModule : Input_ControlManager.Module
{
    public Vector3Int? currentOverTile;
    public CharacterSetup selectedCharacterSetup;
    private Vector3Int? currentGreenTile;

    public BattlePrepareControlModule()
    {
        var battleTileMapView = GameInstance.gameInstance.battleTileMapView;
        battleTileMapView.controller.ClearAllTiles();
        var battleManager = GameInstance.gameInstance.battleManager;
        foreach (var (position, setup) in battleManager.levelSetup.cells)
        {
            if (setup.canSpawn)
            {
                battleTileMapView.controller.SetTile(position, GameInstance.gameInstance.gameConfig.defaultDisplayTile);
            }
        }
    }
    
    public override void Update()
    {
        var battleTileMapView = GameInstance.gameInstance.battleTileMapView;
        
        var tile = battleTileMapView.controller.WorldToCell(
            GameInstance.gameInstance.mainCamera.ScreenToWorldPoint(Input.mousePosition));
        if (battleTileMapView.controller.GetTile(tile) == null)
            currentOverTile = null;
        else
            currentOverTile = tile;
        
        if (!Input.GetMouseButton(0))
        {
            if (selectedCharacterSetup != null)
            {
                if (currentOverTile == null)
                    GameInstance.gameInstance.characterSelectPanel.Refresh();
                else
                {
                    GameInstance.gameInstance.characterSelectPanel.OnSelect(selectedCharacterSetup);
                    GameInstance.gameInstance.battleManager.AddCharacter(selectedCharacterSetup, currentOverTile.Value);
             
                }
              
                selectedCharacterSetup = null;
            }
        }
        
        UpdateTile();
        
        currentOverTile = null;
    }

    private void UpdateTile()
    {
        var battleTileMapView = GameInstance.gameInstance.battleTileMapView;
        if (selectedCharacterSetup == null)
        {
            if (currentGreenTile != null)
                battleTileMapView.controller.SetColor(currentGreenTile.Value, Color.white);
            return;
        }

        if (currentGreenTile != null && currentGreenTile != currentOverTile)
            battleTileMapView.controller.SetColor(currentGreenTile.Value, Color.white);

        if (currentOverTile != null)
            battleTileMapView.controller.SetColor(currentOverTile.Value, Color.green);
        currentGreenTile = currentOverTile;
    }
}

public class BattleControlModule : Input_ControlManager.Module
{
    public BattleCharacter selectedCharacter;
    public State state;
    
    private BattleManager battleManager;
    private BattleTileMapView battleTileMapView;
    private GameConfig gameConfig;

    public Vector3Int? clickedCharacterView;
    private Vector3Int? overTile;
    private Vector3Int? clickedTile;

    
    public BattleControlModule()
    {
        battleManager = GameInstance.gameInstance.battleManager;
        battleTileMapView = GameInstance.gameInstance.battleTileMapView;
        gameConfig = GameInstance.gameInstance.gameConfig;
        state = State.Normal;
        battleTileMapView.controller.ClearAllTiles();
    }
    
    public override void Update()
    {
        UpdateInput();
        switch (state)
        {
            case State.None:
                battleTileMapView.controller.ClearAllTiles();
                break;
            case State.Normal:
                battleTileMapView.controller.ClearAllTiles();
                if (clickedTile != null)
                {
                    selectedCharacter = battleManager.GetPlayerCharacter(clickedTile.Value);
                    if (selectedCharacter != null && !selectedCharacter.moved)
                        state = State.CharacterMove;
                }
                break;
            case State.CharacterMove:
                if (selectedCharacter == null)
                {
                    state = State.Normal;
                    return;
                }
                
                battleTileMapView.controller.ClearAllTiles();
                
                var canMove = selectedCharacter.GetCanMovePositions();
                foreach (var position in canMove)
                {
                    battleTileMapView.controller.SetTile(position, gameConfig.defaultDisplayTile);
                    battleTileMapView.controller.SetColor(position, Color.white);
                }

                if (clickedTile != null)
                {
                    if (canMove.Contains(clickedTile.Value))
                    {
                        selectedCharacter.Move(clickedTile.Value);
                        state = State.CharacterAttack;
                        return;
                    }
                    else
                    {
                        selectedCharacter = null;
                        state = State.Normal;
                        return;
                    }
                       
                }
                return;
            case State.CharacterAttack:
                if (selectedCharacter == null)
                {
                    state = State.Normal;
                    return;
                }
                
                battleTileMapView.controller.ClearAllTiles();
                
                var canAttack2 = selectedCharacter.GetCanAttackPositions();
                foreach (var position in canAttack2)
                {
                    battleTileMapView.controller.SetTile(position, gameConfig.defaultDisplayTile);
                    battleTileMapView.controller.SetColor(position, Color.red);
                }

                if (clickedTile != null)
                {
                    if (canAttack2.Contains(clickedTile.Value))
                    {
                        selectedCharacter.Attack(clickedTile.Value);
                        state = State.Normal;
                        selectedCharacter = null;
                        return;
                    }
                    else
                    {
                        state = State.Normal;
                        selectedCharacter = null;
                    }
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void UpdateInput()
    {
        overTile = null;
        clickedTile = null;
        
        var tile = battleTileMapView.controller.WorldToCell(
            GameInstance.gameInstance.mainCamera.ScreenToWorldPoint(Input.mousePosition));
        if (battleTileMapView.controller.GetTile(tile) == null)
            overTile = null;
        else
            overTile = tile;
        
        if (overTile != null && GameInstance.gameInstance.battleTileMapView.controllerInputEvents.frameData.HasFlag(TileMapInputEvents.Type.Click))
            clickedTile = overTile;
        else if (clickedCharacterView != null)
        {
            clickedTile = clickedCharacterView;
            clickedCharacterView = null;
        }

        if (clickedTile == null && Input.GetMouseButtonUp(0))
        {
            state = State.Normal;
            selectedCharacter = null;
        }
    }
    
    public enum State
    {
        None = 0,
        Normal = 1,
        CharacterMove = 2,
        CharacterAttack = 3,
    }
}