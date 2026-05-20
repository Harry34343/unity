using UnityEngine;

public class MobileUI : MonoBehaviour
{
    void Start()
    {
        gameObject.SetActive(Application.isMobilePlatform);
    }
}
