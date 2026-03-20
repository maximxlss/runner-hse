using SaintsField;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour {
    public string sceneToStart;

    public void StartGame() {
        SceneManager.LoadScene(sceneToStart);
    }

    public void QuitGame() {
        Application.Quit();
    }
}