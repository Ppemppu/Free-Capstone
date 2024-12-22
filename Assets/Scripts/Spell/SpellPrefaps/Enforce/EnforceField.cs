using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnforceField : MonoBehaviour
{
    public float duration;        // 장판 지속시간
    private float durationTimer = 0f;

    private void Update()
    {
        // 지속시간 체크
        durationTimer += Time.deltaTime;
        if (durationTimer >= duration)
        {
            Destroy(gameObject);
            return;
        }


    }
}
