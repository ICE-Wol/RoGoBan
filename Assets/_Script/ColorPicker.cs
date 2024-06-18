using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


public class ColorPicker : MonoBehaviour {
    public static ColorPicker colorPicker;
    
    private void Awake() {
        if (colorPicker == null) {
            colorPicker = this;
        }
        else {
            Destroy(gameObject);
        }
    }
    
    public Color[] basicColors = new [] {
        Color.red,
        Color.green,
           Color.blue,
    };

    public Color[] compoundColors = new[] {
        Color.yellow,
        Color.cyan,
        Color.magenta,
    };
    public Color[] specialColors = new [] {
        Color.black,
        Color.gray, 
        Color.white,
    };
    
    public SpriteRenderer colorBlock;
    public List<SpriteRenderer> colorBlockList;
    
    public SpriteRenderer colorDisplay;
    public Color curColor;

    private void Start() {
        basicColors = new[] {
            Color.red,
            Color.green,
            Color.blue,
        };

        compoundColors = new[] {
            Color.yellow,
            Color.cyan,
            Color.magenta,
        };
        specialColors = new[] {
            Color.black,
            Color.gray,
            Color.white,
        };

        colorBlockList = new List<SpriteRenderer>();
        for (int i = 0; i < basicColors.Length; i++) {
            var b = Instantiate(colorBlock, transform);
            b.transform.localPosition = new Vector3(i, 1, 0);
            b.color = basicColors[i];
            colorBlockList.Add(b);
        }

        for (int i = 0; i < compoundColors.Length; i++) {
            var b = Instantiate(colorBlock, transform);
            b.transform.localPosition = new Vector3(i, 2, 0);
            b.color = compoundColors[i];
            colorBlockList.Add(b);
        }

        for (int i = 0; i < specialColors.Length; i++) {
            var b = Instantiate(colorBlock, transform);
            b.transform.localPosition = new Vector3(i, 3, 0);
            b.color = specialColors[i];
            colorBlockList.Add(b);
        }
        
        curColor = Color.white;
    }
    
    public Color GetColorByMouse() {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //todo 
        if (mousePos.x < 0 || mousePos.y > 0) return Color.clear;
        var hit = Physics2D.Raycast(mousePos, Vector2.zero);
        if (hit.collider != null) {
            var block = hit.collider.GetComponent<SpriteRenderer>();
            if (block != null) {
                colorDisplay.color = block.color;
                return block.color;
            }
        }

        return Color.clear;
    }


    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            var color = GetColorByMouse();
            if (color != Color.clear) {
                curColor = color;
                Debug.Log("Color: " + color);
            }else {
                Debug.Log("No Color");
            }
        }
    }
}
