using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _backToMenuUI;
    [SerializeField] private GameObject _gameplayUI;
    public void BackToMenu()
    {
        _backToMenuUI.SetActive(true);
    }
}
