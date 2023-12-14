using System;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class CraftManager : MonoBehaviour
    {
        public GameObject craftingPanel;
        public KeyCode openCloseCraftButton;
        public Transform craftItemsPanel;
        public GameObject craftItemButtonPrefab;
        public FillCraftItemDetails currentCraftItemDetails;
        public GameObject inventoryPanel;
        public bool isOpened;

        public GameObject UIBG;
        public GameObject crosshair;
        public CinemachineVirtualCamera CVC;

        public List<CraftScriptableObject> allCrafts;

        public Button craftButton;
        [Header("Craft Item Details")] 
        public TMP_Text craftItemName;
        public TMP_Text craftItemDescription;
        public Image craftItemIcon;
        public TMP_Text craftItemDuration;
        public TMP_Text craftItemAmount;

        public void Start()
        {
            GameObject craftItemButton = Instantiate(craftItemButtonPrefab, craftItemsPanel);
            craftItemButton.GetComponent<Image>().sprite = allCrafts[0].finalCraft.icon;
            craftItemButton.GetComponent<FillCraftItemDetails>().currentCraftItem = allCrafts[0];
            craftItemButton.GetComponent<FillCraftItemDetails>().FillItemDetails();
            Destroy(craftItemButton);
            
            craftingPanel.gameObject.SetActive(false);
        }

        public void FillItemDetailsHelper()
        {
            if (currentCraftItemDetails != null)
                currentCraftItemDetails.FillItemDetails();
        }
        public void Update()
        {
            if (Input.GetKeyDown(openCloseCraftButton))
            {
                isOpened = !isOpened;
                GetComponent<InventoryManager>().isOpened = false;
                inventoryPanel.SetActive(false);
                if (isOpened)
                {
                    craftingPanel.SetActive(true);
                    UIBG.SetActive(true);
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
                    craftingPanel.SetActive(false);
                    crosshair.SetActive(true);
                    CVC.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_InputAxisName = "Mouse X";
                    CVC.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_InputAxisName = "Mouse Y";
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }
            }
        }

        public void LoadCraftItems(string craftType)
        {
            for (int i = 0, count = craftItemsPanel.childCount; i < count; i++)
            {
                Destroy(craftItemsPanel.GetChild(i).gameObject);
            }
            foreach (CraftScriptableObject cso in allCrafts)
            {
                if (cso.craftType.ToString().ToLower() == craftType.ToLower())
                {
                    GameObject craftItemButton = Instantiate(craftItemButtonPrefab, craftItemsPanel);
                    craftItemButton.GetComponent<Image>().sprite = cso.finalCraft.icon;
                    craftItemButton.GetComponent<FillCraftItemDetails>().currentCraftItem = cso;
                }
            }
        }
        
    }
}