using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class UI_Panel_Base : MonoBehaviour 
{
    public Text Question;

    private void OnEnable()
    {
        OnShowPanel();
    }

    private void OnDisable()
    {
        OnHidePanel();
    }

    public virtual void OnShowPanel()
    {
    }

    public virtual void OnHidePanel()
    {
    }

    public virtual void ShowQuestion(string question)
    {
    }
}
