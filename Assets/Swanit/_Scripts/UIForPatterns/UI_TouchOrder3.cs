using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Sourav.Utilities.Extensions;
using SwanitLib;

public class UI_TouchOrder3 : UI_Base
{
    public Text QuestionDisplay;
    public List<AnswerButtonHolder> mButtonHolder;
    public List<RectTransform> mImages;

    public RectTransform Answers;

    private List<Vector2> BoxPosition;
    List<int> order;

    private float appearTime;
    private QuestionUIInfo info;

    private bool isUISet = false;

    private bool IsFirstShown;
    private bool IsSecondShown;
    private bool IsThirdShown;
    private bool IsSwapped;
    private int appearTime1 = 40;
    private int appearTime2 = 40;
    private int appearTime3 = 40;
    private int swapTime = 40;


    void Awake()
    {
        BoxPosition = new List<Vector2>();

        for (int i = 0; i < mImages.Count; i++)
        {
            BoxPosition.Add(mImages[i].anchoredPosition);
            mImages[i].gameObject.SetActive(false);
        }
		
    }

    private void OnDisable()
    {
        GameManager.Instance.TimeTicker += Ticker;
    }

    public override void SetUI(QuestionUIInfo info)
    {
        base.SetUI(info);
        if (isUISet)
            return;

        GameManager.Instance.TimeTicker += Ticker;

        this.info = info;

        appearTime = info.QuestionData_Float[0];
        appearTime1 = (int)info.QuestionData_Float[0] + 1;
        appearTime2 = appearTime1 + (int)info.QuestionData_Float[0] + 1;
        appearTime3 = appearTime2 + (int)info.QuestionData_Float[0] + 1;
        swapTime = appearTime3 + (int)info.QuestionData_Float[0] + 1;
        setOrder();
        GameManager.Instance.CanProcessInput = false;
        isUISet = true;
    }

    private void setOrder()
    {
        order = new List<int>();

        for (int i = 0; i < 3; i++)
            order.Add(i);

        order.Shuffle();
    }

    private void Ticker(int timer)
    {
        if (timer > appearTime1 && !IsFirstShown)
        {
            mImages[0].anchoredPosition = BoxPosition[order[0]];
            mImages[0].gameObject.SetActive(true);
            mImages[0].GetComponent<AnswerButtonHolder>().SetAnswerButtonProperties(info.ButtonAnswer[0]);

            IsFirstShown = true;
        }

        if (timer > appearTime2 && !IsSecondShown)
        {
            mImages[1].anchoredPosition = BoxPosition[order[1]];
            mImages[1].gameObject.SetActive(true);
            mImages[1].GetComponent<AnswerButtonHolder>().SetAnswerButtonProperties(info.ButtonAnswer[1]);

            IsSecondShown = true;
        }

        if (timer > appearTime3 && !IsThirdShown)
        {
            mImages[2].anchoredPosition = BoxPosition[order[2]];
            mImages[2].gameObject.SetActive(true);
            mImages[2].GetComponent<AnswerButtonHolder>().SetAnswerButtonProperties(info.ButtonAnswer[2]);

            IsThirdShown = true;
        }

        if (timer > swapTime && !IsSwapped)
        {
            ReassignPos();

            int no1 = Random.Range(0, 3);
            int no2 = getRandom(no1);
            mImages[no2].DOAnchorPos(BoxPosition[no1], appearTime, false);
            mImages[no1].DOAnchorPos(BoxPosition[no2], appearTime, false);

            IsSwapped = true;
            GameManager.Instance.CanProcessInput = true;
        }
    }

    private int getRandom(int no)
    {
        int number = Random.Range(0, 3);

        if (number != no)
            return number;
        else
            return getRandom(no);
    }

    private void ReassignPos()
    {
        for (int i = 0; i < Answers.childCount; i++)
            BoxPosition[i] = Answers.GetChild(i).GetComponent<RectTransform>().anchoredPosition;
    }

    public override void Reset()
    {
        if (isUISet)
        {
            IsFirstShown = false;
            IsSecondShown = false;
            IsThirdShown = false;
            IsSwapped = false;
            isUISet = false;
            appearTime1 = 40;
            appearTime2 = 40;
            appearTime3 = 40;
            swapTime = 40;
            for (int i = 0; i < mImages.Count; i++)
            {
                mImages[i].gameObject.Hide();
            }
        }
    }

}

public static class IListExtensions
{
    /// <summary>
    /// Shuffles the element order of the specified list.
    /// </summary>
    public static void Shuffle<T>(this IList<T> ts)
    {
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }
}