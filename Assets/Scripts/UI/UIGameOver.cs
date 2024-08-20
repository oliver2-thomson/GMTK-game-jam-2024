using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIGameOver : MonoBehaviour
{
    private bool canRestart = false;

    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
        image.enabled = false;
    }

    public void OnGameOver()
    {
        image.enabled = true;
        Time.timeScale = 0;
        canRestart = true;
    }

    public void Update()
    {
        if (image.enabled && canRestart)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Time.timeScale = 1;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
}
