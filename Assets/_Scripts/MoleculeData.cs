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
    public string bondDetails { get; private set; }        // e.g., "Single Covalent" [cite: 30]
    public enum BondType
    {
        Covalent,
        SingleCovalent,
        DoubleCovalent,
        TripleCovalent
    }

    public BondType bondType;        // Enum for bond type (for visuals) [cite: 30]

    private void Awake()
    {
        bondDetails= bondType switch
        {
            BondType.Covalent => "Covalent",
            BondType.SingleCovalent => "Single Covalent",
            BondType.DoubleCovalent => "Double Covalent",
            BondType.TripleCovalent => "Triple Covalent",
            _ => "Unknown Bond Type"
        };
    }
}