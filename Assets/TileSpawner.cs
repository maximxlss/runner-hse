using System;
using SaintsField;
using SaintsField.Playa;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Spawns random tiles aligned to their size defined in the <see cref="TileManager"/> component.
/// </summary>
public class TileSpawner : MonoBehaviour {
    [Required]
    public TileManager[] tiles;

    public float prespawnDistance = 150f;

    private Transform _lastSpawned;
    private TileManager _planToSpawn;

    public void Start() {
        var offset = 0f;
        // it's backwards so we keep a flag
        var setLastSpawned = true;
        while (offset > -prespawnDistance) {
            var prefab = tiles[Random.Range(0, tiles.Length)];
            var newTile =
                GameManager.Instance.environmentManager.SpawnChild(prefab.gameObject, new Vector3(offset, 0, 0));
            offset -= prefab.xSize;
            if (setLastSpawned) {
                _lastSpawned = newTile.transform;
                setLastSpawned = false;
            }
        }

        _planToSpawn = tiles[Random.Range(0, tiles.Length)];
    }

    public void Update() {
        float? lastSpawnedTravel = null;
        if (_lastSpawned) {
            lastSpawnedTravel = GameManager.Instance.environmentManager.spawnX - _lastSpawned.localPosition.x;
        }

        if (lastSpawnedTravel is not null && lastSpawnedTravel < _planToSpawn.xSize) {
            return;
        }

        var offset = _planToSpawn.xSize - lastSpawnedTravel ?? 0;
        var newTile =
            GameManager.Instance.environmentManager.SpawnChild(_planToSpawn.gameObject, new Vector3(offset, 0, 0));
        _lastSpawned = newTile.transform;
        _planToSpawn = tiles[Random.Range(0, tiles.Length)];
    }
}