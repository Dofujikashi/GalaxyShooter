using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    //[SerializeField] private short _rotationSpeed = 20;
    [SerializeField] private GameObject _explosionPrefab;

    public short movementSpeed = 2;
    private short _durability = 9;

    void Update()
    {
        transform.position += movementSpeed * Time.deltaTime * Vector3.down;
        transform.Rotate(20 * Time.deltaTime * Vector3.forward);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "PlayerLazer":
                Destroy(other.gameObject);
                TakeDurability(1);
                break;

            case "EnemyLazer":
                Destroy(other.gameObject);
                TakeDurability(1);
                break;

            case "Enemy":
                if (other.transform.TryGetComponent(out Enemy enemy))
                {
                    enemy.PrepareDestruction();
                    TakeDurability(3);
                }
                break;

            case "Player":
                if (other.transform.TryGetComponent(out Player player))
                {
                    player.TakeDamage(5);

                    if (player.IsShieldActive)
                    {
                        PrepareDestruction();
                    }
                    else
                    {
                        TakeDurability(5);
                    }
                }
                break;
        }
    }

    private void TakeDurability(short hit)
    {
        _durability -= hit;
        CheckDurability();
    }

    private void CheckDurability()
    {
        if (_durability < 1)
        {
            PrepareDestruction();
        }
    }

    private void PrepareDestruction()
    {
        GameObject explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        Destroy(explosion, 2.37f);
        Destroy(gameObject, 0.05f);
    }
}
