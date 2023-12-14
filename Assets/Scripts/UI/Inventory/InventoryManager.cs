using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.Serialization;

public class InventoryManager : MonoBehaviour
{
    public GameObject               UIBG;
    public GameObject               crosshair;
    public Transform                inventoryPanel;
    public Transform                quickSlotPanel;
    public List<InventorySlot>      slots = new List<InventorySlot>();
    public bool                     isOpened;
    public float                    reachDistance = 3f;
    public CinemachineVirtualCamera CVC;
    public GameObject               craftPanel;
    public Transform                playerTransform;

    private Camera       mainCamera;
    private CraftManager craftManager;

    private void Awake()
    {
        UIBG.SetActive(true);
        inventoryPanel.gameObject.SetActive(true);
    }

    void Start()
    {
        mainCamera = Camera.main;
        craftManager = FindObjectOfType<CraftManager>();
        for (int i = 0; i < inventoryPanel.childCount; i++)
        {
            if (inventoryPanel.GetChild(i).GetComponent<InventorySlot>() != null)
            {
                slots.Add(inventoryPanel.GetChild(i).GetComponent<InventorySlot>());
            }
        }

        for (int i = 0; i < quickSlotPanel.childCount; i++)
        {
            if (quickSlotPanel.GetChild(i).GetComponent<InventorySlot>() != null)
            {
                slots.Add(quickSlotPanel.GetChild(i).GetComponent<InventorySlot>());
            }
        }

        inventoryPanel.gameObject.SetActive(false);
        UIBG.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            isOpened = !isOpened;
            craftManager.isOpened = false;
            craftPanel.SetActive(false);
            if (isOpened)
            {
                UIBG.SetActive(true);
                inventoryPanel.gameObject.SetActive(true);
                crosshair.SetActive(false);
                CVC.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_InputAxisName = "";
                CVC.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_InputAxisName = "";
                CVC.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_InputAxisValue = 0;
                CVC.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_InputAxisValue = 0;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                UIBG.SetActive(false);
                inventoryPanel.gameObject.SetActive(false);
                crosshair.SetActive(true);
                CVC.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_InputAxisName = "Mouse X";
                CVC.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_InputAxisName = "Mouse Y";
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Physics.Raycast(ray, out hit, reachDistance))
            {
                if (hit.collider.gameObject.GetComponent<Item>() != null)
                {
                    AddItem(hit.collider.gameObject.GetComponent<Item>().item,
                        hit.collider.gameObject.GetComponent<Item>().amount);
                    craftManager.currentCraftItemDetails.FillItemDetails();
                    Destroy(hit.collider.gameObject);
                }
            }
        }
    }

    public void AddItem(ItemScriptableObject _item, int _amount)
    {
        bool allFull = false;
        int addedAmount = _amount;
        foreach (InventorySlot slot in slots)
        {
            // В слоте уже имеется этот предмет
            if (slot.item == _item)
            {
                if (slot.amount + addedAmount <= _item.maxAmount)
                {
                    slot.amount += addedAmount;
                    slot.itemAmountText.text = slot.amount.ToString();
                    return;
                }
                else
                {
                    addedAmount -= _item.maxAmount - slot.amount;
                    slot.amount = _item.maxAmount;
                    slot.itemAmountText.text = slot.amount.ToString();
                }

                continue;
            }
        }

        foreach (InventorySlot slot in slots)
        {
            if (addedAmount <= 0)
                return;
            if (slot.isEmpty == true)
            {
                slot.item = _item;
                slot.isEmpty = false;
                slot.SetIcon(_item.icon);
                if (slot.item.maxAmount != 1)
                    slot.itemAmountText.text = _amount.ToString();
                if (addedAmount > _item.maxAmount)
                {
                    slot.amount = _item.maxAmount;
                    addedAmount -= _item.maxAmount;
                }
                else
                {
                    slot.amount = addedAmount;
                    break;
                }
            }
        }
        
        allFull = true;
        foreach (InventorySlot inventorySlot in slots)
        {
            if (inventorySlot.isEmpty)
            {
                allFull = false;
                break;
            }
        }

        if (allFull)
        {
            GameObject itemObject = Instantiate(_item.itemPrefab,
                playerTransform.position + Vector3.up + playerTransform.forward, Quaternion.identity);
            itemObject.GetComponent<Item>().amount = addedAmount;
            Debug.Log("Throw out");
            return;
        }
    }
}