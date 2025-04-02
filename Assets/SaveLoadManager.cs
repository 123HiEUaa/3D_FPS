using UnityEngine;
[AddComponentMenu("HMFPS/SaveLoadManager")]

public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager Instance { get; set; }

    string highSroreKey = "BestWaveSaveValue";
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);

        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(this);
    }

    public void SaveHighScore(int score)
    {
        PlayerPrefs.SetInt(highSroreKey, score);
    }

    public int LoadHighScore()
    {
        if (PlayerPrefs.HasKey(highSroreKey))
        {
            return PlayerPrefs.GetInt(highSroreKey);
        }
        else
        {
            return 0;
        }
    }
}
