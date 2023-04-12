using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private GameManagerInputAction _gameController;

    private void Awake()
    {
        _gameController = new GameManagerInputAction();
        _gameController.Enable();
        _gameController.GameOverMap.Disable();

        _gameController.GameOverMap.RestartGame.started += (ctx) => OnRestart();
        _gameController.OptionMap.QuitGame.started += (ctx) => Application.Quit();
    }

    private void OnRestart()
    {
        SceneManager.LoadScene(1);
        _gameController.GameOverMap.Disable();
    }

    public void EnableGameOverControls()
    {
        _gameController.GameOverMap.Enable();
    }
}
