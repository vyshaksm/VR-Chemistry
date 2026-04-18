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

    private void Awake()
    {
        bondManager = FindFirstObjectByType<BondManager>();
    }

    public void AddAtom(string symbol, bool updateNow = true)
    {
        currentElements.Add(symbol);
        if (updateNow) UpdateDisplay();
    }

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
            Debug.Log("Found Match: " + match.formula); // Check your console!
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
            Debug.Log("Intermediate Name: " + intermediate); // Check your console!
            moleculeDetailObj.SetActive(false);
        }
    }

    public bool TryBeginCombination()
    {
        if (IsCombining)
            return false;

        IsCombining = true;
        return true;
    }

    void Update()
    {
        // BILLBOARD EFFECT: Apply to both text elements
        if (playerCamera != null)
        {
            // Formula Text faces player
            if (formulaText != null)
                formulaText.transform.LookAt(formulaText.transform.position + playerCamera.forward);

            // Bond Type Text faces player (only if active)
            if (moleculeDetailObj != null && moleculeDetailObj.activeSelf)
                moleculeDetailObj.transform.LookAt(moleculeDetailObj.transform.position + playerCamera.forward);
        }
    }

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
