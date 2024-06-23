using System;
using _Scripts.Tools;
using UnityEngine;

public class DecoBoxLineCtrl : MonoBehaviour
{
    public SpriteRenderer boxPrefab;
    
    public SpriteRenderer[] boxes;
    
    public int num;
    public float interval;
    
    public bool isReverse;
    public bool isDown;

    private void Start()
    {
        boxes = new SpriteRenderer[num];
        for (int i = 0; i < num; i++)
        {
            var box = Instantiate(boxPrefab, transform);
            box.transform.localPosition = new Vector3(i * interval, 0, 0);
            box.color = i % 3 == 0 ? Color.red : i % 3 == 1 ? Color.green : Color.blue;
            if (isDown) {
                box.color = i % 3 == 0 ? Color.cyan : i % 3 == 1 ? Color.magenta : new Color(1,1,0,1);
            }
            box.color = (box.color / 1.2f).SetAlpha(1f);
            boxes[i] = box;
        }
    }
    
    private void Update()
    {
        if (!isReverse)
        {
            for (int i = 0; i < num; i++)
            {
                boxes[i].transform.localPosition +=
                    Time.deltaTime *
                    Vector3.right; //new Vector3(i * interval + Time.time, Mathf.Sin(Time.time + i) / 5f, 0);
                if (boxes[i].transform.localPosition.x > 21 * 1.5f - 1f)
                {
                    boxes[i].transform.localPosition = new Vector3(-1f, 0, 0);
                }
            }
        }
        else
        {
            for (int i = 0; i < num; i++)
            {
                boxes[i].transform.localPosition -= Time.deltaTime * Vector3.right;
                if (boxes[i].transform.localPosition.x < -1f)
                {
                    boxes[i].transform.localPosition = new Vector3(21 * 1.5f - 1f, 0, 0);
                }
            }
        }
    }
}
