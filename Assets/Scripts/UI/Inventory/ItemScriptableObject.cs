using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ItemType
{
    Default, Food, Weapon, Instrument
}
public class ItemScriptableObject : ScriptableObject
{
    public GameObject itemPrefab;
    public ItemType itemType;
    public string itemName;
    public Sprite icon;
    public int maxAmount;
    public string itemDescription;
    public bool isConsumeable;
    public string inHandName;

    [Header("Consumable Characteristics")]
    public float changeHealth;
    public float changeHunger;
    public float changeThirst;
}
