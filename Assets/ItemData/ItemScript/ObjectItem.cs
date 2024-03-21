using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectItem : MonoBehaviour, IObjectItem
{
    [Header("아이템")] 
    public Item Item;

    [Header("아이템 이미지")] 
    public SpriteRenderer itemImage; 

    void Start()
    {
        itemImage.sprite = Item.itemImage; 
    }

    public Item ClickItem()
    {
        return this.Item; 
    }

}
