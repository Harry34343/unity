using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{
    [SerializeField] private CircularLinkedList menuItems;
    [SerializeField] private GameObject[] items;
    private Node currentNode;
    [SerializeField] private RectHoverShow rectHoverShow;
    private GameObject selected;
    
    private bool inputEnabled = true;
    private bool submitCooldown = false;

    void Awake()
    {
        foreach (GameObject item in items)
        {
            menuItems.AddNode(item);
            
            TMP_InputField input = item.GetComponent<TMP_InputField>();
            if (input != null)
            {
                input.onEndEdit.AddListener(delegate { OnInputEndEdit(input); });
            }
        }
        
        currentNode = menuItems.getHead();
        selected = currentNode.getItem();
        UpdateSelection();
    }

    public bool IsTyping()
    {
        GameObject current = EventSystem.current.currentSelectedGameObject;
        if (current == null) return false;
        TMP_InputField inputField = current.GetComponent<TMP_InputField>();
        return inputField != null && inputField.isFocused;
    }

    private void UpdateSelection()
    {
        if (rectHoverShow != null)
            rectHoverShow.MoveToButton(selected.GetComponent<RectTransform>());

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(selected);
    }

    public void MyInput(InputAction.CallbackContext context)
    {
        if (IsTyping() || !context.performed || !inputEnabled) return;

        Vector2 input = context.ReadValue<Vector2>();
        if (input.x > 0.6f || input.y < -0.6f) MoveToNext();
        else if (input.x < -0.6f || input.y > 0.6f) MoveToPrev();

        StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        inputEnabled = false;
        yield return new WaitForSecondsRealtime(0.2f);
        inputEnabled = true;
    }

    public void Submit(InputAction.CallbackContext context)
    {
        if (!context.performed || submitCooldown) return;

        GameObject current = EventSystem.current.currentSelectedGameObject;
        if (current == null) return;

        TMP_InputField inputField = current.GetComponent<TMP_InputField>();

        if (inputField != null)
        {

            if (inputField.isFocused) return;

            StartCoroutine(SubmitCooldownRoutine());
            inputField.ActivateInputField();
        }
    }

    private void OnInputEndEdit(TMP_InputField input)
    {
        StartCoroutine(SubmitCooldownRoutine());
        input.DeactivateInputField();
        StartCoroutine(ResetSelectionAfterTyping());
    }

    IEnumerator SubmitCooldownRoutine()
    {
        submitCooldown = true;
        yield return new WaitForSecondsRealtime(0.3f); 
        submitCooldown = false;
    }

    IEnumerator ResetSelectionAfterTyping()
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(selected);
        Debug.Log("Returned to Menu Navigation");
    }
    public void MoveToNext()
    {
        currentNode = currentNode.Next();

        selected = currentNode.getItem();

        UpdateSelection();
    }
    public void MoveToPrev()
    {
        Node temp = menuItems.getHead();
        while (temp.Next() != currentNode)
        {
            temp = temp.Next();
        }
        currentNode = temp;

        selected = currentNode.getItem();

        UpdateSelection();
    }
    public CircularLinkedList getMenuItems()
    {
        return menuItems;
    }
    public void setNode(Node node)
    {
        menuItems.SearchNode(node.getItem());
        currentNode = node;
    }
}
