using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SwanitLib;
using Sourav.Utilities.Extensions;

public class UI_ScareTactic : UI_Base
{
    public GameObject spotDiif;
    public GameObject Skull;
    public GameObject ScareTactic;

    private bool isUISet;

    public override void Reset()
    {
        if (isUISet)
        {
            ScareTactic.Hide();
            Skull.Hide();
            spotDiif.Show();
            isUISet = false;
        }

    }

    public override void SetUI(QuestionUIInfo info)
    {
        if (isUISet)
            return;

        base.SetUI(info);
        isUISet = true;

        EProz.INSTANCE.WaitAndCall(7.0f, () =>
            {
                spotDiif.SetActive(false);
                Skull.SetActive(true);

                EProz.INSTANCE.WaitAndCall(2.0f, () =>
                    {
                        Skull.Hide();
                        ScareTactic.SetActive(true);
                        //UIManager.Instance.ShowSecondaryQuestion();
                        Invoke("Clear", 3.0f);
                    });
            });
    }

    private void Clear()
    {
        Debug.Log("Calling Correct Answer");
        GameManager.Instance.AnsweredCorrectly();
    }
}
