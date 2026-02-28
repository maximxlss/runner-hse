using SaintsField;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class TileManager : MonoBehaviour {
    public float xSize = 10;

    public bool offsetToRandomLane;

    public void Start() {
        if (!offsetToRandomLane) {
            return;
        }

        var sideLanes = GameManager.Instance.playerController.sideLanes;
        var laneWidth = GameManager.Instance.playerController.laneWidth;
        var laneOffset = Random.Range(-sideLanes, sideLanes) * laneWidth;
        transform.localPosition += new Vector3(0, 0, laneOffset);
    }

#if UNITY_EDITOR
    public void OnDrawGizmos() {
        Gizmos.color = Color.darkRed;
        Gizmos.DrawLine(transform.position + new Vector3(0, 0, -10),
            transform.position + new Vector3(0, 0, 10));
        Gizmos.color = Color.darkRed;
        Gizmos.DrawLine(transform.position + new Vector3(-xSize, 0, -10),
            transform.position + new Vector3(-xSize, 0, 10));
    }

    public void OnDrawGizmosSelected() {
        Handles.Label(transform.position, "Start of this tile");
        Handles.Label(transform.position + new Vector3(-xSize, 0, 0), "End of this tile (xSize)");
    }
#endif
}