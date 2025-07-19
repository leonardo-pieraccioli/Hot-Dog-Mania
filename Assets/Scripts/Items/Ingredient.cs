using UnityEngine;

public class Ingredient : MonoBehaviour
{   
    [SerializeField] private Mesh[] _ingredientsMeshes;
    
    private void Start()
    {
        if (_ingredientsMeshes.Length > 0)
        {
            int randomIndex = Random.Range(0, _ingredientsMeshes.Length);
            GetComponent<MeshFilter>().mesh = _ingredientsMeshes[randomIndex];
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        // Add ingredient to player 
        Debug.Log("Collision detected with: " + collision.gameObject.name);
        Destroy(gameObject, 0.1f); // Destroy the ingredient on collision
    }
}
