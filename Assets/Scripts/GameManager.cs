// GameManager.cs (Tambahkan bagian ini)
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool isSoalActive = false;

    [Header("Result Settings")]
    [SerializeField] private GameObject resultCanvas;

    private int falseCount = 0;
    private int totalItems;
    private int collectedItems = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddFalseAnswer()
    {
        falseCount++;
    }

    public void AddCompletedItem()
    {
        collectedItems++;
        CheckGameCompletion();
    }

    private void CheckGameCompletion()
    {
        if (collectedItems >= totalItems)
        {
            ShowResult();
        }
    }

    public void SetTotalItems(int count)
    {
        totalItems = count;
    }

    private void ShowResult()
    {
        resultCanvas.SetActive(true);
        ResultManager.Instance.ShowStars(falseCount);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}

