using System.IO;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    // Start() and Update() methods deleted - we don't need them right now

    public static MainManager Instance;
    public Color TeamColor;

    private void Awake()
    {
        // start of new code
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        // end of new code

        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadColor();
    }

    public void SaveColor()
    {
        SaveData data = new SaveData
        {
            TeamColor = TeamColor
        };

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadColor()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            TeamColor = data.TeamColor;
        }
    }

    [System.Serializable]
    class SaveData
    {
        public Color TeamColor;
    }
}
