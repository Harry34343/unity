using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMemory : MonoBehaviour
{
    public static SceneMemory Instance;
    public bool logged=false;
    public string lastScene;
    public Vector3 lastPlayerPosition;
    public Quaternion lastPlayerRotation; 
    public bool isReturning = false;      
    public bool Puzzle1=false;
    public bool Puzzle2=false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Call this when entering a minigame
    void OnEnable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {

        if (!isReturning) return;

        StartCoroutine(ApplyTeleport());
    }

    System.Collections.IEnumerator ApplyTeleport()
    {
        GameObject player = null;

        while (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            yield return null;
        }

        Rigidbody rb = player.GetComponent<Rigidbody>();

        rb.isKinematic = true;

        yield return new WaitForEndOfFrame();

        player.transform.SetPositionAndRotation(lastPlayerPosition, lastPlayerRotation);

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        yield return new WaitForFixedUpdate();

        rb.isKinematic = false;

        isReturning = false;
    }
    public void StorePositionAndLoad(string minigameSceneName, Vector3 playerPos, Quaternion playerRot)
    {
        isReturning = true;

        lastScene = SceneManager.GetActiveScene().name;
        lastPlayerPosition = playerPos;
        lastPlayerRotation = playerRot;

        SceneManager.LoadScene(minigameSceneName);
    }

    public void ReturnToMainWorld()
    {
        if (!string.IsNullOrEmpty(lastScene))
        {
            isReturning = true;
            SceneManager.LoadScene(lastScene);
        }
    }
}