using SaintsField;
using TMPro;
using UnityEngine;

public class ScoreHudController : MonoBehaviour {
    [Required]
    public TextMeshProUGUI textMesh;

    private void Update() {
        var dist = GameManager.Instance.Score;
        var shields = GameManager.Instance.playerController.shields;
        textMesh.text = $"Score: {dist:F1} units\nShields: {shields}";
    }
}