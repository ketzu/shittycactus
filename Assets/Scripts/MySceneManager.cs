using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneManager : MonoBehaviour
{
    private Coroutine _active_loadscene;

    public bool close = true;

    private void Start()
    {
        if(SceneManager.GetActiveScene().name == "MainMenu")
        {
            Cursor.visible = true;
        }
        else
        {
            Cursor.visible = false;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (SceneManager.GetActiveScene().name != "MainMenu" || !close)
                SceneManager.LoadScene("MainMenu");
            else
                Application.Quit();
        }
    }

    public void AllowClosing()
    {
        close = true;
    }

    public void DenyClosing()
    {
        close = false;
    }

    public void loadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void loadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void reloadSceneDelayed(float seconds)
    {
        if (_active_loadscene != null)
        {
            StopCoroutine(_active_loadscene);
        }
        _active_loadscene = StartCoroutine(loadSceneWait(SceneManager.GetActiveScene().buildIndex, seconds));
    }

    private IEnumerator loadSceneWait(int buildindex, float seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);
        SceneManager.LoadScene(buildindex);
    }

    public void QuitApplication()
    {
        Application.Quit();
    }
}
