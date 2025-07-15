// SoalData.cs (Tetap sama tanpa hasBeenUsed)
using UnityEngine;

[CreateAssetMenu(fileName = "New Soal", menuName = "Soal Data")]
public class SoalData : ScriptableObject
{
    public Sprite soalSprite;
    public string correctAnswer;
    public string[] wrongAnswers;
}