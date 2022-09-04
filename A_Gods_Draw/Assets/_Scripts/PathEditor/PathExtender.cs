/*
 * Written by:
 * Henrik
*/

using UnityEngine;

#if UNITY_EDITOR	

[ExecuteInEditMode]

public class PathExtender : MonoBehaviour
{
	/// <summary>
	/// Callback to draw gizmos only if the object is selected.
	/// </summary>
	void OnDrawGizmosSelected()
	{
		PathController parent = GetComponentInParent<PathController>();
		parent.DrawControlPoints();
	}
}
#endif