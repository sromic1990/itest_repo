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

    void Awake()
    {
        BoxPosition = new List<Vector2>();

        for (int i = 0; i < mImages.Count; i++)
        {
            BoxPosition.Add(mImages[i].anchoredPosition);
            mImages[i].gameObject.SetActive(false);
        }
		
    }

    void Start()
    {

    }

    public override void SetUI(QuestionUIInfo info)
    {
        base.SetUI(info);

        this.info = info;

        appearTime = info.QuestionData_Float[0];
        //  QuestionDisplay.text = this.info.Question;

        //for (int i = 0; i < mButtonHolder.Count; i++) 
        //      {
        //	mButtonHolder [i].SetAnswerButtonProperties (info.ButtonAnswer [i]);
        //}
        setOrder();
    }

    private void setOrder()
    {
        order = new List<int>();

        for (int i = 0; i < 3; i++)
            order.Add(i);

        order.Shuffle();
        // StartCoroutine(showImages());
        showImages();
    }

    private void showImages()
    {
//        for (int i = 0; i < mImages.Count; i++)
//        {
//            yield return new WaitForSeconds(appearTime);
//            mImages[i].anchoredPosition = BoxPosition[order[i]];
//            mImages[i].gameObject.SetActive(true);
//            mImages[i].GetComponent<AnswerButtonHolder>().SetAnswerButtonProperties(info.ButtonAnswer[i]);
//        }

        EProz.INSTANCE.WaitAndCall(appearTime, () =>
            {
                mImages[0].anchoredPosition = BoxPosition[order[0]];
                mImages[0].gameObject.SetActive(true);
                mImages[0].GetComponent<AnswerButtonHolder>().SetAnswerButtonProperties(info.ButtonAnswer[0]);

                EProz.INSTANCE.WaitAndCall(appearTime, () =>
                    {
                        mImages[1].anchoredPosition = BoxPosition[order[1]];
                        mImages[1].gameObject.SetActive(true);
                        mImages[1].GetComponent<AnswerButtonHolder>().SetAnswerButtonProperties(info.ButtonAnswer[1]);

                        EProz.INSTANCE.WaitAndCall(appearTime, () =>
                            {
                                mImages[2].anchoredPosition = BoxPosition[order[2]];
                                mImages[2].gameObject.SetActive(true);
                                mImages[2].GetComponent<AnswerButtonHolder>().SetAnswerButtonProperties(info.ButtonAnswer[2]);

                            });
                    });

            });


        EProz.INSTANCE.WaitAndCall(appearTime * mImages.Count + 0.5f, () =>
            {
                ReassignPos();

                int no1 = Random.Range(0, 3);
                int no2 = getRandom(no1);
                mImages[no2].DOAnchorPos(BoxPosition[no1], appearTime, false);
                mImages[no1].DOAnchorPos(BoxPosition[no2], appearTime, false);
            });

      
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
        for (int i = 0; i < mImages.Count; i++)
        {
            mImages[i].gameObject.Hide();
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