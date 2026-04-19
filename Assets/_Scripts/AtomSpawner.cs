using UnityEngine;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class AtomSpawner : MonoBehaviour
{
    public GameObject atomPrefab;
    public Transform spawnAnchor;
    public Vector3 spawnOffset;
    public float spawnDelay = 1.0f;

    // Spawns the initial atom when the spawner starts.
    private void Start()
    {
        SpawnNewAtom();
    }

    // Creates a new atom instance and hooks its grab callback.
    public void SpawnNewAtom()
    {
        Vector3 finalPosition = spawnAnchor.position + spawnOffset;
        GameObject newAtom = Instantiate(atomPrefab, finalPosition, spawnAnchor.rotation);

        XRGrabInteractable grabInteractable = newAtom.GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(OnAtomGrabbed);
    }

    // Removes the one-shot listener and schedules the replacement spawn.
    private void OnAtomGrabbed(SelectEnterEventArgs args)
    {
        XRGrabInteractable grabbedObject = args.interactableObject.transform.GetComponent<XRGrabInteractable>();
        grabbedObject.selectEntered.RemoveListener(OnAtomGrabbed);
        StartCoroutine(DelayedSpawn());
    }

    // Waits before spawning a replacement atom.
    private IEnumerator DelayedSpawn()
    {
        yield return new WaitForSeconds(spawnDelay);
        SpawnNewAtom();
    }

    // Draws the spawn position gizmo in the editor.
    private void OnDrawGizmos()
    {
        if (spawnAnchor != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(spawnAnchor.position + spawnOffset, 0.05f);
        }
    }
}
