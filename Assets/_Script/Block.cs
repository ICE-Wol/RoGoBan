using System;
using System.Collections.Generic;
using UnityEngine;

public class HistoryRecord {
    public Vector2Int pos;
    public Color color;
    public int time;
    public HistoryRecord(Vector2Int pos, Color color) {
        this.pos = pos;
        this.color = color;
    }
}
public class Block : MonoBehaviour {
    public BlockType type;
    public bool isTemplate;
    public Color color;
    /// <summary>
    /// 逻辑位置
    /// </summary>
    public Vector2Int pos;
    
    /// <summary>
    /// 动画目标位置
    /// </summary>
    public Vector3 tarPos;

    public List<HistoryRecord> HistoryPos;

    private void Awake() {
        HistoryPos = new List<HistoryRecord>();
    }

    // public void UndoAction() {
    //     if (HistoryPos.Count <= 1) { return; }
    //     
    //     pos = HistoryPos[^2].pos;
    //     tarPos = new Vector3(pos.x, pos.y, 0);
    //     color = HistoryPos[^2].color;
    //     
    //     var time = HistoryPos[^2].time;
    //     foreach (var b in BoxCtrl.boxList) {
    //         if(b.HistoryPos[^2].time == time) b.UndoAction(time);
    //     }
    //     
    //     MapCtrl.mapCtrl.SetObjectToGrid(HistoryPos[^1].pos, null);
    //     MapCtrl.mapCtrl.SetObjectToGrid(pos, this);
    //     HistoryPos.RemoveAt(HistoryPos.Count - 1);
    //     
    // }
}
