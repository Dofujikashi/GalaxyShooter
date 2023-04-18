using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Master : MonoBehaviour
{
    public static Master Instance {  get; private set; }

    public Player player {  get; private set; }
    public Enemy enemy { get; private set; }
    public Asteroid asteroid { get; private set; }
    public PowerUp powerUp { get; private set; }
    public BorderManager borderManager { get; private set; }
    public SpawnManager spawnManager { get; private set; }
    public UiManager uiManager { get; private set; }
    public AudioManager audioManager { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        player = GetComponentInChildren<Player>();
        enemy = GetComponentInChildren<Enemy>();
        asteroid = GetComponentInChildren<Asteroid>();
        powerUp = GetComponentInChildren<PowerUp>();
        borderManager = GetComponentInChildren<BorderManager>();
        spawnManager = GetComponentInChildren<SpawnManager>();
        uiManager = GetComponentInChildren<UiManager>();
        audioManager = GetComponentInChildren<AudioManager>();
    }
}
