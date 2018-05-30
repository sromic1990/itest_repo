using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SwanitLib;

public class UI_TouchFiveMen : UI_Base
{
    public Text QuestionDisplay;

    public RectTransform AnswerParent;

    public List<AnswerButtonHolder> mButtonHolder;

    public float displayTime = 0.0f;

    //  [SerializeField]
    private List<int> order = new List<int>();
    //  [SerializeField]
    private List<Vector2> positions = new List<Vector2>();


    private bool isUISet = false;

    void Awake()
    {
        for (int i = 0; i < AnswerParent.childCount; i++)
        {
            Vector2 pos = AnswerParent.GetChild(i).GetComponent<RectTransform>().anchoredPosition;
            positions.Add(pos);
            order.Add(i);
        }

    }

    void Start()
    {
    }

    public override void SetUI(QuestionUIInfo info)
    {
        if (isUISet)
            return;
        
        isUISet = true;
        base.SetUI(info);

        //    displayTime = info.QuestionData_Float[0];
        //   QuestionDisplay.text = info.Question;

        AnswerParent.gameObject.SetActive(false);
//		for (int i = 0; i < mButtonHolder.Count; i++)
//			mButtonHolder [i].SetAnswerButtonProperties (info.ButtonAnswer [i]);

        EProz.INSTANCE.WaitAndCall(displayTime, () =>
            {
                order.Shuffle();
                // QuestionDisplay.text = info.SecondaryQuestion[0];
                UIManager.Instance.ShowSecondaryQuestion();

                AnswerParent.gameObject.SetActive(true);

                for (int i = 0; i < mButtonHolder.Count; i++)
                {
                    mButtonHolder[i].GetComponent<RectTransform>().anchoredPosition = positions[order[i]];
                    mButtonHolder[i].SetAnswerButtonProperties(info.ButtonAnswer[i]);
                }
            });
    }

    public override void Reset()
    {
        isUISet = false;
        AnswerParent.gameObject.SetActive(false);
        //   Mathf.Lerp(2, 52, 0.1f);
    }
}