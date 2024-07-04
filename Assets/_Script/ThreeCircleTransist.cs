using System;
using _Scripts.Tools;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ThreeCircleTransist : MonoBehaviour
{
    public Transform[] circles;

    public SpriteRenderer grayCover;
    public float coverAlpha;
    
    public float radius;
    public float radiusLimit;
    public float spdMultiplier;

    public bool isShrinking;

    private void Start() {
        DontDestroyOnLoad(gameObject);
        coverAlpha = 0f;
        //grayCover.color = Color.gray;
    }

    private void Update() {
        transform.position = Camera.main.transform.position + 10f * Vector3.forward;
        grayCover.color = grayCover.color.SetAlpha(coverAlpha);
        if (radius < radiusLimit && !isShrinking) {
            coverAlpha.ApproachRef(1f, 16f);
            radius += Time.deltaTime;
        }else if (!isShrinking) {
            isShrinking = true;
           
            if (SceneManager.GetActiveScene().buildIndex == 2) {
                SceneManager.LoadScene(1);
            }
            else {
                SceneManager.LoadScene(2);
            }
        }
        if (isShrinking) {
            if(radius < 0.2f) coverAlpha.ApproachRef(0f, 128f);
            if(radius > 0f) radius -= Time.deltaTime;
            else {
                radius = 0f;
                Destroy(gameObject);
            }
        }

        for (int i = 0; i < circles.Length; i++) {
            circles[i].localScale = radius * spdMultiplier * Vector3.one;
        }
    }
}
