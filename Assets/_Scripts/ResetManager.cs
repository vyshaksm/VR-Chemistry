using UnityEngine;

public class ResetManager : MonoBehaviour
{
    // Optionally reference your spawners if you want to force them to 
    // refresh immediately after a clear
    private AtomSpawner[] allSpawners;

    void Start()
    {
        allSpawners = FindObjectsByType<AtomSpawner>(FindObjectsSortMode.None);
    }

    public void ResetLab()
    {
        // 1. Find and destroy all Molecules
        MoleculeInstance[] molecules = FindObjectsByType<MoleculeInstance>(FindObjectsSortMode.None);
        foreach (var mol in molecules)
        {
            Destroy(mol.gameObject);
        }

        // 2. Find and destroy all Atoms that have been grabbed/moved
        // We only destroy atoms that are 'Active' to avoid breaking the ones 
        // currently sitting on the spawners
        AtomController[] atoms = FindObjectsByType<AtomController>(FindObjectsSortMode.None);
        foreach (var atom in atoms)
        {
            if (atom.canBond) // If it's been picked up, clear it
            {
                Destroy(atom.gameObject);
            }
        }

        Debug.Log("Lab Reset: All molecules and active atoms cleared.");
    }
}