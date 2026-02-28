using System.Text;
using SaintsField;
using TMPro;
using UnityEngine;

public class HeathHudController : MonoBehaviour {
    [Required]
    public TextMeshProUGUI textMesh;

    private void Update() {
        var player = GameManager.Instance.playerController;
        var text = new StringBuilder(player.maxHealth * 2);
        text.Insert(0, " <3", player.health);
        textMesh.text = text.ToString();
    }
}