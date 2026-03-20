using SaintsField;
using TMPro;
using UnityEngine;

public class HighscoreDisplayController : MonoBehaviour {
    [Required]
    public TextMeshProUGUI textMesh;

    private void Start() {
        var highscore = PlayerPrefs.GetFloat("highscore");
        if (highscore == 0) {
            textMesh.text = "";
        }
        else {
            textMesh.text = $"Your best score: {highscore:F1} units";
        }
    }
}