
using System;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class CustomCursor : MonoBehaviour
{
    [SerializeField]
    private Texture2D _cursorTexture;
    private CursorMode _cursorMode = CursorMode.Auto;
    private Vector2 _screenLocation = Vector2.zero;

    private void OnMouseEnter()
    {
        Cursor.SetCursor(_cursorTexture, _screenLocation, _cursorMode);
    }

    private void OnMouseExit()
    {
        Cursor.SetCursor(null, Vector2.zero, _cursorMode);
    }
}
