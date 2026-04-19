using UnityEngine;
using DG.Tweening;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class AtomAnimation : MonoBehaviour
{
    [Header("Animation Settings")]
    [SerializeField] private float floatDistance = 0.05f;
    [SerializeField] private float floatDuration = 2f;

    private Tween floatTween;
    private XRGrabInteractable grabInteractable;
    private Vector3 originalLocalPosition;

    // Caches required components before the object starts running.
    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    // Stores the starting position and begins the floating motion.
    void Start()
    {
        originalLocalPosition = transform.localPosition;
        StartFloating();
    }

    // Starts or restarts the looping float animation.
    public void StartFloating()
    {
        floatTween?.Kill();
        floatTween = transform.DOLocalMoveY(transform.localPosition.y + floatDistance, floatDuration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }

    // Stops the float animation when the object is grabbed.
    private void OnGrab(SelectEnterEventArgs args)
    {
        floatTween?.Kill();
    }

    // Restarts the float animation after the object is released.
    private void OnRelease(SelectExitEventArgs args)
    {
        StartFloating();
    }

    // Cleans up the active tween when the object is destroyed.
    private void OnDestroy()
    {
        floatTween?.Kill();
    }
}
