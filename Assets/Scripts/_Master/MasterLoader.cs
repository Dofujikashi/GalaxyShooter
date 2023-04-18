using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterLoader : MonoBehaviour
{
    [SerializeField] GameObject masterPrefab;

    private void Awake()
    {
        if (Master.Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instantiate(masterPrefab);
        }
    }
}
