using UnityEngine;
using UnityEngine.SceneManagement;


public class TheColorCode : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private string sceneToLoad;
    private Scene currentScene;
    [SerializeField] private string minigameSceneName;
    [SerializeField] private GameObject outlineObject;
    private bool hasTriggered = false;

    // Call this when the player interacts or hits a trigger
    public void EnterMinigame()
    {
        if (hasTriggered) return;
        hasTriggered = true;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        
        if (player != null)
        {
            SceneMemory.Instance.StorePositionAndLoad(
                minigameSceneName, 
                player.transform.position, 
                player.transform.rotation
            );
        }
    }

    void Start()
    {
        currentScene = SceneManager.GetActiveScene();
        if (SceneMemory.Instance.Puzzle1)
        {
            outlineObject.SetActive(true);
        }
        else
        {
            outlineObject.SetActive(false);
        }
    }

    void OnMouseDown()
    {
       EnterMinigame();
    }
    void OnMouseEnter()
    {
        // Change color when hovered
        GetComponent<Renderer>().material.color = Color.yellow;
    }

    void OnMouseExit()
    {
        // Revert color
        GetComponent<Renderer>().material.color = Color.white;
    }
}
