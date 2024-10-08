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

    private PlayerAttachment Player;
    private PlayerController PlayerController;
    private PlayerMouse PlayersMouse;

    // Private variables
    private UIPauseScreen pauseUI;
    private UniversalAdditionalCameraData cameraData;

    private void Awake()
    {
        Physics2D.queriesHitTriggers = false;
        instance = this;
        if (Camera == null) 
        {
            Camera = Camera.main;
        }

        Player = FindObjectOfType<PlayerAttachment>();
        PlayerController = Player.GetComponent<PlayerController>();
        PlayersMouse = FindObjectOfType<PlayerMouse>();

        pauseUI = FindObjectOfType<UIPauseScreen>();
        cameraData = Camera.GetUniversalAdditionalCameraData();
    }

    private void Update()
    {
        if (PlayerController.InputEnabled)
        {
            // Switching between editing and playing modes
            if (Input.GetKeyDown(KeyCode.Space))
            {
                IsEditingBlocks = true;
                cameraData.cameraStack[0].GetComponentInChildren<Animator>().SetTrigger("FadeIn");
                //cameraData.cameraStack[0].enabled = true;

                Player.ShowAttachmentUI();
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                IsEditingBlocks = false;
                cameraData.cameraStack[0].GetComponentInChildren<Animator>().SetTrigger("FadeOut");
                //cameraData.cameraStack[0].enabled = false;

                PlayersMouse.DropObject();
                Player.HideUI();
            }
        }
        
        // Pausing
        /*
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
        */
    }
}
