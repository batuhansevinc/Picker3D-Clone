using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace Picker3d
{
    public class ObjectHolder : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI CounterText;
        [SerializeField] private int TotalObjectForPass;
        private int TotalObjectsInHolder;

        private void Start()
        {
            UpdateTexts();
        }

        private void Update()
        {
            UpdateTexts();
        }

        private void UpdateTexts()
        {
            CounterText.text = String.Format("{0}/{1}", TotalObjectsInHolder, TotalObjectForPass);
        }

        public bool IsSuccess()
        {
            return TotalObjectsInHolder >= TotalObjectForPass;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Object")
            {
                TotalObjectsInHolder++;
                UpdateTexts();
            }
        }
    }
}