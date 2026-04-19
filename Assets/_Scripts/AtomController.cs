using UnityEngine;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using DG.Tweening;

[RequireComponent(typeof(XRGrabInteractable))]
public class AtomController : MonoBehaviour
{
    [Header("Data & Visuals")]
    public AtomData data;
    [SerializeField] private TextMeshPro symbolText;
    [SerializeField] private MeshRenderer sphereRenderer;

    [Header("State")]
    public bool canBond = false;
    public bool IsCombining { get; private set; }

    private Transform playerCamera;
    private BondManager bondManager;
    private XRGrabInteractable grabInteractable;
    private Tween floatTween;

    // Caches scene references and subscribes to grab events.
    private void Awake()
    {
        bondManager = FindFirstObjectByType<BondManager>();
        grabInteractable = GetComponent<XRGrabInteractable>();
        playerCamera = Camera.main.transform;

        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    // Initializes the atom visuals and starts the floating animation.
    private void Start()
    {
        InitializeAtom();
        StartFloating();
    }

    // Applies the atom symbol and color from its data asset.
    public void InitializeAtom()
    {
        if (data == null) return;

        symbolText.text = data.symbol;

        Color c = data.atomColor;
        c.a = 0.5f;
        sphereRenderer.material.color = c;
    }

    // Keeps the symbol text facing the player camera.
    private void Update()
    {
        if (playerCamera != null && symbolText != null)
        {
            symbolText.transform.LookAt(symbolText.transform.position + playerCamera.forward);
        }
    }

    // Starts or restarts the atom floating animation.
    public void StartFloating()
    {
        floatTween?.Kill();
        floatTween = transform.DOLocalMoveY(transform.localPosition.y + 0.05f, 2f)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }

    // Stops floating and marks the atom as available for bonding.
    private void OnGrab(SelectEnterEventArgs args)
    {
        floatTween?.Kill();
        canBond = true;
    }

    // Restarts floating when the atom is released.
    private void OnRelease(SelectExitEventArgs args)
    {
        StartFloating();
    }

    // Locks the atom so it can only be consumed by one combine flow.
    public bool TryBeginCombination()
    {
        if (IsCombining) return false;
        IsCombining = true;
        return true;
    }

    // Detects trigger-based interactions with atoms and molecules.
    private void OnTriggerEnter(Collider other)
    {
        if (bondManager == null || IsCombining || !canBond)
            return;

        AtomController otherAtom = other.GetComponent<AtomController>();
        if (otherAtom != null && otherAtom.canBond && !otherAtom.IsCombining)
        {
            // Only one of the overlapping atoms should start the combine flow.
            if (gameObject.GetInstanceID() < other.gameObject.GetInstanceID())
            {
                bondManager.OnAtomTrigger(this, otherAtom);
            }
            return;
        }

        MoleculeInstance molecule = other.GetComponentInParent<MoleculeInstance>();
        if (molecule != null && !molecule.IsCombining)
        {
            bondManager.OnAtomMoleculeTrigger(this, molecule);
        }
    }

    // Removes listeners and active tweens during cleanup.
    private void OnDestroy()
    {
        floatTween?.Kill();
        grabInteractable.selectEntered.RemoveListener(OnGrab);
        grabInteractable.selectExited.RemoveListener(OnRelease);
    }
}
