using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    [Header("인벤토리")]
    public Inventory inventory;

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero); 

            if(hit.collider != null)
            {
                HitCheckObject(hit);   
            }
        }
        void HitCheckObject(RaycastHit2D hit)
        {
            IObjectItem clickInetrface = hit.transform.gameObject.GetComponent<IObjectItem>(); 

            if(clickInetrface != null) 
            {
                Item item = clickInetrface.ClickItem();
                print($"{item.itemName}"); 
                inventory.AddItem(item); 
            }
        }
    }
}
