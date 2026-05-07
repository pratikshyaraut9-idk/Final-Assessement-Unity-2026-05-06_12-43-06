using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target;
    public float distance = 4.0f;
    public Vector2 sensitivity = new Vector2(0.2f, 0.2f);
    public Vector3 targetOffset = new Vector3(0, 1.5f, 0);

    public float yMinLimit = -20f;
    public float yMaxLimit = 60f;

    public float smoothTime = 0.12f;
    private Vector3 currentVelocity = Vector3.zero;
    
    private float x = 0.0f;
    private float y = 0.0f;

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        if (!target || Time.timeScale == 0) return;

        var mouse = Mouse.current;
if (mouse != null)
        {
            Vector2 lookInput = mouse.delta.ReadValue();
            x += lookInput.x * sensitivity.x;
            y -= lookInput.y * sensitivity.y;
        }

        y = ClampAngle(y, yMinLimit, yMaxLimit);

        Quaternion rotation = Quaternion.Euler(y, x, 0);

        Vector3 targetPosition = target.position + targetOffset;
        Vector3 desiredPosition = rotation * new Vector3(0, 0, -distance) + targetPosition;

        // Collision check
        RaycastHit hit;
        if (Physics.SphereCast(targetPosition, 0.2f, (desiredPosition - targetPosition).normalized, out hit, distance))
        {
            desiredPosition = targetPosition + (desiredPosition - targetPosition).normalized * hit.distance;
        }

        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref currentVelocity, smoothTime);
        transform.rotation = rotation;
    }

    private static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F) angle += 360F;
        if (angle > 360F) angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }
}
