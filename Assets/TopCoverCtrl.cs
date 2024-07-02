using _Scripts.Tools;
using UnityEngine;

public class TopCoverCtrl : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public bool isTopCoverOpen = false;
    float curAlpha;
    float tarAlpha;
    
    public void OpenTopCover() {
        isTopCoverOpen = true;
        tarAlpha = 1.0f;
        Debug.Log("Top cover is open");
    }
    
    public void CloseTopCover() {
        isTopCoverOpen = false;
        tarAlpha = 0.0f;
        Debug.Log("Top cover is closed");
    }
    
    public void ToggleTopCover() {
        if (isTopCoverOpen) {
            CloseTopCover();
        } else {
            OpenTopCover();
        }
    }
    
    public void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
           // ToggleTopCover();
        //}
        
        if (!curAlpha.Equal(tarAlpha,0.001f)) {
            curAlpha.ApproachRef(tarAlpha, 32f);
            spriteRenderer.color = spriteRenderer.color.SetAlpha(curAlpha);
        }
        
        
    }
}
