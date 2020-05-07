using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_controller : MonoBehaviour
{
    bool fadeinOnce = false;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!fadeinOnce)
        {
            fadeinOnce = true;
            GameEvents.current.FadeStateChange(false);
        }
    }

    public void OnPlayButtonClicked()
    {
        GameEvents.current.FadeStateChange(true);
        SceneManager.LoadScene("PlayGame");
    }

    public void OnTutorialButtonClicked()
    {
        GameEvents.current.FadeStateChange(true);
        SceneManager.LoadScene("Tutorial");
    }

    public void OnMainMenuButtonClicked()
    {
        GameEvents.current.FadeStateChange(true);
        SceneManager.LoadScene("MainMenu");
    }

    public void OnQuitButtonClicked()
    {
        Application.Quit();
    }
}
