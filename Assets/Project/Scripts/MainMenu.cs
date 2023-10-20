using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject mainPanel;
    public GameObject optionsPanel;
    public GameObject sonidoPanel;
    public GameObject controlPanel;
    public GameObject creditsPanel;

    public Button coninueButton;
    public Button newGameButton;
    public Button optionsButton;
    public Button sonidoButton;
    public Button creditsButton;
    public Button controlButton;
    public Button backFromOptionsButton;
    public Button backFromCreditsButton;
    public Button backFromControlButton;
    public Button regresarFromSonidoButton;

    public Slider musicSlider;
    public Slider sFxSlider;
    public Slider ambientalSlider;

    public AudioMixer mainAudioMixer;

    private float currentMusicVolume;
    private float currentSFxVolume;
    private float currentAmbientalVolume;


    void Start()
    {
        coninueButton.onClick.AddListener(PlayGame);
        newGameButton.onClick.AddListener(PlayGame);
        optionsButton.onClick.AddListener(ShowOptionsPanel);
        sonidoButton.onClick.AddListener(ShowSonido);
        creditsButton.onClick.AddListener(ShowCreditsPanel);
        controlButton.onClick.AddListener(ShowControlPanel);
        backFromOptionsButton.onClick.AddListener(QuitOptionsPanel);
        backFromCreditsButton.onClick.AddListener(QuitCreditsPanel);
        backFromControlButton.onClick.AddListener(QuitControlPanel);
        ShowMainPanel();

        //Menu Sonido
        regresarFromSonidoButton.onClick.AddListener(QuitSonido);
        if (mainAudioMixer.GetFloat("musicVolume", out currentMusicVolume))
            musicSlider.value = currentMusicVolume;
        if (mainAudioMixer.GetFloat("SFxVolume", out currentSFxVolume))
            sFxSlider.value = currentSFxVolume;
        if (mainAudioMixer.GetFloat("ambientalVolume", out currentAmbientalVolume))
            ambientalSlider.value = currentAmbientalVolume;
    }

    public void ShowOptionsPanel()
    {
        optionsPanel.SetActive(true);
        SoundFxManager.Instance.ShowMenuPausa();
    }
    void QuitOptionsPanel()
    {
        optionsPanel.SetActive(false);
        SoundFxManager.Instance.ShowMenuPausa();
    }

    void ShowSonido()
    {
        sonidoPanel.SetActive(true);
        SoundFxManager.Instance.ShowMenuPausa();
    }

    void QuitSonido()
    {
        sonidoPanel.SetActive(false);
        SoundFxManager.Instance.ShowMenuPausa();
    }

    public void ShowControlPanel()
    {
        controlPanel.SetActive(true);
        SoundFxManager.Instance.ShowMenuPausa();
    }
    public void QuitControlPanel()
    {
        controlPanel.SetActive(false);
        SoundFxManager.Instance.ShowMenuPausa();
    }

    public void ShowCreditsPanel()
    {
        creditsPanel.SetActive(true);
        SoundFxManager.Instance.ShowMenuPausa();
    }
    public void QuitCreditsPanel()
    {
        creditsPanel.SetActive(false);
        SoundFxManager.Instance.ShowMenuPausa();
    }

    public void ShowMainPanel()
    {
        CleanPanels();
        mainPanel.SetActive(true);
        SoundFxManager.Instance.ShowMenuPausa();
    }

    public void PlayGame()
    {
        SoundFxManager.Instance.ShowMenuPausa();
        SceneManager.LoadScene(1);
    }

    private void CleanPanels()
    {
        mainPanel.SetActive(false);
        optionsPanel.SetActive(false);
        controlPanel.SetActive(false);
        creditsPanel.SetActive(false);
        sonidoPanel.SetActive(false);
    }

    public void OnMusicVolumeChange(float volume)
    {
        mainAudioMixer.SetFloat("musicVolume", volume);
    }

    public void OnSFxVolumeChange(float volume)
    {
        mainAudioMixer.SetFloat("SFxVolume", volume);
    }

    public void OnAmbientalVolumeChange(float volume)
    {
        mainAudioMixer.SetFloat("ambientalVolume", volume);
    }
}
