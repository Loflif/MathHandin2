using TMPro;
using UnityEngine;

[ExecuteAlways]
public class MeshVolumeCalculator : MonoBehaviour
{
    [SerializeField] private Mesh MeshToCalculate = null;
    [SerializeField] private TMP_Text VolumeDisplayText = null;

    private void Update()
    {
        if (MeshToCalculate == null)
            return;

        float volume = CalculateVolume(MeshToCalculate);
        VolumeDisplayText.text = "Volume of mesh is: \n" + volume + " m³";
    }

    private float CalculateVolume(Mesh pMeshToCalculate)
    {
        double totalVolumeCalculated = 0.0f;

        int[] triangleIndices = pMeshToCalculate.triangles;
        Vector3[] vertexPositions = pMeshToCalculate.vertices;
        
        for (int i = 0; i < triangleIndices.Length; i+= 3)
        {
            Vector3 vertOne = vertexPositions[triangleIndices[i]];
            Vector3 vertTwo = vertexPositions[triangleIndices[i + 1]];
            Vector3 vertThree = vertexPositions[triangleIndices[i + 2]];

            float signedTetrahedronVolume = (Vector3.Dot(Vector3.Cross(vertOne, vertTwo), vertThree)/6);
            totalVolumeCalculated += signedTetrahedronVolume;
        }
        return (float)totalVolumeCalculated;
    }

    private void OnDrawGizmos()
    {
        int[] triangleIndices = MeshToCalculate.triangles;
        Vector3[] vertexPositions = MeshToCalculate.vertices;

        Vector3 objectPosition = transform.position;
        for (int i = 0; i < triangleIndices.Length; i += 3)
        {
            Vector3 vertOne = vertexPositions[triangleIndices[i]] + objectPosition;
            Vector3 vertTwo = vertexPositions[triangleIndices[i+1]] + objectPosition;
            Vector3 vertThree = vertexPositions[triangleIndices[i+2]] + objectPosition;

            Gizmos.color = Color.white;
            Gizmos.DrawLine(vertOne, vertTwo);
            Gizmos.DrawLine(vertTwo, vertThree);
            Gizmos.DrawLine(vertThree, vertOne);

            Gizmos.DrawLine(Vector3.zero, vertOne);
            Gizmos.DrawLine(Vector3.zero, vertTwo);
            Gizmos.DrawLine(Vector3.zero, vertThree);
        }
    }
}
