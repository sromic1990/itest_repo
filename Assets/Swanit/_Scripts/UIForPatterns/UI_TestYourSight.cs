using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SwanitLib;
using DG.Tweening;


public class UI_TestYourSight : UI_Base
{
    public Text QuestionDisplay;
    public List<AnswerButtonHolder> mButtonHolder;
    public List<BabushkaBox> mBabushka;
    public RectTransform answers;
    public int iteration;

    [SerializeField]
    private List<Vector2> BoxPositions;
    [SerializeField]
    private List<Vector2> defaultPos;
    private int correctIndex;

    public float duration = 1.0f;
    public float offset = 800.0f;

    [SerializeField]
    private bool hasAnsweredQuestion;

    private bool isPaused;

    private bool isUISet = false;

    private int babushkaNo;
    private int iterationCompleted = 0;

    //    void Awake()
    //    {
    //        GameManager.Instance.PauseAction += OnGamePause;
    //        GameManager.Instance.ResumeAction += OnGameResume;
    //    }

    void OnEnable()
    {
        GameManager.Instance.PauseAction += OnGamePause;
        GameManager.Instance.ResumeAction += OnGameResume;
    }

    public override void SetUI(QuestionUIInfo info)
    {        
        base.SetUI(info);

        if (isUISet)
            return;

        isUISet = true;
//        EProz.INSTANCE.cancelDelayCall = false;
        //   Debug.LogError("Game Staus :" + GameManager.Instance.Status.ToString());

        BoxPositions = new List<Vector2>();
        defaultPos = new List<Vector2>();


        //   Debug.LogError(".....Set UI Called.....");

        babushkaNo = info.QuestionData_Int[0];
        duration = info.QuestionData_Float[0];

        for (int i = 0; i < babushkaNo; i++)
        {
            mButtonHolder[i].gameObject.SetActive(true);
            mButtonHolder[i].SetAnswerButtonProperties(info.ButtonAnswer[i]);
            SetCorrectIndex(info.ButtonAnswer[i], i);
            //      Debug.Log("<color=blue>Correct Index = </color>" + correctIndex);
        }

        setAllBabushkaPos();
        setBabushka(correctIndex);
    }

    private void setAllBabushkaPos()
    {
        //    Debug.Log(answers.rect.width);
        float posTect = answers.rect.width - offset;

        //    Debug.Log(babushkaNo);

        for (int i = 0; i < babushkaNo; i++)
        {
            Vector2 xpos = new Vector2(posTect * i / (babushkaNo - 1) + offset * 0.5f, -70.0f);
            mBabushka[i].Babushka_Parent.anchoredPosition = xpos;
            BoxPositions.Add(xpos);
            defaultPos.Add(xpos);
        }
    }

    //TODO babuska animation

    public override void Reset()
    {
        //    Debug.LogError("Reset Calleeedddd");
        mBabushka[correctIndex].Monkey.DOPause();
        mBabushka[correctIndex].Top.DOPause();

        mBabushka[correctIndex].Top.DOKill();
        mBabushka[correctIndex].Monkey.DOKill();

        hasAnsweredQuestion = false;

        iterationCompleted = 0;

        for (int i = 0; i < babushkaNo; i++)
        {
            RectTransform rt = mButtonHolder[i].GetComponent<RectTransform>();
            rt.anchoredPosition = defaultPos[i];
        }
        isUISet = false;
    }

    private void SetCorrectIndex(ButtonProperties bProp, int index)
    {
        if (bProp.ID == AnswerID.Correct)
        {
            correctIndex = index;
        }
    }

    private void setBabushka(int index)
    {
//        Vector2 midPos = mBabushka[1].Babushka_Parent.anchoredPosition;
//        Vector2 swap = mBabushka[index].Babushka_Parent.anchoredPosition;
//
//        mBabushka[index].Babushka_Parent.anchoredPosition = midPos;
//        mBabushka[1].Babushka_Parent.anchoredPosition = swap;

        ReAssignPos();

        AnimateBabushka();
    }


