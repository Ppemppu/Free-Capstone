using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpellBase : MonoBehaviour
{
    public float radius;
    private ObjectDetector detector;

    protected bool isTargeting = false;
    protected GameObject rangeIndicator;

    [SerializeField] protected Texture2D rangeTexture; // 스펠 범위 텍스처

    private void Awake()
    {
         detector = GetComponent<ObjectDetector>();
    }

    protected virtual void Start()
    {
        CreateRangeIndicator();
        rangeIndicator.SetActive(false);
    }

    protected virtual void CreateRangeIndicator() //스펠 범위 미리보기
    {
        rangeIndicator = new GameObject("RangeIndicator");
        SpriteRenderer spriteRenderer = rangeIndicator.AddComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = 10;
        // rangeTexture가 설정되었는지 확인하고 사용
        if (rangeTexture != null)
        {
            Sprite sprite = Sprite.Create(
                rangeTexture,
                new Rect(0, 0, rangeTexture.width, rangeTexture.height),
                new Vector2(0.5f, 0.5f)
            );

            spriteRenderer.sprite = sprite;
            spriteRenderer.color = new Color(1f, 1f, 1f, 0.5f); // 투명도 조절
        }
        else
        {
            Debug.LogWarning("Range Texture가 설정되지 않았습니다.");
        }
        
        // 범위에 맞게 스프라이트 크기 조정
        float scale = radius * 2 / spriteRenderer.sprite.bounds.size.x;
        rangeIndicator.transform.localScale = new Vector3(scale, scale, 1);
    }
    public virtual void StartTargeting()
    {
        isTargeting = true;
        rangeIndicator.SetActive(true);
        detector.setFlag(false);
    }

    protected virtual void EndTargeting()
    {
        isTargeting = false;
        rangeIndicator.SetActive(false);
    }
    private IEnumerator DelaySetFlag()
    {
        yield return new WaitForSeconds(0.1f); // 0.1초 딜레이
        detector.setFlag(true);
    }

    protected virtual void Update()
    {
        if (isTargeting)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            rangeIndicator.transform.position = mousePosition;

            if (Input.GetMouseButtonDown(0))
            {
                CastSpell(mousePosition);
                EndTargeting();
                StartCoroutine(DelaySetFlag());
            }
        }
    }

    protected abstract void CastSpell(Vector3 position);  // 각 스펠이 구현해야 할 메소드
}

