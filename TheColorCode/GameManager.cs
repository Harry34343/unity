using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float duracion = 30f;
    private float tiempoInicio;
    private bool isPlaying = false;

    public GameObject winPanel;
    public GameObject losePanel;
    public GameObject gamePanel;
    public TMP_Text timerText;
    public List<TMP_Text> orderTexts;

    public List<GameObject> books;
    public List<GameObject> activeBooks = new List<GameObject>();
    public List<string> correctOrder = new List<string>();
    public AudioClip winSound;
    public AudioClip loseSound;
    public AudioClip backgroundMusic;
    private AudioSource audioSource;
    private bool hasTriggered = false;
    private bool hasWon = false;
    private bool hasLost = false;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None; // desbloquea el cursor
        Cursor.visible = true;
        gamePanel.SetActive(true);
        winPanel.SetActive(false);
        losePanel.SetActive(false);

        GenerateOrder();
        ShowOrder();

        tiempoInicio = Time.time;
        isPlaying = true;

        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(backgroundMusic);

        hasTriggered = false;
        hasWon = false;
        hasLost = false;

        

    }

    void Update()
    {
        if (!isPlaying) return;

        float tiempoRestante = duracion - (Time.time - tiempoInicio);
        timerText.text = "Tiempo: " + Mathf.CeilToInt(Mathf.Max(0, tiempoRestante));

        if (tiempoRestante <= 0)
        {
            Lose();
        }
        if (audioSource.isPlaying == false)
        {
            audioSource.PlayOneShot(backgroundMusic);
        }
    }
    void GenerateOrder()
    {
        correctOrder = new List<string>()
        {
            "Rojo", "Naranja", "Rosa", "Amarillo",
            "Verde", "Celeste", "Azul", "Azul oscuro", "Morado"
        };

        for (int i = 0; i < 9; i++)
        {
            string temp = correctOrder[i];
            int randomIndex = Random.Range(i, 9);
            correctOrder[i] = correctOrder[randomIndex];
            correctOrder[randomIndex] = temp;
        }
    }
    void ShowOrder()
    {
        for (int i = 0; i < correctOrder.Count; i++)
        {
            orderTexts[i].text = correctOrder[i];
        }
    }
    List<string> GetCurrentOrder()
    {
        List<Libro> libros = new List<Libro>(
            FindObjectsOfType<Libro>()
        );

        // Sort by X position
        libros.Sort((a, b) => a.transform.position.x.CompareTo(b.transform.position.x));

        List<string> currentOrder = new List<string>();

        foreach (var libro in libros)
        {
            currentOrder.Add(libro.nombre);
        }

        return currentOrder;
    }

    public void Finish()
    {
        if (CheckWinCondition())
        {
            
            Win();
        }
        else
        {
            Lose();
        }
    }

    bool CheckWinCondition()
    {
        List<string> current = GetCurrentOrder();

        for (int i = 0; i < current.Count; i++)
        {
            if (current[i] != correctOrder[i])
                return false;
        }

        return true;
    }

    void Win()
    {
        if (hasWon) return;
        hasWon = true;
        SceneMemory.Instance.Puzzle1=true;
        audioSource.Stop();
        audioSource.PlayOneShot(winSound);
        isPlaying = false;
        gamePanel.SetActive(false);
        winPanel.SetActive(true);
        Debug.Log("¡Ganaste!");
    }

    void Lose()
    {
        if (hasLost) return; // 🔥 ADD THIS
        hasLost = true;
        audioSource.Stop();
        audioSource.PlayOneShot(loseSound);
        isPlaying = false;
        gamePanel.SetActive(false);
        losePanel.SetActive(true);
        Debug.Log("Perdiste");
    }

    public void ExitGame()
    {
        if (hasTriggered) return;
        hasTriggered = true;
        SceneMemory.Instance.ReturnToMainWorld();
    }
}