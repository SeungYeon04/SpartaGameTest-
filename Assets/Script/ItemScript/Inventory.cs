using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static Item;

[System.Serializable]
public class ItemSlot
{
    public string itemName; // ������ �̸��� ����
    public int quantity;
    public Item item;

    public ItemSlot(string newItemName, int newQuantity)
    {
        itemName = newItemName;
        quantity = newQuantity;
        item = null; // �ʱ⿡�� null�� ����
    }

    // ������ �߰��ϴ� �޼ҵ�. �ִ�ġ�� ���� �ʵ��� Ȯ��.
    public void AddQuantity(int amount)
    {
        quantity = Mathf.Min(quantity + amount, 99);
    }
}

public class Inventory : MonoBehaviour
{
    // �κ��丮�� ������ ������ ���� ����Ʈ
    public List<ItemSlot> itemSlots = new List<ItemSlot>();

    // �κ��丮�� ������ ������ ����Ʈ
    public List<Item> items;

    // ���Ե��� �θ� ��ü
    [SerializeField]
    private Transform slotParent;

    // ���� �迭
    [SerializeField]
    private Slot[] slots;

#if UNITY_EDITOR
    // ������ �󿡼��� ȣ��Ǵ� �Լ�
    private void OnValidate()
    {
        // ���� �迭�� �θ� ��ü �Ʒ��� ��� Slot ������Ʈ�� �ʱ�ȭ
        slots = slotParent.GetComponentsInChildren<Slot>();
    }
#endif
    void Awake()
    {
        // �κ��丮 �ʱ�ȭ �Լ� ȣ��
        FreshSlot();
        GameManager.Instance.LoadInventory(); // ���� �Ŵ����� ���� �κ��丮 �ε�
    }

    public int GetItemsCount()
    {
        return items.Count;
    }

    // �������� �߰��ϴ� �Լ�
    public void AddItem(Item itemToAdd)
    {
        // �ش� �������� ���� ������ ã��
        ItemSlot slot = itemSlots.Find(s => s.itemName == itemToAdd.itemName);

        if (slot != null)
        {
            // ������ �̹� ������, ������ ����
            slot.AddQuantity(1);
        }
        else
        {
            // �� ������ �߰�
            if (itemSlots.Count < slots.Length)
            {
                itemSlots.Add(new ItemSlot(itemToAdd.itemName, 1));
            }
            else
            {
                Debug.LogWarning("������ ���� �� �ֽ��ϴ�.");
                return;
            }
        }

        FreshSlot();
        GameManager.Instance.SaveInventory(); // ���� �Ŵ����� ���� �κ��丮 ����
    }

    public void FreshSlot()
    {
        // ��� ������ ��ȸ�ϸ鼭, �� ���Կ� �����ϴ� ������ ���� ������ ������Ʈ�մϴ�.
        // itemSlots ����Ʈ�� ũ�⸦ �Ѿ�� ������ �������� ���ϴ�.
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < itemSlots.Count)
            {
                var slot = itemSlots[i];
                slot.item = FindItemByName(slot.itemName); // itemName�� ������� item ��ü ����
                slots[i].UpdateSlot(slot.item, slot.quantity);
            }
            else
            {
                slots[i].ClearSlot(); // ������ ���� ������ ������ �޼���� �и�
            }
        }
    }

    public Item FindItemByName(string itemName)
    {
        return items.Find(item => item.itemName == itemName);
    }

    //Inventory ��ũ��Ʈ ���� ��带 ��Ÿ���� �������� ������ �߰��մϴ�.
    //�� ������ �κ��丮�� ���� ������ ��� �������, �Ǹ� ��������� ��Ÿ���ϴ� 

    public enum InventoryMode
    {
        UseItem,
        SellItem
    }

    public InventoryMode currentMode = InventoryMode.UseItem;

    //�Ǹ� ��ư�� ������ �� currentMode�� SellItem���� ��ȯ�ϰ�, �κ��丮�� �ݾҴٰ� �ٽ� �� ���� �⺻������ UseItem ���� ���ư����� �մϴ�.
    //�̸� ���� �Ǹ� ��ư�� ������ �޼ҵ带 Inventory ��ũ��Ʈ�� �߰��մϴ�

    // �Ǹ� ���� ��ȯ
    public void EnableSaleMode()
    {
        currentMode = InventoryMode.SellItem;
    }

    // ������ ��� ���� ��ȯ (�⺻ ���)
    public void EnableUseItemMode()
    {
        currentMode = InventoryMode.UseItem;
    }

    public void SellItem(Item item, int quantity)
    {
        if (item == null)
        {
            Debug.LogError("�Ǹ��Ϸ��� �������� null�Դϴ�.");
            return;
        }
        if (PlayerStack.Instance == null)
        {
            Debug.LogError("PlayerStack.Instance�� null�Դϴ�.");
            return;
        }

        for (int i = 0; i < itemSlots.Count; i++)
        {
            if (itemSlots[i].item == item && itemSlots[i].quantity >= quantity)
            {
                itemSlots[i].quantity -= quantity;
                PlayerStack.Instance.AddMoney(item.value * quantity);
                Debug.Log($"{item.itemName} {quantity} ������ {item.value * quantity} �ǸſϷ�");

                if (itemSlots[i].quantity <= 0)
                {
                    itemSlots.RemoveAt(i);
                }
                FreshSlot();
                GameManager.Instance.SaveInventory();
                break;
            }
        }

    }

    public void UseItem(Item item, int quantity)
    {

    }

    public ItemType sellableItemType = ItemType.Fish; // �⺻������ ������ �Ǹ� ����

    // �Ǹ� ��� Ȱ��ȭ �޼ҵ带 ������ Ÿ���� ���ڷ� �޵��� Ȯ��
    public void EnableSaleMode(ItemType itemType)
    {
        currentMode = InventoryMode.SellItem;
        sellableItemType = itemType; // �Ǹ� ������ ������ Ÿ�� ����
    }
}

[System.Serializable]
public class InventoryData
{
    public List<ItemSlot> itemSlots;

    public InventoryData(List<ItemSlot> itemSlots)
    {
        this.itemSlots = itemSlots;
    }
}