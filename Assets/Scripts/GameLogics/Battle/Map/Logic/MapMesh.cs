using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter),typeof(MeshRenderer))]
public class MapMesh : MonoBehaviour
{
    Mesh mapMesh;
    List<Vector3> vertices;
    List<int> triangles;

    MeshCollider meshCollider;
    List<Color> colors;

    private void Awake()
    {
        GetComponent<MeshFilter>().mesh = mapMesh = new Mesh();
        meshCollider = gameObject.AddComponent<MeshCollider>();
        mapMesh.name = "Map Mesh";
        vertices = new List<Vector3>();
        triangles = new List<int>();
        colors = new List<Color>();
    }

    public void Triangulate(MapCell[] cells)
    {
        mapMesh.Clear();
        vertices.Clear();
        triangles.Clear();
        colors.Clear();
        for(int i=0; i < cells.Length; ++i)
        {
            Triangulate(cells[i]);
        }
        mapMesh.vertices = vertices.ToArray();
        mapMesh.triangles = triangles.ToArray();
        mapMesh.colors = colors.ToArray();
        mapMesh.RecalculateNormals();

        meshCollider.sharedMesh = mapMesh;
    }

    void Triangulate(MapCell cell)
    {
        Vector3 cornerPos = cell.coordinates.WorldPosition-new Vector3(MapConstant.sideLength/2,0,MapConstant.sideLength/2);
        AddTriangle(cornerPos+MapConstant.corners[0], cornerPos+MapConstant.corners[1], cornerPos+MapConstant.corners[2]);
        AddTriangleColor(cell.color);
        AddTriangle(cornerPos+MapConstant.corners[2], cornerPos+ MapConstant.corners[3], cornerPos+ MapConstant.corners[0]);
        AddTriangleColor(cell.color);
    }
       

    void AddTriangle(Vector3 v1,Vector3 v2,Vector3 v3)
    {
        int vertexIndex = vertices.Count;
        vertices.Add(v1);
        vertices.Add(v2);
        vertices.Add(v3);
        triangles.Add(vertexIndex);
        triangles.Add(vertexIndex + 1);
        triangles.Add(vertexIndex + 2);
    }

    void AddTriangleColor(Color color)
    {
        colors.Add(color);
        colors.Add(color);
        colors.Add(color);
    }
}
