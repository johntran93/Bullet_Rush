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
        if(_enemies.Count == 1)
        {
            rightHand.transform.LookAt(_enemies[0]);
            leftHand.transform.LookAt(_enemies[0]);
        }
        else if(_enemies.Count > 1)
        {
            rightHand.transform.LookAt(_enemies[0]);
            leftHand.transform.LookAt(_enemies[1]);
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
        ani.SetTrigger("Falling");
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
           if(_enemies.Count == 1)
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
           else if(_enemies.Count > 1)
           {
                var enemy1 = _enemies[0];
                var direction1 = enemy1.transform.position - transform.position;
                direction1.y = 0;
                direction1 = direction1.normalized;
                shootController.Shoot(direction1, rightHandPlayer.transform.position);

                var enemy2 = _enemies[1];
                var direction2 = enemy2.transform.position - transform.position;
                direction2.y = 0;
                direction2 = direction2.normalized;
                shootController.Shoot(direction2, leftHandPlayer.transform.position);
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
