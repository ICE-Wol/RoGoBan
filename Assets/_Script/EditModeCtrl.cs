using UnityEngine;

namespace _Script {
    public class EditModeCtrl : MonoBehaviour {
        public bool isEditMode;
        public GameObject editModePanel;
        public TMPro.TMP_Text editModeText;
        
        public BlockType curEditBlock;
        public SpriteRenderer editBlock;
        public SpriteRenderer[] editBlockList; 
        public Sprite[] blockSprites;

        public Transform selections;
        public WallCtrl wallPrefab;
        public BoxCtrl boxPrefab;
        public PlayerCtrl playerPrefab;
        public SpriteRenderer tagPrefab;
        
        public ColorPicker colorPicker;
        public void InitEditBlock() {
            editBlockList = new SpriteRenderer[4];
            for (int i = 0; i < editBlockList.Length; i++) {
                editBlockList[i] = Instantiate(editBlock);
                editBlockList[i].transform.parent = selections;
                editBlockList[i].transform.localPosition = new Vector3(i, 0, 0);
                editBlockList[i].sprite = blockSprites[i];
                editBlockList[i].GetComponent<Block>().type = (BlockType)i;
                editBlockList[i].GetComponent<Block>().isTemplate = true;
                
                    
            }
        }

        public void SetEditBlockByKeyBoard() {
            if(Input.GetKeyDown(KeyCode.A)) {
                curEditBlock -= 1;
                if (curEditBlock < 0) curEditBlock = BlockType.Player;
                SetEditBlockBigger();
            }

            if (Input.GetKeyDown(KeyCode.D)) {
                curEditBlock += 1;
                if (curEditBlock > BlockType.Player) curEditBlock = BlockType.Empty;
                SetEditBlockBigger();
            }
        }
        
        public void SetEditMode() {
            if (Input.GetKeyDown(KeyCode.E)) {
                isEditMode = !isEditMode;
                editModePanel.SetActive(isEditMode);
                if (isEditMode) {
                    Camera.main.transform.position = new Vector3(0, 0, -10);
                } else {
                    int x = MapCtrl.mapCtrl.mapSize.x;
                    int y = MapCtrl.mapCtrl.mapSize.y;
                    Camera.main.transform.position
                        = new Vector3((x + y) / 4f, (x + y) / 4f, -10);
                }
                editModeText.text = 
                    (isEditMode ? "Edit Mode(E): ON" : "Edit Mode(E): OFF");
            }
        }
        
        public void SetEditBlockByMouse() {
            if (Input.GetMouseButtonDown(0)) {
                var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if (mousePos.x > 0) return;
                var hit = Physics2D.Raycast(mousePos, Vector2.zero);
                if (hit.collider != null) {
                    var block = hit.collider.GetComponent<Block>();
                    if (block != null && block.isTemplate) {
                        curEditBlock = block.type;
                        SetEditBlockBigger();
                    }
                }
            }
        }

        private void SetEditBlockBigger() {
            foreach (var b in editBlockList) {
                b.transform.localScale = Vector3.one;
            }

            editBlockList[(int)curEditBlock].transform.localScale = Vector3.one * 1.2f;
        }

        private void PutTagOnGrid() {
            if (Input.GetMouseButtonDown(2)) {
                var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                var hit = Physics2D.Raycast(mousePos, Vector2.zero);
                if (hit.collider != null) {
                    var block = hit.collider.GetComponent<Block>();
                    if (block.isTemplate) return;
                    if (block != null) {
                        MapCtrl.mapCtrl.SetColorTagToGrid(block.pos, colorPicker.curColor);
                    }
                }
            }
        }
        
        private void PutBlockOnGrid() {
            if (Input.GetMouseButton(0)) {
                var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                var hit = Physics2D.Raycast(mousePos, Vector2.zero);
                if (hit.collider != null) {
                    var block = hit.collider.GetComponent<Block>();
                    if (block.isTemplate) return;
                    if (block != null) {
                        //print(MapCtrl.mapCtrl.GetObjectTypeFromGrid(block.pos));
                        if (curEditBlock ==
                            MapCtrl.mapCtrl.GetObjectTypeFromGrid(block.pos)) {
                            //print("Same Type!");
                            return;
                        }
                        //MapCtrl.mapCtrl.MemGrids();
                        CreateNewBlock(block);
                    }
                }
            }
        }

        private void CreateNewBlock(Block block) {
            Block newBlock = null;
            Vector3 newPos = block.transform.position;
            if (curEditBlock == BlockType.Wall) {
                newBlock = Instantiate(wallPrefab, newPos, Quaternion.identity);
                newBlock.color = Color.white;
            }
            else if (curEditBlock == BlockType.Box) {
                newBlock = Instantiate(boxPrefab, newPos, Quaternion.identity);
                newBlock.tarPos = newPos;
                newBlock.pos = new Vector2Int((int)newPos.x, (int)newPos.y);
                newBlock.color = colorPicker.curColor;
            }
            else if (curEditBlock == BlockType.Player) {
                newBlock = Instantiate(playerPrefab, newPos, Quaternion.identity);
                newBlock.tarPos = newPos;
                newBlock.pos = new Vector2Int((int)newPos.x, (int)newPos.y);
                newBlock.color = colorPicker.curColor;
            }
            else if (curEditBlock == BlockType.Empty) {
                if (block.type != BlockType.Empty) {
                    Destroy(block.gameObject);
                }
            }
            else {
                Debug.LogError("Invalid Block Type!");
            }

            int x = (int)newPos.x;
            int y = (int)newPos.y;
            Vector2Int pos = new Vector2Int(x, y);
            if (MapCtrl.mapCtrl.GetObjectFromGrid(pos) != null) {
                Destroy(MapCtrl.mapCtrl.GetObjectFromGrid(pos).gameObject);
            }
            MapCtrl.mapCtrl.SetObjectToGrid(pos,newBlock);
            if(newBlock != null) newBlock.transform.parent = MapCtrl.mapCtrl.transform;
        }

        private void RemoveBlockOnGrid() {
            if (Input.GetMouseButton(1)) {
                var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                var hit = Physics2D.Raycast(mousePos, Vector2.zero);
                if (hit.collider != null) {
                    var block = hit.collider.GetComponent<Block>();
                    if (block != null) {
                        if (block.isTemplate) return;
                        //print(block.type);
                        //if (block.type == BlockType.Empty) return;
                        if(MapCtrl.mapCtrl.GetObjectFromGrid(block.pos) == null) return;
                        Destroy(MapCtrl.mapCtrl.GetObjectFromGrid(block.pos).gameObject);
                        MapCtrl.mapCtrl.SetObjectToGrid(block.pos, null);
                    }
                }
            }
        }
        

        private void Start() {
            InitEditBlock();
        }

        private void Update() {
            SetEditMode();
            if (isEditMode) {
                //SetEditBlockByKeyBoard();
                SetEditBlockByMouse();
                PutBlockOnGrid();
                PutTagOnGrid();
                RemoveBlockOnGrid();
            }
        }
    }
}