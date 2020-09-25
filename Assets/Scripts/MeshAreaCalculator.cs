using System.Collections.Generic;
using TMPro;
using UnityEngine;

[ExecuteAlways]
public class MeshAreaCalculator : MonoBehaviour
{
    [SerializeField] private Mesh MeshToCalculate = null;
    [SerializeField] private TMP_Text AreaDisplayText = null;
    
    private void Update()
    {
        if (MeshToCalculate == null)
            return;
        float area = CalculateArea(MeshToCalculate);

        AreaDisplayText.text = "Area of mesh is: \n" + area + " m²";
    }

    private float CalculateArea(Mesh pMeshToCalculate)
    {
        float totalAreaCalculated = 0.0f;

        int[] triangleIndices = pMeshToCalculate.triangles;
        Vector3[] vertexPositions = pMeshToCalculate.vertices;
        
        for (int i = 0; i < triangleIndices.Length; i+= 3)
        {
            Vector3 vertOne = vertexPositions[triangleIndices[i]];
            Vector3 vertTwo = vertexPositions[triangleIndices[i + 1]];
            Vector3 vertThree = vertexPositions[triangleIndices[i + 2]];

            Vector3 a = vertTwo - vertOne;
            Vector3 b = vertThree - vertOne;

            float triangleArea = (Vector3.Cross(a, b).magnitude) / 2;
            totalAreaCalculated += triangleArea;
        }
        return totalAreaCalculated;
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
        }
    }
}
