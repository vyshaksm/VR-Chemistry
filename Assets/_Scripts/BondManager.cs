using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class BondManager : MonoBehaviour
{
    public MoleculeDatabase db;
    public GameObject moleculePrefab;
    public AudioSource successSFX;
    public MoleculeLibraryManager libraryUI;

    public void OnAtomTrigger(AtomController a, AtomController b)
    {
        if (a == null || b == null || a == b)
            return;

        if (a.IsCombining || b.IsCombining)
            return;

        if (!a.TryBeginCombination() || !b.TryBeginCombination())
            return;

        Vector3 midPoint = (a.transform.position + b.transform.position) / 2f;
        var elements = new List<string> { a.data.symbol, b.data.symbol };

        CreateMolecule(midPoint, elements, a.gameObject, b.gameObject);
    }

    public void OnAtomMoleculeTrigger(AtomController atom, MoleculeInstance molecule)
    {
        if (atom == null || molecule == null)
            return;

        if (atom.IsCombining || molecule.IsCombining)
            return;

        if (!atom.TryBeginCombination() || !molecule.TryBeginCombination())
            return;

        Vector3 midPoint = (atom.transform.position + molecule.transform.position) / 2f;
        var elements = new List<string>(molecule.currentElements) { atom.data.symbol };

        CreateMolecule(midPoint, elements, atom.gameObject, molecule.gameObject);
    }

    public void OnMoleculeTrigger(MoleculeInstance first, MoleculeInstance second)
    {
        if (first == null || second == null || first == second)
            return;

        if (first.IsCombining || second.IsCombining)
            return;

        if (!first.TryBeginCombination() || !second.TryBeginCombination())
            return;

        Vector3 midPoint = (first.transform.position + second.transform.position) / 2f;
        var elements = new List<string>(first.currentElements);
        elements.AddRange(second.currentElements);

        CreateMolecule(midPoint, elements, first.gameObject, second.gameObject);
    }

    private void CreateMolecule(Vector3 position, List<string> elements, params GameObject[] consumedObjects)
    {
        // 1. Clean up the ingredients
        foreach (var consumedObject in consumedObjects)
        {
            if (consumedObject != null)
                Destroy(consumedObject);
        }

        // 2. Instantiate and setup the new Molecule instance
        GameObject newMol = Instantiate(moleculePrefab, position, Quaternion.identity);
        Vector3 targetScale = newMol.transform.localScale; // Capture the 0.45 scale
        MoleculeInstance instance = newMol.GetComponent<MoleculeInstance>();
        instance.db = db;

        // 3. Populate elements silently (no UI flicker)
        foreach (var element in elements)
            instance.AddAtom(element, false);

        // 4. Update the billboarded TextMeshPro labels
        instance.UpdateDisplay();

        // 5. Play the "Pop" animation
        newMol.transform.localScale = Vector3.zero;
        newMol.transform.DOScale(targetScale, 0.5f).SetEase(Ease.OutBack);

        // 6. Check for Discovery and update the Scroll View Library
        MoleculeData match = db.CheckMatch(instance.currentElements);
        if (match != null)
        {
            if (successSFX != null) successSFX.Play();

            // This line connects to your new MoleculeLibraryManager
            if (libraryUI != null)
                libraryUI.NotifyDiscovery(match.formula);
        }
    }
}
