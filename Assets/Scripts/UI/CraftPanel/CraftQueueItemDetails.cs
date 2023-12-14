using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class CraftQueueItemDetails : MonoBehaviour
{
    public Image icon;
    public TMP_Text amountText, timeText;
    public int craftTime;
    public int craftAmount;
    private InventoryManager _inventoryManager;
    private CraftManager _craftManager;
    public CraftScriptableObject currentCraftItem;

    private int initialCraftTime;
    private void Start()
    {
        initialCraftTime = craftTime;
        _inventoryManager = FindObjectOfType<InventoryManager>();
        _craftManager = FindObjectOfType<CraftManager>();
        if (transform.parent.childCount <= 1)
        {
            InvokeRepeating("UpdateTime", 0f,1f);
        }
        else
        {
            UpdateTime();
        }

        craftTime++;

    }

    public void StartInvoke()
    {
        InvokeRepeating("UpdateTime", 0f,1f);
    }
    
    public void RemoveFromQueue()
    {
        foreach (CraftResource resource in currentCraftItem.craftingResources)
        {
            _inventoryManager.AddItem(resource.craftObject, resource.craftObjectAmount * craftAmount);
        }
        CancelInvoke();
        _craftManager.currentCraftItemDetails.FillItemDetails();
        if (transform.parent.childCount > 1)
            transform.parent.GetChild(1).GetComponent<CraftQueueItemDetails>().StartInvoke();
        Destroy(gameObject);
    }
    
    void UpdateTime()
    {
        amountText.text = "X " +  craftAmount;
        craftTime--;
        if (craftTime <= 0)
        {
            _inventoryManager.AddItem(currentCraftItem.finalCraft,currentCraftItem.craftAmount);
            craftAmount--;
            craftTime = initialCraftTime;
            if (craftAmount <= 0)
            {
                CancelInvoke();
                if (transform.parent.childCount > 1)
                    transform.parent.GetChild(1).GetComponent<CraftQueueItemDetails>().StartInvoke();
                Destroy(gameObject);

            }
        }
        else
        {
            int minutes = Mathf.FloorToInt(craftTime / 60);
            int seconds = craftTime - minutes * 60;
            timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }
}
