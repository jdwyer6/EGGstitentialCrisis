using UnityEngine;

public class SculptingTool : MonoBehaviour
{
    public Camera mainCamera;
    public float sculptStrength = 1.0f;
    public float sculptRadius = 1.0f;

    void Update()
    {
        if (Input.GetMouseButton(0)) // Check if the left mouse button is pressed
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                SculptMesh(hit.point, hit.normal, hit);
            }
        }
    }

    void SculptMesh(Vector3 hitPoint, Vector3 hitNormal, RaycastHit hit)
    {
        MeshFilter meshFilter = hit.collider.GetComponent<MeshFilter>();
        if (meshFilter)
        {
            Mesh mesh = meshFilter.mesh;
            Vector3[] vertices = mesh.vertices;
            for (int i = 0; i < vertices.Length; i++)
            {
                Vector3 vert = meshFilter.transform.TransformPoint(vertices[i]);
                float dist = Vector3.Distance(hitPoint, vert);
                if (dist < sculptRadius)
                {
                    Vector3 direction = hitNormal; // Direction to move the vertex
                    vertices[i] += direction * sculptStrength * (1 - (dist / sculptRadius));
                }
            }
            mesh.vertices = vertices;
            mesh.RecalculateNormals(); // Recalculate normals to update lighting
        }
    }
}
