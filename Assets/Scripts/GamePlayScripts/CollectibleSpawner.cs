using UnityEngine;

public class CollectibleSpawner : MonoBehaviour {

    public Transform[] spawnPlaces;
	public Collectible[] spawnItems;
    public float spawnRate;
    float nextSpawnTime;
	bool[] spawnedPlacesFlags;
	Collectible[] spawnedItems;

    private void Start() {
		spawnedPlacesFlags = new bool[spawnPlaces.GetLength (0)];
		spawnedItems = new Collectible[spawnPlaces.GetLength (0)];
    }

    void Update() {
        if(Time.time > nextSpawnTime) {
            int randomSpawnItemIndex = Random.Range(0, spawnItems.GetLength(0));
            int randomSpawnPlaceIndex = Random.Range(0, spawnPlaces.GetLength(0));

			int i = 0;
			while (spawnedPlacesFlags [randomSpawnPlaceIndex]) {
				if (i < spawnPlaces.GetLength (0)) {
					randomSpawnPlaceIndex = Random.Range (0, spawnPlaces.GetLength (0));
					i++;
				} else {
					nextSpawnTime = Time.time + spawnRate;
					return;
				}
			}

			Collectible spawnItem = Instantiate (spawnItems [randomSpawnItemIndex], spawnPlaces [randomSpawnPlaceIndex], false);
			spawnedPlacesFlags [randomSpawnPlaceIndex] = true;
			spawnItem.OnUsed += OnItemUsed;
			spawnedItems [randomSpawnPlaceIndex] = spawnItem;

			nextSpawnTime = Time.time + spawnRate;
        }
    }

	public void OnItemUsed(){
		for (int i = 0; i < spawnPlaces.GetLength (0); i++) {
			if (spawnedPlacesFlags [i] == true) {
				if (spawnedItems [i] == null) {
					spawnedPlacesFlags[i] = false;
				}
			}
		}
	}

}
