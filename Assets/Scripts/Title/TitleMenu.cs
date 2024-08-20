using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore;
using UnityEngine.SceneManagement;
using TMPro;

public class TitleMenu : MonoBehaviour
{
    // A basic title-screen menu for accessing the options and credits.
    // Test
    enum MenuState
    {
        Title,
        Options,
        Credits
    }
    MenuState currentMenuState;

    public GameObject[] MenuButtons;
    
    [Space(10)]
    public GameObject OptionsUI;
    public GameObject[] OptionsButtons;
    public Transform OptionsHighlight;
    public AudioToggle[] AudioToggles;

    [Space(10)]
    public GameObject CreditsUI;

    private int selectedMenuOption;
    private Animator fadeAnimator;

    private void Awake()
    {
        fadeAnimator = FindObjectOfType<Animator>();
    }

    private void Update()
    {
        // Main menu navigation
        /*
        if (currentMenuState != MenuState.Credits)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                MenuScrollDown();
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                MenuScrollUp();
            }
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            SelectButton();
        }

        if (currentMenuState != MenuState.Title)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                BackButton();
            }
        }

        if (currentMenuState == MenuState.Options)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                ToggleVolume(-1);
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                ToggleVolume(1);
            }
        }
        */
    }

    public void MenuScrollUp()
    {
        selectedMenuOption++;
        SwitchMenuOption();
    }

    public void MenuScrollDown()
    {
        selectedMenuOption--;
        SwitchMenuOption();
    }

    private IEnumerator SceneTransition()
    {
        fadeAnimator.SetTrigger("FadeOut");
        yield return new WaitForSeconds(fadeAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.length);
        SceneManager.LoadScene(2);
        StopAllCoroutines();
    }

    public void SwitchMenuOption()
    {
        switch (currentMenuState)
        {
            case MenuState.Title:
                if (selectedMenuOption > MenuButtons.Length - 1)
                {
                    selectedMenuOption--;
                }

                if (selectedMenuOption < 0)
                {
                    selectedMenuOption++;
                }

                for (int i = 0; i < MenuButtons.Length; i++)
                {
                    if (i == selectedMenuOption)
                    {
                        MenuButtons[i].SetActive(true);
                    }
                    else
                    {
                        MenuButtons[i].SetActive(false);
                    }
                }
                break;

            case MenuState.Options:
                if (selectedMenuOption > MenuButtons.Length - 1)
                {
                    selectedMenuOption = 0;
                }

                if (selectedMenuOption < 0)
                {
                    selectedMenuOption = MenuButtons.Length - 1;
                }

                OptionsHighlight.position = new Vector2(OptionsHighlight.position.x, OptionsButtons[selectedMenuOption].transform.position.y);
                break;
        }
    }

    public void ManualSwitchMenuOption(int newOption)
    {
        selectedMenuOption = newOption;
        OptionsHighlight.position = new Vector2(OptionsHighlight.position.x, OptionsButtons[selectedMenuOption].transform.position.y);
    }

        public void SelectButton()
    {
        switch (currentMenuState)
        {
            case MenuState.Title:
                switch (selectedMenuOption)
                {
                    case 0:
                        StartCoroutine(SceneTransition());
                        break;

                    case 1:
                        OptionsUI.SetActive(true);
                        currentMenuState = MenuState.Options;
                        selectedMenuOption = 0;
                        OptionsHighlight.position = new Vector2(OptionsHighlight.position.x, OptionsButtons[selectedMenuOption].transform.position.y);
                        break;

                    case 2:
                        CreditsUI.SetActive(true);
                        currentMenuState = MenuState.Credits;
                        break;
                }
                break;

            case MenuState.Options:
                switch (selectedMenuOption)
                {
                    case 0:
                    case 1:
                        ToggleVolume(1);
                        break;

                    case 2:
                        OptionsUI.SetActive(false);
                        currentMenuState = MenuState.Title;
                        selectedMenuOption = 1;
                        break;
                }
                break;

            case MenuState.Credits:
                CreditsUI.SetActive(false);
                currentMenuState = MenuState.Title;
                selectedMenuOption = 2;
                break;
        }
    }

    public void BackButton()
    {
        switch (currentMenuState)
        {
            case MenuState.Options:
                OptionsUI.SetActive(false);
                currentMenuState = MenuState.Title;
                selectedMenuOption = 1;
                break;

            case MenuState.Credits:
                CreditsUI.SetActive(false);
                currentMenuState = MenuState.Title;
                selectedMenuOption = 2;
                break;
        }
    }

    public void ToggleVolume(int dir)
    {
        switch (selectedMenuOption)
        {
            case 0:
                AudioToggles[0].ToggleVolume(dir);
                break;

            case 1:
                AudioToggles[1].ToggleVolume(dir);
                break;
        }
    }
}
