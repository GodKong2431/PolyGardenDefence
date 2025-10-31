using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEndButton : MonoBehaviour
{
    public void OnClickRestartButton()
    {
        SceneChanger.Instance.SceneChange(SceneType.StageSelect);
    }
    public void OnClickTitleButton()
    {
        SceneChanger.Instance.SceneChange(SceneType.Title);
    }
    public void OnClickExitButton()
    {
        //Application.Quit();
    }
}
