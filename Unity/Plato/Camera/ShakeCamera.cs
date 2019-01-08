using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeCamera : MonoBehaviour {
    
    public static IEnumerator Shake(Transform camera, float magnitude, float duration) {
        Vector3 originalPos = camera.position;

        float currentTime = 0.0f;

        while (currentTime < duration) {

            float x = Random.Range(-1, 1) * magnitude;
            float y = Random.Range(-1, 1) * magnitude;

            camera.position = new Vector3(x, y, originalPos.z);

            currentTime += Time.deltaTime;

            yield return null;
        }

        camera.position = originalPos;
    }
    public static IEnumerator Shake(Transform camera, float magnitudeX, float magnitudeY, float duration) {
        Vector3 originalPos = camera.position;

        float currentTime = 0.0f;

        while (currentTime < duration) {

            float x = Random.Range(-1f, 1f) * magnitudeX;
            float y = Random.Range(-1f, 1f) * magnitudeY;

            camera.position = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z);

            currentTime += Time.deltaTime;

            yield return null;
        }

        camera.position = originalPos;
    }

}
