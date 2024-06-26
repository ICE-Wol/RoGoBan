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
    
    public void SetColor(Color c) {
        spriteRenderer.color = c;
    }

    private void Start() {
        direction = Random.Range(0, 360);
    }

    public void Update() {
        spriteRenderer.color = spriteRenderer.color.Fade(14f);
        //localScale = Mathf.Sin(Time.time * 10) * 0.2f;
        localScale = Mathf.Sin(Time.time * 2f) * 0.5f;
        transform.localScale = new Vector3(localScale, localScale, 1);
        
        transform.position += direction.Deg2Dir3() * Time.deltaTime * speed;
    }
}
