using UnityEngine;

public class ResetManager : MonoBehaviour
{
    private AtomSpawner[] allSpawners;

    // Caches the available atom spawners in the scene.
    void Start()
    {
        allSpawners = FindObjectsByType<AtomSpawner>(FindObjectsSortMode.None);
    }

    // Clears spawned molecules and reusable atoms from the lab.
    public void ResetLab()
    {
        MoleculeInstance[] molecules = FindObjectsByType<MoleculeInstance>(FindObjectsSortMode.None);
        foreach (var mol in molecules)
        {
            Destroy(mol.gameObject);
        }

        AtomController[] atoms = FindObjectsByType<AtomController>(FindObjectsSortMode.None);
        foreach (var atom in atoms)
        {
            if (atom.canBond)
            {
                Destroy(atom.gameObject);
            }
        }

        Debug.Log("Lab Reset: All molecules and active atoms cleared.");
    }
}
