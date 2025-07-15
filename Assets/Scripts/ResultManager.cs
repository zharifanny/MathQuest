using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultManager : MonoBehaviour
{
    public static ResultManager Instance;

    [SerializeField] private Image[] stars;
    [SerializeField] private Color activeStarColor = Color.white;
    [SerializeField] private Color inactiveStarColor = Color.black;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            gameObject.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowStars(int falseCount)
    {
        // Hitung jumlah bintang berdasarkan kesalahan
        int starCount = 3;

        if (falseCount >= 1 && falseCount <= 2)
        {
            starCount = 2;
        }
        else if (falseCount > 2)
        {
            starCount = 1;
        }

        // Simpan star ke PlayerPrefs berdasarkan nama scene
        string currentScene = SceneManager.GetActiveScene().name;

        if (currentScene == "Level 1")
        {
            PlayerPrefs.SetInt("level1star", starCount);
        }
        else if (currentScene == "Level 2")
        {
            PlayerPrefs.SetInt("level2star", starCount);
        }
        else if (currentScene == "Level 3")
        {
            PlayerPrefs.SetInt("level3star", starCount);
        }

        PlayerPrefs.Save(); // Pastikan data disimpan

        // Update tampilan bintang
        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].color = i < starCount ? activeStarColor : inactiveStarColor;
        }
    }

    public void OnNextButtonClicked()
    {
        // Simpan star ke PlayerPrefs berdasarkan nama scene
        string currentScene = SceneManager.GetActiveScene().name;

        if (currentScene == "Level 1")
        {
            PlayerPrefs.SetInt("level1complete", 1);
        }
        else if (currentScene == "Level 2")
        {
            PlayerPrefs.SetInt("level2complete", 1);
        }
        else if (currentScene == "Level 3")
        {
            PlayerPrefs.SetInt("level3complete", 1);
        }

        PlayerPrefs.Save(); // Pastikan data disimpan
        GameManager.Instance.LoadMainMenu();
    }
}
