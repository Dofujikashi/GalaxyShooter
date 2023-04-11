using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private Sprite[] _healthSprites;
    [SerializeField] private Image _healthImage;
    [SerializeField] private TMP_Text _gameOverText;
    [SerializeField] private TMP_Text _restartGameText;

    private GameManager _gameManager;

    private void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>().GetComponent<GameManager>();
    }

    public void UpdateScore(int score)
    {
        _scoreText.text = $"Score: {score}";
    }

    public void UpdateHealth(short health)
    {
        try
        {
            _healthImage.sprite = _healthSprites[health];
        } 
        catch (IndexOutOfRangeException)
        {
            _healthImage.sprite = _healthSprites[0];
        }

        if (health < 1)
        {
            OnPlayerDeath();
        }
    }

    private void OnPlayerDeath()
    {
        _gameOverText.gameObject.SetActive(true);
        _restartGameText.gameObject.SetActive(true);
        StartCoroutine(GameOverTextFlicker());
        _gameManager.EnableGameOverControls();
    }

    IEnumerator GameOverTextFlicker()
    {
        while (true)
        {
            _gameOverText.text = "Game Over";
            yield return new WaitForSeconds(0.5f);
            _gameOverText.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }
}
