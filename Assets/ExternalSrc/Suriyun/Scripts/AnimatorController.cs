using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
namespace Suriyun
{
    public class AnimatorController : MonoBehaviour
    {

        public Animator[] animators;

        public void SwapVisibility(GameObject obj)
        {
            obj.SetActive(!obj.activeSelf);
        }


        public void SetFloat(string parameter = "key,value")
        {
            char[] separator = { ',', ';' };
            string[] param = parameter.Split(separator);

            string name = param[0];
            float value = (float)Convert.ToDouble(param[1]);

            Debug.Log(name + " " + value);

            foreach (Animator a in animators)
            {
                a.SetFloat(name, value);
            }
        }
        public void SetInt(string parameter = "key,value")
        {
            char[] separator = { ',', ';' };
            string[] param = parameter.Split(separator);

            string name = param[0];
            int value = Convert.ToInt32(param[1]);

            Debug.Log(name + " " + value);

            foreach (Animator a in animators)
            {
                a.SetInteger(name, value);
            }

            // 3초 뒤 0번으로 값을 설정
            Invoke("SetIntToZero", 3.0f);
        }

        private void SetIntToZero()
        {
            foreach (Animator a in animators)
            {
                a.SetInteger("animation", 1);
            }
        }

        public void SetBool(string parameter = "key,value")
        {
            char[] separator = { ',', ';' };
            string[] param = parameter.Split(separator);

            string name = param[0];
            bool value = Convert.ToBoolean(param[1]);

            Debug.Log(name + " " + value);

            foreach (Animator a in animators)
            {
                a.SetBool(name, value);
            }
        }

        public void SetTrigger(string parameter = "key,value")
        {
            char[] separator = { ',', ';' };
            string[] param = parameter.Split(separator);

            string name = param[0];

            Debug.Log(name);

            foreach (Animator a in animators)
            {
                a.SetTrigger(name);
            }
        }
    }
}