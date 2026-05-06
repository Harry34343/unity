using System;
using System.IO;
using System.Security.Cryptography;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.UI;

public class Config : MonoBehaviour
{
    [SerializeField] private Slider MusicSlider;
    [SerializeField] private Slider AmbienceSlider;
    [SerializeField] private Slider BrilloSlider;
    [SerializeField] private Slider SensSlider;
    [SerializeField] private GameObject VolOp;
    [SerializeField] private GameObject BrilloOp;
    [SerializeField] private GameObject SensOp;
    [SerializeField] private GameObject ControlOp;
    [SerializeField] private GameObject IdiomOp;
    [SerializeField] private TextMeshProUGUI textoVolumen;
    [SerializeField] private TextMeshProUGUI textoSensibilidad;
    [SerializeField] private TextMeshProUGUI textoBrillo;
    [SerializeField] private TextMeshProUGUI textoAmbiente;
    private CanvasGroup brightnessOverlay;
    private float music;
    private float ambience;
    private float brillo;
    private float sens;
    private bool key;
    private int language;
    [SerializeField] private Button WASD;
    [SerializeField] private Button Arrow;
    [SerializeField] private Image WASDEnabled;
    [SerializeField] private Image WASDDisabled;
    [SerializeField] private Image ArrowEnabled;
    [SerializeField] private Image ArrowDisabled;
    [SerializeField] private GameObject configOp;
    [SerializeField] private GameObject darkPanel;
    private AudioSource audioSource;
    [SerializeField] private AudioClip hoverSound;
    [SerializeField] private AudioClip clickSound;
    private PlayerMovement playerMovement;
    void Start()
    {
        brightnessOverlay = GameObject.FindAnyObjectByType<CanvasGroup>();
        playerMovement = GameObject.FindAnyObjectByType<PlayerMovement>();
        audioSource = gameObject.GetComponent<AudioSource>();
        VolOp.SetActive(false);
        BrilloOp.SetActive(false);
        SensOp.SetActive(false);
        ControlOp.SetActive(false);
        IdiomOp.SetActive(false);

        CargarSettings();
        CargarValue();
        CargarKey();
    }

    // Update is called once per frame
    void CargarValue()
    {
        MusicSlider.value = music;
        AmbienceSlider.value = ambience;
        BrilloSlider.value = brillo;
        SensSlider.value = sens;
    }

    void guardarSettings()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "settings.txt");
        if (!File.Exists(filePath))
        {
            File.Create(filePath).Close();
            Debug.Log("Settings creado.");
        }
        File.Delete(filePath);
        File.AppendAllText(filePath, "Volumen: "+music+"\n");
        File.AppendAllText(filePath, "Ambiente: "+ambience+"\n");
        File.AppendAllText(filePath,"Brillo: "+brillo+"\n");
        File.AppendAllText(filePath,"Sensibilidad: "+sens+"\n");
        File.AppendAllText(filePath, "Teclas: "+key+"\n");
    }
    void CargarSettings()
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

        music = float.Parse(vol[1].Trim());
        ambience = float.Parse(amb[1].Trim());
        brillo = float.Parse(br[1].Trim());
        this.sens = float.Parse(sens[1].Trim());
        key = bool.Parse(keyboard[1].Trim());
    }

    public void abrirVol()
    {
        ClickSound();
        VolOp.SetActive(true);
        BrilloOp.SetActive(false);
        SensOp.SetActive(false);
        ControlOp.SetActive(false);
        IdiomOp.SetActive(false);
    }

    public void abrirBrillo()
    {
        ClickSound();
        VolOp.SetActive(false);
        BrilloOp.SetActive(true);
        SensOp.SetActive(false);
        ControlOp.SetActive(false);
        IdiomOp.SetActive(false);
    }

    public void abrirSens()
    {
        ClickSound();
        VolOp.SetActive(false);
        BrilloOp.SetActive(false);
        SensOp.SetActive(true);
        ControlOp.SetActive(false);
        IdiomOp.SetActive(false);
    }
    public void abrirControl()
    {
        ClickSound();
        VolOp.SetActive(false);
        BrilloOp.SetActive(false);
        SensOp.SetActive(false);
        ControlOp.SetActive(true);
        IdiomOp.SetActive(false);
    }
    public void abrirIdioma()
    {
        ClickSound();
        VolOp.SetActive(false);
        BrilloOp.SetActive(false);
        SensOp.SetActive(false);
        ControlOp.SetActive(false);
        IdiomOp.SetActive(true);
    }
     public void CambiarVolumen(float vol)
    {
        music = vol;
        int porcentaje = Mathf.RoundToInt(music * 100);
        textoVolumen.text = porcentaje + "%";
        Debug.Log("Volumen cambiado a: " + music);
    }

    public void CambiarAmbiente(float vol)
    {
        ambience = vol;
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
        this.sens= sens;
        int porcentaje = Mathf.RoundToInt(this.sens * 100);
        textoSensibilidad.text = porcentaje + "%";
        Debug.Log("Sensibilidad cambiada a: " + this.sens);
    }

    public void WASDkeys()
    {
        ClickSound();
        key = true;
        CargarKey();
    }
    public void ArrowKey()
    {
        ClickSound();
        key = false;
        CargarKey();
    }
     void CargarKey()
    {
        if (key)
        {
            WASD.image.sprite = WASDEnabled.sprite;
            Arrow.image.sprite = ArrowDisabled.sprite;
            playerMovement.SetInputScheme(true);
        }
        else
        {
            WASD.image.sprite = WASDDisabled.sprite;
            Arrow.image.sprite = ArrowEnabled.sprite;
            playerMovement.SetInputScheme(false);
        }
    }
    public void Salir()
    {
        ClickSound();
        guardarSettings();
        configOp.SetActive(false);
        darkPanel.SetActive(false);
    }
    public void HoverSound()
    {
        audioSource.PlayOneShot(hoverSound);
    }
    void ClickSound()
    {
        audioSource.PlayOneShot(clickSound);
    }
}
