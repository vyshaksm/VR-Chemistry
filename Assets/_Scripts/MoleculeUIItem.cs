using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MoleculeUIItem : MonoBehaviour
{
    public TextMeshProUGUI nameLabel;
    public Image discoveryIcon;
    public string moleculeFormula;

    // Initializes the UI item with its label and hidden discovery icon.
    public void Setup(string displayName, string formula)
    {
        nameLabel.text = displayName;
        moleculeFormula = formula;
        discoveryIcon.gameObject.SetActive(false);
    }

    // Reveals the discovery state for this molecule in the UI.
    public void MarkAsDiscovered()
    {
        discoveryIcon.gameObject.SetActive(true);
        nameLabel.color = Color.green;
    }
}
