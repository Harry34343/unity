using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ShowItem : MonoBehaviour
{
    [SerializeField] private Image itemToShow;
    [SerializeField] private float displayDuration = 2f;
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private float RotationSpeed = 50f;
    [SerializeField] private GameObject BrightnessEffect;
    private float originalScale;
    private float itemOriginalScale;
    private float currentalpha;
    private Image brightImage;
    void Start()
    {
        originalScale = BrightnessEffect.transform.localScale.x;
        itemOriginalScale = itemToShow.transform.localScale.x;
        Color c = itemToShow.color;
        c.a = 0f;
        itemToShow.color = c;
        brightImage = BrightnessEffect.GetComponent<Image>();
        currentalpha = brightImage.color.a;
        brightImage.color = c;
    }
    public void setItem(Sprite item)
    {
        itemToShow.sprite = item;
        display();
    }
    public void display()
    {
        StartCoroutine(DisplayItem());
    }
    IEnumerator DisplayItem()
    {
        BrightnessEffect.transform.localScale = Vector3.one * originalScale;
        itemToShow.transform.localScale = Vector3.one * itemOriginalScale;
        yield return StartCoroutine(FadeIn());
        yield return StartCoroutine(RotateItem(BrightnessEffect.transform, displayDuration));
        yield return StartCoroutine(FadeOut());
    }
    IEnumerator FadeIn()
    {

        float elapsedTime = 0f;
        Color c = itemToShow.color;
        Color c2 = brightImage.color;
        StartCoroutine(RotateItem(BrightnessEffect.transform, fadeDuration));
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            c.a = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            c2.a= Mathf.Lerp(0f, currentalpha, elapsedTime / fadeDuration);
            itemToShow.color = c;
            brightImage.color = c2;

            yield return null;
        }
        itemToShow.color = new Color(c.r, c.g, c.b, 1f);
    }
    IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        Color c = itemToShow.color;
        Color c2 = brightImage.color;
        StartCoroutine(RotateItem(BrightnessEffect.transform, fadeDuration));
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            c.a = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            c2.a = Mathf.Lerp(currentalpha, 0f, elapsedTime / fadeDuration);
            itemToShow.color = c;
            brightImage.color = c2;

            yield return null;
        }
        itemToShow.color = new Color(c.r, c.g, c.b, 0f);
    }
    IEnumerator RotateItem(Transform item, float duration)
    {
        float timeElapsed = 0f;
        while (timeElapsed < duration)        
        {
            StartCoroutine(IncreaseSize());
            timeElapsed += Time.deltaTime;  
            item.Rotate(Vector3.back, RotationSpeed * Time.deltaTime);
            yield return null;
        }
        
    }
    IEnumerator IncreaseSize()
    {
        BrightnessEffect.transform.localScale = Vector3.Lerp(BrightnessEffect.transform.localScale, Vector3.one * originalScale*1.5f, Time.deltaTime);
        itemToShow.transform.localScale = Vector3.Lerp(itemToShow.transform.localScale, Vector3.one * itemOriginalScale*1.2f, Time.deltaTime);
        yield return null;
    }
}
