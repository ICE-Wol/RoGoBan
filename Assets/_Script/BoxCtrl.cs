using System;
using System.Collections.Generic;
using _Scripts.Tools;
using UnityEngine;
using UnityEngine.EventSystems;

public class BoxCtrl : Block {
    public static List<BoxCtrl> boxList = new List<BoxCtrl>();
    
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
    
    private void Start() {
        boxList.Add(this);
        spriteRenderer.color = ColorPicker.colorPicker.curColor;
    }


    private void Update() {
        spriteRenderer.color = color;
        if (!transform.position.Equal(tarPos, 0.01f)) {
            transform.position = transform.position.ApproachValue(tarPos, 16f);
        }
    }
}
