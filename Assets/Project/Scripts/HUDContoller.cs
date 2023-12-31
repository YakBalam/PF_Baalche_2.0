using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.Events;

public class HUDContoller : MonoBehaviour
{
    public PlayerData playerData;

    public GameObject HUDPanel;
    public GameObject poderPanel;
    public GameObject menuPoderPanel;
    public GameObject borrosoPanel;
    public GameObject pausaPanel;
    public GameObject optionsPanel;
    public GameObject advertenciaPanel;
    public GameObject sonidoPanel;
    public GameObject controlesPanel;
    public GameObject vidaBossPanel;

    public Image vida;
    
    public Image aguaImage;
    public Image fuergoImage;    

    public Button continuarButton;
    public Button opcionesButton;
    public Button salirButton;
    
    public Button regresarFromOpcionesButton;
    public Button sonidoButton;
    public Button videoButton;
    public Button controlesButton;

    public Button regresarFromSonidoButton;
    public Slider musicSlider;
    public Slider sFxSlider;
    public Slider ambientalSlider;

    public Button regresarFromControlesButton;

    public Button siFromAdvertenciaButton;
    public Button noFromAdvertenciaButton;

    public Button nullPoderButton;
    public Button poder1Button;
    public Button poder2Button;
    public Button poder3Button;
    public Button poder4Button;
    public Button poder5Button;
    public Button poder6Button;

    public TextMeshProUGUI racionesText;

    public AudioMixer mainAudioMixer;

    public bool menuPower;

    private float currentMusicVolume;
    private float currentSFxVolume;
    private float currentAmbientalVolume;

    void Start()
    {
        CargaDatos();
        CleanPanels();
        int indiceEscenaActual = SceneManager.GetActiveScene().buildIndex;
        if (indiceEscenaActual != 1)
        {
            ShowHUD();
        }
        menuPower = false;
        
        // Menu de Pausa
        continuarButton.onClick.AddListener(QuitPausa);
        opcionesButton.onClick.AddListener(ShowOpciones);
        salirButton.onClick.AddListener(ShowAdvertencia);

        // Menu de Opciones
        regresarFromOpcionesButton.onClick.AddListener(QuitOpciones);
        sonidoButton.onClick.AddListener(ShowSonido);
        //boton de Video
        controlesButton.onClick.AddListener(ShowControles);

        //Menu Sonido
        regresarFromSonidoButton.onClick.AddListener(QuitSonido);
        if(mainAudioMixer.GetFloat("musicVolume", out currentMusicVolume))
            musicSlider.value = currentMusicVolume;
        if (mainAudioMixer.GetFloat("SFxVolume", out currentSFxVolume))
            sFxSlider.value = currentSFxVolume;
        if (mainAudioMixer.GetFloat("ambientalVolume", out currentAmbientalVolume))
            ambientalSlider.value = currentAmbientalVolume;

        //Menu de Controles
        regresarFromControlesButton.onClick.AddListener(QuitControles);

        //Menu de Advertencia
        siFromAdvertenciaButton.onClick.AddListener(ShowMainMenu);
        noFromAdvertenciaButton.onClick.AddListener(QuitAdvertencia);

        //Menu Rueda Poderes
        nullPoderButton.onClick.AddListener(() =>
        {
            SetPoderActivo(0);
        });
        poder1Button.onClick.AddListener(() =>
        {
            SetPoderActivo(1);
        });
        poder2Button.onClick.AddListener(() =>
        {
            SetPoderActivo(2);
        });
        poder3Button.onClick.AddListener(() =>
        {
            SetPoderActivo(3);
        });
        poder4Button.onClick.AddListener(() =>
        {
            SetPoderActivo(4);
        });
        poder5Button.onClick.AddListener(() =>
        {
            SetPoderActivo(5);
        });
        poder6Button.onClick.AddListener(() =>
        {
            SetPoderActivo(6);
        });

    }

