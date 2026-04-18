using UnityEngine;
using System.Collections; // Required for Coroutines
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class AtomSpawner : MonoBehaviour
{
    public GameObject atomPrefab;
    public Transform spawnAnchor;
    public Vector3 spawnOffset;
    public float spawnDelay = 1.0f; // Set the delay time in seconds

    private void Start()
    {
        SpawnNewAtom();
    }

    public void SpawnNewAtom()
    {
        Vector3 finalPosition = spawnAnchor.position + spawnOffset;
        GameObject newAtom = Instantiate(atomPrefab, finalPosition, spawnAnchor.rotation);

        XRGrabInteractable grabInteractable = newAtom.GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(OnAtomGrabbed);
    }

    private void OnAtomGrabbed(SelectEnterEventArgs args)
    {
        // Get the interactable that was just grabbed
        XRGrabInteractable grabbedObject = args.interactableObject.transform.GetComponent<XRGrabInteractable>();

        // Clean up the listener so it doesn't trigger again
        grabbedObject.selectEntered.RemoveListener(OnAtomGrabbed);

        // Start the delay timer
        StartCoroutine(DelayedSpawn());
    }

    private IEnumerator DelayedSpawn()
    {
        // Wait for the specified time
        yield return new WaitForSeconds(spawnDelay);

        // Spawn the new atom
        SpawnNewAtom();
    }

    private void OnDrawGizmos()
    {
        if (spawnAnchor != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(spawnAnchor.position + spawnOffset, 0.05f);
        }
    }
}