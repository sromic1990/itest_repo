using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using SwanitLib;

public class SplineTest : MonoBehaviour
{
    public RectTransform[] mRects;

    [SerializeField]
    private List<Vector2> mPositions;

    [SerializeField]
    private List<Vector2> path;


    private float t = 0;

    void Start()
    {
        mPositions = new List<Vector2>();

        for (int i = 0; i < mRects.Length; i++)
            mPositions.Add(mRects[i].anchoredPosition);  

        int z = Random.Range(43, 344);

        EProz.INSTANCE.MoveInSpline(mRects[0], mPositions[1], false, 2.0f, 1000);
  
    }

    //    IEnumerator GoToB(int time)
    //    {
    //        float t = 0;
    //        Vector2 spos = mPositions[0];
    //        Vector2 endpos = mPositions[1];
    //        yield return new WaitForSeconds(1f);
    //        while (t <= 1)
    //        {
    //            t += Time.deltaTime / time;
    //            ;
    //            mRects[0].anchoredPosition = Vector2.Lerp(spos, endpos, t);
    //            yield return null;
    //        }
    //
    //        Debug.Log(t + "   " + Time.time);
    //    }
       
}
