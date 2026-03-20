using System;
using System.Collections;
using SaintsField;
using SaintsField.Playa;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager Instance { get; private set; }

    public void Awake() {
        if (Instance != null && Instance != this) {
            Debug.LogWarning("More than one GameManager found!");
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    [Required]
    public PlayerController playerController;

    [Required]
    public EnvironmentManager environmentManager;

    [Required]
    public GameObject menu;

    public GameObject startText;

    public bool isPlaying;
    public bool isDead;
    public bool shouldMove => isPlaying && !isDead;

    public float CurrentSpeed => baseSpeed + boostSpeed;

    [Min(0)]
    public float baseSpeed = 10;

    [Min(0)]
    public float boostSpeed;

    [SerializeReference]
    public AccelerationTactic accelerationTactic;

    public bool restartOnDeath = true;

    public float Score;

    public void Start() {
        if (startText && isPlaying) {
            startText.SetActive(false);
        }
        else {
            InputSystem.onAnyButtonPress.CallOnce(StartPlaying);
        }

        InputSystem.actions["Player/Pause"].started += TogglePauseCallback;
    }

    public void OnDestroy() {
        InputSystem.actions["Player/Pause"].started -= TogglePauseCallback;
    }

    public void StartPlaying(InputControl inputControl) {
        if (startText) {
            startText.SetActive(false);
        }

        isPlaying = true;
    }

    public void TogglePauseCallback(InputAction.CallbackContext ctx) {
        TogglePause();
    }

    public void TogglePause() {
        if (isPlaying) {
            Pause();
        }
        else {
            Continue();
        }
    }

    public void Pause() {
        isPlaying = false;
        menu.SetActive(true);
    }

    public void Continue() {
        isPlaying = true;
        menu.SetActive(false);
    }

    public void ReturnToMenu() {
        SceneManager.LoadScene("Scenes/MainMenu");
    }

    private void FixedUpdate() {
        if (!shouldMove) {
            return;
        }

        baseSpeed += accelerationTactic.DeltaSpeed(Time.deltaTime);
    }

    public void OnDeath() {
        isDead = true;
        PlayerPrefs.SetFloat("highscore", Mathf.Max(PlayerPrefs.GetFloat("highscore"), Score));
        PlayerPrefs.Save();
        if (restartOnDeath) {
            StartCoroutine(RestartAfterSomeTime());
        }
    }

    private static IEnumerator RestartAfterSomeTime() {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("Scenes/MainMenu");
    }
}