using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CharacterSelectPanel : MonoBehaviour
{
    [SerializeField] private CharacterView_CharacterSelect characterView;
    [SerializeField] private Button startButton;
    [SerializeField] private HorizontalLayoutGroup layoutGroup;
    
    [NonSerialized] private List<CharacterView_CharacterSelect> showingCharacterView;
    [NonSerialized] private List<CharacterSetup> characterSetups;


    public void Init()
    {
        characterView.gameObject.SetActive(false);
        showingCharacterView = new List<CharacterView_CharacterSelect>();
        startButton.onClick.AddListener(StartGame);
    }
    
    public void Setup()
    {
        gameObject.SetActive(true);
        characterSetups = new List<CharacterSetup>(GameInstance.gameInstance.gameConfig.characterSetups);
        Refresh();
    }

    public void Unset()
    {
        DispawnAll();
        gameObject.SetActive(false);
    }
    
    public void Refresh()
    {
        DispawnAll();
        foreach (var characterSetup in characterSetups)
        {
            var view = Instantiate(characterView, characterView.transform.parent);
            view.Setup(characterSetup);
            view.gameObject.SetActive(true);
            showingCharacterView.Add(view);
        }
    }

    public void OnSelect(CharacterSetup setup)
    {
        characterSetups.Remove(setup);
        Refresh();
    }
    
    private void DispawnAll()
    {
        foreach (var characterView in showingCharacterView)
            Destroy(characterView.gameObject);
        showingCharacterView.Clear();
    }

    private void StartGame()
    {
        GameInstance.gameInstance.battleManager.battleState = BattleManager.BattleState.BattleStart;
    }
}
