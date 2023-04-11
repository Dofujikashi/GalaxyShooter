using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _redLazerPrefab;
    [SerializeField] private GameObject _greenLazerPrefab;
    [SerializeField] private GameObject _greenTripleLazerPrefab;
    [SerializeField] private GameObject _asteroidPrefab;
    [SerializeField] private GameObject[] powerUps;
    [SerializeField] private GameObject _enemyContainer;
    [SerializeField] private GameObject _lazerContainer;
    [SerializeField] private GameObject _player;
    [SerializeField] private float _enemySpawnRate = 3;
    [SerializeField] private float _spawnY = 10.0f;
    [SerializeField] private float _powerUpSpawnRate = 20;
    [SerializeField] private float _asteroidSpawnRate = 10;

    private BorderManager _borderM;
    private bool _isPlayerDead = false;

    private void Awake()
    {
        _borderM = FindObjectOfType<BorderManager>().GetComponent<BorderManager>();
    }

    void Start()
    {
        StartCoroutine(SpawnEnemy());
        StartCoroutine(SpawnPowerUp());
    }

    IEnumerator SpawnEnemy()
    {
        while (!_isPlayerDead)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-_borderM.GetX(), _borderM.GetX()), _spawnY, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(_enemySpawnRate);
        }
    }

    IEnumerator SpawnPowerUp()
    {
        yield return new WaitForSeconds(_powerUpSpawnRate);

        while (!_isPlayerDead)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-_borderM.GetX(), _borderM.GetX()), _spawnY, 0);
            GameObject newPowerUp = Instantiate(powerUps[Random.Range(0, 3)], posToSpawn, Quaternion.identity);
            newPowerUp.transform.parent = transform;
            yield return new WaitForSeconds(_powerUpSpawnRate);
        }
    }

    public void SpawnEnemyLazer(Vector3 position)
    {
        GameObject newLazer = Instantiate(_redLazerPrefab, position + new Vector3(0, -0.706f, 0), Quaternion.identity);
        newLazer.transform.parent = _lazerContainer.transform;
    }

    public void SpawnPlayerLazer(Vector3 position)
    {
        GameObject newLazer = Instantiate(_greenLazerPrefab, position + new Vector3(0, 0.706f, 0), Quaternion.identity);
        newLazer.transform.parent = _lazerContainer.transform;
    }

    public void SpawnPlayerTripleLazer(Vector3 position)
    {
        GameObject newLazer = Instantiate(_greenTripleLazerPrefab, position, Quaternion.identity);
        newLazer.transform.parent = _lazerContainer.transform;
    }

    IEnumerator SpawnAsteroid()
    {
        yield return new WaitForSeconds(_asteroidSpawnRate);

        while (!_isPlayerDead)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-_borderM.GetX(), _borderM.GetX()), _spawnY, 0);
            GameObject newAsteroid = Instantiate(_asteroidPrefab, posToSpawn, Quaternion.identity);
            newAsteroid.transform.parent = transform;
            yield return new WaitForSeconds(_asteroidSpawnRate);
        }
    }

    public void OnPlayerDeath()
    {
        _isPlayerDead = true;
    }

    public GameObject GetPlayer() => _player;
}
