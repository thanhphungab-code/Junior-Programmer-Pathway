using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Prototype2
{
    public class DestroyOutOfBounds : MonoBehaviour
    {
        private float topBound = 40;
        private float lowerBound = -15;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (transform.position.z > topBound)
            {
                gameObject.SetActive(false);

            }
            else if (transform.position.z < lowerBound)
            {
                GameManager.Instance.GameOver();
                Destroy(gameObject);
            }

        }
    }
}
