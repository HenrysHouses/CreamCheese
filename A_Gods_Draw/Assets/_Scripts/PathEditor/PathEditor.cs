/*
 * Written by:
 * Henrik
*/

using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR	

[CustomEditor( typeof( PathExtender ) )]
public class PathEditor : Editor
{
	private PathController newSegment;
	void OnSceneGUI()
	{
		PathExtender t = target as PathExtender;

		Event e = Event.current;
		
		// Get scene view mouse pos and camera
		Camera cam = Camera.current;
		Vector3 pos = Event.current.mousePosition;
		if(cam != null)
		{
			pos.z = -cam.worldToCameraMatrix.MultiplyPoint(t.transform.position).z;
			pos.y = Screen.height - pos.y - 36.0f; // ??? Why that offset?!
			pos = cam.ScreenToWorldPoint (pos);
		}

		// If ctrl + mouse0, create a new curve
		if(e.type == EventType.MouseDown && e.ToString().Contains("Modifiers: Control"))
		{
			createCurve(t.GetComponentInParent<PathController>(), pos);
		}
	}
	
	private void createCurve(PathController target, Vector3 position)
	{
		if(target != null)
		{
			GameObject newPoint = new GameObject();
			newPoint.transform.SetParent(target.gameObject.transform);
			newPoint.transform.position = position;
			int n = target.controlPoints.Count;
			newPoint.name = "p"+n;
			newPoint.AddComponent<PathExtender>();
			target.controlPoints.Add(newPoint.transform);
		}
	}
}
#endif