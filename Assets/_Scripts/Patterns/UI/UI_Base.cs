using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UI_Base : MonoBehaviour, IUISetter
{
    public UI_Base prevPanel;
    public UI_Base nextPanel;
    //  public bool isUISet;

    private void OnEnable()
    {
        GameManager.Instance.TimeTicker += TimePassed;
        GameManager.Instance.ResetAllData += Reset;
        ResetIfGameOutOfSession();

    }

    private void ResetIfGameOutOfSession()
    {
        Debug.Log("Status = " + GameManager.Instance.Status.ToString());

        if (GameManager.Instance.Status == GameStatus.OutOfSession)
        {
            Reset();
        }
    }

    public virtual void SetUI(QuestionUIInfo info)
    {
        StartCoroutine(EnableInput());
        ShowGamePanel();
    }

    private IEnumerator EnableInput()
    {
        if (nextPanel != null)
        {
            yield return null;
        }
        else
        {
            float delay = GameManager.Instance.GetCurrentQuestion().DelayTimeAfterQuestionShow;
//            Debug.Log("Delay TIme ::: + " + delay);
            yield return new WaitForSeconds(delay);
            GameManager.Instance.SetInput();
        }
    }

    public virtual void QuestionAnswered()
    {
        Debug.Log("QuestionAnswered"); 
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.TimeTicker -= TimePassed;
            GameManager.Instance.ResetAllData -= Reset;
        }
        ResetIfGameOutOfSession();
    }

    //public virtual void Reset()
    //{
    //    if (prevPanel != null)
    //    {
    //        this.gameObject.SetActive(false);
    //    }
    //}

    public abstract void Reset();

    public virtual void TimePassed(int timeTick)
    {
        //      Debug.Log("Time passed = " + timeTick);
    }

    private void ShowGamePanel()
    {
        Debug.Log("From ShowGamePanel");
        //GameManager.Instance.ResetTimer();
    }

    public virtual void QuestionTurnOver()
    {
        //Check if timer has expired.
        if (!GameManager.Instance.Answered)
        {
            //Send GameManager.Instance.AnsweredWrong();
        }
    }
}
