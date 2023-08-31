using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
[CreateAssetMenu(fileName = "New MenuButton", menuName = "Button Data")]
public class MenuButtonSO : ScriptableObject
{
    public float sceneIndex;
    public string sceneName;
}
