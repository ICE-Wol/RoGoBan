using System;
using _Scripts.Tools;
using Unity.VisualScripting;
using UnityEngine;

public class ColorHintCtrl : MonoBehaviour
{
    public SpriteRenderer[] circles;
    public Vector3 initPos;
    public Vector3[] finalPos;

    public bool isShowingHint;

    private void Start() {
        finalPos = new Vector3[circles.Length];
        for (int i = 0; i < circles.Length; i++) {
            finalPos[i] = circles[i].transform.position;
        }
    }
    //加入Wwise
    private WwiseSoundManager wwiseSoundManager_Instance => WwiseSoundManager.Instance;
    private void Update() {
        if (Input.GetKeyDown(KeyCode.H))
        {//播放H音效
            wwiseSoundManager_Instance.PostEvent(gameObject, WwiseEventType.H);
            isShowingHint = !isShowingHint;
        }
        
        for (int i = 0; i < circles.Length; i++) {
            if (isShowingHint) {
                //circles[i].transform.position = circles[i].transform.position.
                   // ApproachValue(finalPos[i], 16f);
                transform.localScale = transform.localScale.ApproachValue(Vector3.one, 16f);
                
            }
            else {
               // circles[i].transform.position = circles[i].transform.position.
                   // ApproachValue(initPos, 16f);
                transform.localScale = transform.localScale.ApproachValue(Vector3.zero, 16f);
            }
        }
    }
}
