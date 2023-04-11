using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lazer : MonoBehaviour
{
    [SerializeField] private bool isFriendly;
    [SerializeField] private float speed = 20;
    [SerializeField] private short damage = 1;

    void Update()
    {
        if (isFriendly)
        {
            transform.position += speed * Time.deltaTime * Vector3.up;
        } 
        else
        {
            transform.position += speed * Time.deltaTime * Vector3.down;
        }

        if (transform.parent.CompareTag("PlayerLazer"))
        {
            Destroy(transform.parent.gameObject, 2);
        }
        else
        {
            Destroy(gameObject, 2);
        }
    }

    public short GetDamage() => damage;
}
