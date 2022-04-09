using UnityEngine;

public class BoardCamera
{
    public GameObject boardCamera;

    public BoardCamera(GameObject parent){
        InitializeCamera(parent);
    }
    /// <summary>
    /// Creates a camera object to focus on this board.
    /// TODO : Make the transform position a variable that can be saved.
    /// </summary>
    public void InitializeCamera(GameObject parent) {
        this.boardCamera = new GameObject("camera");
        this.boardCamera.transform.position = new Vector3(0, 0, -10.0f);
        this.boardCamera.SetActive(false);

        this.boardCamera.AddComponent<Camera>();
        this.boardCamera.transform.parent = parent.transform;

        Camera camera = this.boardCamera.GetComponentInChildren<Camera>();
        camera.orthographic = true;
        camera.orthographicSize = 12.00f;
    }

    /// <summary>
    /// Makes this camera the one the user will be viewing.
    /// </summary>
    public void ActivateCamera() {
        this.boardCamera.tag = "MainCamera"; // sets Camera.main property
        this.boardCamera.SetActive(true);
    }

}
