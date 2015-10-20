using UnityEditor;
using UnityEngine;
using System.Collections;

public class ShowColliders {

    private static bool shouldShow;

    [MenuItem("ShowColliders/Toggle Colliders")]
    static void ToggleColliders() {
        shouldShow = !shouldShow;
    }


    [DrawGizmo(GizmoType.NotInSelectionHierarchy)]
    static void RenderCustomGizmo(GameObject gameObject, GizmoType gizmoType) {
        if (!shouldShow) {
            return;
        }

        Collider collider;
        if ((collider = gameObject.GetComponent<Collider>()) == null) {
            return;
        }

        Handles.color = new Color(128 / 255f, 198 / 255f, 112 / 255f, 0.75f);

        if (collider is BoxCollider) {
            Handles.matrix = gameObject.transform.localToWorldMatrix;
            BoxCollider boxCollider = (BoxCollider)collider;

            Vector3 center = boxCollider.center;
            Vector3 halfSize = boxCollider.size / 2;

            Vector3 v0 = center + halfSize;
            Vector3 v1 = center + new Vector3(-halfSize.x, halfSize.y, halfSize.z);
            Vector3 v2 = center + new Vector3(-halfSize.x, -halfSize.y, halfSize.z);
            Vector3 v3 = center + new Vector3(halfSize.x, -halfSize.y, halfSize.z);
            Vector3 v4 = center + new Vector3(halfSize.x, halfSize.y, -halfSize.z);
            Vector3 v5 = center + new Vector3(-halfSize.x, halfSize.y, -halfSize.z);
            Vector3 v6 = center - halfSize;
            Vector3 v7 = center + new Vector3(halfSize.x, -halfSize.y, -halfSize.z);

            Handles.DrawPolyLine(new Vector3[]{
                v0, v1, v2, v3, v7, v6, v5, v4, v0
            });
            
            Handles.DrawLine(v1, v5);
            Handles.DrawLine(v2, v6);
            Handles.DrawLine(v0, v3);
            Handles.DrawLine(v4, v7);
        }
        else if (collider is CapsuleCollider) {
            Handles.matrix = Matrix4x4.TRS(gameObject.transform.position, gameObject.transform.rotation, new Vector3(1, 1, 1));
            Vector3 scale = gameObject.transform.lossyScale;

            CapsuleCollider capsuleCollider = (CapsuleCollider)collider;

            Vector3 center = Vector3.Scale(capsuleCollider.center, scale);
            float radius = capsuleCollider.radius;

            Vector3 d0 = Vector3.right;
            Vector3 d1 = Vector3.up;
            Vector3 d2 = Vector3.forward;
            float scaledRadius = radius * Mathf.Max(Mathf.Abs(scale.y), Mathf.Abs(scale.z));
            float scaledHeight = scale.x;

            if (capsuleCollider.direction == 1) {
                d0 = Vector3.up;
                d1 = Vector3.right;
                d2 = Vector3.back;
                scaledRadius = radius * Mathf.Max(Mathf.Abs(scale.x), Mathf.Abs(scale.z));
                scaledHeight = scale.y;
            }
            else if (capsuleCollider.direction == 2) {
                d0 = Vector3.back;
                d1 = Vector3.up;
                d2 = Vector3.right;
                scaledRadius = radius * Mathf.Max(Mathf.Abs(scale.x), Mathf.Abs(scale.y));
                scaledHeight = scale.z;
            }
            
            float halfCenterHeight = Mathf.Max(0, capsuleCollider.height * scaledHeight - 2 * scaledRadius) / 2;

            if (halfCenterHeight > 0) {
                Handles.DrawWireDisc(center + d0 * halfCenterHeight, d0, scaledRadius);
                Handles.DrawWireDisc(center - d0 * halfCenterHeight, d0, scaledRadius);
                Handles.DrawWireArc(center + d0 * halfCenterHeight, d1, d2, 180, scaledRadius);
                Handles.DrawWireArc(center + d0 * halfCenterHeight, d2, d1, -180, scaledRadius);
                Handles.DrawWireArc(center - d0 * halfCenterHeight, d1, d2, -180, scaledRadius);
                Handles.DrawWireArc(center - d0 * halfCenterHeight, d2, d1, 180, scaledRadius);
                Handles.DrawLine(center - d0 * halfCenterHeight + d1 * scaledRadius, center + d0 * halfCenterHeight + d1 * scaledRadius);
                Handles.DrawLine(center - d0 * halfCenterHeight - d1 * scaledRadius, center + d0 * halfCenterHeight - d1 * scaledRadius);
                Handles.DrawLine(center - d0 * halfCenterHeight + d2 * scaledRadius, center + d0 * halfCenterHeight + d2 * scaledRadius);
                Handles.DrawLine(center - d0 * halfCenterHeight - d2 * scaledRadius, center + d0 * halfCenterHeight - d2 * scaledRadius);
            }
            else {
                Handles.DrawWireDisc(center, Vector3.right, scaledRadius);
                Handles.DrawWireDisc(center, Vector3.up, scaledRadius);
                Handles.DrawWireDisc(center, Vector3.forward, scaledRadius);
            }
        }
        else if (collider is SphereCollider) {
            Handles.matrix = Matrix4x4.TRS(gameObject.transform.position, gameObject.transform.rotation, new Vector3(1, 1, 1));
            Vector3 scale = gameObject.transform.lossyScale;

            SphereCollider sphereCollider = (SphereCollider)collider;
            
            Vector3 center = Vector3.Scale(sphereCollider.center, scale);
            float radius = sphereCollider.radius;

            float maxScale = Mathf.Max(new float[]{Mathf.Abs(scale.x), Mathf.Abs(scale.y), Mathf.Abs(scale.z)});
            float scaledRadius = radius * maxScale;

            Handles.DrawWireDisc(center, Vector3.right, scaledRadius);
            Handles.DrawWireDisc(center, Vector3.up, scaledRadius);
            Handles.DrawWireDisc(center, Vector3.forward, scaledRadius);
        }
        
        Handles.matrix = Matrix4x4.identity;
    }

}
