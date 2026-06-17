using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Counter : MonoBehaviour
{
    public TotalCounter totalCounter;
    public int multiplier;

    private void OnTriggerEnter(Collider other)
    {
        totalCounter.Add(multiplier);
    }
}
