using System;
using UnityEngine;
using System.Collections;
using SaintsField;
using SaintsField.Playa;
using Unity.VisualScripting;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

/// <summary>
/// Spawns random objects from a list, positioning them away from the center, with given density.
/// </summary>
public class BackgroundSpawner : MonoBehaviour {
    [Required]
    public GameObject[] prefabs;

    [Min(0)]
    public float distanceFromCenter = 3;

    [Min(0)]
    public float width = 10;

    [Min(0)]
    public float density = 1;

    [ShowInInspector]
    public float ApproximateNumberOfObjects =>
        (GameManager.Instance.environmentManager.spawnX - GameManager.Instance.environmentManager.despawnX) * width *
        2 * density;

    public float SpawnFrequency => GameManager.Instance.CurrentSpeed * width * 2 * density;
    private float _lastSpawnTime;

    public void Start() {
        var environmentManager = GameManager.Instance.environmentManager;
        Assert.IsTrue(environmentManager.spawnX > environmentManager.despawnX);
        var length = environmentManager.spawnX - environmentManager.despawnX;
        var area = width * length * 2;
        var count = Math.Round(area * density);
        for (var i = 0; i < count; i++) {
            var side = Random.value > 0.5f ? -1 : 1;
            var offset = new Vector3(-Random.Range(0, length),
                0, (distanceFromCenter + Random.Range(0, width)) * side);
            SpawnOne(offset);
        }

        _lastSpawnTime = Time.time;
    }

    public void Update() {
        if (!GameManager.Instance.isPlaying) {
            return;
        }

        var spawnDelta = Time.time - _lastSpawnTime;
        if (spawnDelta < 1.0 / SpawnFrequency) {
            return;
        }

        var side = Random.value > 0.5f ? -1 : 1;
        var windowLength = spawnDelta * GameManager.Instance.CurrentSpeed;
        var offset = new Vector3(Random.Range(-windowLength / 2, windowLength / 2), 0,
            (distanceFromCenter + Random.Range(0, width)) * side);
        SpawnOne(offset);
        _lastSpawnTime = Time.time;
    }

    private void SpawnOne(Vector3 offset) {
        GameManager.Instance.environmentManager.SpawnChild(prefabs[Random.Range(0, prefabs.Length)], offset);
    }
}