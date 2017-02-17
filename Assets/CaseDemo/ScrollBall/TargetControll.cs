using UnityEngine;
using System.Collections;

namespace Assets._Scripts.ScollBall
{
    class TargetControll : MonoBehaviour
    {
        void Update()
        {
            this.GetComponent<Transform>().Rotate(new Vector3(15.0f * Time.deltaTime, 15.0f * Time.deltaTime, 15.0f * Time.deltaTime));
        }
    }
}
