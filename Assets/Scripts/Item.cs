using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SellItem", menuName = "Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public Sprite itemImg;
    public int baseValue;
}
