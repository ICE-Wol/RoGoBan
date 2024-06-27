using System;
using _Scripts.Tools;
using UnityEngine;
using Random = UnityEngine.Random;

public class ParticleCtrl : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    
    public float localScale = 1f;
    public float direction = 0f;
    public float speed = 1f;

    public float initTime;
    
    public void SetColor(Color c) {
        spriteRenderer.color = c;
    }

    private void Start() {
        direction = Random.Range(0, 360);
        initTime = Time.time;
        transform.position += 
            new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0);
    }

    public void Update() {
        spriteRenderer.color = spriteRenderer.color.Fade(32f);
        //localScale = Mathf.Sin(Time.time * 10) * 0.2f;
        localScale = Mathf.Abs(Mathf.Sin((Time.time - initTime) * 2f) * 0.2f);
        transform.localScale = new Vector3(localScale, localScale, 1);
        
        transform.position += direction.Deg2Dir3() * Time.deltaTime * speed;

        if (spriteRenderer.color.a.Equal(0f, 0.01f)) {
            Destroy(gameObject);
        }
    }
}
