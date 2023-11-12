using UnityEngine;
using System;
using System.Collections;

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
            var param = parameter.Split(',', ';');
            string name = param[0];
            float value = Convert.ToSingle(param[1]);

            Debug.Log(name + " " + value);

            foreach (Animator a in animators)
            {
                a.SetFloat(name, value);
            }
        }

        public void SetInt(string parameter = "key,value")
        {
            // 새로운 입력이 들어올 때 모든 Invoke를 취소
            CancelInvoke();

            var param = parameter.Split(',', ';');
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
            var param = parameter.Split(',', ';');
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
            var param = parameter.Split(',', ';');
            string name = param[0];

            Debug.Log(name);

            foreach (Animator a in animators)
            {
                a.SetTrigger(name);
            }
        }
    }
}
