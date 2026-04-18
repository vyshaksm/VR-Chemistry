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
    public bool canBond = false; // Prevents bonding while still on the spawner
    public bool IsCombining { get; private set; }

    private Transform playerCamera;
    private BondManager bondManager;
    private XRGrabInteractable grabInteractable;
    private Tween floatTween;

    private void Awake()
    {
        bondManager = FindFirstObjectByType<BondManager>();
        grabInteractable = GetComponent<XRGrabInteractable>();
        playerCamera = Camera.main.transform;

        // Subscribe to XR events
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    private void Start()
    {
        InitializeAtom();
        StartFloating();
    }

    public void InitializeAtom()
    {
        if (data == null) return;

        symbolText.text = data.symbol;

        // Apply color from ScriptableObject with 50% transparency
        Color c = data.atomColor;
        c.a = 0.5f;
        sphereRenderer.material.color = c;
    }

    private void Update()
    {
        // Billboarding: Always face the player
        if (playerCamera != null && symbolText != null)
        {
            symbolText.transform.LookAt(symbolText.transform.position + playerCamera.forward);
        }
    }

    public void StartFloating()
    {
        floatTween?.Kill();
        floatTween = transform.DOLocalMoveY(transform.localPosition.y + 0.05f, 2f)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        // Stop animation so it doesn't fight the player's hand
        floatTween?.Kill();

        // IMPORTANT: Now this atom is allowed to participate in chemistry
        canBond = true;
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        // Resume floating animation when dropped
        StartFloating();
    }

    public bool TryBeginCombination()
    {
        if (IsCombining) return false;
        IsCombining = true;
        return true;
    }

    private void OnTriggerEnter(Collider other)
    {
        // SAFETY: If bondManager is missing, already combining, 
        // or hasn't been picked up yet, do nothing.
        if (bondManager == null || IsCombining || !canBond)
            return;

        // Check for Atom-to-Atom collision
        AtomController otherAtom = other.GetComponent<AtomController>();
        if (otherAtom != null && otherAtom.canBond && !otherAtom.IsCombining)
        {
            // Use InstanceID to ensure only one of the two atoms triggers the bond
            if (gameObject.GetInstanceID() < other.gameObject.GetInstanceID())
            {
                bondManager.OnAtomTrigger(this, otherAtom);
            }
            return;
        }

        // Check for Atom-to-Molecule collision
        MoleculeInstance molecule = other.GetComponentInParent<MoleculeInstance>();
        if (molecule != null && !molecule.IsCombining)
        {
            bondManager.OnAtomMoleculeTrigger(this, molecule);
        }
    }

    private void OnDestroy()
    {
        floatTween?.Kill();
        grabInteractable.selectEntered.RemoveListener(OnGrab);
        grabInteractable.selectExited.RemoveListener(OnRelease);
    }
}