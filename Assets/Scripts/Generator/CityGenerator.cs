using UnityEngine;

[ExecuteInEditMode]
public class CityGenerator : MonoBehaviour
{
	[Header("Components")]
	[SerializeField] GameObject[] buildings;
	[Header("Settings")]
	[SerializeField] int citySize = 10;
	[SerializeField] float spacing = 1.0f;
	[SerializeField] float spawnThreshold = 0.1f;

	int[] randRotations = new int[]{0, 90, -90, 180};

	public void Generate()
	{
		Clear();

		if (buildings == null || buildings.Length == 0)
		{
			Debug.LogWarning("No building assigned.");
			return;
		}

		for (var i = 0; i < citySize; i++)
		{
			for (var j = 0; j < citySize; j++)
			{
				if (Random.Range(0f, 1f) > spawnThreshold)
				{
					var randBuilding = Random.Range(0, buildings.Length);
					var newBuilding = Instantiate(buildings[randBuilding], transform);
					newBuilding.transform.position = new Vector3(i * spacing, 0, j * spacing) - new Vector3(citySize - 1, 0, citySize - 1) * spacing / 2f;
					newBuilding.transform.Rotate(Vector3.up, randRotations[Random.Range(0, 4)]);
				}
			}
		}
	}

	public void Clear()
	{
		for (int i = transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
	}
}
