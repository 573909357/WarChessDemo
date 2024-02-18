using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattlePanel : MonoBehaviour
{
    [SerializeField] private CharacterView_Battle characterView;
    [SerializeField] private Button endTurnButton;
    
    [NonSerialized] private List<CharacterView_Battle> showingCharacterView;

    private BattleManager battleManager => GameInstance.gameInstance.battleManager;

    public void Init()
    {
        characterView.gameObject.SetActive(false);
        showingCharacterView = new List<CharacterView_Battle>();
        endTurnButton.onClick.AddListener(_OnEndTurnButtonClicked);
    }

    public void Setup()
    {
        gameObject.SetActive(true);
        foreach (var enemy in battleManager.enemies)
            SpawnCharacter(enemy);
    }

    public void OnStartBattle()
    {
        endTurnButton.gameObject.SetActive(true);
    }

    public void Unset()
    {
        foreach (var view in showingCharacterView)
            Destroy(view.gameObject);
        showingCharacterView.Clear();
        gameObject.SetActive(false);
        endTurnButton.gameObject.SetActive(false);
    }
    
    public void UpdateView()
    {
        foreach (var characterView in showingCharacterView)
        {
            characterView.UpdateView();
        }
    }
    
    public void SpawnCharacter(BattleCharacter character)
    {
        var view = Instantiate(characterView, characterView.transform.parent);
        view.Setup(character);
        view.gameObject.SetActive(true);
        showingCharacterView.Add(view);
    }
    
    public void DispawnCharacter(BattleCharacter character)
    {
        var view = showingCharacterView.Find(v => v.character == character);
        Destroy(view.gameObject);
        showingCharacterView.Remove(view);
    }

    private void _OnEndTurnButtonClicked()
    {
        GameInstance.gameInstance.battleManager.moveState = BattleManager.MoveState.EnemyTurn;
    }
}
