using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private GameOverInputAction _gameOverController;

    private void Awake()
    {
        _gameOverController = new GameOverInputAction();
        _gameOverController.Disable();
        _gameOverController.GameOverMap.RestartGame.started += (ctx) => OnRestart();
    }

    private void OnRestart()
    {
        SceneManager.LoadScene(1);
        _gameOverController.Disable();
    }

    public void EnableGameOverControls()
    {
        _gameOverController.Enable();
    }
}
