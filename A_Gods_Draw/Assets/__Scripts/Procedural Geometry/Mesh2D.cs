/*
 * Written by:
 * Henrik
 * 
 */

using UnityEngine;

[CreateAssetMenu(menuName = "Mesh2D")]
public class Mesh2D : ScriptableObject
{
	[System.Serializable]
	public class Vertex
	{
		public Vector2 point;
		public Vector2 normal;
		public float u; // UVs, but like, not V :thinking_face:
	}
	
	public Vertex[] vertices;
	public int[] lineIndices;

	public int VertexCount => vertices.Length;
	public int lineCount => lineIndices.Length;
	
	public float CalcUspan()
	{
		float dist = 0;
		for (int i = 0; i < lineCount; i+=2)
		{
			Vector2 A = vertices[lineIndices[i]].point;
			Vector2 B = vertices[lineIndices[i+1]].point;
			dist += (A - B).magnitude;
		}
		return dist;
	}
}
