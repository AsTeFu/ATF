using UnityEngine;
using System.Collections;

public class ShipController : MonoBehaviour {

    [Header("Variable")]
    [SerializeField][Tooltip("Скорость следования")][Range(0, 1)] private float speed;
    [SerializeField][Tooltip("Угол наклона")][Range(0, 45)] private float rotationAngle;
    [SerializeField][Tooltip("Время наклона")][Range(0, 5)] private float rotationTime;
    [SerializeField]private GameObject mesh;
    private bool downShip = false;

    private float t = 0;
    private bool rotation;

    private BoxCollider colliderObj;
    private RaycastHit hit;
    private Ray ray;
    private Vector3 dir;
    private Vector3 pos;
    private Vector3 newRot;
    private float defRot;
    private Transform transformMesh;


    void Awake() {
        transformMesh = mesh.transform;
        colliderObj = GetComponent<BoxCollider>();
    }



    public void Controller() {
        if (Input.GetMouseButton(0) && downShip) {
#if UNITY_EDITOR
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
#else
            ray = Camera.main.ScreenPointToRay(Input.touches[0].position);
#endif
            if (Physics.Raycast(ray, out hit)) {
                dir = new Vector3(hit.point.x, hit.point.y, transform.position.z);
                pos = transform.position;
                transform.position = Vector3.Lerp(pos, dir, speed);
                RotatePlayer();
            }

        }
    }

    void OnMouseDown() {
        t = 0;
        GetMouse(20, false);
    }

    void OnMouseUp() {
        GetMouse(0.05f, true);
    }

    void GetMouse(float num, bool bl) {
        downShip = !bl;
        rotation = bl;
        colliderObj.size *= num;
    }

    private void RotatePlayer() {
        float ratio = transform.position.x - dir.x;
        transformMesh.localEulerAngles = new Vector3(0, ratio * rotationAngle, 0);
    }
    

    public void ReturnRotate() {
        if (rotation) {
            Vector3 vec = transformMesh.localEulerAngles;
            transformMesh.localEulerAngles = new Vector3(0, Mathf.LerpAngle(vec.y, 0, t), 0);
            t += rotationTime * Time.deltaTime;
            if (t > 1f) {
                t = 0;
                rotation = false;
            }
        }
    }
}
