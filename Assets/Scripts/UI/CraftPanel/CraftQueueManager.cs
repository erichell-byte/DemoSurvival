using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using Unity.VisualScripting;
using UnityEngine;

public class CraftQueueManager : MonoBehaviour
{
    public CraftScriptableObject currentCraftItem;
    public GameObject craftQueuePrefab;
    public TMP_InputField craftAmountInputField;
    public InventoryManager inventoryManager;
    public int craftTime;
    
    
    private CraftManager _craftManager;

    private void Awake()
    {
        _craftManager = FindObjectOfType<CraftManager>();
        inventoryManager = FindObjectOfType<InventoryManager>();

        craftAmountInputField.text = "1";
    }

    public void PlusButtonFunction()
    {
        int newAmount = int.Parse(craftAmountInputField.text) + 1;
        craftAmountInputField.text = newAmount.ToString();
    }

    public void MinusButtonFunction()
    {
        if (int.Parse(craftAmountInputField.text) - 1 <= 0)
            return;
        int newAmount = int.Parse(craftAmountInputField.text) - 1;
        craftAmountInputField.text = newAmount.ToString();
    }
    public void AddToCraftQueue()
    {
        Debug.Log("addItemToCraft");
        foreach (var resource in currentCraftItem.craftingResources)
        {
            int amountToRemove = resource.craftObjectAmount * int.Parse(craftAmountInputField.text);
            foreach (var slot in inventoryManager.slots)
            {
                if (amountToRemove <= 0)
                    continue;
                if (slot.item == resource.craftObject)
                {
                    if (amountToRemove > slot.amount)
                    {
                        amountToRemove -= slot.amount;
                        slot.GetComponentInChildren<DragAndDropItem>().NullifySlotData();
                    }
                    else
                    {
                        slot.amount -= amountToRemove;
                        amountToRemove = 0;
                        if (slot.amount <= 0)
                        {
                            slot.GetComponentInChildren<DragAndDropItem>().NullifySlotData();
                        }
                        else
                        {
                            slot.itemAmountText.text = slot.amount.ToString();
                        }
                    }
                }
            }
        }
        
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<CraftQueueItemDetails>().currentCraftItem == currentCraftItem)
            {
                transform.GetChild(i).GetComponent<CraftQueueItemDetails>().craftAmount += int.Parse(craftAmountInputField.text);
                transform.GetChild(i).GetComponent<CraftQueueItemDetails>().amountText.text = "X " +
                    transform.GetChild(i).GetComponent<CraftQueueItemDetails>().craftAmount;
                _craftManager.currentCraftItemDetails.FillItemDetails();
                return;
            }
        }
        GameObject craftQueueInstance = Instantiate(craftQueuePrefab, transform);
        CraftQueueItemDetails craftQueueItemDetails = craftQueueInstance.GetComponent<CraftQueueItemDetails>();
        craftQueueItemDetails.icon.sprite = currentCraftItem.finalCraft.icon;
        craftQueueItemDetails.amountText.text = craftAmountInputField.text;
        craftQueueItemDetails.craftAmount = int.Parse(craftAmountInputField.text);
        craftTime = currentCraftItem.craftTime;
        int minutes = Mathf.FloorToInt(craftTime / 60);
        int seconds = craftTime - minutes * 60;
        craftQueueItemDetails.timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        craftQueueItemDetails.craftTime = craftTime;
        craftQueueItemDetails.currentCraftItem = currentCraftItem;
        
        _craftManager.currentCraftItemDetails.FillItemDetails();
    }
}
