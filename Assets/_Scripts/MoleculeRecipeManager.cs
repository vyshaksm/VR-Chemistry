using UnityEngine;

public class MoleculeRecipeManager : MonoBehaviour
{
    public MoleculeDatabase db;
    public GameObject recipePrefab;
    public Transform contentPanel;

    // Generates the recipe list when the manager starts.
    void Start()
    {
        GenerateRecipeList();
    }

    // Rebuilds the recipe panel from the molecule database.
    private void GenerateRecipeList()
    {
        foreach (Transform child in contentPanel) Destroy(child.gameObject);

        foreach (var molecule in db.allMolecules)
        {
            GameObject newRecipe = Instantiate(recipePrefab, contentPanel);
            MoleculeRecipeItem itemScript = newRecipe.GetComponent<MoleculeRecipeItem>();

            if (itemScript != null)
            {
                itemScript.SetupRecipe(molecule);
            }
        }
    }
}
