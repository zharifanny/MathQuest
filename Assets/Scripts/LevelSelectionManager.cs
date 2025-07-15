using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionManager : MonoBehaviour
{
    [System.Serializable]
    public class LevelUI
    {
        public Button levelButton;
        public Image[] stars; // Pastikan urutannya 0,1,2
    }

    public LevelUI[] levelUIs; // Harus 3 data, satu untuk setiap level
    public Color activeStarColor = Color.white;
    public Color inactiveStarColor = Color.black;

    void Start()
    {
        SetupLevelUI();
    }

    void SetupLevelUI()
    {
        // Level 1
        SetStars(levelUIs[0].stars, PlayerPrefs.GetInt("level1star", 0));
        levelUIs[0].levelButton.interactable = true;

        // Level 2
        SetStars(levelUIs[1].stars, PlayerPrefs.GetInt("level2star", 0));
        bool level1Complete = PlayerPrefs.GetInt("level1complete", 0) == 1;
        levelUIs[1].levelButton.interactable = level1Complete;

        // Level 3
        SetStars(levelUIs[2].stars, PlayerPrefs.GetInt("level3star", 0));
        bool level2Complete = PlayerPrefs.GetInt("level2complete", 0) == 1;
        levelUIs[2].levelButton.interactable = level2Complete;
    }

    void SetStars(Image[] starImages, int starCount)
    {
        for (int i = 0; i < starImages.Length; i++)
        {
            if (starImages[i] != null)
            {
                starImages[i].color = i < starCount ? activeStarColor : inactiveStarColor;
            }
        }
    }
}
