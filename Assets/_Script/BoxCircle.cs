using _Script;
using _Scripts.Tools;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.Serialization;

public class BoxCircle : MonoBehaviour
{
    [FormerlySerializedAs("boxPrefab")] public DecoBoxCircleCtrl boxCirclePrefab;
    public DecoBoxCircleCtrl[] boxes;
    public float radius;
    public float memRadius;
    public float tarRadius;
    public int num;
    public int scale;
    
    public float spdMultiplier;
    
    private void Start()
    {
        boxes = new DecoBoxCircleCtrl[num];
        for (int i = 0; i < num; i++)
        {
            var box = Instantiate(boxCirclePrefab, transform);
            box.transform.localPosition = new Vector3(
                Mathf.Cos(Mathf.Deg2Rad * (360f / num * i)) * radius,
                Mathf.Sin(Mathf.Deg2Rad * (360f / num * i)) * radius,
                0);
            box.transform.localScale = Vector3.one * scale;
            box.circleHeart = this;
            boxes[i] = box;
            box.index = i;
            box.boxSum = num;
        }
        tarRadius = radius;
        memRadius = radius;
        
        radius = 0;
    }
    
    private void Update()
    {
        if (GameManager.Manager.isLevelComplete) {
            tarRadius = -memRadius;
        }
        else {
            tarRadius = memRadius;
        }
        radius.ApproachRef(tarRadius ,128f);
        
        
        for (int i = 0; i < num; i++)
        {
            var deg = 360f / num * i + Time.time * spdMultiplier;
            boxes[i].transform.localPosition = new Vector3(
                Mathf.Cos(Mathf.Deg2Rad * deg) * radius, Mathf.Sin(Mathf.Deg2Rad * deg) * radius, 0);
            
        }
    }
}
