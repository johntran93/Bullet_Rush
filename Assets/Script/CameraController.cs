using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform target;
    private Vector3 _offset;
    private void Awake() 
    {
        _offset = transform.position - target.transform.position;
    }
    private void LateUpdate() 
    {
        transform.position = target.position + _offset;
    }
}
