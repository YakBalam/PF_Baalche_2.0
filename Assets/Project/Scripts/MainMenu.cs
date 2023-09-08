using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject mainPanel;
    public GameObject optionsPanel;
    public GameObject controlPanel;
    public GameObject creditsPanel;

    public Button coninueButton;
    public Button newGameButton;
    public Button optionsButton;
    public Button creditsButton;
    public Button controlButton;
    public Button backFromOptionsButton;
    public Button backFromCreditsButton;
    public Button backFromControlButton;


    void Start()
    {
        coninueButton.onClick.AddListener(PlayGame);
        newGameButton.onClick.AddListener(PlayGame);
        optionsButton.onClick.AddListener(ShowOptionsPanel);
        creditsButton.onClick.AddListener(ShowCreditsPanel);
        controlButton.onClick.AddListener(ShowControlPanel);
        backFromOptionsButton.onClick.AddListener(ShowMainPanel);
        backFromCreditsButton.onClick.AddListener(ShowMainPanel);
        backFromControlButton.onClick.AddListener(ShowOptionsPanel);
        ShowMainPanel();
    }

    public void ShowOptionsPanel()
    {
        CleanPanels();
        optionsPanel.SetActive(true);
    }

    public void ShowControlPanel()
    {
        CleanPanels();
        controlPanel.SetActive(true);
    }

    public void ShowCreditsPanel()
    {
        CleanPanels();
        creditsPanel.SetActive(true);
    }

    public void ShowMainPanel()
    {
        CleanPanels();
        mainPanel.SetActive(true);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    private void CleanPanels()
    {
        mainPanel.SetActive(false);
        optionsPanel.SetActive(false);
        controlPanel.SetActive(false);
        creditsPanel.SetActive(false);
    }
}
