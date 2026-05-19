using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Libro : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private Vector2 offset;
    private CanvasGroup canvasGroup;
    private Image image;
    public String nombre;
    public AudioClip clickSound;
    private AudioSource audioSource;

   
    private Vector2 targetPosition; 
    public float smoothSpeed = 15f;
    private bool isDragging = false;
    public static Libro currentlyDragging;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        image = GetComponent<Image>();

        targetPosition = rectTransform.anchoredPosition;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!isDragging)
        {
            rectTransform.anchoredPosition = Vector2.Lerp(
                rectTransform.anchoredPosition, 
                targetPosition, 
                Time.deltaTime * smoothSpeed
            );
        }

        if (!isDragging)
        {
            ResolveOverlap();
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
        currentlyDragging = this;
        canvasGroup.blocksRaycasts = false;
        audioSource.PlayOneShot(clickSound);

        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x*1.2f, rectTransform.sizeDelta.y*1.2f);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            canvas.worldCamera,
            out Vector2 localPoint
        );

        offset = rectTransform.anchoredPosition - localPoint;
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            canvas.worldCamera,
            out Vector2 localPoint
        );

        // While dragging, we update both so it feels responsive
        targetPosition = localPoint + offset;
        rectTransform.anchoredPosition = targetPosition;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
        currentlyDragging = null;
        canvasGroup.blocksRaycasts = true;
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x/1.2f, rectTransform.sizeDelta.y/1.2f);

        targetPosition = rectTransform.anchoredPosition;
    }
    
    void ResolveOverlap()
    {
        GameObject[] allBooks = GameObject.FindGameObjectsWithTag("Book");

        foreach (GameObject other in allBooks)
        {
            if (other == this.gameObject) continue;
            
            RectTransform otherRect = other.GetComponent<RectTransform>();

            if (IsOverlapping(rectTransform, otherRect))
            {
                Rect a = GetWorldRect(rectTransform);
                Rect b = GetWorldRect(otherRect);

                float overlapX = Mathf.Min(a.xMax, b.xMax) - Mathf.Max(a.xMin, b.xMin);

                float direction = (rectTransform.position.x > otherRect.position.x) ? 1f : -1f;
                targetPosition += new Vector2((overlapX * 0.1f) * direction, 0f);
            }
        }
    }

    bool IsOverlapping(RectTransform a, RectTransform b)
    {
        return GetWorldRect(a).Overlaps(GetWorldRect(b));
    }

    Rect GetWorldRect(RectTransform rt)
    {
        Vector3[] corners = new Vector3[4];
        rt.GetWorldCorners(corners);
        return new Rect(corners[0].x, corners[0].y, corners[2].x - corners[0].x, corners[2].y - corners[0].y);
    }
}