    void CargaDatos()
    {
        if (PlayerPrefs.HasKey("Vidas"))
        {
            playerData.vidas = PlayerPrefs.GetInt("Vidas");
            playerData.nivelMax = PlayerPrefs.GetInt("NivelMax");
            playerData.raciones = PlayerPrefs.GetInt("Raciones");
            playerData.racionesMax = PlayerPrefs.GetInt("RacionesMax");
            playerData.poderActivo = PlayerPrefs.GetInt("PoderActivo");
            playerData.vida = PlayerPrefs.GetFloat("Vida");
            playerData.vidaMax = PlayerPrefs.GetFloat("VidaMax");
            playerData.racion = PlayerPrefs.GetFloat("Racion");
            playerData.ataqueLigero = PlayerPrefs.GetFloat("AtaqueLigero");
            playerData.ataquePesado = PlayerPrefs.GetFloat("AtaquePesado");
            playerData.poder = PlayerPrefs.GetFloat("Poder");
        }
        else
        {
            playerData.vidas = 5;
            playerData.nivelMax = 2;
            playerData.raciones = 3;
            playerData.racionesMax = 5;
            playerData.poderActivo = 0;
            playerData.vida = 200f;
            playerData.vidaMax = 200f;
            playerData.racion = 20f;
            playerData.ataqueLigero = 8;
            playerData.ataquePesado = 12;
            playerData.poder = 5;
}
    }

    // HUD
    void ShowHUD()
    {
        CleanPanels();
        HUDPanel.SetActive(true);
        ShowVida();
        ShowRaciones();
        
        if (playerData.nivelMax < 1)
        {
            poderPanel.SetActive(false);
        }
        else
        {
            poderPanel.SetActive(true);
            CleanPoderesActivo();
            ActivePoderRueda();

        }
        
    }

    void MaxVida()
    {
        playerData.vidaMax = 100f;
    }

    // Actualiza la barra de vida
    void ShowVida()
    {
        // El porcentaje de la vida actual en PlayerData.vida lo pasamos a un 
        // rango de 0 a 1 para poder pintarlo en la imagen de la barra de vida
        vida.fillAmount = playerData.vida / playerData.vidaMax;
    }

    void ShowRaciones()
    {
        racionesText.text = "x" + playerData.raciones.ToString();
    }

    public void NewRacion()
    {
        playerData.raciones = playerData.raciones + 1;
        ShowRaciones();
    }
    
    void EatRacion()
    {
        playerData.raciones = playerData.raciones - 1;

        if(playerData.vida + playerData.racion > playerData.vidaMax)
        {
            playerData.vida = playerData.vidaMax;
        }
        else
        {
            playerData.vida += playerData.racion;
        }

        ShowRaciones();
    }

    void Damage()
    {
        if (playerData.vida - 16f < 0)
        {
            playerData.vida = 0f;
        }
        else
        {
            playerData.vida -= 16f;
        }
    }

    void CleanPoderesActivo ()
    {
        aguaImage.enabled = false;
        fuergoImage.enabled = false;
        // poder 3
        // poder 4
        // poder 5
        // poder 6
    }

    void GetPoderActivo()
    {
        switch (playerData.poderActivo)
        {
            case 0:
                CleanPoderesActivo();
                break;
            case 1:
                CleanPoderesActivo();
                aguaImage.enabled = true;
                break;
            case 2:
                CleanPoderesActivo();
                fuergoImage.enabled = true;
                break;
            // 4 casos restantes
            // ...
        }
    }

    void SetPoderActivo(int power)
    {
        switch(power)
        {
            case 0:
                playerData.poderActivo = 0;
                break;
            case 1:
                playerData.poderActivo = 1;
                break;
            case 2:
                playerData.poderActivo = 2;
                break;
            case 3:
                playerData.poderActivo = 3;
                break;
            case 4:
                playerData.poderActivo = 4;
                break;
            case 5:
                playerData.poderActivo = 5;
                break;
            case 6:
                playerData.poderActivo = 6;
                break;
        }
    }

    // Rueda Poder
    void ShowMenuPoder()
    {
        menuPoderPanel.SetActive(true);
    }

    void QuitMenuPoder()
    {
        menuPoderPanel.SetActive(false);
        GetPoderActivo();

    }

    void DisablePoderes()
    {
        nullPoderButton.gameObject.SetActive(true);
        poder1Button.gameObject.SetActive(false);
        poder2Button.gameObject.SetActive(false);
        poder3Button.gameObject.SetActive(false);
        poder4Button.gameObject.SetActive(false);
        poder5Button.gameObject.SetActive(false);
        poder6Button.gameObject.SetActive(false);
    }

