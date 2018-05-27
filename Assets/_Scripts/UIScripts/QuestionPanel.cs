using System.Collections;
using System.Collections.Generic;
using Sourav.Utilities.Extensions;
using UnityEngine;

public class QuestionPanel : MonoBehaviour 
{
    //JUGAAD
    private void OnDisable()
    {
        if (GameManager.Instance.Status == GameStatus.InBetweenSession)
            return;

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.Hide();
        }
    }

    public void ResetAllPanels()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if(transform.GetChild(i).gameObject.activeSelf)
            {
                transform.GetChild(i).GetComponent<UI_Base>().Reset();
            }
        }
    }
}
