using Prime31;
using Scripts.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ShareManager : Singleton<ShareManager>
{
    public List<ShareMessages> Messages;

    public void NativeShare(ShareType type, string msg = "")
    {
        string Message = "";

        for (int i = 0; i < Messages.Count; i++)
        {
            if(Messages[i].ShareType == type)
            {
                if (Messages[i].Append.Append == AppendAction.Begin)
                {
                    Message += msg+" ";
                }
                Message = AppendMessages(Message, i, Messages[i].Append.Append, msg);
                if (Messages[i].Append.Append == AppendAction.End)
                {
                    Message += msg;
                }
            }
        }

        Debug.Log(Message);

        #if UNITY_IOS
        string[] array = new string[] { Message };
        SharingBinding.shareItems(array);
        #endif

        #if UNITY_ANDROID
        EtceteraAndroid.shareWithNativeShareIntent(Message, null, null);
        #endif
    }

    private string AppendMessages(string Message, int i, AppendAction Action, string msg)
    {
        for (int j = 0; j < Messages[i].Messages.Count; j++)
        {
            Message += Messages[i].Messages[j]+" ";
            if(Action == AppendAction.Middle)
            {
                if(Messages[i].Append.AppendAfter == j)
                {
                    Message += msg+" ";
                }
            }
        }

        return Message;
    }

    private void OnValidate()
    {
        for (int i = 0; i < Messages.Count; i++)
        {
            Messages[i].ShareTypeName = Messages[i].ShareType.ToString();
        }
    }
}


public enum ShareType
{
    FromGameWin,
    FromGameLose,
    FromMultiplayer
}

public enum AppendAction
{
    None,
    Begin,
    Middle,
    End
}

[Serializable]
public class ShareMessages
{
    public string ShareTypeName;
    public ShareType ShareType;
    public List<string> Messages;
    public AppendMessage Append;
}

[Serializable]
public class AppendMessage
{
    public AppendAction Append;
    public int AppendAfter;
}


