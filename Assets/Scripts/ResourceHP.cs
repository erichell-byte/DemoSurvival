using System.Collections;
using System.Collections.Generic;
using KrisDevelopment.EnvSpawn;
using UnityEngine;

public class ResourceHP : MonoBehaviour
{
    public int startHealth = 5;
    public int health;
    public int destroyTime = 5;
    private Transform resourceSpawner;
    public ScriptableObject resourceType;
    public GameObject rockBreakFX;
    [SerializeField] private string spawnerName = "TreeSpawner";

    public void Start()
    {
        resourceSpawner = GameObject.Find(spawnerName).transform;
        health = startHealth;
    }
    public void TreeFall()
    {
        gameObject.AddComponent<Rigidbody>();
        Rigidbody rig = GetComponent<Rigidbody>();
        rig.isKinematic = false;
        rig.useGravity = true;
        rig.mass = 200;
        rig.constraints = RigidbodyConstraints.FreezeRotationY;

        RespawnResource();
        Destroy(gameObject,destroyTime);
    }

    public void StoneGathered()
    {

        Instantiate(rockBreakFX, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    public void RespawnResource()
    {
        float randomX =
            Random.Range(resourceSpawner.position.x - resourceSpawner.GetComponent<EnviroSpawn_CS>().dimensions.x / 2,
                resourceSpawner.position.x + resourceSpawner.GetComponent<EnviroSpawn_CS>().dimensions.x / 2);
        float randomY =
            Random.Range(resourceSpawner.position.z - resourceSpawner.GetComponent<EnviroSpawn_CS>().dimensions.y / 2,
                resourceSpawner.position.z + resourceSpawner.GetComponent<EnviroSpawn_CS>().dimensions.y / 2);
        Vector3 rayPos = new Vector3(randomX, 100, randomY);
        RaycastHit hit;
        if (Physics.SphereCast(rayPos, 2, Vector3.down, out hit, 200))
        {
            if (hit.collider.gameObject.layer == 3)
            {
                GameObject newTree = Instantiate(gameObject, hit.point, Quaternion.identity);
                Destroy(newTree.GetComponent<Rigidbody>());
                newTree.transform.rotation = Quaternion.identity;
            }
            else
            {
                {
                    RespawnResource();
                }
            }
        }

    }
}
