using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button), typeof(CanvasGroup))]
public class UI_Button_Deactivable : MonoBehaviour 
{
    public Button Button;
    public CanvasGroup ButtonGroup;

    public void ActivateButton()
    {
        Button.interactable = true;
        ButtonGroup.alpha = 1.0f;
    }

    public void DeActivateButton()
    {
        Button.interactable = false;
        ButtonGroup.alpha = 0.5f;
    }
}
