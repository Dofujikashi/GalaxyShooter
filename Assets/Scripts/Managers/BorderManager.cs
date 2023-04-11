using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderManager : MonoBehaviour
{
    [SerializeField] private float _borderX = 8.77f;
    [SerializeField] private float _borderYUp = 0.7f;
    [SerializeField] private float _borderYDown = -2.39f;

    public float GetX() => _borderX;
    public float GetYUp() => _borderYUp;
    public float GetYDown() => _borderYDown;
}
