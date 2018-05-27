using Sourav.Utilities.Extensions;
using UnityEngine;
using UnityEngine.UI;

public class IntroPanel : MonoBehaviour 
{
    public Text IntroText;
    public Button OkButton;

    public UI_Timer IntroTimer;

    private int counter;
    private int timeForIntro;

    //TODO Show or hide timer in intro panel

    public void SetUpIntro(string introText, int timeForIntro, bool showOkButton, bool hasTimerValue)
    {
        IntroText.text = introText;

        if(timeForIntro > 0)
        {
            counter = 0;
            this.timeForIntro = timeForIntro;
            GameManager.Instance.TimeTicker += CountTime;
            IntroTimer.SetTimer(timeForIntro);
            IntroTimer.gameObject.Show();
        }
        else
        {
            IntroTimer.gameObject.Hide();
        }

        if(showOkButton)
        {
            OkButton.gameObject.Show();
        }
        else
        {
            OkButton.gameObject.Hide();
        }
    }

    public void OkButtonClicked()
    {
        GameManager.Instance.PlayButtonClicked();
    }

    private void CountTime(int Time)
    {
        counter++;
        IntroTimer.SetTime(counter);
        if(counter >= timeForIntro)
        {
            OkButtonClicked();
        }
    }

    private void OnDisable()
    {
        GameManager.Instance.TimeTicker -= CountTime;
    }
}