    private void AnimateBabushka()
    {
        //if (GameManager.Instance.Status == GameStatus.InSession)
        //  {
        float val = mBabushka[correctIndex].Top.anchoredPosition.y;

        //    Debug.LogError("//.......Animate Babushkaaaaaa.......");

        EProz.INSTANCE.WaitAndCall(0.5f, () =>
            {
                mBabushka[correctIndex].Top.DOAnchorPosY(val + 400.0f, 0.5f, false);
                //     mBabushka[correctIndex].Monkey.DOScale(2.0f, 0.5f).SetEase(Ease.InBounce);


                EProz.INSTANCE.WaitAndCall(0.8f, () =>
                    {

                        mBabushka[correctIndex].Top.DOAnchorPosY(val, 0.5f, false);
                        //        mBabushka[correctIndex].Monkey.DOScale(1.0f, 0.5f);

                        if (hasAnsweredQuestion)
                            return;

                        EProz.INSTANCE.WaitAndCall(1.0f, () =>
                            {
                                if (GameManager.Instance.Status == GameStatus.InSession)
                                    StartCoroutine(AnimateAll());
                            });
                    });

            });
        //   }
       
    }

    private IEnumerator AnimateAll(int minus = 0)
    {
        //   Debug.Log(iteration - iterationCompleted);

        //  Debug.LogError("Animation Started Again : Iterration = " + (iteration - iterationCompleted).ToString());

        for (int i = 0; i < iteration - minus; i++)
        {
            int x = getRandom(correctIndex);
            int y = getRandom(correctIndex, x);

            int tick = (i == 0) ? 2 : Random.Range(2, 367);
           
            if (tick % 2 == 0)
            {
                bool ClockWise = ((BoxPositions[correctIndex] - BoxPositions[x]).x > 0) ? false : true;
                EProz.INSTANCE.MoveInSpline(mBabushka[correctIndex].Babushka_Parent, BoxPositions[x], ClockWise, duration, 800.0f);
                EProz.INSTANCE.MoveInSpline(mBabushka[x].Babushka_Parent, BoxPositions[correctIndex], !ClockWise, duration, 800.0f); 
            }
            else
            {
                bool ClockWise = ((BoxPositions[y] - BoxPositions[x]).x > 0) ? false : true;
                EProz.INSTANCE.MoveInSpline(mBabushka[y].Babushka_Parent, BoxPositions[x], ClockWise, duration, 800.0f);
                EProz.INSTANCE.MoveInSpline(mBabushka[x].Babushka_Parent, BoxPositions[y], !ClockWise, duration, 800.0f); 
            }

            while (!EProz.INSTANCE.isSplineFinish)
                yield return null;

            iterationCompleted += 1;
            ReAssignPos();
        }

        GameManager.Instance.SetInput();
    }

    private void ReAssignPos()
    {
        for (int i = 0; i < babushkaNo; i++)
            BoxPositions[i] = answers.GetChild(i).GetComponent<RectTransform>().anchoredPosition; 
    }

    private int getRandom(int no, int no2 = -1)
    {
        int number = Random.Range(0, babushkaNo);


        if (number != no && number != no2)
            return number;
        else
            return getRandom(no, no2);
    }

    public override void QuestionAnswered()
    {
        base.QuestionAnswered();
        hasAnsweredQuestion = true;
        AnimateBabushka();
    }

    private void OnGamePause()
    {
        if (gameObject.activeSelf)
        {
            isPaused = true;
            //  Debug.LogError("OnGamePause" + isPaused);
        }
    }

    private void OnGameResume()
    {
        if (isPaused)
        {
            gameObject.SetActive(true);
            EProz.INSTANCE.WaitAndCall(0.05f, () =>
                {
                    StartCoroutine(waitAndAnimate());
                    //     Debug.LogError("On Resume =" + isPaused);
                    isPaused = false;
                });
        }
    }

    void Disable()
    {
        GameManager.Instance.PauseAction -= OnGamePause;
        GameManager.Instance.ResumeAction -= OnGameResume;
    }

    IEnumerator waitAndAnimate()
    {
        while (!EProz.INSTANCE.isSplineFinish)
            yield return null;

        ReAssignPos();
        StartCoroutine(AnimateAll(iterationCompleted + 1));
    }

}

[System.Serializable]
public struct BabushkaBox
{
    public RectTransform Babushka_Parent;
    public RectTransform Top;
    public RectTransform Monkey;
};
