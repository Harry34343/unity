using System.Collections;
using System.IO;
using System.Numerics;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] private float moveSpeed;
    [SerializeField] private float playerHeight;
    [SerializeField] private LayerMask whatIsGround;
    private float groundDrag;
    private bool grounded;
    [SerializeField] private Transform orientation;

    [SerializeField] private PlayerInput pi;

    [SerializeField] private float jumpforce = 5f;
    private bool jump=false;
    private bool KeyInput;

    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

    }

    // Update is called once per frame

    private void FixedUpdate()
    {
        MovePlayer();
    }

    public void Jump(InputAction.CallbackContext input)
    {
        Debug.Log(input.phase);
        if (input.phase == InputActionPhase.Performed)
        {

            if(jump)
            {
                Debug.Log("Jump");
                rb.AddForce(UnityEngine.Vector3.up*jumpforce, ForceMode.Impulse);
            }
        }
    }

    void OnCollisionStay(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            if (contact.normal.y > 0.8f)
            {
                jump = true;
                return;
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        jump=false;   
    }

    private void MovePlayer()
    {
        UnityEngine.Vector2 m;
        if (KeyInput)
        {
            m = pi.actions["Move"].ReadValue<UnityEngine.Vector2>();
        }
        else
        {
            m = pi.actions["MoveArrow"].ReadValue<UnityEngine.Vector2>();
        }
        
        rb.AddForce(new UnityEngine.Vector3(m.x*orientation.right.x + m.y*orientation.forward.x, 0, m.x*orientation.right.z + m.y*orientation.forward.z) * moveSpeed *10f);
    }
    void LeerKey()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "settings.txt");
        if(!File.Exists(filePath))
        {
            return;
        }
        string[] lineas = File.ReadAllLines(filePath);

        string[] keyboard = lineas[4].Split(":");

        KeyInput = bool.Parse(keyboard[1].Trim());
    }

    public void SetInputScheme(bool wasd)
    {
        KeyInput = wasd;
    }
}
