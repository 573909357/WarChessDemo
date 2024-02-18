using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterView_Battle : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI attackText;

    [NonSerialized] public BattleCharacter character;
    
    public void Setup(BattleCharacter character)
    {
        this.character = character;
        UpdateView();
    }

    public void UpdateView()
    {
        icon.sprite = character.setup.icon;
        healthText.text = character.health.ToString();
        attackText.text = character.attack.ToString();
        transform.position = GameInstance.gameInstance.battleTileMapView.controller.GetCellCenterWorld(character.position);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (GameInstance.gameInstance.input_ControlManager.currentModule is BattleControlModule battleControlModule)
        {
            battleControlModule.clickedCharacterView = character.position;
        }
    }
}
