using System.Collections;
using UnityEngine;

public class CameraController : Singleton<CameraController>
{
    [SerializeField] float smoothing = 0.2f;
    public Vector2 minCameraBoundary;
    public Vector2 maxCameraBoundary;
    public bool cameraSmoothMove;

    [Header("Camera Shake")]
    // 카메라 흔들기
    // 흔들림 강도와 지속 시간
    public float shakeDuration = 0.5f;
    public float shakeMagnitude = 0.1f;

    // 초기 위치 저장
    private Vector3 originalPos;

    [Header("Camera Zoom")]
    public float zoomSpeed = 5f;  // 줌 속도
    public float targetZoomIn = 1f;  // 줌 인 목표값
    public float targetZoomOut = 5f;  // 줌 아웃 목표값
    private float defaultZoom;
    private Camera cameraComponent;


    private void Start()
    {
        cameraComponent = GetComponent<Camera>();
        if (!cameraComponent.orthographic)
        {
            return;
        }
        defaultZoom = cameraComponent.orthographicSize;
    }
   

    private void LateUpdate()
    {
        if (cameraSmoothMove)
        {
            Vector3 targetPos = new Vector3(Player.Instance.transform.position.x, Player.Instance.transform.position.y, transform.position.z);

            targetPos.x = Mathf.Clamp(targetPos.x, minCameraBoundary.x, maxCameraBoundary.x);
            targetPos.y = Mathf.Clamp(targetPos.y, minCameraBoundary.y, maxCameraBoundary.y);

            transform.position = Vector3.Lerp(transform.position, targetPos, smoothing);
        }
        else
        {
            transform.position = new Vector3(Player.Instance.transform.position.x, Player.Instance.transform.position.y, transform.position.z);
            cameraSmoothMove = true;
        }
    }

    public void CameraShake(float duration, float magnitude)
    {
        originalPos = transform.localPosition;
        StartCoroutine(Shake(duration, magnitude));
    }

    IEnumerator Shake(float duration, float magnitude)
    {
        float elapsed = 0.0f;
        shakeDuration = duration;
        shakeMagnitude = magnitude;

        while (elapsed < shakeDuration)
        {
            // 임의의 오프셋 생성
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;

            // 카메라 위치 조정
            transform.localPosition = new Vector3(x + originalPos.x, y + originalPos.y, originalPos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        // 원래 위치로 되돌림
        transform.localPosition = originalPos;
    }

    public void TriggerZoomIn()
    {
        StartCoroutine(ZoomIn());
    }

    public void TriggerZoomOut()
    {
        StartCoroutine(ZoomOut());
    }

    public void TriggerZoomReturn()
    {
        StartCoroutine(ZoomReturn());
    }

    IEnumerator ZoomIn()
    {
        float elapsed = 0f;
        float startingZoom = cameraComponent.orthographicSize;
        while (elapsed < 1f / zoomSpeed)
        {
            elapsed += Time.deltaTime;
            cameraComponent.orthographicSize = Mathf.Lerp(startingZoom, targetZoomIn, elapsed * zoomSpeed);
            yield return null;
        }
        cameraComponent.orthographicSize = targetZoomIn;
    }

    IEnumerator ZoomOut()
    {
        float elapsed = 0f;
        float startingZoom = cameraComponent.orthographicSize;
        while (elapsed < 1f / zoomSpeed)
        {
            elapsed += Time.deltaTime;
            cameraComponent.orthographicSize = Mathf.Lerp(startingZoom, targetZoomOut, elapsed * zoomSpeed);
            yield return null;
        }
        cameraComponent.orthographicSize = targetZoomOut;
    }

    IEnumerator ZoomReturn()
    {
        float elapsed = 0f;
        float startingZoom = cameraComponent.orthographicSize;
        while (elapsed < 1f / zoomSpeed)
        {
            elapsed += Time.deltaTime;
            cameraComponent.orthographicSize = Mathf.Lerp(startingZoom, defaultZoom, elapsed * zoomSpeed);
            yield return null;
        }
        cameraComponent.orthographicSize = defaultZoom;
    }
}
