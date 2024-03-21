using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    [SerializeField] Image image; //이미지 컴포넌트를 담을 곳 추가 

    private Item _item;  
    public Item item
    {
        get { return _item; } //슬롯의 아이템 정보를 넘겨줄 때 사용 
        set
        {
            _item = value; //아이템에 들어오는 정보 값은 _item에 저장됨 
            if (_item != null)
            {
                image.sprite = item.itemImage;
                image.color = new Color(1, 1, 1, 1); 
            }
            else
            {
                image.color = new Color(1, 1, 1, 0);
            }
        }
    }
}
