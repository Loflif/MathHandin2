using UnityEditor;
using UnityEngine;

public class TurretPlacement : MonoBehaviour
{
    [SerializeField] private float GunHeight = 1.3f;
    [SerializeField] private float GunSeparation = 0.3f;
    [SerializeField] private float GunLength = 0.8f;
    [SerializeField] private float GunWidth = 20.0f;
    
    private void OnDrawGizmos()
    {
        Vector3 headPosition = transform.position;
        Vector3 lookDirection = transform.forward;
        
        if (Physics.Raycast(headPosition, lookDirection, out RaycastHit hit))
        {
            Vector3 hitPoint = hit.point;

            Handles.color = Color.white;
            Handles.DrawAAPolyLine(headPosition, hitPoint);
            
            Vector3 turretUp = hit.normal;
            Vector3 turretRight = Vector3.Cross(turretUp, lookDirection).normalized;
            Vector3 turretForward = Vector3.Cross(turretRight, turretUp);
            
            DrawTurretBasisVectors(hit.point, turretUp, turretRight, turretForward);
            Matrix4x4 turretTransform = ConstructBoxWireFrameMatrix(hitPoint, turretUp, turretRight, turretForward);
            DrawBoundingBoxWireframe(turretTransform);
            DrawGunBarrels(turretTransform);
        }
        else
        {
            Handles.color = Color.black;
            Handles.DrawAAPolyLine(headPosition, headPosition + lookDirection);
        }


        void DrawTurretBasisVectors(Vector3 pPosition, Vector3 pUp, Vector3 pRight, Vector3 pForward)
        {
            Handles.color = Color.green;
            Handles.DrawAAPolyLine(EditorGUIUtility.whiteTexture, 5, pPosition, pPosition + pUp);

            Handles.color = Color.red;
            Handles.DrawAAPolyLine(EditorGUIUtility.whiteTexture, 5, pPosition, pPosition + pRight);

            Handles.color = Color.cyan;
            Handles.DrawAAPolyLine(EditorGUIUtility.whiteTexture, 5, pPosition, pPosition + pForward);
        }

        Matrix4x4 ConstructBoxWireFrameMatrix(Vector3 pPosition, Vector3 pUp, Vector3 pRight, Vector3 pForward)
        {
            Matrix4x4 result = new Matrix4x4(pRight, pUp, pForward, pPosition);
            return result;
        }

        void DrawBoundingBoxWireframe(Matrix4x4 pTransform)
        {
            Vector3[] vertices =
            {
                // bottom 4 positions:
                new Vector3(1, 0, 1),
                new Vector3(-1, 0, 1),
                new Vector3(-1, 0, -1),
                new Vector3(1, 0, -1),
                // top 4 positions:
                new Vector3(1, 2, 1),
                new Vector3(-1, 2, 1),
                new Vector3(-1, 2, -1),
                new Vector3(1, 2, -1)
            };

            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = pTransform.MultiplyPoint3x4(vertices[i]);
            }

            Handles.color = Color.yellow;
            Vector3[] bottomVerts =
            {
                vertices[0],
                vertices[1],
                vertices[2],
                vertices[3],
                vertices[0]
            };
            Handles.DrawAAPolyLine(EditorGUIUtility.whiteTexture, 10, bottomVerts);
            Vector3[] topVerts =
            {
                vertices[4],
                vertices[5],
                vertices[6],
                vertices[7],
                vertices[4]
            };
            Handles.DrawAAPolyLine(EditorGUIUtility.whiteTexture, 10, topVerts);
            Vector3[] backVerts =
            {
                vertices[0],
                vertices[4],
                vertices[5],
                vertices[1],
                vertices[0]
            };
            Handles.DrawAAPolyLine(EditorGUIUtility.whiteTexture, 10, backVerts);
            Vector3[] frontVerts =
            {
                vertices[2],
                vertices[6],
                vertices[7],
                vertices[3],
                vertices[2]
            };
            Handles.DrawAAPolyLine(EditorGUIUtility.whiteTexture, 10, frontVerts);
        }

        void DrawGunBarrels(Matrix4x4 pTransform)
        {
            Vector3[] leftGunVerts =
            {
                new Vector3(-GunSeparation * 0.5f, GunHeight, -GunLength * 0.5f),
                new Vector3(-GunSeparation * 0.5f, GunHeight, GunLength * 0.5f)
            };
            
            for (int i = 0; i < leftGunVerts.Length; i++)
            {
                leftGunVerts[i] = pTransform.MultiplyPoint3x4(leftGunVerts[i]);
            }
            
            Handles.DrawAAPolyLine(EditorGUIUtility.whiteTexture, GunWidth, leftGunVerts);
            
            Vector3[] rightGunVerts =
            {
                new Vector3(GunSeparation * 0.5f, GunHeight, -GunLength * 0.5f),
                new Vector3(GunSeparation * 0.5f, GunHeight, GunLength * 0.5f)
            };
            
            for (int i = 0; i < rightGunVerts.Length; i++)
            {
                rightGunVerts[i] = pTransform.MultiplyPoint3x4(rightGunVerts[i]);
            }
            
            Handles.DrawAAPolyLine(EditorGUIUtility.whiteTexture, GunWidth, rightGunVerts);
        }
    }
}
