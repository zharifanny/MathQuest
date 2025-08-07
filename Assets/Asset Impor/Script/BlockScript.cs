using UnityEngine;
using UnityEngine.EventSystems;

public class ClickBlocker : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Cegah klik kalau mouse lagi di atas UI
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            // Lanjut raycast ke objek 3D
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Debug.Log("Klik objek: " + hit.collider.name);
                // tambahkan aksi lain di sini
            }
        }
    }
}
