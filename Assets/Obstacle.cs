using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Obstacle : MonoBehaviour {
    public int deltaHealth;

    public void OnTriggerEnter(Collider other) {
        if (other.gameObject != GameManager.Instance.playerController.gameObject ||
            !GameManager.Instance.isPlaying) {
            return;
        }

        var player = other.gameObject.GetComponent<PlayerController>();
        player.health += deltaHealth;
        player.CheckHealth();
    }
}