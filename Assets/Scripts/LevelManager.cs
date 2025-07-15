// LevelManager.cs (jika diperlukan)
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private int JumlahItem = 3;
    void Start()
    {
        // Panggil di awal level
        GameManager.Instance.SetTotalItems(JumlahItem); // Sesuaikan dengan jumlah item
    }
}