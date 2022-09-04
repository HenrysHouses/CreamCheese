/*
 * Written by:
 * Henrik
*/

using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class PathController : MonoBehaviour
{
	public bool _DrawBezierCurve = true;
	public bool controlPointsEnabled = true;
	[Range(1, 50)]
	[SerializeField] int VertexPathAccuracy = 8;
	[SerializeField] bool DrawEvenPoints = true;
	[SerializeField] bool DrawUpVector;
	private int LOD;
	[SerializeField] bool DrawtTest;
	[SerializeField, Range(0,1)] float tTest = 0;
	[HideInInspector] public Transform startPoint;
	[HideInInspector] public Transform endPoint;
	public List<Transform> controlPoints = new List<Transform>();
	
	[SerializeField] private OrientedPoint[] evenlySpacedPoints;
	private float length;
	[SerializeField] bool Recalculate;
	
	/// <summary>Get position of control points</summary>
	/// <param name="pair">curve point</param>
	/// <param name="i">sub controlpoint for the curve</param>
	/// <returns>position of control point</returns>
	Vector3 GetPos(int pair, int i)
	{
		GetStartEndPoints(pair, ref startPoint, ref endPoint);
		
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
	// Vector3 GetPos(int i ) => controlPoints[i].position;

	void Start()
	{
		LOD = VertexPathAccuracy*3;
		length = GetApproxLength();
		// Debug.Log(length);

	#if UNITY_EDITOR	
		evenlySpacedPoints = calculateEvenlySpacedPoints(length/LOD);
	#endif
	}

#if UNITY_EDITOR
	
	/// <summary>
	/// Callback to draw gizmos only if the object is selected.
	/// </summary>
	void OnDrawGizmosSelected()
	{
		if(controlPointsEnabled)
			DrawControlPoints();
	}


	/// <summary>
	/// Callback to draw gizmos that are pickable and always drawn.
	/// </summary>
	void OnDrawGizmos()
	{
		if(Recalculate)
			OnValidate();
		
		refreshControlPoints();
		if(_DrawBezierCurve)
			DrawBezierCurve();
		if(DrawUpVector)
			DrawUpVectorGizmo();
	}
#endif
	private bool nullCheckControlPoints()
	{
		// stop if no points are found or if any points == null
		if(controlPoints.Count == 0)
			return true;
		foreach(Transform found in controlPoints)
		{
			if(found == null)
				return true;
		}	
		return false;
	}
	
#if UNITY_EDITOR
	
	private void DrawBezierCurve()
	{
		if(nullCheckControlPoints())
			return;
		// Draw all Path Segments
		for (int j = 0; j < controlPoints.Count-1; j++)
		{
			// Draw Bezier curve
			Handles.DrawBezier(
				GetPos(j, 0), 
				GetPos(j, 3), 
				GetPos(j, 1), 
				GetPos(j, 2), Color.white, EditorGUIUtility.whiteTexture, 1f);
			
			if(DrawEvenPoints && evenlySpacedPoints != null)
			{
				Gizmos.color = Color.magenta;
				for (int i = 0; i < evenlySpacedPoints.Length; i++)
				{
					Gizmos.DrawCube(evenlySpacedPoints[i].pos, new Vector3(0.02f,0.02f,0.02f));
				}
			}
		}
	}
	
	private void DrawUpVectorGizmo()
	{
		for (int j = 0; j < controlPoints.Count-1; j++)
		{
			// Draw Up Vector Gizmos;
			for (int i = 0; i < LOD+1; i++)
			{
				float t = (1f / LOD) * i;
				OrientedPoint point = GetBezierOP(j, t);
				
				GetStartEndPoints(j, ref startPoint, ref endPoint);
				Vector3 up = Vector3.Lerp(startPoint.up, endPoint.up, t).normalized;
				float GizmoSize = 1;
				if(Camera.current != null)
					GizmoSize = Vector3.Distance(Camera.current.gameObject.transform.position, gameObject.transform.position);
				GizmoSize = GizmoSize * 0.1f;
				up = point.LocalToWorldPos(up);
				up = new Vector3(up.x, up.y * GizmoSize, up.z);
				Handles.DrawLine(point.pos, up);
			}
		}
	}
	
	public void DrawControlPoints()
	{
		if(nullCheckControlPoints())
			return;
		for (int j = 0; j < controlPoints.Count-1; j++)
		{
			// Draw Control Points
			for(int i = 0; i < 4; i++)
			{
				if(i == 0 || i == 3)
					Gizmos.color = Color.cyan;
				if(i == 1 || i == 2)
					Gizmos.color = Color.red;
				
				float handleSize = 1;
				if(Camera.current != null)
					handleSize = Vector3.Distance(Camera.current.gameObject.transform.position, GetPos(j, i));
				Gizmos.DrawSphere(GetPos(j, i), 0.02f* handleSize);
				Gizmos.DrawLine(GetPos(j, 1), GetPos(j, 0));
				Gizmos.DrawLine(GetPos(j, 2), GetPos(j, 3));
			}	
		}
		// Draw GetPathOP tTest
		if(DrawtTest)
		{
			OrientedPoint testPoint = GetPathOP(tTest);
			Handles.PositionHandle(testPoint.pos, testPoint.rot);
		}
	}
#endif
	
	/// <summary>Get a OrientedPoint of a specific bezier curve within the path</summary>
	/// <param name="pair">One of the control points for the curve wanted</param>
	/// <param name="t">path position range(0,1)</param>
	/// <returns>OrientedPoint, contains transform data</returns>
	OrientedPoint GetBezierOP(int pair, float t)
	{
		Vector3 p0 = GetPos(pair, 0); 
		Vector3 p1 = GetPos(pair, 1); 
		Vector3 p2 = GetPos(pair, 2); 
		Vector3 p3 = GetPos(pair, 3);
		
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
	
	/// <summary>Get a OrientedPoint from the path. !! This method is not speed accurate !!</summary>
	/// <param name="t">path position, range(0,1)</param>
	/// <returns>OrientedPoint, contains path transform data</returns>
	public OrientedPoint GetPathOP(float t)
	{
		// Remaps path t each curve t
		int selectedSegment = 0;
		float[] segments = new float[controlPoints.Count];
		for (int i = 0; i <= segments.Length-1; i++)
		{
			segments[i] = 1 / ((float)segments.Length-1) * i;
		}
		for (int i = 1; i < segments.Length; i++)
		{
			if(t <= segments[i])
			{
				selectedSegment = i;
				break;
			}
		}
		float tPos;
		if(selectedSegment == segments.Length-1)
			tPos = ExtensionMethods.RemapT(t, segments[selectedSegment], segments[selectedSegment-1], 0, 1);
		else
			tPos = ExtensionMethods.RemapT(t, segments[selectedSegment], segments[selectedSegment+1], 0, 1);
		tPos = 1-tPos;
		selectedSegment = Mathf.Clamp((selectedSegment-1), 0, segments.Length-1);
		// Returns the point
		OrientedPoint point = GetBezierOP(selectedSegment, tPos);
		return point;
	}
	
		
	public float GetEvenPathTOffset(int pointOffset)
	{
		float offset = 1/((float)evenlySpacedPoints.Length-1) * pointOffset;
		return offset;
	}
		
	/// <summary>Get a OrientedPoint from the path calculated from even points across the path</summary>
	/// <param name="t">path position, range(0,1)</param>
	/// <returns>OrientedPoint, contains path transform data</returns>
	public OrientedPoint GetEvenPathOP(float t)
	{
		// Remaps path t each point lerp t
		int selectedSegment = 0;
		float[] segments = new float[evenlySpacedPoints.Length];
		for (int i = 0; i <= segments.Length-1; i++)
		{
			segments[i] = 1 / ((float)segments.Length-1) * i;
		}
		
		for (int i = 1; i < segments.Length; i++)
		{
			if(t <= segments[i])
			{
				selectedSegment = i;
				break;
			}
		}
		float tPos;
		if(selectedSegment == segments.Length-1)
			tPos = ExtensionMethods.RemapT(t, segments[selectedSegment], segments[selectedSegment-1], 0, 1);
		else
			tPos = ExtensionMethods.RemapT(t, segments[selectedSegment], segments[selectedSegment+1], 0, 1);
		tPos = 1-tPos;
		selectedSegment = Mathf.Clamp((selectedSegment-1), 0, segments.Length-1);
	
		OrientedPoint point = new OrientedPoint();
		point.pos = Vector3.Lerp(evenlySpacedPoints[selectedSegment].pos, evenlySpacedPoints[selectedSegment+1].pos, tPos);
		point.rot = GetPathOP(t).rot;
		
		return point;
	}
	
	public OrientedPoint getClosestOP(Transform Object, ref int pointIndex)
	{
		float closestPoint = Vector3.Distance(evenlySpacedPoints[0].pos, Object.position);
		int index = 0;
		for (int i = 0; i < evenlySpacedPoints.Length; i++)
		{
			if(Vector3.Distance(evenlySpacedPoints[i].pos, Object.position) < closestPoint)
			{
				closestPoint = Vector3.Distance(evenlySpacedPoints[i].pos, Object.position);
				index = i;
			}
		}
		pointIndex = index;
		return evenlySpacedPoints[index];
	}
	
	public float GetApproxLength(int precision = 8)
	{
		float dist = 0;
		for (int j = 0; j < controlPoints.Count; j++)
		{
			Vector3[] points = new Vector3[precision];
			for (int i = 0; i < precision; i++)
			{
				float t = i / (precision-1);
				points[i] = GetBezierOP(j, t).pos;
			}
			
			for (int i = 0; i < precision-1; i++)
			{
				Vector3 a = points[i];
				Vector3 b = points[i+1];
				dist += Vector3.Distance(a,b);
			}
		}
		return dist;
	}
	
	/// <summary>Refresh start and end references for a specific curve</summary>
	/// <param name="i">curve index</param>
	/// <returns>re-initialize start and end point references</returns>
	void GetStartEndPoints(int i, ref Transform start, ref Transform end)
	{
		if(i == 0) // first segment
		{
			start = controlPoints[0];
			end = controlPoints[1];
			return;
		}
		if(i == controlPoints.Count-1) //! bug fix, not sure what the bug is but it seems to work by doing this
			return;
		start = controlPoints[i];
		end = controlPoints[i+1];
	}
	
	/// <summary>Update the control point count and references</summary>
	void refreshControlPoints()
	{
		List<Transform> remove = new List<Transform>();
		for (int i = 0; i < controlPoints.Count; i++)
		{
			if(controlPoints[i] == null)
			{
				remove.Add(controlPoints[i]);
				
				controlPoints.Remove(controlPoints[i]);
				return;
			}
		}
		if(remove.Count != 0)
		{
			foreach(Transform found in remove)
			{
				controlPoints.Remove(found);
			}
		}
		for (int i = 0; i < transform.childCount; i++)
		{
			if(!controlPoints.Contains(transform.GetChild(i)))
			{
				controlPoints.Add(transform.GetChild(i));
			}
		}
		
		if(transform.childCount == 0)
			DestroyImmediate(gameObject);
	}
	
	// ? BUILD DEBUGGING METHOD
	public void SpawnCubeMeshesAtEvenPoints()
	{
		evenlySpacedPoints = calculateEvenlySpacedPoints(length/LOD);
		foreach (var point in evenlySpacedPoints)
		{
			GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
			cube.transform.localScale = new Vector3(0.03f, 0.03f, 0.03f);
			cube.transform.position = point.pos;
			Destroy(cube.GetComponent<Collider>());
		}
	}


	OrientedPoint[] calculateEvenlySpacedPoints(float spacing)
	{
		List<OrientedPoint> points = new List<OrientedPoint>();
		Vector3 previousPoint = controlPoints[0].position;
		points.Add(new OrientedPoint(previousPoint, Quaternion.identity));
		float dstSinceLastEvenPoint = 0;
		
		for (int j = 0; j < controlPoints.Count-1; j++)
		{
			// Vector3[] p = 
			float t = 0;
			while(t <= 1)
			{
				t += 0.1f;
				Vector3 pointOnCurve = GetBezierOP(j, t).pos;
				dstSinceLastEvenPoint += Vector3.Distance(previousPoint, pointOnCurve);
				
				while(dstSinceLastEvenPoint >= spacing)
				{
					float overshootDst = dstSinceLastEvenPoint - spacing;
					Vector3 newEvenlySpacedPoint = pointOnCurve + (previousPoint - pointOnCurve).normalized * overshootDst;
					OrientedPoint point = GetPathOP(t);
					points.Add(new OrientedPoint(newEvenlySpacedPoint, point.rot));
					dstSinceLastEvenPoint = overshootDst;
					previousPoint = newEvenlySpacedPoint;
				}
				previousPoint = pointOnCurve;
			}	
		}
		return points.ToArray();
	}
	
#if UNITY_EDITOR	

	/// <summary>
	/// Called when the script is loaded or a value is changed in the
	/// inspector (Called in the editor only).
	/// </summary>
	void OnValidate()
	{
		if(Recalculate)
		{
			LOD = VertexPathAccuracy*3;
			length = GetApproxLength();
			
			evenlySpacedPoints = calculateEvenlySpacedPoints(length/LOD);
		}
		
		if(controlPoints.Count != transform.childCount)
		{
			for (int i = 0; i < transform.childCount; i++)
			{
				if(!controlPoints.Contains(transform.GetChild(i)))
					controlPoints.Add(transform.GetChild(i));
			}
		}
		Recalculate = false;
		
		if(!controlPointsEnabled)
		{
			foreach (var point in controlPoints)
			{
				point.gameObject.hideFlags = HideFlags.HideInHierarchy | HideFlags.NotEditable;
			}
		}
		else
		{
			foreach (var point in controlPoints)
			{
				point.gameObject.hideFlags = HideFlags.None;
			}			
		}
	}
	
#endif
}
