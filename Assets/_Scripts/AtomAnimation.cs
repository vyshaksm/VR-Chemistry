using UnityEngine;
using DG.Tweening; // Import DOTween
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class AtomAnimation : MonoBehaviour
{
    [Header("Animation Settings")]
    [SerializeField] private float floatDistance = 0.05f; // How high it floats
    [SerializeField] private float floatDuration = 2f;    // Speed of the float

    private Tween floatTween;
    private XRGrabInteractable grabInteractable;
    private Vector3 originalLocalPosition;

    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();

       // grabInteractable.selectEntered.AddListener(OnGrab);

       // grabInteractable.selectExited.AddListener(OnRelease);
    }

    void Start()
    {
        originalLocalPosition = transform.localPosition;
        StartFloating();
    }

    public void StartFloating()
    {
        // Safety: kill any existing tween first
        floatTween?.Kill();

        // Create a smooth Y-axis loop
        // Move from original position to +floatDistance and back
        floatTween = transform.DOLocalMoveY(transform.localPosition.y + floatDistance, floatDuration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        // "When grabbed a atom another is enabled... no animation"
        floatTween?.Kill();
    }
    private void OnRelease(SelectExitEventArgs args)
    {
        // "When released the same animation"
        StartFloating();
    }

    private void OnDestroy()
    {
        // Best practice: cleanup tweens when object is destroyed
        floatTween?.Kill();
    }
}