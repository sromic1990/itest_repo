﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SwanitLib;

public class UI_HomoPhone : UI_Base
{
    public Text Quesion;
    public List<AnswerButtonHolder> mButtonHolders;

    public override void Reset()
    {
    }

    public override void SetUI(QuestionUIInfo info)
    {
        base.SetUI(info);
//        EProz.INSTANCE.cancelDelayCall = false;
        // Quesion.text = info.Question;

        for (int i = 0; i < mButtonHolders.Count; i++)
        {
            mButtonHolders[i].SetAnswerButtonProperties(info.ButtonAnswer[i]);
        }
    }
}