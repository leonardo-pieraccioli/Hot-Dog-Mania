using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private Mesh[] _obstacleMeshes;
    [Range(300, 1200)]
    public float _torqueMultiplier = 500f;
    
    private void Start()
    {
        if (_obstacleMeshes.Length > 0)
        {
            int randomIndex = Random.Range(0, _obstacleMeshes.Length);
            GetComponent<MeshFilter>().mesh = _obstacleMeshes[randomIndex];
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            return; // Ignore collisions with the player
        }
        Debug.Log("Collision detected with: " + collision.gameObject.name);
        Vector3 randomDir = new Vector3(
            Random.Range(-.01f, .01f), 
            Random.Range(-1f, 1f), 
            Random.Range(-.01f, .01f)
        ).normalized;
        collision.rigidbody.AddTorque(_torqueMultiplier * randomDir,
            ForceMode.Impulse
        );
        Destroy(gameObject, 0.1f); 
    }
}
