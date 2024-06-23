using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using _Scripts.Tools;
using JetBrains.Annotations;
using UnityEngine;



public class PlayerCtrl : Block {
    public List<(int,float)> actionList = new List<(int,float)>();
    public float tolerateTime = 2f;
    
    public Sprite normalSprite;
    public Sprite yinyangSprite;
    
    
    public Vector2Int[] dirVector = 
    {
        new Vector2Int(-1, 0),
        new Vector2Int(0, -1),
        new Vector2Int(1, 0),
        new Vector2Int(0, 1),
    };

    float[] timers = new float[4]; 
    public void GetInput() {

        for (int i = 0; i < timers.Length; i++) {
            timers[i] += Time.deltaTime;
        }
        
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            if (timers[0] > 0.15f) {
                actionList.Add((0,Time.time));
                timers[0] = 0;
            }
        }

        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow)) {
            timers[0] = 0;
        }

        // 检测S或下箭头键
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            if (timers[1] > 0.15f)
            {
                actionList.Add((1, Time.time));
                timers[1] = 0;
            }
        }

        // 检测S或下箭头键松开
        if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow))
        {
            timers[1] = 0;
        }

        // 检测D或右箭头键
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            if (timers[2] > 0.15f)
            {
                actionList.Add((2, Time.time));
                timers[2] = 0;
            }
        }

        // 检测D或右箭头键松开
        if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            timers[2] = 0;
        }

        // 检测W或上箭头键
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            if (timers[3] > 0.15f)
            {
                actionList.Add((3, Time.time));
                timers[3] = 0;
            }
        }

        // 检测W或上箭头键松开
        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow))
        {
            timers[3] = 0;
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
                //if (TryTransitPush(dir, targetPos)) return;  
                if (TryMergePush(dir, targetPos)) return; 
                if (TryDirectPush(dir, targetPos)) return;
                
            }
        }
    }

    private bool TryMergePush(int dir, Vector2Int targetPos) {
        var wallExist = FindFirstBlockWithoutAir(BlockType.Wall, dir, out var step);
        if (!wallExist) return false; 
               // Debug.Log("enter try");

        // 有破坏箱子的操作总是消耗pos1保留pos2
        for (int i = step - 2; i >= 0; i--) {
            var pos1 = this.pos + dirVector[dir] * i;
            var pos2 = this.pos + dirVector[dir] * (i + 1);
            

            var col1 = MapCtrl.mapCtrl.GetColorFromObject(pos1);
            var col2 = MapCtrl.mapCtrl.GetColorFromObject(pos2);
            
            var box1 = MapCtrl.mapCtrl.GetObjectFromGrid(pos1);
            var box2 = MapCtrl.mapCtrl.GetObjectFromGrid(pos2);
            
            // print(pos1 + " " + pos2);
            // print(box1.type + " " + box2.type);
            // print(col1 + " " + col2);
            
            //机制1 组合
            if (ColorPicker.colorPicker.GetColorLevel(col1) == 1 &&
                ColorPicker.colorPicker.GetColorLevel(col2) == 1 &&
                col1 != col2) {
                print("entered merge");
                MapCtrl.mapCtrl.MemGrids();

                MergeSecondBoxToFirst(pos2, pos1, col1, col2);
                PushLineOfBoxes(dir, i);
                MovePlayer(targetPos);

                return true;
            }
            
            //机制1.5 混合成白色(黑白不算互补色,不能混合成白色(机制3中和 黑白变阴阳色)
            if ((col1 + col2).SetAlpha(1f) == Color.white &&
                ((col1 != Color.white && col2 != Color.black)
                && (col1 != Color.black && col2 != Color.white))
                && (col1 != Color.gray && col2 != Color.gray)) {
                print("entered merge white");
                MapCtrl.mapCtrl.MemGrids();

                MergeSecondBoxToFirst(pos2, pos1, col1, col2);
                PushLineOfBoxes(dir, i);
                MovePlayer(targetPos);

                return true;
            }

            //机制2 转移
            if (ColorPicker.colorPicker.GetColorLevel(col1) == 2 &&
                ColorPicker.colorPicker.GetColorLevel(col2) == 2 &&
                col1 != col2) {

                MapCtrl.mapCtrl.MemGrids();
                print("entered transit");

                var color = Color.white;
                var colorExtract = color - col2;
                var colorRemain = col1 - colorExtract;

                box2.SetColor(Color.white);
                box1.SetColor(colorRemain);

                //保留退回语句以保证每一次操作都至多只发生一次操作
                return true;
            }
            
            //机制3 中和
            if((col1 == Color.white && col2 == Color.black)
               || (col1 == Color.black && col2 == Color.white)) {
                MapCtrl.mapCtrl.MemGrids();
                MergeSecondBoxToFirst(pos2, pos1, col1, col2);
                MapCtrl.mapCtrl.GetObjectFromGrid(pos2).SetColor(Color.gray);
                PushLineOfBoxes(dir, i);
                MovePlayer(targetPos);
                return true;
            }
            
            //机制4 互补 保留离墙远的箱子保证玩家留存
            if((col1 == Color.gray
                && col2 != Color.black 
                && col2 != Color.white
                && col2 != Color.gray)
               ||(col2 == Color.gray
                  && col1 != Color.black
                  && col1 != Color.white 
                  && col1 != Color.gray)
               ){
                MapCtrl.mapCtrl.MemGrids();
                
                //阴阳箱 box1
                //正常箱 box2
                
                //反色
                var newColor = (Color.white - col2).SetAlpha(1f);
                if(col2 == Color.gray) {
                    newColor = (Color.white - col1).SetAlpha(1f);
                }
                
                box1.SetColor(newColor);
                
                box2.gameObject.SetActive(false);
                MapCtrl.mapCtrl.SetObjectToGrid(pos2, null);
                
                PushLineOfBoxes(dir, i + 1);
                MovePlayer(targetPos);
                
                return true;
            }
            
            //机制5 爆发
            if(col1 == Color.gray && col2 == Color.gray) {
                MapCtrl.mapCtrl.MemGrids();
                for (int x = 0; x < MapCtrl.mapCtrl.mapSize.x; x++) {
                    for (int y = 0; y < MapCtrl.mapCtrl.mapSize.y; y++) {
                        var pos = new Vector2Int(x, y);
                        var obj = MapCtrl.mapCtrl.GetObjectFromGrid(pos);
                        if(obj == null) continue;
                        if (obj.type == BlockType.Box || obj.type == BlockType.Player) {
                            if(obj.color != Color.black && obj.color != Color.white
                               && obj.color != Color.gray) {
                                var newColor = (Color.white - obj.color).SetAlpha(1f);
                                obj.SetColor(newColor);
                            }
                           
                        }
                    }
                    
                }
                box1.gameObject.SetActive(false);
                box2.gameObject.SetActive(false);
                MapCtrl.mapCtrl.SetObjectToGrid(pos1, null);
                MapCtrl.mapCtrl.SetObjectToGrid(pos2, null);
                
                PushLineOfBoxes(dir, i);
                MovePlayer(targetPos);
                
                return true;
            }
            
            
            
        }
        return true;
    }

    private static void MergeSecondBoxToFirst(Vector2Int pos2, Vector2Int pos1, Color col1, Color col2) {
        var box2 = MapCtrl.mapCtrl.GetObjectFromGrid(pos2);
        var box1 = MapCtrl.mapCtrl.GetObjectFromGrid(pos1);
                
        //todo 或者把他放在很远的地方。为了保留地图拷贝里的索引
        //todo 回退功能关键点
        box2.gameObject.SetActive(false);
                
        // [step = 0 .. i ] [step = i + 1 .. step - 1]
        // (1) 把 pos2 的箱子颜色改掉
        // (2) 把 pos1 的箱子隐藏
        // (3) 移动它之前 [0 .. i - 1] 的箱子的位置
                
        box1.SetColor((col1 + col2).SetAlpha(1f));
        box1.pos = pos2;
        box1.tarPos = new Vector3(pos2.x, pos2.y, 0);
        MapCtrl.mapCtrl.SetObjectToGrid(pos2, box1);
        MapCtrl.mapCtrl.SetObjectToGrid(pos1, null);
                
        //Debug.LogWarning($"{this.pos} => {pos1}");
    }

    private void PushLineOfBoxes(int dir, int i) {
        //合并之后往前推
        for (int j = i - 1; j >= 0; j--) {
            // this.pos 玩家当前的位置
            // j - (0 ---- i-1) 未参加交互的箱子列
            // (i, i+1)是参与交互的两个箱子 
            var pos = this.pos + dirVector[dir] * j;
            var obj = MapCtrl.mapCtrl.GetObjectFromGrid(pos);
            MapCtrl.mapCtrl.SetObjectToGrid(pos + dirVector[dir], obj);
            MapCtrl.mapCtrl.SetObjectToGrid(pos, null);
            if (obj is BoxCtrl box) {
                box.pos = pos + dirVector[dir];
                box.tarPos = (Vector2)(pos + dirVector[dir]);
            }
        }
    }

    private void MovePlayer(Vector2Int targetPos) {
        MapCtrl.mapCtrl.SetObjectToGrid(pos, null);
        MapCtrl.mapCtrl.SetObjectToGrid(targetPos, this);
        this.pos = targetPos;
        this.tarPos = (Vector2)targetPos;
    }

    bool FindFirstBlockWithoutAir(BlockType type, int dir, out int step) {
        step = 0;
        for (int i = 1; i < 1000; i++) {
            step += 1;
            var pos = this.pos + dirVector[dir] * i;
            if (!MapCtrl.mapCtrl.IsPosValid(pos)) return false;
            if (MapCtrl.mapCtrl.GetObjectTypeFromGrid(pos) == BlockType.Empty) return false;
            if (MapCtrl.mapCtrl.GetObjectTypeFromGrid(pos) == type) return true;
        }
        return false;
    }

    private bool TryDirectPush(int dir, Vector2Int targetPos) {
        // (1) 找到第一格空格子， 没有就不动
        int pushBoxCount = 0;
        for (int i = 0; i < 1000; i++) {
            var pos = targetPos + dirVector[dir] * i;
            if (!MapCtrl.mapCtrl.IsPosValid(pos)) return false;
            if (MapCtrl.mapCtrl.GetObjectTypeFromGrid(pos) == BlockType.Wall) 
                return false;
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
        }
                
        // (3) 移动玩家
        MapCtrl.mapCtrl.SetObjectToGrid(pos, null);
        MapCtrl.mapCtrl.SetObjectToGrid(targetPos, this);
        this.pos = targetPos;
        this.tarPos = (Vector2)targetPos;

        return true;
    }

    
    public bool isEyeColorReverse = false;
    public SpriteRenderer leftEye;
    public SpriteRenderer rightEye;
    public bool isMoving => !transform.position.Equal(tarPos,0.05f);
    private void Update() {
        spriteRenderer.color = color;
        leftEye .color = isEyeColorReverse ? (Color.white-color).SetAlpha(1f) : color;
        rightEye.color = isEyeColorReverse ? (Color.white-color).SetAlpha(1f) : color;
        if (color == Color.gray) {
            spriteRenderer.sprite = yinyangSprite;
            spriteRenderer.color = Color.white;
            leftEye.color = Color.black;
            rightEye.color = Color.white;
            
            
            
        }else if(spriteRenderer.sprite) {
            spriteRenderer.sprite = normalSprite;
        }
        
        
        if (isMoving) {
            transform.position = transform.position.ApproachValue(tarPos, 6f * Vector3.one, 0.01f);
        }
        else {
            if (actionList.Count > 0) {
                if (Time.time - actionList[0].Item2 <= tolerateTime) {
                    Move(actionList[0].Item1);
                }
                actionList.RemoveAt(0);
            }
        }

        if(!MapLoader.mapLoader.CheckLevelComplete()) GetInput();
    }
}
