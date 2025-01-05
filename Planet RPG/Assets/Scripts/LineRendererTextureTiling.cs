using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendererTextureTiling : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public float textureTilingFactor = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        if (lineRenderer == null)
        {
            lineRenderer = GetComponent<LineRenderer>();
        }

        // Set the texture mode to Tile
        lineRenderer.textureMode = LineTextureMode.Tile;

        // Set the material's texture scale
        UpdateTextureTiling();
    }

    void UpdateTextureTiling()
    {
        if (lineRenderer.material != null)
        {
            float length = Vector3.Distance(lineRenderer.GetPosition(0), lineRenderer.GetPosition(1));
            lineRenderer.material.mainTextureScale = new Vector2(length * textureTilingFactor, 1);
        }
    }

    void Update()
    {
        // Call this if the length of the line changes
        UpdateTextureTiling();
    }
}
