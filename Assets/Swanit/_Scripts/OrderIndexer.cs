using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class OrderIndexer : MonoBehaviour
{
    public QuestionHolder qData;
    public QuestionHolder AnotherHolder;
    public QuestionHolder Buckets;
    public QuestionHolder test;

    public List<int> Qand4A;
    public List<int> Homofon;
    public List<int> Picand4A;
    public List<int> TrueFalse;
    public List<int> uniqueIndex;

    public List<string> indexes;

    public List<patternCode> mCodesList;
    public List<PatternIndex> pIndex;

    public List<int> QuestionOrder;

    void Start()
    {
       
    }

    [ContextMenu("MyTempFunc")]
    public void aksjdak()
    {
        List<BaseQuestion> QuestionAnd4Ans = qData.Questions.Where(f => (f.Pattern == QPattern.QuestionAnd4Answers)).ToList();
        List<BaseQuestion> PicAnd4Ans = qData.Questions.Where(f => (f.Pattern == QPattern.PictureAnd4Answers)).ToList();
        List<BaseQuestion> CatchMonkey = qData.Questions.Where(f => (f.Pattern == QPattern.CatchMonkey)).ToList();
        List<BaseQuestion> SlowFast = qData.Questions.Where(f => (f.Pattern == QPattern.SlowFast)).ToList();
        List<BaseQuestion> TchAppearOrder3 = qData.Questions.Where(f => (f.Pattern == QPattern.TouchAppearOrder3)).ToList();
        List<BaseQuestion> TchAppearOrder5 = qData.Questions.Where(f => (f.Pattern == QPattern.TouchAppearOrder5)).ToList();
        List<BaseQuestion> TestSight = qData.Questions.Where(f => (f.Pattern == QPattern.TestYourSight)).ToList();
        List<BaseQuestion> ConfusionTch = qData.Questions.Where(f => (f.Pattern == QPattern.ConfusionTouch)).ToList();
        List<BaseQuestion> SpeedTch = qData.Questions.Where(f => (f.Pattern == QPattern.SpeedTouch)).ToList();
        List<BaseQuestion> objInPrev = qData.Questions.Where(f => (f.Pattern == QPattern.ObjectInPrevious)).ToList();
        List<BaseQuestion> trufals = qData.Questions.Where(f => (f.Pattern == QPattern.TrueFalse)).ToList();
        List<BaseQuestion> ifElse = qData.Questions.Where(f => (f.Pattern == QPattern.IfElse)).ToList();
        List<BaseQuestion> TextAnd4Ans = qData.Questions.Where(f => (f.Pattern == QPattern.TextAnd4Answers)).ToList();
        List<BaseQuestion> smallBig = qData.Questions.Where(f => (f.Pattern == QPattern.SmallestAndBiggest)).ToList();
        List<BaseQuestion> xyz = qData.Questions.Where(f => (f.Pattern == QPattern.XYZSeconds)).ToList();
        List<BaseQuestion> homofon = qData.Questions.Where(f => (f.Pattern == QPattern.Homophone)).ToList();
        List<BaseQuestion> tchOneMan = qData.Questions.Where(f => (f.Pattern == QPattern.TouchOneManQuickly)).ToList();
        List<BaseQuestion> tchFiveman = qData.Questions.Where(f => (f.Pattern == QPattern.TouchFiveMen)).ToList();
        List<BaseQuestion> uniq = qData.Questions.Where(t => (t.QuestionType == QuestionType.NoBase)).ToList();

        int Qand4A_count = 0;
        int picAnd4A_count = 0;
        int catchMonkey_count = 0;
        int slowfast_count = 0;
        int tao3_count = 0;
        int tao5_count = 0;
        int testSight_count = 0;
        int confuseTch_count = 0;
        int speedTch_count = 0;
        int objInPrev_count = 0;
        int tf_count = 0;
        int ifels_count = 0;
        int text4ans_count = 0;
        int sb_count = 0;
        int xyz_count = 0;
        int homof_count = 0;
        int tch1_count = 0;
        int tch5_count = 0;
        int uniq_count = 0;

        for (int i = 0; i < QuestionAnd4Ans.Count; i++)
        {
            QuestionAnd4Ans[i].sortOrder = Qand4A[i];
        }

        for (int i = 0; i < trufals.Count; i++)
        {
            trufals[i].sortOrder = TrueFalse[i];
            //TrueFalse[i] - 1;
        }

        for (int i = 0; i < homofon.Count; i++)
        {
            homofon[i].sortOrder = Homofon[i] - 1;
        }

        for (int i = 0; i < PicAnd4Ans.Count; i++)
        {
            PicAnd4Ans[i].sortOrder = Picand4A[i] - 1;
        }

        for (int i = 0; i < uniq.Count; i++)
        {
            uniq[i].sortOrder = uniqueIndex[i] - 1;
        }


        QuestionAnd4Ans = QuestionAnd4Ans.OrderBy(x => x.sortOrder).ToList();
        PicAnd4Ans = PicAnd4Ans.OrderBy(x => x.sortOrder).ToList();
        trufals = trufals.OrderBy(x => x.sortOrder).ToList();
        homofon = homofon.OrderBy(x => x.sortOrder).ToList();
        uniq = uniq.OrderBy(x => x.sortOrder).ToList();

        BaseQuestion bq = new BaseQuestion();

        //<<<<<<< HEAD
        #if UNITY_EDITOR
        EditorUtility.SetDirty(AnotherHolder);
        #endif
        //=======
        //        EditorUtility.SetDirty(AnotherHolder);
        //>>>>>>> 8566da3ed22e947d6d1543226c6f15a83d76b111

        for (int i = 0; i < indexes.Count; i++)
        {

//            if (indexes[i] == "P")
//                Debug.Log(indexes[i] + "   " + (i + 1).ToString());

            switch (indexes[i].Trim())
            {
                case "M":
                    bq = CatchMonkey[catchMonkey_count++];
                    break;
                case "C":
         //           Debug.Log(ConfusionTch.Count + "     " + confuseTch_count);
                    bq = ConfusionTch[confuseTch_count++];
                  //  confuseTch_count = Mathf.Clamp(confuseTch_count, 0, ConfusionTch.Count - 1);
                    break;
                case "H":
                    bq = homofon[homof_count++];
                    //return QPattern.Homophone;
                    break;
                case "E":
                    bq = ifElse[ifels_count++];
                    //return QPattern.IfElse;
                    break;
                case "O":
                    bq = objInPrev[objInPrev_count++];
                //    return QPattern.ObjectInPrevious;
                    break;
                case "P":
           //         Debug.Log(picAnd4A_count + "   " + PicAnd4Ans.Count);
                    bq = PicAnd4Ans[picAnd4A_count++];
                 //   return QPattern.PictureAnd4Answers;
                    break;
                case "Q":
                    bq = QuestionAnd4Ans[Qand4A_count++];
                   // return QPattern.QuestionAnd4Answers;
                    break;
                case "F":
                    bq = SlowFast[slowfast_count++];
               //     return QPattern.SlowFast;
                    break;
                case "S":
                    bq = SpeedTch[speedTch_count++];
                //    return QPattern.SpeedTouch;
                    break;
                case "Y":
                    bq = TestSight[testSight_count++];
//                    return QPattern.TestYourSight;
                    break;
                case "TA":
                    bq = TchAppearOrder3[tao3_count++];
          //          return QPattern.TouchAppearOrder3;
                    break;
                case "TAf":
                    bq = TchAppearOrder5[tao5_count++];
               //     return QPattern.TouchAppearOrder5;
                    break;
                case "TF":
                    bq = tchFiveman[tch5_count++];
           //         return QPattern.TouchFiveMen;
                    break;
                case "TO":
                    bq = tchOneMan[tch1_count++];
         //           return QPattern.TouchOneManQuickly;
                    break;
                case "X":
                    bq = xyz[xyz_count++];
      //              return QPattern.XYZSeconds;
                    break;
                case "T":
                    bq = trufals[tf_count++];
                    //return QPattern.TrueFalse;
                    break;
                case "B":
                    bq = smallBig[sb_count++];
  //                  return QPattern.SmallestAndBiggest;
                    break;
                case "A":
                    bq = TextAnd4Ans[text4ans_count++];
  //                  return QPattern.TextAnd4Answers;
                    break;
                default:
                    bq = uniq[uniq_count++];
  //                  return QPattern.Default;
                    break;


            }

            //   Debug.Log("   " + bq.Question + "        P   :     " + bq.Pattern.ToString());
           
            AnotherHolder.Questions.Add(bq);
        }
    }

    [ContextMenu("Rearrange")]
    public void sdjk()
    {
        List<BaseQuestion> q4a = qData.Questions.Where(f => (f.Pattern == QPattern.QuestionAnd4Answers)).ToList();

        for (int i = 0; i < q4a.Count; i++)
        {
            q4a[i].sortOrder = Qand4A[i] - 1;
        }

        q4a = q4a.OrderBy(a => a.sortOrder).ToList();

        for (int i = 0; i < q4a.Count; i++)
        {
            Debug.Log(q4a[i].Question);
        }
    }

    [ContextMenu("Edit Wait time")]
    public void kasdkajklsd()
    {
        #if UNITY_EDITOR
        EditorUtility.SetDirty(AnotherHolder);
        EditorUtility.SetDirty(Buckets);
        #endif


        for (int i = 0; i < AnotherHolder.Questions.Count; i++)
        {
//            if (AnotherHolder.Questions[i].Pattern == QPattern.CatchMonkey)
//            {
//                AnotherHolder.Questions[i].HasBoolValue = true;
//                AnotherHolder.Questions[i].WaitTimeAfterAnswer = 2;
//            }
//
//            if (AnotherHolder.Questions[i].Pattern == QPattern.TouchBallsWrongCount)
//            {
//                AnotherHolder.Questions[i].AllowMultipleAnswers = true;
//                AnotherHolder.Questions[i].WaitTimeAfterAnswer = 4;
//            }
//
//            if (AnotherHolder.Questions[i].Pattern == QPattern.KnockTheDoor || AnotherHolder.Questions[i].Pattern == QPattern.KnockTheDoor)
//            {
//                AnotherHolder.Questions[i].AllowMultipleAnswers = true;
//                AnotherHolder.Questions[i].WaitTimeAfterAnswer = 3;
//            }
//
//            if (AnotherHolder.Questions[i].Pattern == QPattern.TestYourSight)
//            {
//                AnotherHolder.Questions[i].WaitTimeAfterAnswer = 3;
//            }

//            if (Buckets.Questions[i].Pattern == QPattern.TrueFalse)
//            {
//                Buckets.Questions[i].DelayTimeAfterQuestionShow = 0;
//                Debug.Log(AnotherHolder.Questions[i].Question);
//            }


            if (AnotherHolder.Questions[i].Pattern == QPattern.TouchAppearOrder3 || AnotherHolder.Questions[i].Pattern == QPattern.TouchAppearOrder5)
            {
                float delay = AnotherHolder.Questions[i].QuestionData_Float[0];
                delay *= AnotherHolder.Questions[i].Options.Count;
                delay += 0.5f;
                Debug.LogError("Pattern=" + AnotherHolder.Questions[i].Pattern.ToString() + "  Delay = " + delay);
                AnotherHolder.Questions[i].DelayTimeAfterQuestionShow = delay;

            }
        }
    }


    [ContextMenu("TestHolder")]
    public void ekjaksd()
    {
        //<<<<<<< HEAD
        #if UNITY_EDITOR
        EditorUtility.SetDirty(test);
        #endif
        //=======
        //        EditorUtility.SetDirty(test);
        //>>>>>>> 8566da3ed22e947d6d1543226c6f15a83d76b111

        List<BaseQuestion> bq = AnotherHolder.Questions.GroupBy(f => f.Pattern, (key, g) => g.OrderBy(e => e.Pattern).FirstOrDefault()).ToList();
       
        for (int i = 0; i < bq.Count; i++)
        {
            test.Questions.Add(bq[i]);
        }
    }

    public void swapElem(int index1, int index2)
    {
        //index1 = testHold.Questions.FindIndex(f => f.sortOrder == 1004);
        //index2 = testHold.Questions.FindIndex(f => f.sortOrder == 1005);

        BaseQuestion bq = AnotherHolder.Questions[index2];
        AnotherHolder.Questions[index2] = AnotherHolder.Questions[index1];
        AnotherHolder.Questions[index1] = bq;

        for (int i = 0; i < AnotherHolder.Questions.Count; i++)
            AnotherHolder.Questions[i].sortOrder = -1;
    }

    [ContextMenu("Create Buckets")]
    public void Multiplayer()
    {
        #if UNITY_EDITOR
        EditorUtility.SetDirty(AnotherHolder);
        EditorUtility.SetDirty(Buckets);
        #endif

     
        List<BaseQuestion> P_list = AnotherHolder.Questions.Where(f => (f.Pattern == QPattern.PictureAnd4Answers)).ToList();
        List<BaseQuestion> Q_list = AnotherHolder.Questions.Where(f => (f.Pattern == QPattern.QuestionAnd4Answers)).ToList();
        List<BaseQuestion> T_list = AnotherHolder.Questions.Where(f => (f.Pattern == QPattern.TrueFalse)).ToList();
        List<BaseQuestion> H_list = AnotherHolder.Questions.Where(f => (f.Pattern == QPattern.Homophone)).ToList();
        List<BaseQuestion> A_list = AnotherHolder.Questions.Where(f => (f.Pattern == QPattern.TextAnd4Answers)).ToList();
        List<BaseQuestion> U_list = AnotherHolder.Questions.Where(t => (t.QuestionType == QuestionType.NoBase)).ToList();
        List<BaseQuestion> B_list = AnotherHolder.Questions.Where(f => (f.Pattern == QPattern.SmallestAndBiggest)).ToList();
        List<BaseQuestion> O_list = AnotherHolder.Questions.Where(f => (f.Pattern == QPattern.ObjectInPrevious)).ToList();
        List<BaseQuestion> F_list = AnotherHolder.Questions.Where(f => (f.Pattern == QPattern.SlowFast)).ToList();
        List<BaseQuestion> TO_list = AnotherHolder.Questions.Where(f => (f.Pattern == QPattern.TouchOneManQuickly)).ToList();
        List<BaseQuestion> M_list = AnotherHolder.Questions.Where(f => (f.Pattern == QPattern.CatchMonkey)).ToList();
        List<BaseQuestion> TA_list = AnotherHolder.Questions.Where(f => (f.Pattern == QPattern.TouchAppearOrder3 || f.Pattern == QPattern.TouchAppearOrder5)).ToList();
        List<BaseQuestion> Y_list = AnotherHolder.Questions.Where(f => (f.Pattern == QPattern.TestYourSight)).ToList();
        List<BaseQuestion> TF_list = AnotherHolder.Questions.Where(f => (f.Pattern == QPattern.TouchFiveMen)).ToList();
        List<BaseQuestion> E_list = AnotherHolder.Questions.Where(f => (f.Pattern == QPattern.IfElse)).ToList();


        //Removing 1 pic and 4 answer of pack of 4
        BaseQuestion rem = P_list.Find(q => q.Question.Contains("suite"));
        Debug.Log(rem.Question);
        P_list.Remove(rem);

        // Removing 2 true false of pack of 4
        rem = T_list.Find(q => q.Question.Contains("dice"));
        Debug.Log(rem.Question);
        T_list.Remove(rem);
        rem = T_list.Find(q => q.Question.Contains("potbellied"));
        Debug.Log(rem.Question);
        T_list.Remove(rem);


        // Removing 1 Homophone pattern of pack of 4
        rem = H_list.Find(q => q.Question.Contains("Waste"));
        Debug.Log(rem.Question);
        H_list.Remove(rem);

        // Removing 4 question and 4 answer of pack of 4
        rem = Q_list.Find(q => q.Question.Contains("black poker"));
        Debug.Log(rem.Question);
        Q_list.Remove(rem);
        rem = Q_list.Find(q => q.Question.Contains("total poker"));
        Debug.Log(rem.Question);
        Q_list.Remove(rem);
        rem = Q_list.Find(q => q.Question.Contains("picture was ..."));
        Debug.Log(rem.Question);
        Q_list.Remove(rem);
        rem = Q_list.Find(q => q.Question.Contains("basket was ..."));
        Debug.Log(rem.Question);
        Q_list.Remove(rem);

        //Removing Specified Questions

        Q_list.RemoveAt(15);
        Q_list.RemoveAt(16);
        Q_list.RemoveAt(17);
       

        U_list.RemoveAt(3);


        List<BaseQuestion> col1 = new List<BaseQuestion>();
        List<BaseQuestion> col2 = new List<BaseQuestion>();
        List<BaseQuestion> col3 = new List<BaseQuestion>();
        List<BaseQuestion> col4 = new List<BaseQuestion>();
        List<BaseQuestion> col5 = new List<BaseQuestion>();

        int P_Index = 0;
        int Q_Index = 0;
        int T_Index = 0;
        int H_Index = 0;
        int A_Index = 0;
        int U_Index = 0;
        int B_Index = 0;
        int O_Index = 0;
        int F_Index = 0;
        int TO_Index = 0;
        int M_Index = 0;
        int TA_Index = 0;
        int Y_Index = 0;
        int TF_Index = 0;
        int E_Index = 0; 

        // Filling First Column

        for (int i = 0; i < 42; i++)
        {
            if (i < 33)
            {
                col1.Add(P_list[P_Index]);
                P_Index++;
            }
            else
            {
                col1.Add(Q_list[Q_Index]);
                Q_Index++;
            }


            
        }

        Debug.Log("COL1 COUNT = " + col1.Count);

        // Filling Second Column

        for (int i = 0; i < 42; i++)
        {
            if (i < 6)
            {
                col2.Add(Q_list[Q_Index]);
                Q_Index++;
            }
            else if (i >= 6 && i < 9)
            {
                col2.Add(T_list[T_Index]);
                T_Index++;
            }
            else if (i >= 9 && i < 23)
            {
                col2.Add(Q_list[Q_Index]);
                Q_Index++;
            }
            else if (i >= 23 && i < 26)
            {
                col2.Add(T_list[T_Index]);
                T_Index++;
            }
            else if (i == 26)
            {
                col2.Add(Q_list[Q_Index]);
                Q_Index++;
            }
            else if (i > 26 && i < 40)
            {
                col2.Add(T_list[T_Index]);
                T_Index++;
            }
            else if (i >= 40)
            {
                col2.Add(H_list[H_Index]);
                H_Index++;
            }
        }

        Debug.Log("COL2 COUNT = " + col2.Count);

        // Filling 3rd Column

        for (int i = 0; i < 42; i++)
        {
            if (i < 5)
            {
                col3.Add(H_list[H_Index]);
                H_Index++;
            }
            else if (i >= 5 && i < 10)
            {
                col3.Add(A_list[A_Index]);
                A_Index++;
            }
            else if (i >= 10 && i < 12)
            {
                col3.Add(T_list[T_Index]);
                T_Index++;
            }
            else if (i >= 12 && i < 18)
            {
                col3.Add(H_list[H_Index]);
                H_Index++;
            }
            else if (i >= 18 && i < 21)
            {
                col3.Add(U_list[U_Index]);
                U_Index++;
            }
            else if (i >= 21 && i < 31)
            {
                col3.Add(B_list[B_Index]);
                B_Index++;
            }
            else if (i >= 31 && i < 37)
            {
                col3.Add(U_list[U_Index]);
                U_Index++;
            }
            else if (i >= 37)
            {
                col3.Add(O_list[O_Index]);
                O_Index++;
            }
        }

        Debug.Log("COL3 COUNT = " + col3.Count);

        // Filling 4th column

        for (int i = 0; i < 42; i++)
        {
            if (i < 2)
            {
                col4.Add(O_list[O_Index]);
                O_Index++;
            }
            else if (i >= 2 && i < 4)
            {
                col4.Add(F_list[F_Index]);
                F_Index++;
            }
            else if (i >= 4 && i < 14)
            {
                col4.Add(U_list[U_Index]);
                U_Index++;
            }
            else if (i >= 14 && i < 19)
            {
                col4.Add(TO_list[TO_Index]);
                TO_Index++;
            }
            else if (i >= 19 && i < 24)
            {
                col4.Add(M_list[M_Index]);
                M_Index++;
            }
            else if (i >= 24 && i < 28)
            {
                col4.Add(TA_list[TA_Index]);
                TA_Index++;
            }
            else if (i >= 28 && i < 33)
            {
                col4.Add(Y_list[5 + Y_Index]);
                Y_Index++;
            }
            else if (i >= 33 && i < 37)
            {
                col4.Add(TF_list[TF_Index]);
                TF_Index++;
            }
            else if (i >= 33)
            {
                col4.Add(Y_list[Y_Index - 5]);
                Y_Index++;
            }
        }

        Debug.Log("COL4 COUNT = " + col4.Count);

        //Filling 5th column

        for (int i = 0; i < 42; i++)
        {
            if (i < 5)
            {
                // TO
                col5.Add(TO_list[TO_Index]);
                TO_Index++;
            }
            else if (i >= 5 && i < 10)
            {
                //M
                col5.Add(M_list[M_Index]);
                M_Index++;
            }
            else if (i >= 10 && i < 15)
            {
                // TA
                col5.Add(TA_list[TA_Index]);
                TA_Index++;
            }
            else if (i >= 15 && i < 25)
            {
                //Y
                col5.Add(Y_list[Y_Index]);
                Y_Index++;
            }
            else if (i >= 25 && i < 30)
            {
                //TF
                col5.Add(TF_list[TF_Index]);
                TF_Index++;
            }
            else if (i >= 30 && i < 32)
            {
                //O
                col5.Add(O_list[O_Index]);
                O_Index++;
            }
            else if (i >= 32 && i < 37)
            {
                col5.Add(F_list[F_Index]);
                F_Index++;
            }
            else if (i >= 37)
            {
                col5.Add(E_list[E_Index]);
                E_Index++;
            }
        }

        Debug.Log("COL5 COUNT = " + col5.Count);

        //Adding in questions in bucket from each column

        for (int i = 0; i < 42; i++)
        {
            Buckets.Questions.Add(col1[i]);
            Buckets.Questions.Add(col2[i]);
            Buckets.Questions.Add(col3[i]);
            Buckets.Questions.Add(col4[i]);
            Buckets.Questions.Add(col5[i]);
        }
    }


    [ContextMenu("Change Text Color")]
    public void changeTexCol()
    {
        Color col = new Color();
        ColorUtility.TryParseHtmlString("#632C10FF", out col);

        #if UNITY_EDITOR
        EditorUtility.SetDirty(AnotherHolder);
        EditorUtility.SetDirty(Buckets);
        #endif


        for (int i = 0; i < AnotherHolder.Questions.Count; i++)
        {
            for (int j = 0; j < AnotherHolder.Questions[i].Options.Count; j++)
            {
                Debug.Log(AnotherHolder.Questions[i].Pattern.ToString());
                AnotherHolder.Questions[i].Options[j].TextColor = col;
            }
        }
    }

}

[System.Serializable]
public struct PatternIndex
{
    public QPattern mPattern;
    public int Qindex;
};


[System.Serializable]
public struct IndexSwap
{
    public int index;
    public int swappedInex;
};

[System.Serializable]
public struct patternCode
{
    public string code;
    public QPattern mCodePattern;
};