using UnityEngine;
using System.Collections;

public class HideableObject : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private Vector3 moveOffset;
    [SerializeField] private Vector3 rotateAngles;
    [SerializeField] private float animationDuration = 0.8f;

    [Header("References")]
    [SerializeField] private GameObject hiddenItem;
    [SerializeField] private AudioClip moveSound;

    private Vector3 originalLocalPosition;
    private Quaternion originalLocalRotation;
    private bool isOpen = false;
    private AudioSource audioSource;
    private Collider objectCollider;
    private bool isPermanentlyClosed = false;
    private int hiddenItemOriginalLayer;
    private bool isAnimating = false; // Flag untuk tracking animasi

    void Start()
    {
        originalLocalPosition = transform.localPosition;
        originalLocalRotation = transform.localRotation;
        audioSource = GetComponent<AudioSource>();
        objectCollider = GetComponent<Collider>();

        if (hiddenItem != null)
        {
            hiddenItemOriginalLayer = hiddenItem.layer;
            hiddenItem.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100f, LayerMask.GetMask("Interactable")))
            {
                Debug.Log($"Klik pada: {hit.collider.name}");

                ItemController item = hit.collider.GetComponent<ItemController>();
                if (item != null)
                {
                    item.OnClicked(); // Panggil fungsi interaksi langsung
                }
            }
        }
    }

    private void OnMouseDown()
    {
        if (isPermanentlyClosed || isAnimating) return;
        if (IsPointerOverItem()) return;

        StartCoroutine(HandleInteraction());
    }

    private IEnumerator HandleInteraction()
    {
        isAnimating = true;

        if (!isOpen)
        {
            yield return StartCoroutine(OpenObject());
        }
        else
        {
            yield return StartCoroutine(CloseObject());
        }

        isAnimating = false;
    }

    private IEnumerator OpenObject()
    {
        objectCollider.enabled = false;
        isOpen = true;

        if (moveSound != null) audioSource.PlayOneShot(moveSound);

        Vector3 targetLocalPosition = originalLocalPosition + moveOffset;
        Quaternion targetLocalRotation = originalLocalRotation * Quaternion.Euler(rotateAngles);

        float elapsed = 0;
        while (elapsed < animationDuration)
        {
            transform.localPosition = Vector3.Lerp(
                originalLocalPosition,
                targetLocalPosition,
                elapsed / animationDuration
            );

            transform.localRotation = Quaternion.Lerp(
                originalLocalRotation,
                targetLocalRotation,
                elapsed / animationDuration
            );

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = targetLocalPosition;
        transform.localRotation = targetLocalRotation;

        if (hiddenItem != null)
        {
            hiddenItem.SetActive(true);
            hiddenItem.layer = LayerMask.NameToLayer("Interactable");
        }

        objectCollider.enabled = true;
    }

    public void ForceClose()
    {
        if (!isPermanentlyClosed)
        {
            StartCoroutine(ForceCloseRoutine());
        }
    }

    private IEnumerator ForceCloseRoutine()
    {
        yield return StartCoroutine(CloseObject());
        isPermanentlyClosed = true;
        objectCollider.enabled = false;
        this.enabled = false;
    }

    private IEnumerator CloseObject()
    {
        objectCollider.enabled = false;
        isOpen = false;

        if (moveSound != null) audioSource.PlayOneShot(moveSound);

        Vector3 startLocalPosition = transform.localPosition;
        Quaternion startLocalRotation = transform.localRotation;

        float elapsed = 0;
        while (elapsed < animationDuration)
        {
            transform.localPosition = Vector3.Lerp(
                startLocalPosition,
                originalLocalPosition,
                elapsed / animationDuration
            );

            transform.localRotation = Quaternion.Lerp(
                startLocalRotation,
                originalLocalRotation,
                elapsed / animationDuration
            );

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalLocalPosition;
        transform.localRotation = originalLocalRotation;

        if (hiddenItem != null)
        {
            hiddenItem.SetActive(false);
            hiddenItem.layer = hiddenItemOriginalLayer;
        }

        objectCollider.enabled = true;
    }

    private bool IsPointerOverItem()
    {
        if (hiddenItem == null || !hiddenItem.activeSelf) return false;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray, 100f, LayerMask.GetMask("Interactable"));

        foreach (RaycastHit hit in hits)
        {
            // Cek apakah yang diklik adalah item atau child-nya
            if (hit.collider.transform.IsChildOf(hiddenItem.transform))
            {
                return true;
            }
        }
        return false;
    }
}