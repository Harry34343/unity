using UnityEngine;

public class Doorscript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (SceneMemory.Instance.Puzzle1 && SceneMemory.Instance.Puzzle2)
        {
            Open();
        }
    }

    void Open()
    {
        transform.Rotate(0,-90, 0);
    }
}
