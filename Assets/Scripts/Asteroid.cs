using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private short rotationSpeed = 1;
    [SerializeField] private short movementSpeed = 1;

    void Update()
    {
        transform.position += movementSpeed * Time.deltaTime * Vector3.down;
        transform.Rotate(rotationSpeed * Time.deltaTime * Vector3.forward);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Enemy":
                if (other.transform.TryGetComponent(out Enemy enemy))
                {
                    enemy.PrepareDestruction();
                }
                break;

            case "Player":
                if (other.transform.TryGetComponent(out Player player))
                {
                    player.PrepareDestruction();
                }
                break;
        } 
    }
}
