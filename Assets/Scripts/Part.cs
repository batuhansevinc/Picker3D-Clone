using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Part : MonoBehaviour
{
    [SerializeField]
    private int partNumber;

    public int PartNumber { get; set; }

    private void Start()
    {
        PartNumber = partNumber;
    }
}