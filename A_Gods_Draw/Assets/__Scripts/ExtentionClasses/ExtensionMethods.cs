using UnityEngine;
using System.Collections.Generic;

public static class ExtensionMethods
{
	public static float RemapT (this float from, float fromMin, float fromMax, float toMin,  float toMax)
	{
		var fromAbs  =  from - fromMin;
		var fromMaxAbs = fromMax - fromMin;      
	   
		var normal = fromAbs / fromMaxAbs;
 
		var toMaxAbs = toMax - toMin;
		var toAbs = toMaxAbs * normal;
 
		var to = toAbs + toMin;
	   
		if(to < 0) // Only positive numbers
	   		to *= -1;
		return to;
	}
	
	public static float Remap (this float from, float fromMin, float fromMax, float toMin,  float toMax)
	{
		var fromAbs  =  from - fromMin;
		var fromMaxAbs = fromMax - fromMin;      
	   
		var normal = fromAbs / fromMaxAbs;
 
		var toMaxAbs = toMax - toMin;
		var toAbs = toMaxAbs * normal;
 
		var to = toAbs + toMin;
	   
		return to;
	}

	public static void SetGlobalScale (this Transform transform, Vector3 globalScale)
 	{
		transform.localScale = Vector3.one;
		transform.localScale = new Vector3 (globalScale.x/transform.lossyScale.x, globalScale.y/transform.lossyScale.y, globalScale.z/transform.lossyScale.z);
 	}

	public static float PingPong(float t, float rangeFrom, float rangeTo)
    {
		float pong = Mathf.PingPong(t, 1);
		float value = Mathf.Lerp(rangeFrom, rangeTo, pong);
		return value;
    }

	public static void SawtoothWave(float In, out float Out)
	{
		Out = 2 * (In - Mathf.Floor(0.5f + In));
	}

	public static void PrintCollection(object[] collection)
	{
		string Log = "Print of Collection \"" + collection.ToString() + "\": ";
		foreach (var item in collection)
		{	
			Log += item.ToString() + ", ";
		}

		Debug.Log(Log);
	}

	public static void PrintCollection(IReadOnlyList<object> collection)
	{
		string Log = "Print of Collection \"" + collection.ToString() + "\": ";
		foreach (var item in collection)
		{	
			Log += item.ToString() + ", ";
		}

		Debug.Log(Log);
	}
}
