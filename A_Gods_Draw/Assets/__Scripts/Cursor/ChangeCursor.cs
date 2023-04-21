//charlie

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCursor : MonoBehaviour
{
    public Texture2D[] cursors;
    public static ChangeCursor instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        DefaultCursor();
    }

    public void DefaultCursor()
    {
        Cursor.SetCursor(cursors[0], Vector2.zero, CursorMode.Auto);
    }

    public void AttackCursor()
    {
        Cursor.SetCursor(cursors[1], Vector2.zero, CursorMode.Auto);
    }

    public void BuffCursor()
    {
        Cursor.SetCursor(cursors[2], Vector2.zero, CursorMode.Auto);
    }

    public void DefenceCursor()
    {
        Cursor.SetCursor(cursors[3], Vector2.zero, CursorMode.Auto);
    }

    public void GodCursor()
    {
        Cursor.SetCursor(cursors[4], Vector2.zero, CursorMode.Auto);
    }

    public void UtilityCursor()
    {
        Cursor.SetCursor(cursors[5], Vector2.zero, CursorMode.Auto);
    }
}
