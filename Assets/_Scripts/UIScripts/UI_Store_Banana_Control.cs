using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Store_Banana_Control : MonoBehaviour
{
    public List<Vector2> Positions;

    public Button PrevButton;
    public Button NextButton;
    public RectTransform StoreItemsHolder;

    private int posIndex;

    private void OnEnable()
    {
        OnPrevButton();
    }

    private void OnDisable()
    {
        OnPrevButton();
    }

    private void SetPosIndex(int index)
    {
        posIndex = index;

        if(posIndex < Positions.Count)
        {
            SetPosition();
        }
    }

    private void SetPosition()
    {
        StoreItemsHolder.localPosition = Positions[posIndex];
        if(posIndex > 0)
        {
            NextButton.interactable = false;
            PrevButton.interactable = true;
        }
        else
        {
            NextButton.interactable = true;
            PrevButton.interactable = false;
        }
    }

    public void OnPrevButton()
    {
        SetPosIndex(0);
    }

    public void OnNextButton()
    {
        SetPosIndex(1);
    }
}