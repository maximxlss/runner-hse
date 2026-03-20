using System;
using System.Linq;
using SaintsField;
using SaintsField.Playa;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Spawns random tiles aligned to their size defined in the <see cref="TileManager"/> component.
/// </summary>
public class TileSpawner : MonoBehaviour {
    [Serializable]
    public struct TileOption {
        [Required]
        public TileManager TileManager;

        [Min(0)]
        public int Weight;
    }

    public TileOption[] tiles;

    [ShowInInspector]
    public int TotalWeight => tiles.Sum(t => t.Weight);

    public float prespawnDistance = 150f;

    private Transform _lastSpawned;
    private TileManager _planToSpawn;

    // ReSharper disable Unity.PerformanceAnalysis
    // this is not actually frequently called and the "costly" debug should be unreachable.
    private void PickNextToSpawn() {
        var target = Random.Range(0, TotalWeight);
        foreach (var tile in tiles) {
            target -= tile.Weight;
            if (target < 0) {
                _planToSpawn = tile.TileManager;
                return;
            }
        }

        Debug.LogError("pickNextToSpawn didn't finish (invalid tiles or bug)");
    }

    public void Start() {
        var offset = 0f;
        // it's backwards so we keep a flag
        var setLastSpawned = true;
        while (offset > -prespawnDistance) {
            PickNextToSpawn();
            var newTile =
                GameManager.Instance.environmentManager.SpawnChild(_planToSpawn.gameObject, new Vector3(offset, 0, 0));
            offset -= _planToSpawn.xSize;
            if (setLastSpawned) {
                _lastSpawned = newTile.transform;
                setLastSpawned = false;
            }
        }

        PickNextToSpawn();
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
        PickNextToSpawn();
    }
}