using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Obstacle : MonoBehaviour {
    public int deltaHealth;
    public int deltaShields;
    public int deltaScore;
    public int shieldsToBypass;

    public bool destroyOnHit;

    public void OnTriggerEnter(Collider other) {
        if (other.gameObject != GameManager.Instance.playerController.gameObject ||
            !GameManager.Instance.shouldMove) {
            return;
        }

        var player = other.gameObject.GetComponent<PlayerController>();

        bool destroySelf;

        if (shieldsToBypass > 0 && GameManager.Instance.playerController.shields >= shieldsToBypass) {
            GameManager.Instance.playerController.shields -= shieldsToBypass;
            destroySelf = true;
        }
        else {
            player.health += deltaHealth;
            player.shields += deltaShields;
            GameManager.Instance.Score += deltaScore;
            destroySelf = destroyOnHit;
        }

        player.Check();

        if (destroySelf) {
            Destroy(gameObject);
        }
    }
}