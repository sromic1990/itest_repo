using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using SwanitLib;


public class UI_TouchOrder5 : UI_Base
{
    public Text QuestionDisplay;
    public List<AnswerButtonHolder> mButtonHolder;
    public List<RectTransform> mImages;

    public RectTransform Answers;

    private List<Vector2> BoxPosition;
    private float appearTime;
    List<int> order;

    private QuestionUIInfo info;

    private bool isUISet = false;
    private bool HasFirstAppeared = false;
    private bool HasSecondAppeared = false;
    private bool HasThirdAppeared = false;
    private bool HasFourthAppeared = false;
    private bool HasFiveAppeared = false;
    private bool HasSwapped = false;
    private int appearTime1 = 40;
    private int appearTime2 = 40;
    private int appearTime3 = 40;
    private int appearTime4 = 40;
    private int appearTime5 = 40;
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


    public override void SetUI(QuestionUIInfo info)
    {
        base.SetUI(info);

        if (isUISet)
            return;

        GameManager.Instance.TimeTicker += Ticker;

        this.info = info;
        QuestionDisplay.text = this.info.Question;
        appearTime = info.QuestionData_Float[0];
        appearTime1 = (int)info.QuestionData_Float[0] + 1;
        appearTime2 = appearTime1 + (int)info.QuestionData_Float[0] + 1;
        appearTime3 = appearTime2 + (int)info.QuestionData_Float[0] + 1;
        appearTime4 = appearTime3 + (int)info.QuestionData_Float[0] + 1;
        appearTime5 = appearTime4 + (int)info.QuestionData_Float[0] + 1;
        swapTime = appearTime5 + (int)info.QuestionData_Float[0] + 1;
        setOrder();
        GameManager.Instance.CanProcessInput = false;
        isUISet = true;

    }

    private void setOrder()
    {
        order = new List<int>();

        for (int i = 0; i < 5; i++)
            order.Add(i);

        order.Shuffle();
        //StartCoroutine(showImages());
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

        //EProz.INSTANCE.WaitAndCall(appearTime, () =>
        //    {
        //        mImages[0].anchoredPosition = BoxPosition[order[0]];
        //        mImages[0].gameObject.SetActive(true);
        //        mImages[0].GetComponent<AnswerButtonHolder>().SetAnswerButtonProperties(info.ButtonAnswer[0]);

        //        EProz.INSTANCE.WaitAndCall(appearTime, () =>
        //            {
        //                mImages[1].anchoredPosition = BoxPosition[order[1]];
        //                mImages[1].gameObject.SetActive(true);
        //                mImages[1].GetComponent<AnswerButtonHolder>().SetAnswerButtonProperties(info.ButtonAnswer[1]);

        //                EProz.INSTANCE.WaitAndCall(appearTime, () =>
        //                    {
        //                        mImages[2].anchoredPosition = BoxPosition[order[2]];
        //                        mImages[2].gameObject.SetActive(true);
        //                        mImages[2].GetComponent<AnswerButtonHolder>().SetAnswerButtonProperties(info.ButtonAnswer[2]);

        //                        EProz.INSTANCE.WaitAndCall(appearTime, () =>
        //                            {
        //                                mImages[3].anchoredPosition = BoxPosition[order[3]];
        //                                mImages[3].gameObject.SetActive(true);
        //                                mImages[3].GetComponent<AnswerButtonHolder>().SetAnswerButtonProperties(info.ButtonAnswer[3]);

        //                                EProz.INSTANCE.WaitAndCall(appearTime, () =>
        //                                    {
        //                                        mImages[4].anchoredPosition = BoxPosition[order[4]];
        //                                        mImages[4].gameObject.SetActive(true);
        //                                        mImages[4].GetComponent<AnswerButtonHolder>().SetAnswerButtonProperties(info.ButtonAnswer[4]);
        //                                    });
        //                            });

        //                    });
        //            });

        //    });
        


        //EProz.INSTANCE.WaitAndCall(appearTime * mImages.Count + 0.5f, () =>
            //{
            //    ReassignPos();

            //    int no1 = Random.Range(0, 5);
            //    int no2 = getRandom(no1);
            //    mImages[no2].DOAnchorPos(BoxPosition[no1], appearTime, false);
            //    mImages[no1].DOAnchorPos(BoxPosition[no2], appearTime, false);
            //});
        
    }

    private void Ticker(int timer)
    {
        if (timer > appearTime1 && !HasFirstAppeared)
        {
            mImages[0].anchoredPosition = BoxPosition[order[0]];
            mImages[0].gameObject.SetActive(true);
            mImages[0].GetComponent<AnswerButtonHolder>().SetAnswerButtonProperties(info.ButtonAnswer[0]);

            HasFirstAppeared = true;
        }

        if (timer > appearTime2 && !HasSecondAppeared)
        {
            mImages[1].anchoredPosition = BoxPosition[order[1]];
            mImages[1].gameObject.SetActive(true);
            mImages[1].GetComponent<AnswerButtonHolder>().SetAnswerButtonProperties(info.ButtonAnswer[1]);

            HasSecondAppeared = true;
        }

        if (timer > appearTime3 && !HasThirdAppeared)
        {
            mImages[2].anchoredPosition = BoxPosition[order[2]];
            mImages[2].gameObject.SetActive(true);
            mImages[2].GetComponent<AnswerButtonHolder>().SetAnswerButtonProperties(info.ButtonAnswer[2]);

            HasThirdAppeared = true;
        }

        if (timer > appearTime4 && !HasFourthAppeared)
        {
            mImages[3].anchoredPosition = BoxPosition[order[3]];
            mImages[3].gameObject.SetActive(true);
            mImages[3].GetComponent<AnswerButtonHolder>().SetAnswerButtonProperties(info.ButtonAnswer[3]);

            HasFourthAppeared = true;
        }

        if(timer > appearTime5 && !HasFiveAppeared)
        {
            mImages[4].anchoredPosition = BoxPosition[order[4]];
            mImages[4].gameObject.SetActive(true);
            mImages[4].GetComponent<AnswerButtonHolder>().SetAnswerButtonProperties(info.ButtonAnswer[4]);

            HasFiveAppeared = true;
        }

        if (timer > swapTime && !HasSwapped)
        {
            ReassignPos();

            int no1 = Random.Range(0, 5);
            int no2 = getRandom(no1);
            mImages[no2].DOAnchorPos(BoxPosition[no1], appearTime, false);
            mImages[no1].DOAnchorPos(BoxPosition[no2], appearTime, false);

            HasSwapped = true;
            GameManager.Instance.CanProcessInput = true;
        }
    }

    private int getRandom(int no)
    {
        int number = Random.Range(0, 5);

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
        isUISet = false;
        HasFirstAppeared = false;
        HasSecondAppeared = false;
        HasThirdAppeared = false;
        HasFourthAppeared = false;
        HasFiveAppeared = false;
        HasSwapped = false;
        appearTime1 = 40;
        appearTime2 = 40;
        appearTime3 = 40;
        appearTime4 = 40;
        appearTime5 = 40;
        swapTime = 40;
        for (int i = 0; i < mImages.Count; i++)
        {
            mImages[i].gameObject.SetActive(false);
        }
    }
}