using System;
using _Scripts.Tools;
using UnityEngine;

public class HintCtrl : MonoBehaviour
{
    public Vector3 hintPos;
    
    public int hintIndex;
    public SpriteRenderer[] hintObject;
    public float[] hintAlpha;
    void Awake()
    {
        hintAlpha = new float[hintObject.Length];
        foreach (var hint in hintObject) {
            hint.transform.position = hintPos;
            hint.color = hint.color.SetAlpha(0f);
        }
    }

    private void Update() {
        for (int i = 0; i < hintObject.Length; i++) {
            var a = hintObject[i].color.a;
            a.ApproachRef(hintAlpha[i], 8f);
            hintObject[i].color = hintObject[i].color.SetAlpha(a);
        }
        
        // if(Input.GetKeyDown(KeyCode.Space)) {
        //     hintIndex++;
        //     hintIndex %= hintObject.Length;
        //     SetHintIndex(hintIndex);
        // }
    }

    public void SetHintIndex(int index) {
        if (index == -1) {
            for (int i = 0; i < hintObject.Length; i++) {
                hintAlpha[i] = 0f;
            }
            return;
        }
        hintIndex = index;
        for (int i = 0; i < hintObject.Length; i++) {
            hintAlpha[i] = 0f;
        }
        hintAlpha[hintIndex] = 1f;
        
    }
}
