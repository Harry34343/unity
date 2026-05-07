using System.Numerics;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] private Transform Camera;
    [SerializeField] private UnityEngine.Vector3 offset;
    
    void LateUpdate()
    {
        transform.position = Camera.position + offset;
    }
}
