using System.Collections;
using System.Collections.Generic;
using Sourav.Utilities.Extensions;
using UnityEngine;

public class UI_BasePanel : MonoBehaviour 
{
    public GameObject LifeButton;
    public GameObject BananaButton;

    public void GameModeChanged(GameMode mode)
    {
        switch(mode)
        {
            case GameMode.SinglePlayer:
                LifeButton.Show();
                BananaButton.Hide();
                break;

            case GameMode.Multiplayer:
                LifeButton.Hide();
                BananaButton.Show();
                break;
                
        }
    }
}
