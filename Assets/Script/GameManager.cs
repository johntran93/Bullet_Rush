using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject wingameUI;
    public static GameManager Instance { get; private set; }
    public int EnemyAmount { get; set; }

    public int EnemyDeadCounter
    {
        get => _enemyDeadCounter;
        set
        {
            _enemyDeadCounter = value;
            barController.Display(SuccessValue);
        }
    }

    private int _enemyDeadCounter;

    private float SuccessValue => EnemyDeadCounter / (float)EnemyAmount;
    [SerializeField] private BarController barController;
    private void Awake()
    {
        Instance = this;
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(.2f);
        barController.Display(SuccessValue);
    }
    public void Win()
    {
        Debug.Log("Win");
        Time.timeScale = 0;
        wingameUI.SetActive(true);
    }
}
