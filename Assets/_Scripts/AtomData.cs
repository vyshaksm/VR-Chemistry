using UnityEngine;

[CreateAssetMenu(fileName = "NewAtomData", menuName = "Chemistry/Atom Data")]
public class AtomData : ScriptableObject
{
    public string elementName;
    public string symbol;
    public Color atomColor;
    public float atomicRadius;
}
