using System;
using System.Collections.Generic;
using _Scripts.Tools;
using UnityEngine;
using UnityEngine.EventSystems;

public class BoxCtrl : Block {
    
    public Sprite normalBoxSprite;
    public Sprite yinyangBoxSprite;
    
    public Vector2Int[] dirVector = 
    {
        new Vector2Int(-1, 0),
        new Vector2Int(0, -1),
        new Vector2Int(1, 0),
        new Vector2Int(0, 1),
    };
    
    public void Move(int dir) {
        tarPos = (Vector2)(pos + dirVector[dir]);
        MapCtrl.mapCtrl.SetObjectToGrid(pos, null);
        MapCtrl.mapCtrl.SetObjectToGrid(pos + dirVector[dir], this); 
    }
    
    protected new void Start() {
        base.Start();
        spriteRenderer.color = ColorPicker.colorPicker.curColor;
    }


    protected void Update() {
        base.Update();
        //spriteRenderer.color = color;
        if (color == Color.gray) {
            spriteRenderer.sprite = yinyangBoxSprite;
            spriteRenderer.color = Color.white;
        }else if(spriteRenderer.sprite) {
            spriteRenderer.sprite = normalBoxSprite;
        }
        
        if (!transform.position.Equal(tarPos, 0.01f)) {
            transform.position = transform.position.ApproachValue(tarPos, 8f);
        }
    }
}
