using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnforceField : MonoBehaviour
{
    public float duration;        // ���� ���ӽð�
    private float durationTimer = 0f;

    private void Update()
    {
        // ���ӽð� üũ
        durationTimer += Time.deltaTime;
        if (durationTimer >= duration)
        {
            Destroy(gameObject);
            return;
        }


    }
}
