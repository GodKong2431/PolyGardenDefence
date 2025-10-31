using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayButton : MonoBehaviour
{
    [Header("SceneChanger")]
    [SerializeField]private SceneChanger _sceneChanger;


    public void OnClickPlayButton()
    {
        _sceneChanger.SceneChange(SceneType.StageSelect);
    }
}
