using UnityEngine;

public class FollowCar : MonoBehaviour
{
    [SerializeField] public Transform _carTransform;
    [SerializeField] private float _lerpSpeed;
    private Vector3 _carPosition;
    private Vector3 _previousCarPosition;

    private Vector3 _originalOffset;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _originalOffset = _carTransform.position - transform.position;
        _previousCarPosition = _carTransform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 _currentOffset = _carTransform.position - transform.position;
        Vector3 targetPosition = _currentOffset - _originalOffset;

        transform.position = Vector3.Lerp(transform.position, transform.position + targetPosition, _lerpSpeed * Time.deltaTime);
        _previousCarPosition = _carTransform.position;

        transform.LookAt(_carTransform.position);
    }
}
