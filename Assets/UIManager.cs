using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject mainMenu, joystick, spawnController;
    public Button startGame, weaponShop, skinShop;


    private void Start()
    {
        startGame.onClick.AddListener(TurnOFfMainMenu);
    }

    private void TurnOFfMainMenu()
    {
        mainMenu.SetActive(false);
        joystick.SetActive(true);
        spawnController.SetActive(true);
    }
}
