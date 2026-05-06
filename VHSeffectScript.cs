using UnityEngine;
using UnityEngine.Video;

public class VHSeffectScript : MonoBehaviour
{

    public VideoPlayer videoPlayer;
    void Start()
    {
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        gameObject.SetActive(false);
    }

    void OnDestroy()
    {
    }

}
