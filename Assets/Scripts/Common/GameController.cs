using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GameController : MonoBehaviour
{
    // Singleton script for main game mechanics

    public static GameController instance;

    // Basic settings
    public bool IsEditingBlocks;
    public bool IsPaused;
    [Space(10)]
    public Camera Camera;
    public PlayerAttachment Player;

    // Private variables
    private UIPauseScreen pauseUI;
    private UniversalAdditionalCameraData cameraData;

    private void Awake()
    {
        instance = this;

        pauseUI = FindObjectOfType<UIPauseScreen>();
        cameraData = Camera.GetUniversalAdditionalCameraData();
    }

    private void Update()
    {
        // Switching between editing and playing modes
        if (Input.GetKeyDown(KeyCode.Space))
        {
            IsEditingBlocks = true;
            foreach (Camera newCam in cameraData.cameraStack)
            {
                newCam.enabled = true;
            }

            Player.ShowAttachmentUI();
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            IsEditingBlocks = false;
            foreach (Camera newCam in cameraData.cameraStack)
            {
                newCam.enabled = false;
            }

            Player.HideUI();
        }
        
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
