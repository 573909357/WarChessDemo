using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterView_CharacterSelect : MonoBehaviour, IDragHandler, IPointerClickHandler
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI attackText;

    [NonSerialized] private CharacterSetup setup;
    
    public void Setup(CharacterSetup setup)
    {
        this.setup = setup;
        icon.sprite = setup.icon;
        nameText.text = setup.characterName;
        healthText.text = setup.maxHealth.ToString();
        attackText.text = setup.attack.ToString();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (GameInstance.gameInstance.input_ControlManager.currentModule is BattlePrepareControlModule prepareControlModule)
        {
            var position = GameInstance.gameInstance.mainCamera.ScreenToWorldPoint(eventData.position);
            position.z = transform.position.z;
            transform.position = position;
            prepareControlModule.selectedCharacterSetup = setup;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        
    }
}
