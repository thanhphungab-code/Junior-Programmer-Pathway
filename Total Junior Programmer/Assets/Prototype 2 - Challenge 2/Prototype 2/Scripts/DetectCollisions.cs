using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Prototype2
{
    public class DetectCollisions : MonoBehaviour
    {
        void OnTriggerEnter(Collider other)
        {
            other.gameObject.SetActive(false);
            Destroy(gameObject);
        }

    }
}