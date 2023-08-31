using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuButton : MonoBehaviour
{
    [SerializeField]
    private MenuButtonSO buttonData;
    private bool aux; 
    private Vector3 initialSize;
    void Start()
    {
        initialSize = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.localScale = aux ? Vector3.Lerp(initialSize, initialSize * 1.2f, 2): Vector3.Lerp(transform.localScale, initialSize, 1);
    }
    public void HoverEnter()
    {
        Debug.Log($"Hover enter {gameObject.name}");

        aux = true;
        transform.localScale = Vector3.Lerp(initialSize, initialSize * 1.2f, 1);
    }
    public void HoverExit()
    {
        aux = false;
        Debug.Log($"Hover exit {gameObject.name}");
        transform.localScale = Vector3.Lerp(transform.localScale, initialSize, 1);
    }
    public void OnClick()
    {
        SceneManager.LoadScene(buttonData.sceneName);
    }
}
