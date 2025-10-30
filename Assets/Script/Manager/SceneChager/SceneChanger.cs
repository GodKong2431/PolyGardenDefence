using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneType
{
    Title,
    StageSelect,
    Stage1,
    Stage2,
    Stage3
}
public class SceneChanger : SingleTon<SceneChanger>
{
    public void SceneChange(SceneType scene)
    {
        SceneManager.LoadScene(scene.ToString());
    }
}
