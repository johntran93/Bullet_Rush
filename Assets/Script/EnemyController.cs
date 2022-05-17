using UnityEngine;

public class EnemyController : MyCharacterController
{
    [SerializeField] private ParticleController deadParticlePrefab;
    [SerializeField] private float maxHealth;
    private float helth;
    private float dist;
    private void Start()
    {
        GameManager.Instance.EnemyAmount++;
        helth = maxHealth;
    }
    private  void FixedUpdate() 
    {
        var player = PlayerController.Instance;
        dist = Vector3.Distance(player.transform.position, transform.position);
        var delta = -transform.position + player.transform.position;
        delta.y = 0;
        var direction = delta.normalized;
        if(dist <= howclose)
        {
        Move(direction);
        transform.LookAt(player.transform);
        }
    }
    private void OnTriggerEnter(Collider other) 
    {
        if(other.transform.CompareTag("Bullet"))
        {
            TakeDamege(1);
            Vector3 closestPoint = other.ClosestPointOnBounds(gameObject.transform.position);
            other.gameObject.SetActive(false);
            Instantiate(deadParticlePrefab, other.transform.position, Quaternion.identity);   
        }
    }
    public void TakeDamege(float damageAmount)
    {
        helth -= damageAmount;
        ChangeScale();
        if (helth <= 0)
        {
            gameObject.SetActive(false);
            GameManager.Instance.EnemyDeadCounter++;
        }
    }
    public void ChangeScale()
    {
        transform.localScale -= new Vector3(0.25f, 0.25f, 0.25f);
        Vector3 pos = gameObject.transform.position;
        pos.y /= 2f;
        gameObject.transform.position = pos;
    }
}

