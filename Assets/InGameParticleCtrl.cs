using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class InGameParticleCtrl : MonoBehaviour
{
    public Vector3 centerPos;
    public float radius;
    
    public float baseDegree;
    public float degree;
    
    public float floatSpeed; 
    
    private void Update() {
        radius += Time.deltaTime * floatSpeed;
        degree = baseDegree * Mathf.Sin(Time.time * 2);
        transform.position = centerPos + new Vector3(
            Mathf.Cos(Mathf.Deg2Rad * degree) * radius, 
            Mathf.Sin(Mathf.Deg2Rad * degree) * radius, 0);
    }
}
