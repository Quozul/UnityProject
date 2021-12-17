using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionMenuScript : MonoBehaviour
{
    
    public GameObject inPausePanel;
    public GameObject inGamePanel;

    private void switchPanel(GameObject currentPanel, GameObject nextPanel)
    {
        currentPanel.SetActive(false);
        nextPanel.SetActive(true);
    }
    
    public void enablePause()
    {
        Time.timeScale = 0.0f;
        switchPanel(inGamePanel,inPausePanel);
    }
    public void disablePause()
    {
        Time.timeScale = 1.0f;
        switchPanel(inPausePanel,inGamePanel);
    }
    public void exitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
            Application.Quit();
    }
}
