using System;
using UnityEngine;

public class BoxCircleDecoCtrl : MonoBehaviour
{
   public MeshRenderer meshRenderer;

   private void Start() {
      meshRenderer.sortingLayerName = "UpDeco";
   }
}
