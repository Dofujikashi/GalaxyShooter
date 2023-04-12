using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed = 1;
    [SerializeField] private short _health = 3;
    [SerializeField] private float _enemyLineY = 3;
    [SerializeField] private GameObject _explosionPrefab;

    private BorderManager _borderM;
    private SpawnManager _spawnM;
    private bool _isRotating = false;
    private bool _rotationDirection = false;
    private bool _isCharging = false;
    private Player _player;
    private AudioSource _audioSource;

    private void Awake()
    {
        _borderM = FindObjectOfType<BorderManager>().GetComponent<BorderManager>();
        _spawnM = FindObjectOfType<SpawnManager>().GetComponent<SpawnManager>();
        _player = _spawnM.GetPlayer().GetComponent<Player>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        StartCoroutine(Attack());
        _isCharging = (Random.value >= 0.5f) ? true : false;
    }

    void Update()
    {
        ControlMovement();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "PlayerLazer":
                if (other.transform.TryGetComponent(out Lazer lazer))
                {
                    TakeDamage(lazer.GetDamage());
                    Destroy(other.gameObject);
                }
                break;

            case "Enemy":
                _rotationDirection = (_rotationDirection == true) ? false : true;
                break;

            case "Player":
                _player.EnemyKilled();
                _player.TakeDamage();
                PrepareDestruction();
                break;
        }
    }

    public void PrepareDestruction()
    {
        GameObject explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        explosion.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
        Destroy(explosion, 2.37f);
        Destroy(gameObject, 0.05f);
    }

    void ControlMovement()
    {
        // Enemy will charge towards you
        if (_isCharging)
        {
            if (transform.position.y > _borderM.GetYDown() - 2)
            {
                // Enemy charging towards player
                transform.position += _speed * Time.deltaTime * Vector3.down;
            }
            else
            {
                // Enemy charged past you, and will reposition
                transform.position = new Vector3(Random.Range(-_borderM.GetX(), _borderM.GetX()), 10.0f, 0);
            }
        } 
        // Enemy will stay in line and rotate left/right
        else
        {
            if (transform.position.y > _enemyLineY)
            {
                // Enemy coming towards player
                transform.position += _speed * Time.deltaTime * Vector3.down;
            }
            else
            {
                // Enemy has come to the middle, now will go left or right
                if (!_isRotating)
                {
                    _rotationDirection = (Random.value >= 0.5f) ? true : false;
                    _isRotating = true;
                }
                else
                {
                    // Enemy is already in the middle and doing rotation
                    if (_rotationDirection)
                    {
                        // Enemy will go towards right
                        if (transform.position.x < _borderM.GetX())
                            transform.position += Vector3.right * _speed * Time.deltaTime;
                        else _rotationDirection = false;
                    }
                    else
                    {
                        // Enemy will go towards left
                        if (transform.position.x > -_borderM.GetX())
                            transform.position += Vector3.left * _speed * Time.deltaTime;
                        else _rotationDirection = true;
                    }
                }
            }
        }
    }

    IEnumerator Attack()
    {
        while (_health > 1)
        {
            _spawnM.SpawnEnemyLazer(transform.position);
            _audioSource.Play();
            yield return new WaitForSeconds(Random.Range(0.5f, 2.5f));
        }
    }

    public void TakeDamage()
    {
        _health--;
        CheckHealth();
    }

    public void TakeDamage(short damage)
    {
        _health -= damage;
        CheckHealth();
    }

    public void CheckHealth() 
    {
        if (_health < 1)
        {
            PrepareDestruction();
            _player?.EnemyKilled();
        }
    }
}
