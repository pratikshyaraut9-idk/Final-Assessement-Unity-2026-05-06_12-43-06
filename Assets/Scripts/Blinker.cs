using UnityEngine;

public class Blinker : MonoBehaviour
{
    private Renderer _renderer;
    private Material _mat;
    private float _timer;

    void Start()
    {
        _renderer = GetComponent<Renderer>();
        if (_renderer != null)
        {
            _mat = _renderer.material;
        }
    }

    void Update()
    {
        if (_mat == null) return;
        _timer += Time.deltaTime * 5f;
        float val = (Mathf.Sin(_timer) + 1f) / 2f;
        _mat.SetColor("_EmissionColor", Color.green * val * 4f);
    }
}
