using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "MoleculeDatabase", menuName = "Chemistry/Molecule Database")]
public class MoleculeDatabase : ScriptableObject
{
    public List<MoleculeData> allMolecules;

    // Finds the molecule recipe that matches the provided atom list.
    public MoleculeData CheckMatch(List<string> currentAtoms)
    {
        currentAtoms.Sort();

        foreach (var molecule in allMolecules)
        {
            if (molecule.requiredAtoms.Count != currentAtoms.Count) continue;

            List<string> recipe = new List<string>(molecule.requiredAtoms);
            recipe.Sort();

            bool match = true;
            for (int i = 0; i < recipe.Count; i++)
            {
                if (recipe[i] != currentAtoms[i])
                {
                    match = false;
                    break;
                }
            }

            if (match) return molecule;
        }
        return null;
    }
}
