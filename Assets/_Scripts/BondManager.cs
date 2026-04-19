using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class BondManager : MonoBehaviour
{
    public MoleculeDatabase db;
    public GameObject moleculePrefab;
    public AudioSource successSFX;
    public MoleculeLibraryManager libraryUI;

    // Combines two atoms into a new molecule instance.
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

    // Combines an atom and an existing molecule into a new molecule instance.
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

    // Combines two existing molecules into a new molecule instance.
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

    // Spawns a replacement molecule, updates its display, and notifies discovery UI.
    private void CreateMolecule(Vector3 position, List<string> elements, params GameObject[] consumedObjects)
    {
        foreach (var consumedObject in consumedObjects)
        {
            if (consumedObject != null)
                Destroy(consumedObject);
        }

        GameObject newMol = Instantiate(moleculePrefab, position, Quaternion.identity);
        Vector3 targetScale = newMol.transform.localScale;
        MoleculeInstance instance = newMol.GetComponent<MoleculeInstance>();
        instance.db = db;

        foreach (var element in elements)
            instance.AddAtom(element, false);

        instance.UpdateDisplay();

        newMol.transform.localScale = Vector3.zero;
        newMol.transform.DOScale(targetScale, 0.5f).SetEase(Ease.OutBack);

        MoleculeData match = db.CheckMatch(instance.currentElements);
        if (match != null)
        {
            if (successSFX != null) successSFX.Play();
            if (libraryUI != null)
                libraryUI.NotifyDiscovery(match.formula);
        }
    }
}
