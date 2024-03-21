using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

    public GameObject MenuScrin;
    public GameObject MyStatsScrin;
    public GameObject InventoryScrin; 

    //메뉴 터치와 창 닫기 

    public void MenuBtn()
    {
        MenuScrin.SetActive(true);
    }

    public void MenuBackBtn()
    {
        MenuScrin.SetActive(false);
    }

    public void BackStats()
    {
        MyStatsScrin.SetActive(false);
    }

    //메뉴 내 버튼들 

    public void MenuMyStats()
    {
        MenuScrin.SetActive(false);
        MyStatsScrin.SetActive(true);
    }

    public void MenuATM()
    {
        SceneManager.LoadScene("ATM");
    }

    public void InventoryOpen()
    {
        InventoryScrin.SetActive(true); 
    }

    public void InventoryBack()
    {
        InventoryScrin.SetActive(false);
    }
}
