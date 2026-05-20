using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class thumbsup : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    [SerializeField] private UnityEngine.Video.VideoPlayer videoPlayer;
    [SerializeField] private RawImage image;
    [SerializeField] private float displayDuration = 0.2f;

    void Start()
    {
        Color c = image.color;
        c.a = 0f;
        image.color = c;
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    IEnumerator fade(float startAlpha, float endAlpha)
    {
        Color c = image.color;
        c.a = startAlpha;
        float duration = displayDuration;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            c.a = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            image.color = c;    
            yield return null;
        }

        image.color = new Color(c.r, c.g, c.b, endAlpha);
    }
    void OnVideoEnd(UnityEngine.Video.VideoPlayer vp)
    {
        HideThumbsUp();
    }
    public void ShowThumbsUp()
    {
        StartCoroutine(fade(0f, 1f));
    }
    public void HideThumbsUp()
    {
        StartCoroutine(fade(1f, 0f));
    }
    public void playVideo()
    {
        videoPlayer.Play();
    }
}
