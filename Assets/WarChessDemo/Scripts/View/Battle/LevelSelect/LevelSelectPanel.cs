using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectPanel : MonoBehaviour
{
    [SerializeField] private LevelView levelViewPrefab;
    
    public void Init()
    {
        levelViewPrefab.gameObject.SetActive(false);
        foreach (var levelSetup in GameInstance.gameInstance.gameConfig.levelSetups)
        {
            var view = Instantiate(levelViewPrefab, levelViewPrefab.transform.parent);
            view.Setup(levelSetup);
            view.gameObject.SetActive(true);
        }
    }

    public void Setup()
    {
        gameObject.SetActive(true);
    }
}
