using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneReloader : MonoBehaviour
{

    private void Update()
    {
        if (Input.GetButtonDown("Reload"))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        if (Input.GetButtonDown("Cancel"))
            SceneManager.LoadScene("MainMenu");
    }
}
