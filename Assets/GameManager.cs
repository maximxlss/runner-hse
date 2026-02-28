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

    public GameObject startText;

    public bool isPlaying;

    public float CurrentSpeed => baseSpeed + boostSpeed;

    [Min(0)]
    public float baseSpeed = 10;

    [Min(0)]
    public float boostSpeed;

    [SerializeReference]
    public AccelerationTactic accelerationTactic;

    public bool restartOnDeath = true;

    public void Start() {
        if (startText && isPlaying) {
            startText.SetActive(false);
        }
        else {
            InputSystem.onAnyButtonPress.CallOnce(StartPlaying);
        }
    }

    public void StartPlaying(InputControl inputControl) {
        if (startText) {
            startText.SetActive(false);
        }

        isPlaying = true;
    }

    private void FixedUpdate() {
        if (!isPlaying) {
            return;
        }

        baseSpeed += accelerationTactic.DeltaSpeed(Time.deltaTime);
    }

    public void OnDeath() {
        isPlaying = false;
        if (restartOnDeath) {
            StartCoroutine(RestartAfterSomeTime());
        }
    }

    private static IEnumerator RestartAfterSomeTime() {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}