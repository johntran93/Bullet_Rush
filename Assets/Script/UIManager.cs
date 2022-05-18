using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _backToMenuUI;
    [SerializeField] private GameObject _gameplayUI;
    [SerializeField] private TMP_Text _enemyKilled;
    public void BackToMenu()
    {
        _backToMenuUI.SetActive(true);
        Time.timeScale = 0;
    }
    public void XButtonClick()
    {
        _backToMenuUI.SetActive(false);
        Time.timeScale = 1;
    }
    public void VButtonClick()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }
    public void ContinueButtonWin()
    {
        Debug.Log("Load map 2");
        Time.timeScale = 1;
    }
    private void Update() 
    {
        _enemyKilled.text = GameManager.Instance.EnemyDeadCounter.ToString() + "/" + GameManager.Instance.EnemyAmount.ToString();
    }
}
