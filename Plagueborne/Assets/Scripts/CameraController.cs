using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform _target;
    private Vector3 _offset = new Vector3(0, 0, -10);
    private Vector3 velocity;
    public float smoothTime = 0.25f;
    void Start()
    {
        _target = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void Update()
    {
        Vector3 targetPos = _target.position + _offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
    }
}
