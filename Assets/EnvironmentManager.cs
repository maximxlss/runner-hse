using System;
using SaintsField;
using SaintsField.Playa;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class EnvironmentManager : MonoBehaviour {
    public float spawnX = 100;
    public float despawnX = -10;

    public bool StopMoving => !GameManager.Instance.isPlaying;

    public float Distance { get; private set; }

    public void FixedUpdate() {
        if (StopMoving) {
            return;
        }

        var curSpeed = GameManager.Instance.CurrentSpeed;
        var deltaDistance = curSpeed * Time.deltaTime;
        foreach (Transform o in transform) {
            o.localPosition += deltaDistance * new Vector3(-1, 0, 0);
        }

        Distance += deltaDistance;
    }

    public void Update() {
        if (StopMoving) {
            return;
        }

        foreach (Transform o in transform) {
            if (o.localPosition.x < despawnX) {
                Destroy(o.gameObject);
            }
        }
    }

    [Button]
    public GameObject SpawnChild(GameObject prefab, Vector3 offset) {
        var go = Instantiate(prefab, transform);
        go.transform.localPosition = new Vector3(spawnX, 0, 0) + offset;
        go.transform.localRotation = Quaternion.identity;
        return go;
    }

#if UNITY_EDITOR
    public void OnDrawGizmosSelected() {
        Gizmos.color = Color.darkGreen;
        Handles.Label(transform.position + new Vector3(spawnX, 0, 0), "Spawn point");
        Gizmos.DrawLine(transform.position + new Vector3(spawnX, 0, -10),
            transform.position + new Vector3(spawnX, 0, 10));
        Gizmos.color = Color.darkRed;
        Handles.Label(transform.position + new Vector3(despawnX, 0, 0), "Despawn point");
        Gizmos.DrawLine(transform.position + new Vector3(despawnX, 0, -10),
            transform.position + new Vector3(despawnX, 0, 10));
    }
#endif
}