//charlie
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// When you have a card selected it will have an arrow to where you want to place the card
/// if you are hovering over an enemy it will lock itself to it
/// when the mouse is no longer on the enemy it will continue to follow the mouse
/// </summary>
public class Card_Line : MonoBehaviour
{
    [Header("line render variables")]
    public LineRenderer line;
    [Range(2, 30)]
    public int resolution;

    [Range(2, 30)]
    public int linecastResolution;
    public LayerMask canHit;

    public Vector3 vel;
    public float yLimit;
    private float g;

    private void Start()
    {
        g = Mathf.Abs(Physics.gravity.y);
    }

    private void Update()
    {
        StartCoroutine(RenderArt());
    }

    private IEnumerator RenderArt()
    {
        line.positionCount = resolution + 1;
        line.SetPositions(CalculateLineArray());
        yield return null;
    }

    private Vector3[] CalculateLineArray()
    {
        Vector3[] lineArray = new Vector3[resolution + 1];
        var lowestTimeValueX = MaxTimeX() / resolution;
        var lowestTimeValueZ = MaxTimeZ() / resolution;
        var lowestTimeValue = lowestTimeValueX > lowestTimeValueZ ? lowestTimeValueZ : lowestTimeValueX;

        for (int i = 0; i < lineArray.Length; i++)
        {
            var t = lowestTimeValue * i;
            lineArray[i] = CalculateLinePoint(t);
        }

        return lineArray;
    }

    private Vector3 HitPosition()
    {
        var lowestTimeValue = MaxTimeY() / linecastResolution;

        for(int i = 0; i < linecastResolution + 1; i++)
        {
            RaycastHit rayHit;

            var t = lowestTimeValue * i;
            var tt = lowestTimeValue * (i + 1);

            if (Physics.Linecast(CalculateLinePoint(t), CalculateLinePoint(tt), out rayHit, canHit))
            {
                return rayHit.point;
            }
        }

        return CalculateLinePoint(MaxTimeY());
    }

    private Vector3 CalculateLinePoint(float t)
    {
        float x = vel.x * t;
        float z = vel.z * t;
        float y = (vel.y * t) - (g * Mathf.Pow(t, 2) / 2);
        return new Vector3(x + transform.position.x, y + transform.position.y, z + transform.position.z);
    }

    private float MaxTimeY()
    {
        var v = vel.y;
        var vv = v * v;

        var t = (v + Mathf.Sqrt(vv + 2 * g * (transform.position.y - yLimit))) / g;
        return t;
    }

    private float MaxTimeX()
    {
        if (IsValueAlmostZero(vel.x))
        {
            SetValueToAlmostZero(ref vel.x);
        }

        var x = vel.x;

        var t = (HitPosition().x - transform.position.x) / x;
        return t;
    }
    private float MaxTimeZ()
    {
        if (IsValueAlmostZero(vel.z))
        {
            SetValueToAlmostZero(ref vel.z);
        }

        var z = vel.z;

        var t = (HitPosition().z - transform.position.z) / z;
        return t;
    }

    private bool IsValueAlmostZero(float value)
    {
        return value < 0.0001f && value > -0.0001f;
    }

    private void SetValueToAlmostZero(ref float value)
    {
        value = 0.0001f;
    }
}
