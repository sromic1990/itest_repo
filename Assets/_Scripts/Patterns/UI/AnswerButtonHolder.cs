using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button), typeof(Image))]
public class AnswerButtonHolder : MonoBehaviour
{
    private Sprite image;
    private string ButtonText;
    private Color ButtonTextColor;
    [SerializeField]
    private AnswerID ID;
    private Action<int, int, bool, List<SequenceOfClick>> OnClickAction;
    [SerializeField]
    private int ClickCounter;
    [SerializeField]
    private bool isCorrect;
    private Image m_Image;
    private Button m_Button;
    private Text m_ButtonText;
    private List<SequenceOfClick> sequenceOfClick;
    private List<SpriteID> secondaryImages;
    private int currentSelectedSecondarySprite;

    private float lastClickTime = 0.0f;
    private float debounceDelay = 0.005f;

    public AnswerID mID{ get { return ID; } }

    public bool CorrectSet{ get { return isCorrect; } set { isCorrect = value; } }

    private void Awake()
    {
        InitializeAnswerButton();

        ResetAllData();

        GameManager.Instance.ResetAllData += ResetClickCounter;

    }

    public void SetAnswerButtonProperties(ButtonProperties buttonProperties, bool forceInitializeButton = false)
    {
        //    Debug.Log("Setting Answer Button Properties");

        if (m_Image == null || m_Button == null || m_ButtonText == null || forceInitializeButton)
        {
            InitializeAnswerButton();
        }

        if (buttonProperties.image != null)
        {
            this.image = buttonProperties.image;
        }
        else
        {
            this.image = GetCorrectSpriteByID.Instance.GetSpriteFromID(SpriteID.DummySprite);
        }
        this.ButtonText = buttonProperties.ButtonText;

        Debug.Log("BUtton Text COlor = " + buttonProperties.ButtonTextColor.ToString());

        this.ButtonTextColor = buttonProperties.ButtonTextColor;
        this.ID = buttonProperties.ID;
        this.isCorrect = buttonProperties.isCorrect;
        this.sequenceOfClick = buttonProperties.SequenceInfo;
        this.secondaryImages = buttonProperties.secondaryImages;
        this.OnClickAction = buttonProperties.OnClickAction;
        currentSelectedSecondarySprite = 0;

        SetAllData();
    }

    private void InitializeAnswerButton()
    {

        Debug.Log("Initializing answer button");

        m_Image = GetComponent<Image>();
        if (m_Image == null)
        {
            Debug.Log("m_Image is null at " + gameObject.name);
        }
        m_Button = GetComponent<Button>();
        m_ButtonText = GetComponentInChildren<Text>();

        m_Button.onClick.RemoveAllListeners();
        m_Button.onClick.AddListener(OnClickedButton);
    }

    private void SetAllData()
    {
        if (m_ButtonText != null)
        {
            m_ButtonText.text = ButtonText;
            //Debug.Log(ButtonTextColor.r + "   " + ButtonTextColor.g + "   " + ButtonTextColor.b + "   " + ButtonTextColor.a);
            m_ButtonText.color = ButtonTextColor;
        }

        m_Image.sprite = image;
    }

    private void ResetAllData()
    {
        ID = AnswerID.None;
        image = GetCorrectSpriteByID.Instance.GetSpriteFromID(SpriteID.DummySprite);
        OnClickAction = null;
        ButtonText = "";
        ButtonTextColor = Color.black;
        ResetClickCounter();
        SetAllData();
    }

    public void ResetClickCounter()
    {
        Debug.Log("ResetClickCounter");
        ClickCounter = 0;
    }

    public void OnClickedButton()
    {
        Debug.Log("<color=#0000AA>On CLick Btnn Entered</color>");

        if (Time.time - lastClickTime < debounceDelay)
        {
            Debug.Log("Debounce dlayy  returning");
            return;
        }

        if (!GameManager.Instance.CanProcessInput)
        {
            Debug.Log("Cant process input so returning");
            return;
        }

        lastClickTime = Time.time;

        //Debug.Log("OnClickedButton");

        ClickCounter++;
        GameManager.Instance.OnClickedButton(ClickCounter, (int)ID);


        UIManager.Instance.LastClickedButton = this;

        //Debug.Log("Checking null and invoking action");
        if (OnClickAction != null)
        {
            //Debug.Log("Invoking action");
            OnClickAction.Invoke((int)ID, ClickCounter, isCorrect, sequenceOfClick);
        }
    }

    public void ChangeButtonImageToClickedImage()
    {
        if (secondaryImages == null)
        {
            Debug.Log("secondaryImages == null");
            return;
        }

        if (secondaryImages.Count > 0 && currentSelectedSecondarySprite < secondaryImages.Count)
        {
            m_Image.sprite = GetCorrectSpriteByID.Instance.GetSpriteFromID(secondaryImages[currentSelectedSecondarySprite]);
            currentSelectedSecondarySprite++;
        }
    }
}

[Serializable]
public struct ButtonProperties
{
    public Sprite image;
    public List<SpriteID> secondaryImages;
    public string ButtonText;
    public Color ButtonTextColor;
    public AnswerID ID;
    public Action<int, int, bool, List<SequenceOfClick>> OnClickAction;
    public bool isCorrect;
    public List<SequenceOfClick> SequenceInfo;

    public ButtonProperties(SpriteID image, List<SpriteID> secondaryImages, string ButtonText, AnswerID ID, Action<int, int, bool, List<SequenceOfClick>> OnClickAction, bool isCorrect, List<SequenceOfClick> SequenceInfo)
        : this(image, secondaryImages, ButtonText, ID, OnClickAction, isCorrect, SequenceInfo, Color.black)
    {
    }

    public ButtonProperties(SpriteID image, List<SpriteID> secondaryImages, string ButtonText, AnswerID ID, Action<int, int, bool, List<SequenceOfClick>> OnClickAction, bool isCorrect, List<SequenceOfClick> SequenceInfo, Color ButtonTextColor)
    {
        this.image = GetCorrectSpriteByID.Instance.GetSpriteFromID(image);
        this.secondaryImages = secondaryImages;
        this.ButtonText = ButtonText;
        this.ButtonTextColor = ButtonTextColor;
        this.ID = ID;
        this.isCorrect = isCorrect;
        this.SequenceInfo = SequenceInfo;
        this.OnClickAction = OnClickAction;
    }
}