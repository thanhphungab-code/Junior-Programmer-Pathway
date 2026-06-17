using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TotalCounter : MonoBehaviour
{
    public Text textCounter;
    private int count = 0;
    private void Start()
    {
        count = 0;
    }

    public void Add(int num)
    {
        count += num;
        textCounter.text = "Score: " + count;
    }
}
