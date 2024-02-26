using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIShopElement : MonoBehaviour
{
    public int id;
    public int cost;
    public Text costText;
    public Button purchaseButton;

    public void Awake()
    {
        purchaseButton.onClick.AddListener(OnPurchase);
    }

    private void UpdateView()
    {
        costText.text = cost.ToString();
    }

    public void OnPurchase()
    {
        Debug.Log("purchase sucess");
    }
}
