using UnityEngine;

public class RectHoverShow : MonoBehaviour
{
    [SerializeField] private RectTransform highlightRect;
    [SerializeField] private float Speed=20f;
    private Vector3 targetPosition;
    [SerializeField] private bool MenuHover=false;
    [SerializeField] private float ExtraWidth;
    void Start()
    {
        targetPosition = highlightRect.position;
    }

    void Update()
    {
        highlightRect.position = Vector3.Lerp(
            highlightRect.position,
            targetPosition,
            Time.unscaledDeltaTime * Speed
        );
    }

    public void MoveToButton(RectTransform buttonRect)
    {
        Debug.Log("Movido");
        Debug.Log("Moving to: " + buttonRect.name);
        targetPosition = buttonRect.position;
        ShowRectangle();
        if (!MenuHover)
        {
            highlightRect.SetSizeWithCurrentAnchors(
                RectTransform.Axis.Horizontal,
                buttonRect.rect.width
            );

            highlightRect.SetSizeWithCurrentAnchors(
                RectTransform.Axis.Vertical,
                buttonRect.rect.height
            );
        }
        else
        {
            highlightRect.SetSizeWithCurrentAnchors(
                RectTransform.Axis.Horizontal,
                buttonRect.rect.width+ExtraWidth
            );

            highlightRect.SetSizeWithCurrentAnchors(
                RectTransform.Axis.Vertical,
                buttonRect.rect.height
            );
            targetPosition.x += ExtraWidth/4f;
        }
        
    }
    void ShowRectangle()
    {
        highlightRect.gameObject.SetActive(true);
    }
    public void ResetHover()
    {
        highlightRect.gameObject.SetActive(false);
    }
}