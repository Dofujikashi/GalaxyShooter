using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 5;
    [SerializeField] private float _fireRate = 0.5f;
    [SerializeField] private short _health = 5;
    [SerializeField] private GameObject _shield;

    private BorderManager _borderM;
    private PlayerInputAction _controller;
    private SpawnManager _spawnM;
    private UiManager _uiM;

    private float _nextFire = 0.0f;
    private bool _isTripleLazerActive = false;
    private bool _isSpeedBoostActive = false;
    private bool _isShieldActive = false;
    private short _tripleLazerStack = 0;
    private short _speedBoostStack = 0;
    private short _shieldStack = 0;
    private int _score = 0;

    private void Awake()
    {
        _borderM = FindObjectOfType<BorderManager>().GetComponent<BorderManager>();
        _spawnM = FindObjectOfType<SpawnManager>().GetComponent<SpawnManager>();
        _uiM = FindObjectOfType<UiManager>().GetComponent<UiManager>();
        _controller = new PlayerInputAction();
    }

    private void Start()
    {
        _controller.PlayerMap.Enable();
        _controller.PlayerMap.Attack.started += (ctx) => Attack();
    }

    private void Update()
    {
        ControlMovement();
    }

    private void OnDestroy()
    {
        _controller.PlayerMap.Disable();
        _spawnM.OnPlayerDeath();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EnemyLazer"))
        {
            if (other.transform.TryGetComponent(out Lazer lazer))
            {
                TakeDamage(lazer.GetDamage());
                Destroy(other.gameObject);
            }
        }
    }

    public void PrepareDestruction()
    {
        Destroy(gameObject);
    }

    private void ControlMovement()
    {
        Vector2 rawMovement = _controller.PlayerMap.Move.ReadValue<Vector2>().normalized;
        Vector3 movement = new Vector3(rawMovement.x, rawMovement.y, 0);
        transform.position += movement * _speed * Time.deltaTime;

        // Check X Axis
        if (transform.position.x >= _borderM.GetX())
        {
            transform.position = new Vector3(_borderM.GetX(), transform.position.y, 0);
        }
        else if (transform.position.x <= -_borderM.GetX())
        {
            transform.position = new Vector3(-_borderM.GetX(), transform.position.y, 0);
        }

        // Check Y Axis
        if (transform.position.y >= _borderM.GetYUp())
        {
            transform.position = new Vector3(transform.position.x, _borderM.GetYUp(), 0);
        }
        else if (transform.position.y <= _borderM.GetYDown())
        {
            transform.position = new Vector3(transform.position.x, _borderM.GetYDown(), 0);
        }
    }

    public void Attack()
    {
        if (Time.time > _nextFire)
        {
            _nextFire = Time.time + _fireRate;
            if (_isTripleLazerActive)
            {
                _spawnM.SpawnPlayerTripleLazer(transform.position);
            }
            else
            {
                _spawnM.SpawnPlayerLazer(transform.position);
            }
        }
    }

    public void TakeDamage()
    {
        if (_isShieldActive) return;
        _health--;
        _uiM.UpdateHealth(_health);
        CheckHealth();
    }

    public void TakeDamage(short damage)
    {
        if (_isShieldActive) return;
        _health -= damage;
        _uiM.UpdateHealth(_health);
        CheckHealth();
    }

    private void CheckHealth()
    {
        if (_health < 1)
        {
            Destroy(gameObject);
        }
    }

    public void EnableTripleLazer()
    {
        _tripleLazerStack++;
        if (!_isTripleLazerActive)
        {
            StartCoroutine(TripleLazerDuration());
        }
    }

    private IEnumerator TripleLazerDuration()
    {
        _isTripleLazerActive = true;
        while (_tripleLazerStack > 0)
        {
            yield return new WaitForSeconds(10);
            _tripleLazerStack--;
        }
        _isTripleLazerActive = false;
    }

    public void EnableSpeedBoost()
    {
        _speedBoostStack++;
        if (!_isSpeedBoostActive)
        {
            StartCoroutine(SpeedBoostDuration());
        }
    }

    private IEnumerator SpeedBoostDuration()
    {
        _isSpeedBoostActive = true;
        _speed *= 1.5f;
        while (_speedBoostStack > 0)
        {
            yield return new WaitForSeconds(10);
            _speedBoostStack--;
        }
        _speed /= 1.5f;
        _isSpeedBoostActive = false;
    }

    public void EnableShield()
    {
        _shieldStack++;
        while (!_isShieldActive)
        {
            StartCoroutine(ShieldDuration());
        }
    }

    private IEnumerator ShieldDuration()
    {
        _isShieldActive = true;
        _shield.SetActive(true);
        while (_shieldStack > 0)
        {
            yield return new WaitForSeconds(10);
            _shieldStack--;
        }
        _isShieldActive = false;
        _shield.SetActive(false);
    }

    public void EnemyKilled() 
    {
        _score++;
        _uiM.UpdateScore(_score);
    }
}
