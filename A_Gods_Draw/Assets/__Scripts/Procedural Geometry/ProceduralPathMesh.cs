/*
 * Written by:
 * Henrik
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq; 


[RequireComponent(typeof(MeshFilter))]
public class ProceduralPathMesh : MonoBehaviour
{
	[SerializeField] Transform PathPoint0, PathPoint1;
	[SerializeField] PathController Path;
	[SerializeField] Mesh2D shape2D;
	[SerializeField] Material Mat;
	public void setShape(Mesh2D shape) => shape2D = shape; 
	public void setMaterial(Material mat) => GetComponent<MeshRenderer>().material = mat; 
	
	[Range(2, 32)]
	[SerializeField] int edgeRingCount = 8;
	
	// [Range(0,1)]
	// [SerializeField] float tTest = 0;
	public Transform startPoint;
	public Transform endPoint;
	
	Vector3 GetPos(int i)
	{
		if(i == 0)
			return startPoint.position;
		if(i == 1)
			return startPoint.TransformPoint(Vector3.forward * startPoint.localScale.z);
		if(i == 2)
			return endPoint.TransformPoint(Vector3.back * endPoint.localScale.z);
		if(i == 3)
			return endPoint.position;
		Debug.LogWarning("GetPos was out of range");
		return default;
	}
	// [SerializeField] Transform[] controlPoints = new Transform[4];
	// Vector3 GetPos(int i ) => controlPoints[i].position;
	
	
	
	Mesh mesh;
	
	/// <summary>
	/// Awake is called when the script instance is being loaded.
	/// </summary>
	void Awake()
	{
		mesh = new Mesh();
		mesh.name = "roadSegment";
		GetComponent<MeshFilter>().sharedMesh = mesh;
		GetComponent<MeshRenderer>().material = Mat;
	}
	
	void Update() => GenerateMesh();
	
	void GenerateMesh()
	{
		mesh.Clear();
		
		// vertices
		float uSpan = shape2D.CalcUspan();
		List<Vector3> verts = new List<Vector3>();
		List<Vector3> normals = new List<Vector3>();
		List<Vector2> uvs = new List<Vector2>();
		for (int ring = 0; ring < edgeRingCount; ring++)
		{
			float t = ring / (edgeRingCount-1f);
			OrientedPoint op = GetBezierOP(t);
			for (int i = 0; i < shape2D.VertexCount; i++)
			{
				verts.Add(op.LocalToWorldPos(shape2D.vertices[i].point));
				normals.Add(op.LocalToWorldVect(shape2D.vertices[i].normal));
				uvs.Add(new Vector2(shape2D.vertices[i].u, t * GetApproxLength() / uSpan));
			}
		}
		
		// triangles
		List<int> triIndices = new List<int>();
		for (int ring = 0; ring < edgeRingCount-1; ring++)
		{
			int rootIndex = ring * shape2D.VertexCount;
			int rootIndexNext = (ring+1) * shape2D.VertexCount;
			
			for (int line = 0; line < shape2D.lineCount; line+=2)
			{
				int lineIndexA = shape2D.lineIndices[line];
				int lineIndexB = shape2D.lineIndices[line+1];
				
				// Current means the current edge, next means the edge to connect the mesh to
				int currentA = rootIndex + lineIndexA;
				int currentB = rootIndex + lineIndexB;
				int nextA = rootIndexNext + lineIndexA;
				int nextB = rootIndexNext + lineIndexB;
				triIndices.Add(currentA);
				triIndices.Add(nextA);
				triIndices.Add(nextB);
				triIndices.Add(currentA);
				triIndices.Add(nextB);
				triIndices.Add(currentB);
			}
		}
		
		
		mesh.SetVertices(verts);
		mesh.SetTriangles(triIndices, 0);
		mesh.SetNormals(normals);
		mesh.SetUVs(0, uvs);

		PathPoint0.position = startPoint.position;
		PathPoint1.position = endPoint.position;
	}

	public PathController getPath()
	{
		Path.recalculatePath();
		return Path;
	}

#if UNITY_EDITOR
	/// <summary>
	/// Callback to draw gizmos that are pickable and always drawn.
	/// </summary>
	void OnDrawGizmos()
	{
		if(gameObject.name.Contains("p") && endPoint == null)
			DestroyImmediate(this);
		if(endPoint == null || startPoint == null)
			return;
		// if(endPoint.childCount == 0 && endPoint.gameObject.GetComponent<CurveExtender>() == null)
		// 	endPoint.gameObject.AddComponent<CurveExtender>();
		// if(startPoint.childCount == 0 && startPoint.gameObject.GetComponent<CurveExtender>() == null)
		// 	startPoint.gameObject.AddComponent<CurveExtender>();
		
		for(int i = 0; i < 4; i++)
		{
			if(i == 0 || i == 3)
				Gizmos.color = Color.cyan;
			if(i == 1 || i == 2)
				Gizmos.color = Color.red;
			
			float handleSize = 1;
			if(Camera.current != null)
				handleSize = Vector3.Distance(Camera.current.gameObject.transform.position, GetPos(i));
			Gizmos.DrawSphere(GetPos(i), 0.02f* handleSize);
		}	
		
		Handles.DrawBezier(
			GetPos(0), 
			GetPos(3), 
			GetPos(1), 
			GetPos(2), Color.white, EditorGUIUtility.whiteTexture, 1f);
			 
		for (int i = 0; i < edgeRingCount+1; i++)
		{
			float t = (1f / edgeRingCount) * i;
			OrientedPoint point = GetBezierOP(t);
			Vector3 up = Vector3.Lerp(startPoint.up, endPoint.up, t).normalized;
			
			// float x = 2f * (point.rot.x*point.rot.y - point.rot.w*point.rot.z);
			// float y = 1f - 2f * (point.rot.x*point.rot.x + point.rot.z*point.rot.z);
			// float z = 2f * (point.rot.y*point.rot.z + point.rot.w*point.rot.x);
			// float x = 2 * (point.rot.x*point.rot.z + point.rot.w*point.rot.y);
			// float y = 2 * (point.rot.y*point.rot.z - point.rot.w*point.rot.x);
			// float z = 1 - 2 * (point.rot.x*point.rot.x + point.rot.y*point.rot.y);
			
			// Vector3 up = new Vector3(x,y,z);
			// Vector3 up = point.tangent.normalized;
			
			
			Handles.DrawLine(point.pos, point.LocalToWorldPos(up));
		}
		// Gizmos.color = Color.green;
		
		// OrientedPoint testPoint = GetBezierOP(tTest);

		// Handles.PositionHandle(testPoint.pos, testPoint.rot);
		
		// void DrawPoint(Vector2 localPos) => Gizmos.DrawSphere(testPoint.LocalToWorldPos(localPos), 0.15f);
		
		// Vector3[] Verts = shape2D.vertices.Select(v => testPoint.LocalToWorldPos(v.point)).ToArray();
		
		// for (int i = 0; i < shape2D.lineIndices.Length; i+=2)
		// {
		// 	Vector3 a = Verts[shape2D.lineIndices[i]];
		// 	Vector3 b = Verts[shape2D.lineIndices[i+1]];
		// 	Gizmos.DrawLine(a,b);
				
		// }
		
		Gizmos.color = Color.white;
	}
#endif	

	OrientedPoint GetBezierOP(float t)
	{
		Vector3 p0 = GetPos(0); 
		Vector3 p1 = GetPos(1); 
		Vector3 p2 = GetPos(2); 
		Vector3 p3 = GetPos(3);
		
		Vector3 a = Vector3.Lerp(p0, p1, t); 
		Vector3 b = Vector3.Lerp(p1, p2, t); 
		Vector3 c = Vector3.Lerp(p2, p3, t);
		
		Vector3 d = Vector3.Lerp(a, b, t); 
		Vector3 e = Vector3.Lerp(b, c, t);
		
		Vector3 pos = Vector3.Lerp(d, e , t);
		Vector3 tangent = (e-d).normalized;
		
		Vector3 up = Vector3.Lerp(startPoint.up, endPoint.up, t).normalized;
		Quaternion rot = Quaternion.LookRotation(tangent, up);
		
		return new OrientedPoint(pos, rot);
	}
	
	float GetApproxLength(int precision = 8)
	{
		Vector3[] points = new Vector3[precision];
		for (int i = 0; i < precision; i++)
		{
			float t = i / (precision-1);
			points[i] = GetBezierOP(t).pos;
		}
		
		float dist = 0;
		for (int i = 0; i < precision-1; i++)
		{
			Vector3 a = points[i];
			Vector3 b = points[i+1];
			dist += Vector3.Distance(a,b);
		}
		return dist;
	}
}
