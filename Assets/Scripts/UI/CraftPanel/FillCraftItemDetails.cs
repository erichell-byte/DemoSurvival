using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class FillCraftItemDetails : MonoBehaviour
{
    public CraftScriptableObject currentCraftItem;
    public CraftManager craftManager;
    public CraftQueueManager craftQueueManager;
    public GameObject craftResourcePrefab;
    public string craftInfoPanelName;

    private Transform craftInfoPanelTransform;

    public void Awake()
    {
        craftInfoPanelTransform = GameObject.Find(craftInfoPanelName).transform;
        craftManager = FindObjectOfType<CraftManager>();
        craftQueueManager = FindObjectOfType<CraftQueueManager>();
    }

    public void FillItemDetails()
    {
        craftManager.currentCraftItemDetails = this;
        for (int i = 0; i < craftInfoPanelTransform.childCount; i++)
        {
            Destroy(craftInfoPanelTransform.GetChild(i).gameObject);
        }
        craftManager.craftItemName.text = currentCraftItem.finalCraft.name;
        craftManager.craftItemDescription.text = currentCraftItem.finalCraft.itemDescription;
        craftManager.craftItemIcon.sprite = currentCraftItem.finalCraft.icon;
        craftManager.craftItemDuration.text = currentCraftItem.craftTime.ToString();
        craftManager.craftItemAmount.text = currentCraftItem.craftAmount.ToString();
        
        bool canCraft = true;
        
        for (int i = 0; i < currentCraftItem.craftingResources.Count; i++)
        {
            var craftItem = currentCraftItem.craftingResources[i];
            GameObject craftResourceGO = Instantiate(craftResourcePrefab, craftInfoPanelTransform);
            CraftResourceDetails crd = craftResourceGO.GetComponent<CraftResourceDetails>();
            crd.amountText.text = craftItem.craftObjectAmount.ToString();
            crd.itemTypeText.text = craftItem.craftObject.itemName;
            int totalAmount = (int)(currentCraftItem.craftingResources[i].craftObjectAmount * int.Parse(craftQueueManager.craftAmountInputField.text));
            crd.totalText.text = totalAmount.ToString();
            int resourceAmount = 0;
            foreach (var slot in FindObjectsOfType<InventoryManager>()[0].slots)
            {
                if (slot.isEmpty)
                    continue;
                if (slot.item.itemName == craftItem.craftObject.itemName)
                {
                    resourceAmount += slot.amount;
                }
            }
            crd.haveText.text = resourceAmount.ToString();

            if (resourceAmount < totalAmount)
            {
                canCraft = false;
            }

            if (canCraft)
                craftManager.craftButton.interactable = true;
            else
                craftManager.craftButton.interactable = false;

            craftQueueManager.currentCraftItem = currentCraftItem;
            

        }
    }
}
