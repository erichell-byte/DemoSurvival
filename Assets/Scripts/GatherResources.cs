using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatherResources : MonoBehaviour
{
    public Camera mainCamera;
    public LayerMask layerMask;
    public InventoryManager inventoryManager;
    public ItemScriptableObject resource;
    public int resourceItem;
    public GameObject hitFX;
    public void GatherResource()
    {
        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
        if (Physics.Raycast(ray, out hit,1.5f, layerMask))
        {
            if (resource.name == hit.collider.GetComponent<ResourceHP>().resourceType.name)
            {
                if (hit.collider.GetComponent<ResourceHP>().health >= 1)
                {
                    Instantiate(hitFX, hit.point, Quaternion.Euler(hit.normal));
                    inventoryManager.AddItem(resource, resourceItem);
                    hit.collider.GetComponent<ResourceHP>().health--;
                    if (hit.collider.GetComponent<ResourceHP>().health <= 0 && hit.collider.gameObject.layer == 6)
                    {
                        hit.collider.GetComponent<ResourceHP>().TreeFall();
                        hit.collider.GetComponent<Rigidbody>().AddForce(mainCamera.transform.forward * 50, ForceMode.Impulse);
                    }
                    if (hit.collider.GetComponent<ResourceHP>().health <= 0 && hit.collider.gameObject.layer == 7)
                    {
                        hit.collider.GetComponent<ResourceHP>().StoneGathered();
                    }
                }
            }
        }

    }
}
