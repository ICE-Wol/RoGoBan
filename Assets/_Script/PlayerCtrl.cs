using System;
using System.Collections.Generic;
using _Scripts.Tools;
using JetBrains.Annotations;
using UnityEngine;



public class PlayerCtrl : Block {
    public BlockType blockType;
    
    public List<(int,float)> actionList = new List<(int,float)>();
    public float tolerateTime = 2f;
    
    
    public Vector2Int[] dirVector = 
    {
        new Vector2Int(-1, 0),
        new Vector2Int(0, -1),
        new Vector2Int(1, 0),
        new Vector2Int(0, 1),
    };

    public int GetInput() {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) {
            return 0;
        } else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) {
            return 1;
        } else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) {
            return 2;
        } else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) {
            return 3;
        } else {
            return -1;
        }
    }


    public void Move(int dir) {
        var targetPos = pos + dirVector[dir];
        if (MapCtrl.mapCtrl.IsPosValid(targetPos)) {
            var targetBlock = MapCtrl.mapCtrl.GetObjectFromGrid(targetPos);
            if (targetBlock == null) {
                MapCtrl.mapCtrl.MemGrids();
                
                MapCtrl.mapCtrl.SetObjectToGrid(pos, null);
                MapCtrl.mapCtrl.SetObjectToGrid(targetPos, this);
                pos = targetPos;
                tarPos = new Vector3(pos.x, pos.y, 0);
            }else if (targetBlock.type == BlockType.Box) {
                
                // (1) 找到第一格空格子， 没有就不动
                int pushBoxCount = 0;
                for (int i = 0; i < 1000; i++) {
                    var pos = targetPos + dirVector[dir] * i;
                    if(!MapCtrl.mapCtrl.IsPosValid(pos)) return;
                    if(MapCtrl.mapCtrl.GetObjectTypeFromGrid(pos) == BlockType.Wall) return;
                    if (MapCtrl.mapCtrl.GetObjectTypeFromGrid(pos) == BlockType.Empty) {
                        pushBoxCount = i;
                        break;
                    }
                }
                
                //移动前拷贝
                MapCtrl.mapCtrl.MemGrids();
                // (2) 移动箱子
                for(int i = pushBoxCount - 1; i >= 0; i--) {
                    var pos = targetPos + dirVector[dir] * i;
                    var obj = MapCtrl.mapCtrl.GetObjectFromGrid(pos);
                    MapCtrl.mapCtrl.SetObjectToGrid(pos + dirVector[dir], obj);
                    MapCtrl.mapCtrl.SetObjectToGrid(pos, null);
                    if (obj is BoxCtrl box) {
                        box.tarPos = (Vector2)(pos + dirVector[dir]);
                    }
                    // (obj as BoxCtrl).Move(dir);
                }
                
                // (3) 移动玩家
                MapCtrl.mapCtrl.SetObjectToGrid(pos, null);
                MapCtrl.mapCtrl.SetObjectToGrid(targetPos, this);
                this.pos = targetPos;
                this.tarPos = (Vector2)targetPos;
                
                
            }
        }
    }
    
    public bool isMoving => !transform.position.Equal(tarPos,0.01f);
    private void Update() {
        
        if (isMoving) {
            transform.position = transform.position.ApproachValue(tarPos, 8f);
        }
        else {
            if (actionList.Count > 0) {
                if (Time.time - actionList[0].Item2 <= tolerateTime) {
                    Move(actionList[0].Item1);
                }
                actionList.RemoveAt(0);
            }
        }

        int dir = GetInput();
        if(dir == -1) return;
        actionList.Add((dir,Time.time));
    }
}
