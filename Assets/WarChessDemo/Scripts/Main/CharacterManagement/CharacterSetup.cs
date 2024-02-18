using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class CharacterSetup : SerializedScriptableObject
{
    [SerializeField] public string characterName;
    [SerializeField] public Sprite icon;
    [SerializeField] public int maxHealth;
    [SerializeField] public int attack;
    [SerializeField] public int move;
}
