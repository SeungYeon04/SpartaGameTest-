using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Item> items;

    [SerializeField]
    private Transform slotParent;

    [SerializeField]
    private Slot[] slots;

    #if UNITY_EDITOR
    private void OnValidate()
    {
        slots = slotParent.GetComponentsInChildren<Slot>(); 
    }

#endif

    void Awake()
    {
        FreshSlot(); 
    }

    public void FreshSlot() //템 들어오거나 나가면 다시 정리해서 보여줌
    {
        int i = 0; 
        for(; i < items.Count && i < slots.Length; i++)
        {
            slots[i].item = items[i];   
        }
        for(; i < slots.Length; i++)
        {
            slots[i].item = null;
        }
    } 

    public void AddItem(Item _item)
    {
        if(items.Count < slots.Length)
        {
            items.Add(_item);
            FreshSlot(); 
        }
        else
        {
            print("슬롯이 가득 차 있습니다. ");
        }
    }
}
