// ItemController.cs (Modifikasi)
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;
using System;
using UnityEngine.EventSystems;
using TMPro;

[RequireComponent(typeof(Collider))]
public class ItemController : MonoBehaviour
{
    [Header("Soal Settings")]
    [SerializeField] private GameObject soalCanvas;
    [SerializeField] private Image soalImage;

    [Header("Button Settings")]
    [SerializeField] private Button[] answerButtons;
    [SerializeField] private TMP_Text[] optionTexts;

    [Header("World Canvas Settings")]
    [SerializeField] private Canvas itemCanvas;
    [SerializeField] private Image cooldownFill;
    [SerializeField] private Vector3 canvasOffset = new Vector3(0, 0.5f, 0);

    [Header("Audio Settings")]
    [SerializeField] private AudioClip correctSound;
    [SerializeField] private AudioClip wrongSound;

    [Header("Animation Settings")]
    [SerializeField] private float scaleDownDuration = 0.5f;

    private bool isActive = true;
    private SoalData currentSoal;
    private int correctAnswerIndex;
    private AudioSource audioSource;
    private static System.Random rng = new System.Random();
    [Header("Parent Object Settings")]
    [SerializeField] private HideableObject parentContainer; // Tambahkan referensi ke script HideableObject di TV

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        InitializeWorldCanvas();
        ValidateReferences();
    }

    void InitializeWorldCanvas()
    {
        if (itemCanvas != null)
        {
            itemCanvas.worldCamera = Camera.main;
            itemCanvas.gameObject.SetActive(false);
            itemCanvas.gameObject.AddComponent<Billboard>();
        }
    }

    void ValidateReferences()
    {
        if (answerButtons.Length != 3 || optionTexts.Length != 3)
        {
            Debug.LogError("Pastikan ada 3 button dan 3 text option!");
        }
    }

    /*private void OnMouseDown()
    {
        if (isActive  *//*&& !IsPointerOverUI()*//*)
        {
            StartCoroutine(ShowSoalRoutine());
        }
    }*/

    public void OnClicked()
    {
        if (GameManager.Instance.isSoalActive)
            return;
        Debug.Log("Item diklik: " + name);
        // logic soal muncul di sini
        if (isActive)
        {
            StartCoroutine(ShowSoalRoutine());
        }
    }

    IEnumerator ShowSoalRoutine()
    {
        isActive = false;
        GameManager.Instance.isSoalActive = true;
        SetRandomSoal();
        SetupButtonListeners();
        ShowQuestionUI();

        yield return StartCoroutine(WaitForAnswer());

        HandlePostAnswer();
        GameManager.Instance.isSoalActive = false;
    }

    void SetRandomSoal()
    {
        currentSoal = GlobalQuestionManager.Instance.GetRandomUnusedQuestion();
        UpdateQuestionDisplay();
        ShuffleOptions();
    }

    void UpdateQuestionDisplay()
    {
        soalImage.sprite = currentSoal.soalSprite;
    }

    void ShuffleOptions()
    {
        string[] options = new string[] {
            currentSoal.correctAnswer,
            currentSoal.wrongAnswers[0],
            currentSoal.wrongAnswers[1]
        };

        options = options.OrderBy(_ => rng.Next()).ToArray();

        for (int i = 0; i < 3; i++)
        {
            optionTexts[i].text = options[i];
            if (options[i] == currentSoal.correctAnswer)
            {
                correctAnswerIndex = i;
            }
        }
    }

    void SetupButtonListeners()
    {
        for (int i = 0; i < answerButtons.Length; i++)
        {
            int index = i;
            answerButtons[i].onClick.RemoveAllListeners();
            answerButtons[i].onClick.AddListener(() => OnAnswerSelected(index));
        }
    }

    void OnAnswerSelected(int selectedIndex)
    {
        if (selectedIndex == correctAnswerIndex)
        {
            HandleCorrectAnswer();
        }
        else
        {
            HandleWrongAnswer();
        }

        CloseQuestionUI();
    }

    void HandleCorrectAnswer()
    {
        audioSource.PlayOneShot(correctSound);
        currentSoal = null;
        StartCoroutine(DisableItem());
    }

    void HandleWrongAnswer()
    {
        GameManager.Instance.AddFalseAnswer();
        audioSource.PlayOneShot(wrongSound);
        GlobalQuestionManager.Instance.ReturnQuestion(currentSoal);
        StartCoroutine(CooldownRoutine());
    }

    IEnumerator WaitForAnswer()
    {
        //GameManager.Instance.PauseGame();
        yield return new WaitWhile(() => soalCanvas.activeSelf);
    }

    IEnumerator DisableItem()
{
    GetComponent<Collider>().enabled = false;
    yield return StartCoroutine(ScaleDownAnimation());
    
    if(parentContainer != null)
    {
        parentContainer.ForceClose(); // Hanya nonaktifkan script dan collider
    }
    
    gameObject.SetActive(false);
    GameManager.Instance.AddCompletedItem();
}

    IEnumerator ScaleDownAnimation()
    {
        Vector3 originalScale = transform.localScale;
        float timer = 0f;

        while (timer < scaleDownDuration)
        {
            transform.localScale = originalScale * (1 - (timer / scaleDownDuration));
            timer += Time.deltaTime;
            yield return null;
        }
    }

    /*IEnumerator CooldownRoutine()
    {
        if (itemCanvas != null)
        {
            itemCanvas.gameObject.SetActive(true);
            float timer = 5f;

            while (timer > 0)
            {
                cooldownFill.fillAmount = timer / 5f;
                itemCanvas.transform.position = transform.position + canvasOffset;
                timer -= Time.deltaTime;
                yield return null;
            }

            itemCanvas.gameObject.SetActive(false);
        }

        isActive = true;
    }*/
    IEnumerator CooldownRoutine()
    {
        if (parentContainer != null)
        {
            // Disable collider milik parent (objek yang menutupi item)
            Collider parentCollider = parentContainer.GetComponent<Collider>();
            if (parentCollider != null)
            {
                parentCollider.enabled = false;
            }
        }

        if (itemCanvas != null)
        {
            itemCanvas.gameObject.SetActive(true);
            float timer = 5f;

            while (timer > 0)
            {
                cooldownFill.fillAmount = timer / 5f;
                itemCanvas.transform.position = transform.position + canvasOffset;
                timer -= Time.deltaTime;
                yield return null;
            }

            itemCanvas.gameObject.SetActive(false);
        }

        if (parentContainer != null)
        {
            // Re-enable collider setelah cooldown selesai
            Collider parentCollider = parentContainer.GetComponent<Collider>();
            if (parentCollider != null)
            {
                parentCollider.enabled = true;
            }
        }

        isActive = true;
    }


    void ShowQuestionUI()
    {
        soalCanvas.SetActive(true);
    }

    void CloseQuestionUI()
    {
        soalCanvas.SetActive(false);
        //GameManager.Instance.ResumeGame();
    }

    void HandlePostAnswer()
    {
        if (currentSoal != null)
        {
            GlobalQuestionManager.Instance.ReturnQuestion(currentSoal);
            currentSoal = null;
        }
    }

    bool IsPointerOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
}

public static class ArrayExtensions
{
    public static T RandomElement<T>(this T[] array)
    {
        return array[UnityEngine.Random.Range(0, array.Length)];
    }
}