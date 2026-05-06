using System;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class UIManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private GameObject popupLogin;
    [SerializeField] private GameObject popupRegister;
    [SerializeField] private GameObject darkBg;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject gameMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject volumenMenu;
    [SerializeField] private GameObject brilloMenu;
    [SerializeField] private GameObject sensibilidadMenu;
    [SerializeField] private GameObject contolMenu;
    [SerializeField] private GameObject idiomaMenu;
    [SerializeField] private GameObject popupDifficulty;
    [SerializeField] private GameObject VideoPlayer;
    public float volumen;
    public float Ambience;
    public float brillo;
    public float sensibilidad;
    public bool WASDkeys;
    [SerializeField] private Slider sliderVolumen;
    [SerializeField] private Slider sliderAmbiente;
    [SerializeField] private Slider sliderBrillo;
    [SerializeField] private Slider sliderSensibilidad;
    [SerializeField] private TMP_InputField usernameInput;
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] private TMP_InputField registerUsernameInput;
    [SerializeField] private TMP_InputField registerPasswordInput;
    [SerializeField] private TMP_InputField registerConfirmPasswordInput;
    private bool isPasswordHidden;
    private bool isRegPasswordHidden;
    private bool isConfirmPasswordHidden;
    [SerializeField] private Button passB;
    [SerializeField] private Button regpassB;
    [SerializeField] private Button confpassB;
    [SerializeField] private Sprite iconVisible;
    [SerializeField] private Sprite iconHidden;
    [SerializeField] private UserDatabase database;
    [SerializeField] private TextMeshProUGUI textoVolumen;
    [SerializeField] private TextMeshProUGUI textoAmbiente;
    [SerializeField] private TextMeshProUGUI textoBrillo;
    [SerializeField] private TextMeshProUGUI textoSensibilidad;
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private AudioClip hoverSound;
    [SerializeField] private AudioClip PopUpSound;
    private AudioSource audioSource;
    private GameObject FadeManager;
    private GameObject VideoPlayerManager;

    [SerializeField] private TextMeshProUGUI ErrorReg;
    [SerializeField] private TextMeshProUGUI ErrorLogIn;
    [SerializeField] private CanvasGroup brightnessOverlay;
    [SerializeField] private RectHoverShow rectHoverShow;
    static int Difficulty = 1;
    void Awake()
    {
        DontDestroyOnLoad(brightnessOverlay.transform.root.gameObject);
    }
    void Start()
    {
        FadeManager = GameObject.Find("FadeManager");
        VideoPlayerManager = GameObject.Find("VideoPlayer");
        audioSource = GetComponent<AudioSource>();
        
        audioSource.clip = backgroundMusic;
        audioSource.loop = true;
        audioSource.Play();

        if (SceneMemory.Instance.logged == false)
        {
            mainMenu.SetActive(true);
            gameMenu.SetActive(false);
        }
        else 
        {
            mainMenu.SetActive(false);
            gameMenu.SetActive(true);
        }
        popupLogin.SetActive(false);
        popupRegister.SetActive(false);
        darkBg.SetActive(false);
        settingsMenu.SetActive(false);

        CambiarVolumen(0.5f);
        CambiarAmbiente(0.5f);
        CambiarBrightness(0.5f);
        CambiarSensibilidad(0.5f);
        WASDkeys=true;

        CargarSettings();

        sliderVolumen.value = volumen;
        sliderAmbiente.value = Ambience;
        sliderBrillo.value = brillo;
        sliderSensibilidad.value = sensibilidad;

        passwordInput.contentType = TMP_InputField.ContentType.Password;
        registerConfirmPasswordInput.contentType = TMP_InputField.ContentType.Password;
        registerConfirmPasswordInput.contentType = TMP_InputField.ContentType.Password;

        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(backgroundMusic);
    } 
    public void AbrirLogin()
    {
        audioSource.PlayOneShot(clickSound);
        audioSource.PlayOneShot(PopUpSound);
        popupRegister.SetActive(false);
        darkBg.SetActive(true);
        popupLogin.SetActive(true);
    }

    public void AbrirRegister()
    {
        audioSource.PlayOneShot(clickSound);
        audioSource.PlayOneShot(PopUpSound);
        popupLogin.SetActive(false);
        darkBg.SetActive(true);
        popupRegister.SetActive(true);
    }

    // CERRAR TODO
    public void CerrarPopups()
    {
        audioSource.PlayOneShot(clickSound);
        darkBg.SetActive(false);
        popupLogin.SetActive(false);
        popupRegister.SetActive(false);
        popupDifficulty.SetActive(false);
    }

    public void LoginIrAlMenuJuego()
    {
        audioSource.PlayOneShot(clickSound);
        OnLogin();
    }

    public void OnRegister()
    {
        audioSource.PlayOneShot(clickSound);
        if(database.UserExists(registerUsernameInput.text))
        {
            ErrorReg.gameObject.SetActive(true);
            ErrorReg.text = "Usuario ya existe.";
            return;
        }
        if (registerPasswordInput.text != registerConfirmPasswordInput.text && !string.IsNullOrEmpty(registerPasswordInput.text)&& !string.IsNullOrEmpty(registerUsernameInput.text))
        {
            ErrorReg.gameObject.SetActive(true);
            ErrorReg.text = "Las contraseñas no coinciden.";
            return;
        }
        database.Register(registerUsernameInput.text, registerPasswordInput.text);
        ErrorReg.gameObject.SetActive(false);
        CerrarPopups();
    }

    public void OnLogin()
    {
        bool success = database.Login(usernameInput.text, passwordInput.text);

        if (success)
        {
            mainMenu.SetActive(false);
            gameMenu.SetActive(true);
            rectHoverShow.ResetHover();
            Debug.Log("Welcome!");
            CerrarPopups();
            ErrorLogIn.gameObject.SetActive(false);
            SceneMemory.Instance.logged=true;
            return;
        }
        ErrorLogIn.gameObject.SetActive(true);

    }

    public void VolverMenu()
    {
        audioSource.PlayOneShot(clickSound);
        gameMenu.SetActive(false);
        mainMenu.SetActive(true);
        rectHoverShow.ResetHover();
        SceneMemory.Instance.logged=false;
    }

    // SALIR
    public void Salir()
    {
        audioSource.PlayOneShot(clickSound);
        Application.Quit();
        Debug.Log("Salir");
    }

    public void NuevaPartida()
    {
        audioSource.PlayOneShot(clickSound);
        darkBg.SetActive(true);
        popupDifficulty.SetActive(true);
    }

    public void ContinuarPartida()
    {
        audioSource.PlayOneShot(clickSound);
        SceneManager.LoadScene("Classroom1");
    }

    public void SeleccionarGrado1()
    {
        Difficulty = 1;
        CargarEscena();
    }
    public void SeleccionarGrado2()
    {
        Difficulty = 2;
        CargarEscena();
    }
    public void SeleccionarGrado3()
    {
        Difficulty = 3;
        CargarEscena();
    }
    void CargarEscena()
    {
        FadeManager.SetActive(true);
        audioSource.PlayOneShot(clickSound);
        popupDifficulty.SetActive(false);
        VideoPlayer.SetActive(true);
    }

    public void AbrirSettings()
    {
        
        audioSource.PlayOneShot(clickSound);
        settingsMenu.SetActive(true);
    }
    
    public void CerrarSettings()
    {
        audioSource.PlayOneShot(clickSound);
        settingsMenu.SetActive(false);
        GuardarSettings();
    }

    public void CambiarVolumen(float vol)
    {
        volumen = vol;
        int porcentaje = Mathf.RoundToInt(volumen * 100);
        textoVolumen.text = porcentaje + "%";
        Debug.Log("Volumen cambiado a: " + volumen);
    }

    public void CambiarAmbiente(float vol)
    {
        Ambience = vol;
        int porcentaje = Mathf.RoundToInt(vol * 100);
        textoAmbiente.text = porcentaje + "%";
        Debug.Log("Ambiente cambiado a: " + porcentaje + "%");
    }

    public void CambiarBrightness(float b)
    {
        brillo = b;
        int porcentaje = Mathf.RoundToInt(brillo * 100);
        textoBrillo.text = porcentaje + "%";
        Debug.Log("Brillo cambiado a: " + porcentaje + "%");

        if (brightnessOverlay != null)
        {
            
           brightnessOverlay.alpha=1-brillo;
           brightnessOverlay.blocksRaycasts = false;
           brightnessOverlay.interactable = false;
        }
    }
    public void CambiarSensibilidad(float sens)
    {
        sensibilidad = sens;
        int porcentaje = Mathf.RoundToInt(sensibilidad * 100);
        textoSensibilidad.text = porcentaje + "%";
        Debug.Log("Sensibilidad cambiada a: " + sensibilidad);
    }

    public void AbrirVolumen()
    {
        audioSource.PlayOneShot(clickSound);
        brilloMenu.SetActive(false);
        sensibilidadMenu.SetActive(false);
        contolMenu.SetActive(false);
        idiomaMenu.SetActive(false);
        volumenMenu.SetActive(true);
    }

    public void AbrirBrillo()
    {
        audioSource.PlayOneShot(clickSound);
        volumenMenu.SetActive(false);
        sensibilidadMenu.SetActive(false);
        contolMenu.SetActive(false);
        idiomaMenu.SetActive(false);
        brilloMenu.SetActive(true);
    }

    public void AbrirSensibilidad()
    {
        audioSource.PlayOneShot(clickSound);
        volumenMenu.SetActive(false);
        brilloMenu.SetActive(false);
        contolMenu.SetActive(false);
        idiomaMenu.SetActive(false);
        sensibilidadMenu.SetActive(true);
    }

    public void AbrirControles()
    {
        audioSource.PlayOneShot(clickSound);
        volumenMenu.SetActive(false);
        brilloMenu.SetActive(false);
        sensibilidadMenu.SetActive(false);
        idiomaMenu.SetActive(false);
        contolMenu.SetActive(true);
    }

    public void AbrirIdioma()
    {
        audioSource.PlayOneShot(clickSound);
        volumenMenu.SetActive(false);
        brilloMenu.SetActive(false);
        sensibilidadMenu.SetActive(false);
        contolMenu.SetActive(false);
        idiomaMenu.SetActive(true);
    }
    public void PlayHoverSound()
    {
        audioSource.PlayOneShot(hoverSound);
    }

    public void LogPasswordVisibility()
    {
        isPasswordHidden = !isPasswordHidden;
        if (isPasswordHidden)
        {
            passwordInput.contentType = TMP_InputField.ContentType.Password;

        }
        else
        {
            passwordInput.contentType = TMP_InputField.ContentType.Standard;
        }
        passwordInput.ForceLabelUpdate();
        passB.image.sprite = isPasswordHidden ? iconHidden : iconVisible;
    }

    public void RegPasswordVisibility()
    {
        isRegPasswordHidden = !isRegPasswordHidden;
        if(isRegPasswordHidden)
        {
            registerPasswordInput.contentType = TMP_InputField.ContentType.Password;
        }
        else
        {
            registerPasswordInput.contentType = TMP_InputField.ContentType.Standard;
        }
        registerPasswordInput.ForceLabelUpdate();
        regpassB.image.sprite = isRegPasswordHidden ? iconHidden : iconVisible;
    }

    public void ConfPasswordVisibility()
    {
        isConfirmPasswordHidden = !isConfirmPasswordHidden;
        if(isConfirmPasswordHidden)
        {
            registerConfirmPasswordInput.contentType = TMP_InputField.ContentType.Password;
        }
        else
        {
            registerConfirmPasswordInput.contentType = TMP_InputField.ContentType.Standard;
        }
        registerConfirmPasswordInput.ForceLabelUpdate();
        confpassB.image.sprite = isConfirmPasswordHidden ? iconHidden : iconVisible;
    }

    public void GuardarSettings()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "settings.txt");
        if (!File.Exists(filePath))
        {
            File.Create(filePath).Close();
            Debug.Log("Settings creado.");
        }
        File.Delete(filePath);
        File.AppendAllText(filePath, "Volumen: "+volumen+"\n");
        File.AppendAllText(filePath, "Ambiente: "+Ambience+"\n");
        File.AppendAllText(filePath,"Brillo: "+brillo+"\n");
        File.AppendAllText(filePath,"Sensibilidad: "+sensibilidad+"\n");
        File.AppendAllText(filePath, "Teclas: "+WASDkeys+"\n");
    }
    public void CargarSettings()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "settings.txt");
        if(!File.Exists(filePath))
        {
            return;
        }
        string[] lineas = File.ReadAllLines(filePath);

        string[] vol = lineas[0].Split(":");
        string[] amb = lineas[1].Split(":");
        string[] br = lineas[2].Split(":");
        string[] sens = lineas[3].Split(":");
        string[] keyboard = lineas[4].Split(":");

        volumen = float.Parse(vol[1].Trim());
        Ambience = float.Parse(amb[1].Trim());
        brillo = float.Parse(br[1].Trim());
        sensibilidad = float.Parse(sens[1].Trim());
        WASDkeys = bool.Parse(keyboard[1].Trim());
    }

}
