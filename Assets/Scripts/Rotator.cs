using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField]
    private float SpeedX = 0;

    [SerializeField]
    private float SpeedY = 0;

    [SerializeField]
    private float SpeedZ = 0;

    void Update()
    {
        transform.Rotate(SpeedX * Time.deltaTime, SpeedY * Time.deltaTime, SpeedZ * Time.deltaTime);
    }
}
