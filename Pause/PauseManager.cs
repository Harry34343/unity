using System;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    private GameObject player;
    [SerializeField] private GameObject PauseMenu;
    [SerializeField] private AudioClip hoverSound;
    [SerializeField] private AudioClip clickSound;
    private AudioSource audioSource;
    [SerializeField] private GameObject configMenu;
     [SerializeField] private GameObject darkPanel;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform[] bullyPositions;
    [SerializeField] private Transform[] spawnbullyPosition;
    private bool isPaused;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
       PauseMenu.SetActive(false); 
       audioSource = GetComponent<AudioSource>();
       configMenu.SetActive(false);
       darkPanel.SetActive(false);
       player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    public void EnablePauseMenu(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            isPaused = !isPaused;

            PauseMenu.SetActive(isPaused);
            if (isPaused)
            {
                Time.timeScale = 0f;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Time.timeScale = 1f;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }
    public void Salir()
    {
        ClickSound();
        Application.Quit();
    }
    public void HoverSound()
    {
        audioSource.PlayOneShot(hoverSound);
    }
    public void ClickSound()
    {
        audioSource.PlayOneShot(clickSound);
    }
    public void AbrirConfig()
    {
        ClickSound();
        darkPanel.SetActive(true);
        configMenu.SetActive(true);
    }
    public void Reiniciar()
    {
        ClickSound();
        if (spawnPoint != null) player.transform.position = spawnPoint.position;
        int i=0;
        foreach (Transform bully in bullyPositions)
        {
            bully.position = spawnbullyPosition[i].position;
            i++;
        }
    }
    public void Continue()
    {
        ClickSound();
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        PauseMenu.SetActive(false);
    }
    public void Menu()
    {
        ClickSound();
        SceneManager.LoadScene("mainmenu");
    }
}
