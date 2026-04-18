using UnityEngine;

[CreateAssetMenu(fileName = "NewAtomData", menuName = "Chemistry/Atom Data")]
public class AtomData : ScriptableObject
{
    public string elementName; // e.g., "Hydrogen" [cite: 12]
    public string symbol;      // e.g., "H" [cite: 16]
    public Color atomColor;    // To differentiate atoms visually
    public float atomicRadius; // Useful for snapping logic
}