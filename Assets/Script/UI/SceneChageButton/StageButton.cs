using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageButton : MonoBehaviour
{
    public void OnClickStage1Button()
    {
        SceneChanger.Instance.SceneChange(SceneType.Stage1);
    }
    public void OnClickStage2Button()
    {
        SceneChanger.Instance.SceneChange(SceneType.Stage2);
    }
    public void OnClickStage3Button()
    {
        SceneChanger.Instance.SceneChange(SceneType.Stage3);
    }
}
