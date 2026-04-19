using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine.UI;

public class MoleculeInstance : MonoBehaviour
{
    public GameObject moleculeDetailObj;
    public TextMeshPro formulaText;
    public TextMeshProUGUI bondTypeDetailText;
    public Text moleculeDetailText;
    public TextMeshProUGUI formulaDetailText;
    public List<string> currentElements = new List<string>();
    public MoleculeDatabase db;

    private BondManager bondManager;
    private Transform playerCamera;

    public bool IsCombining { get; private set; }

    // Caches the bond manager reference for later trigger handling.
    private void Awake()
    {
        bondManager = FindFirstObjectByType<BondManager>();
        playerCamera = Camera.main.transform;
    }

    // Adds an atom symbol to the molecule and optionally refreshes the UI.
    public void AddAtom(string symbol, bool updateNow = true)
    {
        currentElements.Add(symbol);
        if (updateNow) UpdateDisplay();
    }

    // Updates the molecule text and detail panel based on the current atom list.
    public void UpdateDisplay()
    {
        if (db == null)
        {
            Debug.LogError("Database is missing on MoleculeInstance!");
            return;
        }

        MoleculeData match = db.CheckMatch(currentElements);

        if (match != null)
        {
            formulaText.text = match.formula;
            bondTypeDetailText.text = match.bondDetails;
            moleculeDetailText.text = match.moleculeName;
            formulaDetailText.text = match.formula;
            moleculeDetailObj.SetActive(true);
        }
        else
        {
            var counts = currentElements.OrderBy(e => e)
                                        .GroupBy(e => e)
                                        .Select(g => g.Key + (g.Count() > 1 ? g.Count().ToString() : ""));
            string intermediate = string.Concat(counts);
            formulaText.text = intermediate;
            moleculeDetailObj.SetActive(false);
        }
    }

    // Locks the molecule so only one combine operation can consume it.
    public bool TryBeginCombination()
    {
        if (IsCombining)
            return false;

        IsCombining = true;
        return true;
    }

    // Rotates the molecule labels to face the player.
    void Update()
    {
        if (playerCamera != null)
        {
            if (formulaText != null)
                formulaText.transform.LookAt(formulaText.transform.position + playerCamera.forward);

            if (moleculeDetailObj != null && moleculeDetailObj.activeSelf)
                moleculeDetailObj.transform.LookAt(moleculeDetailObj.transform.position + playerCamera.forward);
        }
    }

    // Detects trigger-based interactions with atoms and other molecules.
    private void OnTriggerEnter(Collider other)
    {
        if (bondManager == null || IsCombining)
            return;

        AtomController atom = other.GetComponent<AtomController>();
        if (atom != null)
        {
            bondManager.OnAtomMoleculeTrigger(atom, this);
            return;
        }

        MoleculeInstance otherMolecule = other.GetComponent<MoleculeInstance>();
        if (otherMolecule != null)
            bondManager.OnMoleculeTrigger(this, otherMolecule);
    }
}
