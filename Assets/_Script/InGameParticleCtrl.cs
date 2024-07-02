using System;
using _Scripts.Tools;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;

public class InGameParticleCtrl : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Rotator rotator;
    
    public Vector3 centerPos;
    public float radius;
    public float floatSpeed;
    
    public float degree;
    public float baseDegree;
    public float degAmplitude;
    public float spdMultiplier;

    public float curScale;
    public float tarScale;

    public float lifeTimer;

    private void Start() {
        spriteRenderer.color = GameManager.Manager.playerColor.SetAlpha(0f);
        curScale = 0;
        tarScale = Random.Range(0.5f, 1.5f);
        lifeTimer = Random.Range(0f, 6f);
        radius = Random.Range(2f, 6f);
        floatSpeed = Random.Range(0.5f, 2f);
        baseDegree = Random.Range(0, 360f);
        degAmplitude = Random.Range(-20f, 20f);
        spdMultiplier = Random.Range(-2f, 2f);
        
        rotator.initDegree = Random.Range(0, 360f);
        rotator.rotateMultiplier = Random.Range(-2f, 2f);
    }

    private void Update() {
        centerPos = GameManager.Manager.cameraPos.SetZ(10f);
        
        lifeTimer -= Time.deltaTime;
        
        curScale.ApproachRef(tarScale, 128f);
        transform.localScale = Vector3.one * curScale;
        if (lifeTimer <= 0) {
            tarScale = 0;
            if(curScale <= 0.01f)
                Destroy(gameObject);
        }

        spriteRenderer.color = spriteRenderer.color.Appear(32f);
        
        radius += Time.deltaTime * floatSpeed;
        degree = baseDegree + degAmplitude * Mathf.Sin(Time.time * spdMultiplier);
        transform.position = centerPos + new Vector3(
            Mathf.Cos(Mathf.Deg2Rad * degree) * radius, 
            Mathf.Sin(Mathf.Deg2Rad * degree) * radius, 0);
        
    }
}
