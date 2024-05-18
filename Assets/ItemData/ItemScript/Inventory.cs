using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Item;

[System.Serializable]
public class ItemSlot
{
    public Item item;
    public int quantity;

    public ItemSlot(Item newItem, int newQuantity)
    {
        item = newItem;
        quantity = newQuantity;
    }

    // 수량을 추가하는 메소드. 최대치를 넘지 않도록 확인.
    public void AddQuantity(int amount)
    {
        quantity = Mathf.Min(quantity + amount, 99);
    }
}

public class Inventory : MonoBehaviour
{
    // 인벤토리에 보유한 아이템 슬롯 리스트
    public List<ItemSlot> itemSlots = new List<ItemSlot>();

    // 인벤토리에 보유한 아이템 리스트
    public List<Item> items;

    // 슬롯들의 부모 객체
    [SerializeField]
    private Transform slotParent;

    // 슬롯 배열
    [SerializeField]
    private Slot[] slots;

#if UNITY_EDITOR
    // 에디터 상에서만 호출되는 함수
    private void OnValidate()
    {
        // 슬롯 배열을 부모 객체 아래의 모든 Slot 컴포넌트로 초기화
        slots = slotParent.GetComponentsInChildren<Slot>();
    }
     #endif

    void Awake()
    {
        // 인벤토리 초기화 함수 호출
        FreshSlot(); 
    }

    // 아이템을 추가하는 함수
    public void AddItem(Item itemToAdd)
    {
        // 해당 아이템을 가진 슬롯을 찾음
        ItemSlot slot = itemSlots.Find(s => s.item == itemToAdd);

        if (slot != null)
        {
            // 슬롯이 이미 있으면, 수량을 증가
            slot.AddQuantity(1);
        }
        else
        {
            // 새 슬롯을 추가
            if (itemSlots.Count < slots.Length)
            {
                itemSlots.Add(new ItemSlot(itemToAdd, 1));
            }
            else
            {
                Debug.LogWarning("슬롯이 가득 차 있습니다.");
                return;
            }
        }

        FreshSlot();
    }
    public void FreshSlot()
    {
        // 모든 슬롯을 순회하면서, 각 슬롯에 대응하는 아이템 슬롯 정보를 업데이트합니다.
        // itemSlots 리스트의 크기를 넘어서는 슬롯은 아이템을 비웁니다.
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < itemSlots.Count)
            {
                var slot = itemSlots[i];
                slots[i].UpdateSlot(slot.item, slot.quantity);
            }
            else
            {
                slots[i].ClearSlot(); // 아이템 비우는 로직을 별도의 메서드로 분리
            }
        }
    }


    //Inventory 스크립트 내에 모드를 나타내는 열거형과 변수를 추가합니다.
    //이 변수는 인벤토리가 현재 아이템 사용 모드인지, 판매 모드인지를 나타냅니다 

    public enum InventoryMode
    {
        UseItem,
        SellItem
    }

    public InventoryMode currentMode = InventoryMode.UseItem;



    //판매 버튼을 눌렀을 때 currentMode를 SellItem으로 전환하고, 인벤토리를 닫았다가 다시 열 때는 기본적으로 UseItem 모드로 돌아가도록 합니다.
    //이를 위해 판매 버튼에 연결할 메소드를 Inventory 스크립트에 추가합니다

    // 판매 모드로 전환
    public void EnableSaleMode()
    {
        currentMode = InventoryMode.SellItem;
    }

    // 아이템 사용 모드로 전환 (기본 모드)
    public void EnableUseItemMode()
    {
        currentMode = InventoryMode.UseItem;
    } 

    public void SellItem(Item item, int quantity)
    {
        if (item == null)
        {
            Debug.LogError("판매하려는 아이템이 null입니다.");
            return;
        }
        if (PlayerStack.Instance == null)
        {
            Debug.LogError("PlayerStack.Instance가 null입니다.");
            return;
        }

        // 아이템 슬롯 관리를 위한 해시 테이블
        var itemSlotDictionary = new Dictionary<Item, ItemSlot>();
        foreach (var itemSlot in itemSlots)
        {
            itemSlotDictionary[itemSlot.item] = itemSlot;
        }

        // 판매하려는 아이템 슬롯 가져오기
        if (!itemSlotDictionary.TryGetValue(item, out var targetSlot))
        {
            Debug.LogError("판매하려는 아이템을 찾을 수 없습니다.");
            return;
        }
    }

    public ItemType sellableItemType = ItemType.Fish; // 기본적으로 생선만 판매 가능

    // 판매 모드 활성화 메소드를 아이템 타입을 인자로 받도록 확장
    public void EnableSaleMode(ItemType itemType)
    {
        currentMode = InventoryMode.SellItem;
        sellableItemType = itemType; // 판매 가능한 아이템 타입 설정
    }


}



