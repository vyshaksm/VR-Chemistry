using UnityEngine;
using TMPro;
using System.Linq;

public class MoleculeRecipeItem : MonoBehaviour
{
    public TextMeshProUGUI recipeText;

    // Formats and displays one recipe entry for a molecule.
    public void SetupRecipe(MoleculeData data)
    {
        var atomGroups = data.requiredAtoms
            .GroupBy(a => a)
            .Select(g => $"{g.Count()}{g.Key}");

        string recipeString = string.Join(" + ", atomGroups);
        recipeText.text = $"{data.moleculeName} = {recipeString}";
    }
}
