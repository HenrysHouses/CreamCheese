/*
 * Written by:
 * Henrik
*/

using UnityEngine;
using UnityEditor;


#if UNITY_EDITOR
static public class PathCreator
{	
	[MenuItem("GameObject/Level Design/Path", false, 10)]
	static public void CreatePath()
	{
		GameObject gameObject = new GameObject("Path");
		gameObject.hideFlags = HideFlags.NotEditable;
		PathController controller = gameObject.AddComponent<PathController>();
		controller.hideFlags = HideFlags.None;
		for (int i = 0; i < 2; i++)
		{
			GameObject controlPoint = new GameObject();
			controlPoint.transform.SetParent(gameObject.transform);
			controlPoint.name = "p" + i;
			controlPoint.transform.localScale = new Vector3(0,0,3);
			controlPoint.AddComponent<PathExtender>();
			controller.controlPoints.Add(controlPoint.transform);
		}
	}
}
#endif