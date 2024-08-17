using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Singleton script for main game mechanics

    public static GameController instance;

    // Basic settings
    public bool IsEditingBlocks;
    public bool IsPaused;

    // Private variables
    private UIPauseScreen pauseUI;

    private void Awake()
    {
        instance = this;

        pauseUI = FindObjectOfType<UIPauseScreen>();
    }

    private void Update()
    {
        // Switching between editing and playing modes
        
        // Pausing
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (!IsPaused)
            {
                Time.timeScale = 0;
                IsPaused = true;
                pauseUI.PauseAnim();
            }
            else
            {
                Time.timeScale = 1;
                IsPaused = false;
                pauseUI.UnpauseAnim();
            }
        }
    }
}
