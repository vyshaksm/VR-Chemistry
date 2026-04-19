using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewMoleculeData", menuName = "Chemistry/Molecule Data")]
public class MoleculeData : ScriptableObject
{
    public string moleculeName;
    public string formula;

    [Header("Recipe")]
    public List<string> requiredAtoms;
    public string bondDetails { get; private set; }
    public enum BondType
    {
        Covalent,
        SingleCovalent,
        DoubleCovalent,
        TripleCovalent
    }

    public BondType bondType;

    // Derives the displayable bond description from the selected bond type.
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
