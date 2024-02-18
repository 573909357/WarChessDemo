using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelView : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TextMeshProUGUI nameText;
    [NonSerialized] private LevelSetup levelSetup;
    
    public void Setup(LevelSetup levelSetup)
    {
        this.levelSetup = levelSetup;
        nameText.text = levelSetup.name;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GameInstance.gameInstance.battleManager.StartBattle(levelSetup);
        GameInstance.gameInstance.levelSelectPanel.gameObject.SetActive(false);
    }
}
