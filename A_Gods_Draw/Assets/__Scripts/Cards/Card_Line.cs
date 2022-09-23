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
    Vector3 startPos, endPos, mousePos, mouseDirection;
    Camera camera;
    LineRenderer line;
    private float max = 5f;

    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = camera.ScreenToWorldPoint(Input.mousePosition);
        mouseDirection = mousePos - gameObject.transform.position;
        mouseDirection.z = 0;
        mouseDirection = mouseDirection.normalized;

        if (Input.GetMouseButtonDown(0))
        {
            line.enabled = true;
        }

        if (Input.GetMouseButton(0))
        {
            startPos = gameObject.transform.position;
            startPos.z = 0;
            line.SetPosition(0, startPos);

            endPos = mousePos;
            endPos.z = 0;
            float capLenght = Mathf.Clamp(Vector2.Distance(startPos, endPos), 0, max);
            endPos = startPos + (mouseDirection * capLenght);
            line.SetPosition(1, endPos);
        }

        if (Input.GetMouseButtonUp(0))
        {
            line.enabled = false;
        }
    }
}
