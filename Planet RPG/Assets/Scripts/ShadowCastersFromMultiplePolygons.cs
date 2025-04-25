using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ShadowCastersFromMultiplePolygons : MonoBehaviour
{
    void Start()
    {
        GenerateShadowCasters();
    }

    void GenerateShadowCasters()
    {
        PolygonCollider2D[] polys = GetComponents<PolygonCollider2D>();
        if (polys.Length == 0) return;

        // Clear previous generated shadows
        foreach (Transform child in transform)
        {
            if (child.name.StartsWith("ShadowCaster_"))
                DestroyImmediate(child.gameObject);
        }

        int index = 0;

        foreach (var poly in polys)
        {
            for (int pathIndex = 0; pathIndex < poly.pathCount; pathIndex++)
            {
                GameObject casterObj = new GameObject("ShadowCaster_" + index++);
                casterObj.transform.parent = transform;
                casterObj.transform.localPosition = Vector3.zero;
                casterObj.transform.localRotation = Quaternion.identity;

                var caster = casterObj.AddComponent<ShadowCaster2D>();
                caster.useRendererSilhouette = false;
                caster.selfShadows = false;
                //caster.mesh.SetVertices(polys.po)
                Mesh mesh = GenerateMeshFromPoints(poly.GetPath(pathIndex));
                //caster.mesh = mesh;
            }
        }
    }

    Mesh GenerateMeshFromPoints(Vector2[] points)
    {
        if (points.Length < 3) return null;

        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        for (int i = 1; i < points.Length - 1; i++)
        {
            vertices.Add(points[0]);
            vertices.Add(points[i]);
            vertices.Add(points[i + 1]);

            int idx = vertices.Count;
            triangles.Add(idx - 3);
            triangles.Add(idx - 2);
            triangles.Add(idx - 1);
        }

        Mesh mesh = new Mesh();
        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles, 0);
        return mesh;
    }
}
