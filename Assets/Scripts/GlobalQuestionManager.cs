// GlobalQuestionManager.cs
using UnityEngine;
using System.Collections.Generic;

public class GlobalQuestionManager : MonoBehaviour
{
    public static GlobalQuestionManager Instance;

    [SerializeField] private List<SoalData> allSoal;
    private List<SoalData> availableSoal = new List<SoalData>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitializeQuestions();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void InitializeQuestions()
    {
        availableSoal = new List<SoalData>(allSoal);
    }

    public SoalData GetRandomUnusedQuestion()
    {
        if (availableSoal.Count == 0)
        {
            ResetQuestions();
            Debug.Log("Reset semua soal karena sudah terpakai semua");
        }

        int randomIndex = Random.Range(0, availableSoal.Count);
        SoalData selectedSoal = availableSoal[randomIndex];
        availableSoal.RemoveAt(randomIndex);

        return selectedSoal;
    }

    public void ReturnQuestion(SoalData soal)
    {
        if (!availableSoal.Contains(soal))
        {
            availableSoal.Add(soal);
        }
    }

    public void ResetQuestions()
    {
        availableSoal = new List<SoalData>(allSoal);
    }
}