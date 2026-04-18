using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewMoleculeData", menuName = "Chemistry/Molecule Data")]
public class MoleculeData : ScriptableObject
{
    public string moleculeName; // e.g., "Water" [cite: 30]
    public string formula;      // e.g., "H2O" [cite: 30]

    [Header("Recipe")]
    // List of symbols required (e.g., ["H", "H", "O"])
    public List<string> requiredAtoms;

    [Header("Visuals & Info")]
    public GameObject moleculePrefab; // The 3D model with bonds [cite: 20, 24]
    [TextArea]
    public string bondDetails;        // e.g., "Single Covalent" [cite: 30]
}