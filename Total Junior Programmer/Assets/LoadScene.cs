using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScene : MonoBehaviour
{
    [SerializeField] private List<Button> prototypeButtons;
    [SerializeField] private List<Button> challengeButtons;

    void Start()
    {
        for (int i = 0; i < prototypeButtons.Count; i++)
        {
            int index = i; // Capture the current value of i
            prototypeButtons[i].onClick.AddListener(() => LoadSceneByName("Prototype", index + 1));
        }

        for (int i = 0; i < challengeButtons.Count; i++)
        {
            int index = i; // Capture the current value of i
            challengeButtons[i].onClick.AddListener(() => LoadSceneByName("Challenge", index + 1));
        }
    }

    private void LoadSceneByName(string contextName, int index = 0)
    {
        string sceneName = contextName + " " + index;
        SceneManager.LoadScene(contextName + " " + index);
    }
}
