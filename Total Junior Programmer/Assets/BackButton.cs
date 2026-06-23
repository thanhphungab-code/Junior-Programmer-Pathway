using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BackButton : MonoBehaviour
{
    private Button backButton;
    void Start()
    {
        backButton = GetComponent<Button>();
        backButton.onClick.AddListener(OnBackButtonClicked);
    }

    private void OnBackButtonClicked()
    {
        Physics.gravity = new Vector3(0, -9.81f, 0);
        SceneManager.LoadScene("_Summary");
    }

}
