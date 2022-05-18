using UnityEngine;

public class MyCharacterController : MonoBehaviour
{
    [SerializeField] private Rigidbody myRigidbody;
    [Range(200,2000)][SerializeField] private float moveSpeed;
    [Range(0,2000)][SerializeField] public float howclose;
    protected void Move(Vector3 direction)
    {
        myRigidbody.velocity = direction * moveSpeed * Time.deltaTime;
    }
}
