using System;
using UnityEngine;

public class BoardCamera
{
    private readonly GameObject _boardCamera;
    private readonly Camera _camera;

    public BoardCamera(GameObject parent, Vector3Int cameraPosition){
        _boardCamera = new GameObject("Camera")
        {
            transform =
            {
                position = (Vector3)cameraPosition
            }
        };
        _boardCamera.SetActive(false);

        _boardCamera.AddComponent<Camera>();
        _boardCamera.transform.parent = parent.transform;

        _camera = _boardCamera.GetComponentInChildren<Camera>();
        _camera.orthographic = true;
        _camera.orthographicSize = 12.00f;
        
        // TODO : Pull magic number into static variable, probably in gameData or something
        _camera.clearFlags = CameraClearFlags.SolidColor;
        var color = Helpers.HexStringToColor("171B1F");
        _camera.backgroundColor = color;
    }
    
    /// <summary>
    /// Makes this camera the one the user will be viewing.
    /// </summary>
    public void ActivateCamera() {
        _boardCamera.tag = "MainCamera"; // sets Camera.main property
        _boardCamera.SetActive(true);
        _camera.enabled = true;

    }

    public void DeactivateCamera() {
        _camera.enabled = false;
        _boardCamera.SetActive(false);
    }

}