    void ActivePoderRueda()
    {
        DisablePoderes();
        switch (playerData.nivelMax)
        {
            case 1:
                nullPoderButton.gameObject.SetActive(true);
                poder1Button.gameObject.SetActive(true);
                break;
            case 2:
                nullPoderButton.gameObject.SetActive(true);
                poder1Button.gameObject.SetActive(true);
                poder2Button.gameObject.SetActive(true);
                break;
            case 3:
                nullPoderButton.gameObject.SetActive(true);
                poder1Button.gameObject.SetActive(true);
                poder2Button.gameObject.SetActive(true);
                poder3Button.gameObject.SetActive(true);
                break;
            case 4:
                nullPoderButton.gameObject.SetActive(true);
                poder1Button.gameObject.SetActive(true);
                poder2Button.gameObject.SetActive(true);
                poder3Button.gameObject.SetActive(true);
                poder4Button.gameObject.SetActive(true);
                break;
            case 5:
                nullPoderButton.gameObject.SetActive(true);
                poder1Button.gameObject.SetActive(true);
                poder2Button.gameObject.SetActive(true);
                poder3Button.gameObject.SetActive(true);
                poder4Button.gameObject.SetActive(true);
                poder5Button.gameObject.SetActive(true);
                break;
            case 6:
                nullPoderButton.gameObject.SetActive(true);
                poder1Button.gameObject.SetActive(true);
                poder2Button.gameObject.SetActive(true);
                poder3Button.gameObject.SetActive(true);
                poder4Button.gameObject.SetActive(true);
                poder5Button.gameObject.SetActive(true);
                poder6Button.gameObject.SetActive(true);
                break;
            case 0:
                DisablePoderes();
                break;
        }
    }

    // Menus de Pausa
    void ShowPausa()
    {
        borrosoPanel.SetActive(true);
        pausaPanel.SetActive(true);
    }

    void QuitPausa()
    {
        borrosoPanel.SetActive(false);
        pausaPanel.SetActive(false);
    }

    void ShowOpciones()
    {
        optionsPanel.SetActive(true);
    }

    void QuitOpciones()
    {
        optionsPanel.SetActive(false);
    }

    void ShowSonido()
    {
        sonidoPanel.SetActive(true);
    }

    void QuitSonido()
    {
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

    void ShowControles()
    {
        controlesPanel.SetActive(true);
    }

    void QuitControles()
    {
        controlesPanel.SetActive(false);
    }

    void ShowAdvertencia()
    {
        advertenciaPanel.SetActive(true);
    }

    void QuitAdvertencia()
    {
        advertenciaPanel.SetActive(false);
    }

    void ShowMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    void ShowBossLife()
    {
        vidaBossPanel.SetActive(true);
    }

    void QuitBossLife()
    {
        vidaBossPanel.SetActive(false);
    }

    public void StartIntroBoss()
    {
        CleanPanels();
    }
    public void EndIntroBoss()
    {
        ShowHUD();
        ShowBossLife();
    }

    void CleanPanels ()
    {
        HUDPanel.SetActive(false);
        menuPoderPanel.SetActive(false);
        borrosoPanel.SetActive(false);
        pausaPanel.SetActive(false);
        optionsPanel.SetActive(false);
        advertenciaPanel.SetActive(false);
        sonidoPanel.SetActive(false);
        controlesPanel.SetActive(false);
        vidaBossPanel.SetActive(false);
    }


    void Update()
    {
        
        // Menu Pausa
        if(Input.GetKeyDown(KeyCode.P))
        {
            if (pausaPanel.activeSelf == false)
            {
                Debug.Log("Pausa");
                SoundFxManager.Instance.ShowMenuPausa();
                ShowPausa();
            }
        }

        // Menu Poderes
        if(Input.GetKey(KeyCode.Q))
        {
            ShowMenuPoder();
            if(menuPower==false)
            {
                menuPower = true;
                SoundFxManager.Instance.ShowMenuPower();
            }
        }
        else
        {
            QuitMenuPoder();
            menuPower = false;
        }

        // Recuperar salud
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (playerData.raciones > 0 && playerData.vida < playerData.vidaMax)
            {
                EatRacion();
                ShowVida();
            }
            else
            {
                Debug.Log("No puedes curarte");
            }
        }

        // Da�o
        if (Input.GetKeyDown(KeyCode.M))
        {
            Damage();
            ShowVida();
        }

        ShowVida();



    }


}
