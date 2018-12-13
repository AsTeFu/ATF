using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallowCamera : MonoBehaviour {

    [Header("Target and Offset")]
    [SerializeField] [Tooltip("Transform of Player or Target")] private Rigidbody2D[] targets;
    [Space(15)]
    [SerializeField] [Tooltip("Offset from Target by axis X")] private float offsetPosX;
    [SerializeField] [Tooltip("Offset from Target by axis Y")] private float offsetPosY;

    [Header("Zoom")]
    [SerializeField] private float maxZoom;
    [SerializeField] private float minZoom;
    [SerializeField] private float limiter;
    [SerializeField] private float smoothnessZoom;
    private float currentZoom;
    private float velocityZoom;

    [Header("Smoothness")]
    [SerializeField] [Tooltip("Smoothing of camerа by axis X. 0 - Faster. 1 - Slower")] [Range(0, 1)] private float smoothnessX = 0.2f;
    [SerializeField] [Tooltip("Smoothing of camerа by axis Y. 0 - Faster. 1 - Slower")] [Range(0, 1)] private float smoothnessY = 0.2f;
    private float velocityX;
    private float velocityY;

    [Header("Limit of Position of Camera")]
    [SerializeField] private bool toLimit = false;
    [SerializeField] private bool drawBorderOfLimit = false;
    [SerializeField] private Vector2 center;
    [SerializeField] private Vector2 size;
    private float offsetSizeX;
    private float offsetSizeY;

    [Header("Draw camera fallow")]
    [SerializeField] private bool drawLineFolowing;
    private float side = 0.1f; //Side Hexagon

    private Camera camera;

    private void Awake() {
        camera = GetComponent<Camera>();
    }

    private void Start() {
        transform.position = new Vector3(0, 0, transform.position.z);
        //offsetSizeX = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, 0)).x;
        //offsetSizeY = Camera.main.orthographicSize;
    }

    private Vector2 GetCenter() {
        if (targets != null) {
            if (targets.Length == 1) {
                return targets[0].position;
            } else if (targets.Length > 1) {
                var bounds = new Bounds(targets[0].position, Vector3.zero);
                for (int i = 0; i < targets.Length; i++) {
                    bounds.Encapsulate(targets[i].position);
                }

                return bounds.center;
            }
        }
        return Vector2.zero;
    }
    private void ZoomCamera() {
        currentZoom = Mathf.Lerp(maxZoom, minZoom, GetGreatestDistance() / limiter);
        camera.orthographicSize = Mathf.SmoothDamp(camera.orthographicSize, currentZoom, ref velocityZoom, smoothnessZoom, 100f);
    }
    private float GetGreatestDistance() {

        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Length; i++) {
            bounds.Encapsulate(targets[i].position);
        }
        
        return (bounds.size.x > bounds.size.y ? bounds.size.x : bounds.size.y);
    }

    private void Fallow(Vector2 targetPosition) {
        
         float targetPositionX = Mathf.SmoothDamp(transform.position.x, targetPosition.x + offsetPosX, ref velocityX, smoothnessX);
         float targetPositionY = Mathf.SmoothDamp(transform.position.y, targetPosition.y + offsetPosY, ref velocityY, smoothnessY);

         transform.position = new Vector3(targetPositionX, targetPositionY, transform.position.z);

         DrawCameraFollowing(targetPositionX, targetPositionY);
        

        transform.position = LimitCamera(transform.position);
    }

    private void LateUpdate() {
        ZoomCamera();
        Fallow(GetCenter());
    }

    private Vector3 LimitCamera(Vector3 pos) {
        if (toLimit) {
            
            offsetSizeX = (camera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - camera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x) / 2;
            offsetSizeY = (camera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - camera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y) / 2;

            pos = new Vector3(Mathf.Clamp(pos.x, center.x - size.x + offsetSizeX, center.x + size.x - offsetSizeX), Mathf.Clamp(pos.y, center.y - size.y + offsetSizeY, center.y + size.y - offsetSizeY), pos.z);
            
        }
        return pos;
    }
    
    #region Draw editor
    private void DrawCameraFollowing(float targetPositionX, float targetPositionY) {
        if (drawLineFolowing) {
            Debug.DrawLine(transform.position, new Vector3(center.x, center.y, transform.position.z), Color.cyan, 0.0001f);
            DrawHexagon(side);
            DrawHexagon(side - 0.05f);
        }
    }
    private void OnDrawGizmosSelected() {
        if (toLimit && drawBorderOfLimit)
            DrawSquare(center, size);
    }
    private void DrawHexagon(float a) {
        float r = 0.866f * a;

        Debug.DrawLine(new Vector3(transform.position.x + r, transform.position.y - r, transform.position.z), new Vector3(transform.position.x + r, transform.position.y + r, transform.position.z), Color.cyan, 0.0001f);

        Debug.DrawLine(new Vector3(transform.position.x + r, transform.position.y + r, transform.position.z), new Vector3(transform.position.x, transform.position.y + a, transform.position.z), Color.cyan, 0.0001f);
        Debug.DrawLine(new Vector3(transform.position.x, transform.position.y + a, transform.position.z), new Vector3(transform.position.x - r, transform.position.y + r, transform.position.z), Color.cyan, 0.0001f);

        Debug.DrawLine(new Vector3(transform.position.x - r, transform.position.y + r, transform.position.z), new Vector3(transform.position.x - r, transform.position.y - r, transform.position.z), Color.cyan, 0.0001f);

        Debug.DrawLine(new Vector3(transform.position.x - r, transform.position.y - r, transform.position.z), new Vector3(transform.position.x, transform.position.y - a, transform.position.z), Color.cyan, 0.0001f);
        Debug.DrawLine(new Vector3(transform.position.x, transform.position.y - a, transform.position.z), new Vector3(transform.position.x + r, transform.position.y - r, transform.position.z), Color.cyan, 0.0001f);
    }
    private void DrawSquare(Vector2 center, Vector2 size) {
        //For draw limit positiom;
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(new Vector3(center.x + size.x, center.y - size.y, 0), new Vector3(center.x + size.x, center.y + size.y, 0));
        Gizmos.DrawLine(new Vector3(center.x + size.x, center.y + size.y, 0), new Vector3(center.x - size.x, center.y + size.y, 0));
        Gizmos.DrawLine(new Vector3(center.x - size.x, center.y + size.y, 0), new Vector3(center.x - size.x, center.y - size.y, 0));
        Gizmos.DrawLine(new Vector3(center.x - size.x, center.y - size.y, 0), new Vector3(center.x + size.x, center.y - size.y, 0));
    }
    #endregion
}
