using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using Debug = UnityEngine.Debug;


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
    
    static Color Red => Color.red;
    static Color Green => Color.green;
    static Color Blue => Color.blue;
        
    static Color Yellow => new Color(1,1,0,1);
    static Color Cyan => Color.cyan;
    static Color Magenta => Color.magenta;
    
    static Color Black => Color.black;
    static Color YinYang => Color.gray;
    static Color White => Color.white;

    public int GetColorLevel(Color c) {
        if (c == Red || c == Green || c == Blue) {
            return 1;
        }else if (c == Yellow || c == Cyan || c == Magenta) {
            return 2;
        }else if (c == Black || c == YinYang || c == White) {
            return 3;
        }else {
            return -1;
        }
    }
    
    
    public Color[] basicColors = new [] {
        Color.red,
        Color.green,
           Color.blue,
    };

            
    public Color[] compoundColors = new[] {
        new Color(1,1,0,1),
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
            new Color(1,1,0,1),
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
                //Debug.Log("Color: " + color);
            }else {
                //.Log("No Color");
            }
        }
    }
}
