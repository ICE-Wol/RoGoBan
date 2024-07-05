using System;
using System.Collections;
using _Scripts.Tools;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class TitleCtrl : MonoBehaviour
{
    public Sprite[] bannerSprites;
    public SpriteRenderer charTemplate;
    public SpriteRenderer[] chars;
    public Vector3[] charTargetPos;
    public bool[] isFollowing;
    public bool[] isLeaving;

    public float charInterval;
    public bool isBannerShowed;
    private void Start() {
        chars = new SpriteRenderer[bannerSprites.Length];
        charTargetPos = new Vector3[bannerSprites.Length];
        isFollowing = new bool[bannerSprites.Length];
        isLeaving = new bool[bannerSprites.Length];
        for (int i = 0; i < bannerSprites.Length; i++) {
            chars[i] = Instantiate(charTemplate, transform);
            chars[i].sprite = bannerSprites[i];
            chars[i].transform.localPosition = new Vector3(i * charInterval, 10, 0);
            charTargetPos[i] = new Vector3(i * charInterval, 10, 0);
        }

        StartCoroutine(ShowBanner());
    }
    //加入Wwise
    private WwiseSoundManager wwiseSoundManager_Instance => WwiseSoundManager.Instance;
    private void Update() {
        for (int i = 0; i < chars.Length; i++) {
            charTargetPos[i] = new Vector3(i * charInterval, Mathf.Sin(Time.time + i), 0);
        }
        
        if(Input.anyKeyDown) StartCoroutine(HideBanner());
        

        for (int i = 0; i < chars.Length; i++) {
            if(isFollowing[i])
                chars[i].transform.localPosition = 
                    chars[i].transform.localPosition.ApproachValue
                        (charTargetPos[i], 32f);
            if(isLeaving[i])
                chars[i].transform.localPosition = 
                    chars[i].transform.localPosition.ApproachValue
                        (new Vector3(i * charInterval, -12f, 0), 32f);
        }
    }


    public IEnumerator ShowBanner() {
        isBannerShowed = true;
        for (int i = 0; i < chars.Length; i++) {
            isFollowing[i] = true;
            isLeaving[i] = false;
            yield return new WaitForSeconds(0.1f);
        }
    }

    public IEnumerator HideBanner()
    {//播放Title音效
        wwiseSoundManager_Instance.PostEvent(gameObject, WwiseEventType.Title_Enter);
        isBannerShowed = false;
        for (int i = 0; i < chars.Length; i++) {
            isFollowing[i] = false;
            isLeaving[i] = true;
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(1f);
        ResetBanner();
    }
    
    public void ResetBanner() {
        for (int i = 0; i < chars.Length; i++) {
            isFollowing[i] = false;
            isLeaving[i] = false;
            chars[i].transform.localPosition = new Vector3(i * charInterval, 10, 0);
        }
    }
}