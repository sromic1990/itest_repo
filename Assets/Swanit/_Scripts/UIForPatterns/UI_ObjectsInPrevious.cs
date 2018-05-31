using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SwanitLib;
using Sourav.Utilities.Extensions;

public class UI_ObjectsInPrevious : UI_Base
{
    public Text QuestionDisplay;
    public List<Image> QuestionImages;
    public List<AnswerButtonHolder> mButtonHolder;
    public RectTransform QimageParent;
    public RectTransform AnswerParent;
    public float displayTime;
    // = 5.0f;

    private QuestionUIInfo info;
    private bool hasQuestionStarted = false;
    private int showAfterSeconds = 40;

    private bool isUISet = false;

    public void OnDisable()
    {
        GameManager.Instance.TimeTicker += TimerTick;
    }

    public override void SetUI(QuestionUIInfo info)
    {
        if (isUISet)
            return;
        
        AnswerParent.gameObject.Hide();
        base.SetUI(info);
        this.info = info;
        GameManager.Instance.TimeTicker += TimerTick;


        BaseQuestion question = GameManager.Instance.GetCurrentQuestion();


        hasQuestionStarted = true;
        displayTime = info.QuestionData_Float[0];
        showAfterSeconds = GameManager.Instance.timerInt + (int)displayTime;

        for (int i = 0; i < question.QuestionSprite.Count; i++)
            QuestionImages[i].sprite = GetCorrectSpriteByID.Instance.GetSpriteFromID(question.QuestionSprite[i].Sprite);

        QimageParent.gameObject.Show();
        isUISet = true;


        //EProz.INSTANCE.WaitAndCall(displayTime, () =>
        //{
        //QimageParent.gameObject.SetActive(false);
        ////QuestionDisplay.text = info.SecondaryQuestion [0];
        //
        //UIManager.Instance.ShowSecondaryQuestion();
        //AnswerParent.gameObject.SetActive(true);
        ////    UIManager.Instance.ShowSecondaryQuestion();
        //
        //for (int i = 0; i < mButtonHolder.Count; i++)
        //{
        //    mButtonHolder[i].SetAnswerButtonProperties(info.ButtonAnswer[i]);
        //}
        //
        //});
    }

    public void TimerTick(int timer)
    {
        if (timer > showAfterSeconds && hasQuestionStarted)
        {
            hasQuestionStarted = false;
            QimageParent.gameObject.Hide();

            UIManager.Instance.ShowSecondaryQuestion();
            AnswerParent.gameObject.Show();
            //    UIManager.Instance.ShowSecondaryQuestion();

            for (int i = 0; i < mButtonHolder.Count; i++)
            {
                mButtonHolder[i].SetAnswerButtonProperties(info.ButtonAnswer[i]);
            }


        }
    }

    public override void Reset()
    {
        if (isUISet)
        {
            AnswerParent.gameObject.Hide();
            QimageParent.gameObject.Show();
            isUISet = false;
            hasQuestionStarted = false;
            showAfterSeconds = 40;
        }


    }
}
