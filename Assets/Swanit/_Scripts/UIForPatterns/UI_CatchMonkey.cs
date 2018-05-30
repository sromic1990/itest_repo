using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sourav.Utilities.Scripts.Attributes;
using DG.Tweening;

public class UI_CatchMonkey : UI_Base
{
    public Text QuestionDisplay;
    public Image QuestionImage;
    public RectTransform detector, _pencil;
    public bool isCorrect;
    public AnswerButtonHolder mAnswerButton;

    private int xScale = 1;

    public float LowerLimit;
    public float UpperLimit;


    private float InitStart = 30.0f;
    private float InitEnd = 2265.0f;

    private float start = 30.0f;
    private float end = 2265.0f;
 
    private float MonkeySpeed;
    private bool tapeClicked = false;

    private float tVal = 0;
    private float t = 0;

    private float Ypos = -314.0f;
    //  private float Xpos = 30;

    [SerializeField]
    private Vector2 pencilPos;

    //	public void Start ()
    //	{
    //		//	CatchMonkey_UIInfo uinfo = new CatchMonkey_UIInfo ("Hiii", 3.0f, 5.0f);
    ////		SetUI (uinfo);
    //}

    void Awake()
    {
        pencilPos = _pencil.anchoredPosition;
    }

    public override void QuestionAnswered()
    {
        base.QuestionAnswered();
        RectTransform r = QuestionImage.GetComponent<RectTransform>();
        tapeClicked = true;
        StopCoroutine(MoveMonkey());
        StopAllCoroutines();

        Debug.Log(r.anchoredPosition.x + (r.rect.width * 0.5f));

        int x = (r.pivot.x == 0) ? 1 : -1;

        _pencil.DOAnchorPos(r.anchoredPosition + new Vector2(r.rect.width * 0.5f, 700f), 0.7f, false);
        //   EProz.INSTANCE.MoveInSpline(_pencil, r.anchoredPosition + new Vector2(r.rect.width * 0.5f, 700), false, 0.2f, 100);
        _pencil.DOLocalRotate(new Vector3(0, 0, -180), 0.5f);


//        QuestionImage.GetComponent<DOTweenAnimation>().DOPause();
    }


    public override void Reset()
    {
        Debug.Log("<color=#9BFF00>Reset Called......</color>");
        tapeClicked = false;
//
//        EProz.INSTANCE.WaitAndCall(3.0f, () =>
//            {
        QuestionImage.rectTransform.anchoredPosition = new Vector2(InitStart, Ypos);
        QuestionImage.rectTransform.pivot = new Vector2(0, 0.5f);
        QuestionImage.rectTransform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        xScale = 1;

        start = InitStart;
        end = InitEnd;
        tVal = t = 0;
        _pencil.DOKill(false);
        _pencil.anchoredPosition = pencilPos;
        _pencil.rotation = Quaternion.Euler(0, 0, 0);
//            });
    }

    void OnEnable()
    {
        GameManager.Instance.ResumeAction += OnGameResume;
        GameManager.Instance.PauseAction += OnGamePause;
    }

    void OnDisable()
    {
        GameManager.Instance.PauseAction -= OnGamePause;
        GameManager.Instance.ResumeAction -= OnGameResume;
    }


    void OnGamePause()
    {
        Debug.Log("Paused");
        StopAllCoroutines();
    }

    void OnGameResume()
    {
        Debug.Log("Resume");
        StartCoroutine(MoveMonkey());
        StartCoroutine(DetectMonkey());
    }

    public override void SetUI(QuestionUIInfo info)
    {
        base.SetUI(info);


        //Debug.LogError("Class :" + this.name);
        //   QuestionDisplay.text = info.Question;

//        EProz.INSTANCE.cancelDelayCall = false;

        Debug.Log("Number = " + GameManager.Instance.GetCurrentQuestion().QuestionData_Float[0]);
        LowerLimit = GameManager.Instance.GetCurrentQuestion().QuestionData_Float[0]; // First element contains lower Limit
        Debug.Log("LowerLimit = " + LowerLimit);
        UpperLimit = GameManager.Instance.GetCurrentQuestion().QuestionData_Float[1]; // Second element contains Upper Limit
        MonkeySpeed = GameManager.Instance.GetCurrentQuestion().QuestionData_Float[2]; // Third element is speed duration

        setDetector();

        //Debug.Log("L limit = " + LowerLimit + "\tU Limit = " + UpperLimit);

        mAnswerButton.SetAnswerButtonProperties(info.ButtonAnswer[0]);

        StartCoroutine(MoveMonkey());
        StartCoroutine(DetectMonkey());

    }

    private IEnumerator MoveMonkey()
    {
        yield return new WaitForSeconds(0.5f);

        while (!tapeClicked)
        {
            t = 0;
            t += tVal;
            while (t < 1 && !tapeClicked)
            {
                t += Time.deltaTime / MonkeySpeed;
                tVal = t;
                float x = Mathf.Lerp(start, end, t);
                QuestionImage.rectTransform.anchoredPosition = new Vector2(x, Ypos);
                yield return null;
            }

            start = start + end;
            end = start - end;
            start = start - end;
            MonkeyReachedEnd();
            tVal = 0;
            yield return null;
            //new WaitForEndOfFrame();
        }

    }

    IEnumerator DetectMonkey()
    {
        yield return new WaitForEndOfFrame();
        RectTransform Qrect = QuestionImage.GetComponent<RectTransform>();

        Rect dectRect = new Rect(detector.anchoredPosition.x, detector.anchoredPosition.y, detector.rect.width, detector.rect.height);
        yield return new WaitForEndOfFrame();

        while (true)
        {
            float xPos = Qrect.anchoredPosition.x;
            bool b1 = (xPos >= dectRect.xMin && xPos <= dectRect.xMax) ? true : false;
            bool b2 = (xPos - Qrect.rect.width * 0.5f >= dectRect.xMin && xPos - Qrect.rect.width * 0.5f <= dectRect.xMax);

            bool temp = isCorrect;

            isCorrect = (b1 || b2);

            if (temp != isCorrect)
            {
                //Setting whether the bool is correct or not
                GameManager.Instance.GetCurrentQuestion().ReturnValue_Bool = isCorrect;
            }

            yield return null;
        }
    }


    public void setDetector()
    {
        resetDetector();

        float normalizePos = Mathf.InverseLerp(0, 9, LowerLimit);
        float normalizeWidth = Mathf.InverseLerp(0, 9, UpperLimit);

        float pos = Mathf.Lerp(0, detector.rect.width, normalizePos);
        float width = Mathf.Lerp(0, detector.rect.width, normalizeWidth);


        detector.anchoredPosition = new Vector2(pos, detector.anchoredPosition.y);
        detector.sizeDelta = new Vector2(width - pos, detector.rect.height);
    }

    private void resetDetector()
    {
        detector.anchoredPosition = new Vector2(0, detector.anchoredPosition.y);
        detector.sizeDelta = new Vector2(2208, detector.rect.height);
    }

    public void MonkeyReachedEnd()
    {
        if (!tapeClicked)
        {
            Debug.Log("MOnkey Reached ENd caleeddd");
            xScale *= -1;
            int x = (xScale > 0) ? 0 : 1;
            QuestionImage.rectTransform.localScale = new Vector3(xScale, 1, 1);
            QuestionImage.rectTransform.pivot = new Vector2(x, 0.5f);
        }    
    }

    public override void TimePassed(int timeTick)
    {
        
    }
}