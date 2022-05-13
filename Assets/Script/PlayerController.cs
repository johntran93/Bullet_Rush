using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PlayerController : MyCharacterController
{
    public static PlayerController Instance { get; private set; }
    [SerializeField] private ScreenTouchController input;
    [SerializeField] private ShootController shootController;
    [SerializeField] private GameObject rightHandPlayer;
    [SerializeField] private GameObject leftHandPlayer;
    [SerializeField] private GameObject rightHand;
    [SerializeField] private GameObject leftHand;
    private readonly List<Transform> _enemies = new List<Transform>();
    private bool _isShooting;
    Animator ani;
    private void Awake()
		{
			Instance = this;
		}
    private void Start() 
    {
        ani = gameObject.GetComponent<Animator>();
    }
    private  void FixedUpdate() 
    {
        var direction = new Vector3(input.Direction.x , 0, input.Direction.y);
        Move(direction);
    }
    private void Update() 
    {
        if(_enemies.Count > 0)
        {
            rightHand.transform.LookAt(_enemies[0]);
            leftHand.transform.LookAt(_enemies[0]);
        }
    }
    private void OnCollisionEnter(Collision collision) 
    {
        if(collision.transform.CompareTag("Enemy"))
        {
            StartCoroutine(Dead());
        }
    }
    IEnumerator Dead()
    {
        Debug.Log("Dead");
        ani.SetBool("Falling", true);
        yield return new WaitForSeconds(0.75f);
        Time.timeScale = 0;
    }
    private void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Finish"))
        {
            OnReachSavePoint();
        }
    }
    private void OnTriggerStay(Collider other) 
    {
       if(other.transform.CompareTag("Enemy"))
       {
           if(!_enemies.Contains(other.transform))
           {
               _enemies.Add(other.transform);
           }
           AutoShoot();
       }
    }
    private void OnTriggerExit(Collider other) 
    {
        if(other.transform.CompareTag("Enemy"))
        {
            _enemies.Remove(other.transform);
        }
    }

    private void AutoShoot()
    {
       IEnumerator Do()
       {
           while(_enemies.Count > 0)
           {
                var enemy = _enemies[0];
                var direction = enemy.transform.position - transform.position;
                direction.y = 0;
                direction = direction.normalized;
                shootController.Shoot(direction, rightHandPlayer.transform.position);
                shootController.Shoot(direction, leftHandPlayer.transform.position);
                _enemies.RemoveAt(0);
                yield return new WaitForSeconds(shootController.Delay);
           }
           _isShooting = false;
       }
       if(!_isShooting)
       {
           _isShooting = true;
            StartCoroutine(Do());
       }
       
    }
    public void SetAnimationRunTrue()
    {
        ani.SetBool("Run", true);
    }
    public void SetAnimationRunFalse()
    {
        ani.SetBool("Run", false);
    }
    private void OnReachSavePoint()
    {
        GameManager.Instance.Win();
    }
}
