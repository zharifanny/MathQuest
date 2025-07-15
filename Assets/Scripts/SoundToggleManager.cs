using UnityEngine;
using UnityEngine.UI;

public class SoundToggleManager : MonoBehaviour
{
    public static SoundToggleManager Instance;

    [Header("Sound Button Settings")]
    public Button[] soundButtons;               // Semua tombol yang ingin bisa mute/unmute
    public Sprite soundOnSprite;
    public Sprite soundOffSprite;

    private bool isMuted;

    private void Awake()
    {
        /*// Singleton optional
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // opsional jika ingin persist
        }
        else
        {
            Destroy(gameObject);
        }*/
    }

    private void Start()
    {
        // Ambil status mute dari PlayerPrefs
        isMuted = PlayerPrefs.GetInt("isMuted", 0) == 1;

        UpdateAudioState();
        SetupButtons();
    }

    private void SetupButtons()
    {
        foreach (Button btn in soundButtons)
        {
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(ToggleSound);
        }

        UpdateButtonSprites();
    }

    public void ToggleSound()
    {
        isMuted = !isMuted;
        PlayerPrefs.SetInt("isMuted", isMuted ? 1 : 0);
        PlayerPrefs.Save();

        UpdateAudioState();
        UpdateButtonSprites();
    }

    private void UpdateAudioState()
    {
        AudioListener.volume = isMuted ? 0 : 1;
    }

    private void UpdateButtonSprites()
    {
        foreach (Button btn in soundButtons)
        {
            Image btnImage = btn.GetComponent<Image>();
            if (btnImage != null)
            {
                btnImage.sprite = isMuted ? soundOffSprite : soundOnSprite;
            }
        }
    }
}
