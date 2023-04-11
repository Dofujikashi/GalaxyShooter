using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    // 0: Triple Lazer
    // 1: Speed Boost
    // 2. Shield
    [SerializeField] private short id;

    void Update()
    {
        transform.position += Time.deltaTime * 3 * Vector3.down;
        Destroy(gameObject, 5);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.transform.TryGetComponent(out Player player))
            {
                switch (id)
                {
                    case 0: player.EnableTripleLazer(); break;
                    case 1: player.EnableSpeedBoost(); break;
                    case 2: player.EnableShield(); break;
                }
            }
            Destroy(gameObject);
        }
    }
}
