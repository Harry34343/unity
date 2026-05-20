using System.IO;
using JetBrains.Annotations;
using UnityEngine;

public class ConfigUserOp : MonoBehaviour
{
    private static ConfigUserOp Instance;
    private float music;
    private float ambience;
    private float brillo;
    private float sens;
    private bool key;
    private int language;
    private string crouch = "c";
    private string jump = "space";
    private string interact = "click";
    private string inv = "e";
    private string speed = "leftShift";
    private string pause = "escape";

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            CargarSettings();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
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
        string[] language = lineas[5].Split(":");

        music = float.Parse(vol[1].Trim());
        ambience = float.Parse(amb[1].Trim());
        brillo = float.Parse(br[1].Trim());
        this.sens = float.Parse(sens[1].Trim());
        key = bool.Parse(keyboard[1].Trim());
        this.language = int.Parse(language[1].Trim());
    }

    public void guardarSettings()
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
        File.AppendAllText(filePath, "Idioma: "+language+"\n");
    }

    public void setBrillo(float b)
    {
        this.brillo = b;
    }
    
    public void setMusic(float m)
    {
        this.music = m;
    }
    public void setAmbience(float a)
    {
        this.ambience = a;
    }
    public void setSens(float s)
    {
        this.sens = s;
    }
    public void setLanguage(int l)
    {
        this.language = l;
    }
    public void setKey(bool k)
    {
        this.key = k;
    }
    
    public bool getKey()
    {
        return this.key;
    }
    public int getLanguage()
    {
        return this.language;
    }
    public float getSens()
    {
        return this.sens;
    }
    public float getAmbience()
    {
        return this.ambience;
    }
    public float getMusic()
    {
        return this.music;
    }
    public float getBrillo()
    {
        return this.brillo;
    }
    
    public void setCrouch(string button)
    {
        this.crouch = button;
    }
    public void setJump(string button)
    {
        this.jump = button;
    }
    public void setRun(string button)
    {
        this.speed = button;
    }
    public void setInteract(string button)
    {
        this.interact = button;
    }
    public void setInventory(string button)
    {
        this.inv = button;
    }
    public void setPause(string button)
    {
        this.pause = button;
    }
    public string getPause()
    {
        return this.pause;
    }
    public string getInventory()
    {
        return this.inv;
    }
    public string getInteract()
    {
        return this.interact;
    }
    public string getRun()
    {
        return this.speed;
    }
    public string getJump()
    {
        return this.jump;
    }
    public string getCrouch()
    {
        return this.crouch;
    }
    public static ConfigUserOp getInstance()
    {
        return Instance;
    }
}
