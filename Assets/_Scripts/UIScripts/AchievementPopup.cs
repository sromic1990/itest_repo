using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementPopup : MonoBehaviour 
{
    public Image AchievementImage;
    public Text AchievementText;

    public void Setup_AchievementPopup(Sprite AchievementImage, string AchievementText)
    {
        this.AchievementImage.sprite = AchievementImage;
        this.AchievementText.text = AchievementText;
    }
}
