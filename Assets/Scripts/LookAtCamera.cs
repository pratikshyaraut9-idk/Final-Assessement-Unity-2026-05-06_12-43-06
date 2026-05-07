using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Transform _mainCam;

    void Start()
    {
        if (Camera.main != null)
        {
            _mainCam = Camera.main.transform;
        }
    }

    void LateUpdate()
    {
        if (_mainCam != null)
        {
            transform.LookAt(transform.position + _mainCam.forward);
        }
    }
}
