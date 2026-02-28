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

    public void Start() {
        InputSystem.actions["Player/Move"].started += OnMove;
    }

    // TODO: fix this not firing when you start pressing left or right when boosting (so you can't move while boosting)
    public void OnMove(InputAction.CallbackContext ctx) {
        if (!GameManager.Instance.isPlaying) {
            return;
        }

        var pos = ctx.ReadValue<Vector2>();
        switch (pos.x) {
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
        var pos = InputSystem.actions["Player/Move"].ReadValue<Vector2>();
        if (pos.y > 0.5 && GameManager.Instance.boostSpeed < maxBoost) {
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

    public void CheckHealth() {
        if (health >= maxHealth) {
            health = maxHealth;
            return;
        }

        if (health > 0) {
            return;
        }

        health = 0;
        GameManager.Instance.OnDeath();
    }
}