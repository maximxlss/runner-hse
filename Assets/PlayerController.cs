using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    public int health = 3;
    public int maxHealth = 3;

    [Min(0)]
    public int sideLanes = 2;

    public int currentLane;

    public float laneWidth = 1;

    [Range(0, 1)]
    public float switchSpeed = 0.1f;

    public float maxBoost = 25;
    public int shields;

    public void Start() {
        InputSystem.actions["Player/Move"].started += OnMove;
    }

    public void OnDestroy() {
        InputSystem.actions["Player/Move"].started -= OnMove;
    }

    public void OnMove(InputAction.CallbackContext ctx) {
        if (!GameManager.Instance.shouldMove) {
            return;
        }

        var pos = ctx.ReadValue<float>();
        switch (pos) {
            case > 0 when currentLane < sideLanes:
                currentLane++;
                break;
            case < 0 when currentLane > -sideLanes:
                currentLane--;
                break;
        }
    }

    private Vector3 TargetPosition() {
        return new Vector3(0, 0, -laneWidth * currentLane);
    }

    public void Update() {
        if (InputSystem.actions["Player/Sprint"].IsPressed() && GameManager.Instance.boostSpeed < maxBoost) {
            GameManager.Instance.boostSpeed += 20f * Time.deltaTime;
        }
        else if (GameManager.Instance.boostSpeed > 0) {
            GameManager.Instance.boostSpeed -= 30f * Time.deltaTime;
            if (GameManager.Instance.boostSpeed < 0) {
                GameManager.Instance.boostSpeed = 0;
            }
        }
    }

    public void FixedUpdate() {
        var neededDisp = TargetPosition() - transform.localPosition;
        transform.localPosition += neededDisp * switchSpeed;
    }

    public void Check() {
        health = Math.Clamp(health, 0, maxHealth);
        shields = Math.Max(shields, 0);

        if (health > 0) {
            return;
        }

        health = 0;
        GameManager.Instance.OnDeath();
    }
}