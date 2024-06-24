using System;
using _Scripts.Tools;
using UnityEngine;

namespace _Script
{
    public class DecoBoxCircleCtrl : MonoBehaviour
    {
        public BoxCircle circleHeart;

        public float curHeight;
        public float tarHeight;

        public float index;
        public float boxSum;

        private void Start()
        {
            tarHeight = 15f;
        }

        private void Update() {
            curHeight.ApproachRef(tarHeight, 16f);
            tarHeight = 15f * Mathf.Sin(Time.time / 2f + (index * 2f / boxSum * 2f * Mathf.PI));
            //if (Input.anyKeyDown) tarHeight *= -1f;
            //else tarHeight = 15f;
            
            // 计算从当前对象到目标对象的方向
            Vector3 direction = circleHeart.transform.position + curHeight * Vector3.back - transform.position;

            // 计算朝向目标的旋转
            Quaternion lookRotation = Quaternion.LookRotation(direction, Vector3.forward);


            transform.rotation = lookRotation;


        }
    }
}