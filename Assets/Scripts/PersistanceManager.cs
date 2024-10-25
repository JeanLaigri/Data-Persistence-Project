using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PersistanceManager : MonoBehaviour
{
    public static PersistanceManager Instance;

    [SerializeField] TMP_Text bestScoreText;
    [SerializeField] TMP_InputField nameField;

    public string username;

    public string bestName;
    public int bestScore;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadGameInfo();
    }

    void Start()
    {
        if (bestName != "")
        {
            bestScoreText.text = "Best Score : " + bestName + " : " + bestScore;
        }
    }

    public void SetBestScore(int score)
    {
        if (score > bestScore)
        {
            bestScore = score;
            bestName = username;
            SaveGameInfo();
            MainManager.Instance.BestScoreText.text = "Best Score : " + bestName + " : " + bestScore;
        }
        Debug.Log("Score: " + score + "  Player: " + username);
    }

    public void StartNew()
    {
        if (nameField.text != "")
        {
            username = nameField.text;
            SceneManager.LoadScene(1);
        }
        else
        {
            Debug.LogWarning("Please enter a name!");
        }
    }

    public void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
    Application.Quit();
#endif
    }

    [System.Serializable]
    class SaveData
    {
        public string username;
        public int bestScore;
    }

    public void SaveGameInfo()
    {
        SaveData data = new SaveData();
        data.username = bestName;
        data.bestScore = bestScore;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadGameInfo()
    {
        string path = Application.persistentDataPath + "/savefile.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);

            SaveData data = JsonUtility.FromJson<SaveData>(json);

            bestName = data.username;
            bestScore = data.bestScore;
            if (!string.IsNullOrEmpty(bestName))
                nameField.text = bestName;
        }
    }
}
