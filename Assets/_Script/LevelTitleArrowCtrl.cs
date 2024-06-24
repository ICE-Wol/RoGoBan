using UnityEngine;

public class LevelTitleArrowCtrl : MonoBehaviour
{
    public Transform leftArrow;
    public Transform rightArrow;
    
    public int basicOffset = 5;
    public float speedMultiplier = 1;
    void Update() {
        leftArrow.localPosition =
            new Vector3(basicOffset + Mathf.Abs(Mathf.Sin(speedMultiplier * Time.time)), 3, 0);
        rightArrow.localPosition =
            new Vector3(-basicOffset - Mathf.Abs(Mathf.Sin(speedMultiplier * Time.time)), 3, 0);
    }
}
