using UnityEngine;
using System.Collections.Generic;

public class MoleculeLibraryManager : MonoBehaviour
{
    public MoleculeDatabase db;
    public GameObject uiItemPrefab;
    public Transform contentPanel;

    private Dictionary<string, MoleculeUIItem> activeUIItems = new Dictionary<string, MoleculeUIItem>();

    void Start()
    {
        PopulateLibrary();
    }

    private void PopulateLibrary()
    {
        // Clear existing items if any
        foreach (Transform child in contentPanel) Destroy(child.gameObject);

        // Create a list item for every molecule defined in your ScriptableObject
        foreach (var molecule in db.allMolecules)
        {
            GameObject newItem = Instantiate(uiItemPrefab, contentPanel);
            MoleculeUIItem script = newItem.GetComponent<MoleculeUIItem>();

            script.Setup(molecule.moleculeName, molecule.formula);

            // Store in dictionary for quick lookup by formula
            activeUIItems.Add(molecule.formula, script);
        }
    }

    // Call this from BondManager when a successful match is made
    public void NotifyDiscovery(string formula)
    {
        if (activeUIItems.ContainsKey(formula))
        {
            activeUIItems[formula].MarkAsDiscovered();
        }
    }
}