using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SwanitLib;
using UnityEngine.UI;

public class UI_BounceMonkey : UI_Base
{
    public float shrink;
    public RectTransform monkey;
    private List<Vector2> randomPos;
    private AnswerButtonHolder mAnswerHolder;
    public Button Clicked;

    private bool isCorrect = false;

    private Animator MonkeyAnim;

    void Start()
    {
    }

    //    IEnumerator Monkey_poscheck()
    //    {
    //        for (int i = 0; i < 10; i++)
    //        {
    //            monkey.anchoredPosition = randomPos[i];
    //            yield return new WaitForSeconds(0.7f);
    //        }
    //    }

    public override void SetUI(QuestionUIInfo info)
    {
        InitMonkey();
        base.SetUI(info);
        //mAnswerHolder.SetAnswerButtonProperties(info.ButtonAnswer[0], true);
//        MonkeyAnim.speed = 0;
        // StartCoroutine(Monkey_Jump());
    }

    void InitMonkey()
    {
        randomPos = new List<Vector2>();
        mAnswerHolder = monkey.GetComponent<AnswerButtonHolder>();
        MonkeyAnim = monkey.GetChild(0).gameObject.GetComponent<Animator>();
        float width = GetComponent<RectTransform>().rect.width - shrink;
        for (int i = 0; i < 10; i++)
        {
            Vector2 vecPos = new Vector2(width * i / 9 + shrink * 0.5f, monkey.anchoredPosition.y);
            randomPos.Add(vecPos);
        }
        Clicked.onClick.AddListener(OnClicked);
        StartCoroutine(Monkey_Jump());
    }

    private int pos = -1;

    private IEnumerator Monkey_Jump()
    {
        pos = getRandom(pos);
        bool clockWise = ((randomPos[pos] - monkey.anchoredPosition).x > 0) ? true : false;
        EProz.INSTANCE.MoveInSpline(monkey, randomPos[pos], clockWise, 1f);
        MonkeyAnim.speed = 0.7f;
        //      MonkeyAnim.Play("Jump");
        mAnswerHolder.CorrectSet = true;
        isCorrect = true;
        yield return new WaitForSeconds(1.5f);
        mAnswerHolder.CorrectSet = false;
        isCorrect = false;
        monkey.gameObject.SetActive(false);
        //      yield return new WaitForEndOfFrame();
        monkey.gameObject.SetActive(true);
        MonkeyAnim.speed = 0;
        int wait = Random.Range(2, 9);
        yield return new WaitForSeconds(wait);
        StartCoroutine(Monkey_Jump());

    }

    private int getRandom(int no)
    {
        int number = Random.Range(0, 9);

        if (number != no)
            return number;
        else
            return getRandom(no);
    }

    public void OnClicked()
    {
        Debug.Log("OnClicked");

        if (isCorrect)
        {
            GameManager.Instance.AnsweredCorrectly();
        }
        else
        {
            GameManager.Instance.AnsweredWrongly();
        }
    }

    void OnDisable()
    {
        StopCoroutine(Monkey_Jump());
    }

    public override void Reset()
    {
    }
}
