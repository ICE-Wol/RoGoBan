using _Script;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.Serialization;

public class BoxCircle : MonoBehaviour
{
    [FormerlySerializedAs("boxPrefab")] public DecoBoxCircleCtrl boxCirclePrefab;
    public DecoBoxCircleCtrl[] boxes;
    public float radius;
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
        }
    }
    
    private void Update()
    {
        for (int i = 0; i < num; i++)
        {
            boxes[i].transform.localPosition = new Vector3(
                Mathf.Cos(Mathf.Deg2Rad * (360f / num * i + Time.time * spdMultiplier)) * radius,
                Mathf.Sin(Mathf.Deg2Rad * (360f / num * i + Time.time * spdMultiplier)) * radius,
                0);
            
        }
    }
}
