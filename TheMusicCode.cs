using UnityEngine;
using UnityEngine.SceneManagement;


public class TheMusicCode: MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private string sceneToLoad;
    private Scene currentScene;
    [SerializeField] private Transform playerPosition;
    [SerializeField] private string minigameSceneName;
    private bool hasTriggered = false; 
    [SerializeField] private GameObject outlineObject;
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
        if (SceneMemory.Instance.Puzzle2)
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
        // This is called when the 3D object is clicked
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

