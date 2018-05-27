using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using IdiotTest.Scripts.GameScripts;
using DG.Tweening;
using SwanitLib;

public class LevelClear : MonoBehaviour
{
    public RectTransform slide;
    public Text clearedLevel;
    public Text reachedLevel;
    public Text nextLevel;

    public Image door;
    public Sprite[] door_frames;

    void OnEnable()
    {
        int currLevel = GameDataManager.Instance.CurrentLevel;
        //   int currLevel = 1;
        clearedLevel.text = "Level " + (currLevel - 1).ToString();
        reachedLevel.text = "Level " + currLevel.ToString();
        nextLevel.text = "Level " + (currLevel + 1).ToString();

        Invoke("MoveBar", 2.5f);
        StartCoroutine(OpenDoor());
    }

    private void MoveBar()
    {
        slide.DOAnchorPosX(-1600.0f, 1.0f, true).SetEase(Ease.Linear).OnComplete(() =>
            {
                EProz.INSTANCE.WaitAndCall(1.0f, () =>
                    {
                        UIManager.Instance.CompletedMonkeyLevelAnimation();
                    });
            });
    }

    private IEnumerator OpenDoor()
    {
        yield return new WaitForSeconds(2.0f);

        for (int i = 0; i < door_frames.Length; i++)
        {
            door.sprite = door_frames[i];
            yield return new WaitForEndOfFrame();
        }
    }

    private void OnDisable()
    {
        door.sprite = door_frames[0];
        slide.anchoredPosition = Vector2.zero;

    }


}
