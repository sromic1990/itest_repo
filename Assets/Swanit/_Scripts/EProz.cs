using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text.RegularExpressions;

//using GooglePlayServices;
namespace SwanitLib
{

    public class EProz : MonoBehaviour
    {
        private static EProz _instance;

        public static EProz INSTANCE
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<EProz>();

                    if (_instance == null)
                    {
                        GameObject g = new GameObject();
                        g.AddComponent<EProz>();
                        g.name = "mEproz";
                        _instance = g.GetComponent<EProz>();
                    }
                    else
                    {
                        _instance.gameObject.name = "mEproz";
                    }
                }

                return _instance;
            }
        }

        /// <summary>
        /// Waits for given seconds and performs function.
        /// </summary>
        /// <param name="delay">Delay.</param>
        /// <param name="onComplete">On completeWait.</param>
       
        // public bool cancelDelayCall = false;

        public void WaitAndCall(float delay, Action onComplete)
        {
            StartCoroutine(wait_and_call(delay, Time.unscaledTime, onComplete));
            //      Debug.LogError("Unscaled Time = " + Time.unscaledTime);
        }

        private IEnumerator wait_and_call(float t, float time, Action a)
        {
            //      Debug.LogError("Unscaled Time = " + time + "Wait time=" + t);

            yield return new WaitForSeconds(t);

            Debug.Log("<color=#19F333>Called Afert  " + t + "  Seconds</color>");

            //     Debug.LogError("Time.unscaledTime - t" + (Time.unscaledTime - t).ToString());

            if (approx(time, Time.unscaledTime - t))
                a();

        }

        private bool approx(float f1, float f2)
        {
            float temp = Mathf.Abs(f1 - f2);

            Debug.LogError("Temp = " + temp + "   " + (temp > 0.0f && temp < 0.05f));

            if (temp > 0.0f && temp < 0.2f)
                return true;
            else
                return false;
        }

        public string Texture2DToBase64(Texture2D tex)
        {
            byte[] bytes = tex.EncodeToPNG();
            string b64 = Convert.ToBase64String(bytes);
            return b64;
        }

        public Texture2D Base64ToTexture2D(string bstring)
        {
            Texture2D tex = new Texture2D(10, 10);
            byte[] bytes = Convert.FromBase64String(bstring);
            tex.LoadImage(bytes, false);
            tex.Apply();
            return tex;
        }

        public bool isSplineFinish;

        public void MoveInSpline(RectTransform trans, Vector2 targetPos, bool CloclkWise, float duration, float h = 500f)
        {
            //  Debug.LogError(trans.gameObject.name);
            isSplineFinish = false;

            Vector2 p0 = trans.anchoredPosition;

            Vector2 p2 = targetPos;

            Vector2 p1 = (CloclkWise) ? p2 - p0 : p0 - p2;

            p1 = new Vector2(-p1.y, p1.x);//.normalized;

            Vector2 midPoint = new Vector2((p0.x + p2.x) * 0.5f, (p0.y + p2.y) * 0.5f);

            p1 = midPoint + p1.normalized * h;

            StartCoroutine(FillList(trans, p0, p1, p2, duration));
        }

        private IEnumerator FillList(RectTransform r, Vector2 p0, Vector2 p1, Vector2 p2, float duration)
        {
            float t = 0;
            List<Vector2> path = new List<Vector2>();

            //Debug.LogError("List Being Filled   ::  " + r.gameObject.name);

            while (t <= 1)
            {
                t += 0.1f;
                //    t += Time.deltaTime;
                Vector2 p = ((1 - t) * (1 - t) * p0) + (2 * (1 - t) * t * p1) + (t * t * p2);
                path.Add(p);

                yield return null;
            }

            yield return new WaitForEndOfFrame();

            StartCoroutine(Animate(path, r, duration));
        }

        private IEnumerator Animate(List<Vector2> path, RectTransform trans, float duration)
        {
            int count = 0;
            float timer = 0;
 
            while (count < path.Count)
            {
                //    bool finish = false;

                Vector2 startPos = trans.anchoredPosition;
                float t = 0;

                while (t <= 1)
                {
                    t += Time.deltaTime / duration * path.Count;
                    trans.anchoredPosition = Vector2.Lerp(startPos, path[count], t);
                   
                    timer += Time.deltaTime;
                 
                    yield return null;
                }

                count += 1;
                //    Debug.Log("point=" + count);
                yield return null;
            }

            //      Debug.Log("Timer = " + timer);
            isSplineFinish = true;
            //     System.GC.Collect();
            yield return null;

        }


        public string getCorrectString(string message)
        {
            string[] split = Regex.Split(message, "(?!^)(?=[A-Z])");

            for (int i = 1; i < split.Length; i++)
            {
                split[i] = split[i].ToLower();
            }

            string res = string.Join(" ", split);
            return res;
        }

        public int GetRandomBetween(int min, int max)
        {
            int x = System.Environment.TickCount;
           
            x = (x > 0) ? x : -x; 
       
            x = x % max;

            x = (x + min > max) ? ((2 * max) - (x + min)) : x + min;
       
            return x;
        }
    }

   
}