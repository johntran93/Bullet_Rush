using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PlayerController : MyCharacterController
{
    [SerializeField] private ScreenTouchController input;
    [SerializeField] private ShootController shootController;
    [SerializeField] private GameObject rightHandPlayer;
    //[SerializeField] private GameObject leftHandPlayer;
    private readonly List<Transform> _enemies = new List<Transform>();
    private bool _isShooting;
    private int _enemyAmount;
    private void Start() 
    {
        _enemyAmount = FindObjectsOfType<EnemyController>().Length;
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
            transform.LookAt(_enemies[0]);
        }
    }
    private void OnCollisionEnter(Collision collision) 
    {
        if(collision.transform.CompareTag("Enemy"))
        {
            Dead();
        }
    }
    private void Dead()
    {
        Debug.Log("Dead");
        Time.timeScale = 0;
    }
    private void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Finish"))
        {
            Win();
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
    private void Win() {
        {
            Debug.Log("Win");
            Time.timeScale = 0;
            int current = FindObjectsOfType<EnemyController>().Length;
            float result = current / (float)_enemyAmount;
            float success = Mathf.Lerp(100, 0, result);
            Debug.Log($"Complete {success}%");
        }
    }
}
