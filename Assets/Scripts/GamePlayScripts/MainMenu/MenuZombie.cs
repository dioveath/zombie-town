using UnityEngine;
using UnityEngine.AI;


public class MenuZombie : MonoBehaviour {

    public GameObject[] zombies;
    GameObject zombie;

    private void Start() {
        int randomIndex = Random.Range(0, zombies.GetLength(0) - 1);
        GameObject zombiePrefab = zombies[randomIndex];
        zombie = Instantiate(zombiePrefab, transform, false);
        Destroy(zombie.GetComponent<BoxCollider>());
        Destroy(zombie.GetComponent<Zombie>());
        Destroy(zombie.GetComponent<NavMeshAgent>());
    }   

    private void Update() {
        zombie.transform.Rotate(Vector3.up * 45 * Time.deltaTime);
    }

}
