using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sourav.Utilities.Extensions;
using SwanitLib;

public class UI_XYZseconds : UI_Base
{
    public Text QuestionDisplay;
    public Text Timer;
    public List<AnswerButtonHolder> mButtonHolder;
    public List<Animator> monkeyAnimator = new List<Animator>();
    private List<Vector2> defaultSize = new List<Vector2>();

    public override void SetUI(QuestionUIInfo info)
    {
        base.SetUI(info);

        QuestionDisplay.text = info.Question;

        for (int i = 0; i < mButtonHolder.Count; i++)
        {
            mButtonHolder[i].gameObject.Show();
            mButtonHolder[i].SetAnswerButtonProperties(info.ButtonAnswer[i]);
            monkeyAnimator.Add(mButtonHolder[i].transform.GetChild(0).GetComponent<Animator>());
            monkeyAnimator[i].speed = 0;
            SetSize(mButtonHolder[i]);
        }

        CancelInvoke("animate");
        InvokeRepeating("animate", 0, 2.5f);
    }

    void OnEnable()
    {
        GameManager.Instance.PauseAction += OnGamePause;
        GameManager.Instance.ResumeAction += OnGameResume;
    }

    private void animate()
    {
        int num = Random.Range(0, 3);
      
        Debug.Log("num =" + num);

        monkeyAnimator[num].Play("Jump");
        monkeyAnimator[num].speed = 1;


        EProz.INSTANCE.WaitAndCall(1.0f, () =>
            {
                monkeyAnimator[num].speed = 0;
            });
    }


    private void SetSize(AnswerButtonHolder abh)
    {
//       / RectTransform rt = abh.GetComponent<RectTransform>();
        Transform rt = abh.gameObject.transform;
        defaultSize.Add(rt.localScale);

        Debug.Log(abh.ToString());

        switch (abh.mID)
        {
            case AnswerID.Big_MonkeySanta:
                rt.localScale *= 1.4f;
                break;
            case AnswerID.Medium_MonkeySanta:
                rt.localScale *= 1.2f;
                break;
            case AnswerID.Small_MonkeySanta:
                rt.localScale *= 1.0f;
                break;
        }
    }

    void OnDisable()
    {
        GameManager.Instance.PauseAction -= OnGamePause;
        GameManager.Instance.ResumeAction -= OnGameResume;

        CancelInvoke("animate");
    }

    private void OnGamePause()
    {
        CancelInvoke("animate");
        for (int i = 0; i < monkeyAnimator.Count; i++)
        {
            monkeyAnimator[i].speed = 1;
            monkeyAnimator[i].Play("Jump");
            monkeyAnimator[i].speed = 0;
        }
    }

    private void OnGameResume()
    {
        InvokeRepeating("animate", 0.5f, 2.5f); 
    }

    public override void Reset()
    {
        for (int i = 0; i < mButtonHolder.Count; i++)
        {
            mButtonHolder[i].gameObject.Hide();
            mButtonHolder[i].gameObject.transform.localScale = defaultSize[i];//Vector2.one;
        }
    }
}
