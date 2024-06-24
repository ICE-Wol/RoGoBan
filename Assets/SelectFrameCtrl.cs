using System;
using _Scripts.Tools;
using UnityEngine;

public class SelectFrameCtrl : MonoBehaviour
{
    public float scale;
    public float basicOffset = 2f;
    public float speedMultiplier = 2f;
    public float scaleXMultiplier = 1f;
    public float scaleYMultiplier = 1f;

    private void Update() {
        scale = basicOffset + Mathf.Abs(Mathf.Sin(speedMultiplier * Time.time));
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
            scaleXMultiplier *= -1;
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S))
            scaleYMultiplier *= -1;
        transform.localScale = transform.localScale.ApproachValue(
            new Vector3(scale * scaleXMultiplier, scale * scaleYMultiplier, 1), 8f);
    }
}
