using System;
using System.Collections;
using _Scripts.Tools;
using NUnit.Framework;
using UnityEngine;

public class PKBannerCtrl : MonoBehaviour {
    public SpriteRenderer spriteRenderer;
    public SpriteRenderer glowRenderer;

    public bool isAppearing;
    public bool isFading;

    public float baseTime;
    private void Start() {
        isAppearing = true;
        spriteRenderer.color = spriteRenderer.color.SetAlpha(0);
        glowRenderer.color = glowRenderer.color.SetAlpha(0);
    }

    void Update() {
        if (Input.anyKeyDown) {
            isFading = true;
        }

        if (isAppearing) {
            spriteRenderer.color = spriteRenderer.color.Appear(8f);
            //glowRenderer.color = glowRenderer.color.Appear(16f);
            if (spriteRenderer.color.a.Equal(1f, 0.01f)) {
                isAppearing = false;
                baseTime = Time.time;
            }
        }

        if (!isFading && !isAppearing)
            glowRenderer.color = glowRenderer.color.SetAlpha((Mathf.Sin(Time.time - baseTime - Mathf.PI /2f) + 1f) / 3f);
        else if (!isAppearing) {
            spriteRenderer.color = spriteRenderer.color.Fade(16f);
            glowRenderer.color = glowRenderer.color.Fade(16f);
        }
    }
}
