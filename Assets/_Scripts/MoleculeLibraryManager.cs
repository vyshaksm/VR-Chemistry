using UnityEngine;
using System.Collections.Generic;

public class MoleculeLibraryManager : MonoBehaviour
{
    public MoleculeDatabase db;
    public GameObject uiItemPrefab;
    public Transform contentPanel;

    private Dictionary<string, MoleculeUIItem> activeUIItems = new Dictionary<string, MoleculeUIItem>();

    // Populates the molecule library UI when the manager starts.
    void Start()
    {
        PopulateLibrary();
    }

    // Rebuilds the scroll list from the molecule database.
    private void PopulateLibrary()
    {
        foreach (Transform child in contentPanel) Destroy(child.gameObject);

        foreach (var molecule in db.allMolecules)
        {
            GameObject newItem = Instantiate(uiItemPrefab, contentPanel);
            MoleculeUIItem script = newItem.GetComponent<MoleculeUIItem>();

            script.Setup(molecule.moleculeName, molecule.formula);
            activeUIItems.Add(molecule.formula, script);
        }
    }

    // Marks a discovered molecule entry as unlocked in the UI.
    public void NotifyDiscovery(string formula)
    {
        if (activeUIItems.ContainsKey(formula))
        {
            activeUIItems[formula].MarkAsDiscovered();
        }
    }
}
