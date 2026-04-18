using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MoleculeUIItem : MonoBehaviour
{
    public TextMeshProUGUI nameLabel;
    public Image discoveryIcon; // The image to enable (e.g., a checkmark or icon)
    public string moleculeFormula; // Used to identify this UI item

    public void Setup(string displayName, string formula)
    {
        nameLabel.text = displayName;
        moleculeFormula = formula;
        discoveryIcon.gameObject.SetActive(false); // Hide until discovered
    }

    public void MarkAsDiscovered()
    {
        discoveryIcon.gameObject.SetActive(true);
        // Optional: Change text color or play a small UI animation
        nameLabel.color = Color.green;
    }
}