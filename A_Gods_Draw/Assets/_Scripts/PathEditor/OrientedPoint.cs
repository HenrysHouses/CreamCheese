/*
 * Written by:
 * Henrik
*/

using UnityEngine;

[System.Serializable]
public class OrientedPoint
{
	public Vector3 pos;
	public Quaternion rot;
	
	public OrientedPoint()
	{
		this.pos = default;
		this.rot = default;
	}
	
	public OrientedPoint(Vector3 pos, Quaternion rot)
	{
		this.pos = pos;
		this.rot = rot;
	}
	
	public OrientedPoint(Vector3 pos, Vector3 forward)
	{
		this.pos = pos;
		this.rot = Quaternion.LookRotation(forward);
	}
	
	public Vector3 LocalToWorldPos(Vector3 localSpacePos)
	{
		return pos + rot * localSpacePos;
	}
	
	public Vector3 LocalToWorldVect(Vector3 localSpacePos)
	{
		return rot * localSpacePos;
	}
	
}
