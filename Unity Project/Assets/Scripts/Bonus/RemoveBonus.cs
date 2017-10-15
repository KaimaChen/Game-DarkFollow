using UnityEngine;
using System.Collections;

/// <summary>
/// 消除影子的奖励
/// </summary>
public class RemoveBonus : MonoBehaviour {
    public float Speed = 2;
    public float Phase = 0.05f;
    public float ChaseSpeed = 12;
    
    private bool _ChaseShadow = false;
    
    void Start () {
        gameObject.SetActive(false);
	}
	
	void Update () {
        if(_ChaseShadow)
        {
            ChaseShadow();
        }
        else
        {
            Move();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Border"))
        {
            gameObject.SetActive(false);
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            _ChaseShadow = true;
        }
        else if(other.gameObject.layer == LayerMask.NameToLayer("Shadow"))
        {
            if(_ChaseShadow)
            {
                _ChaseShadow = false;
                gameObject.SetActive(false);
                ShadowManager.Instance.RemoveShadow();
            }
        }
    }

    public void CreateAt(Vector3 pos)
    {
        gameObject.SetActive(true);
        transform.position = pos;
    }

    /// <summary>
    /// 波浪形移动（三角函数模拟）
    /// </summary>
    void Move()
    {
        Vector3 oldPos = transform.position;
        float x = oldPos.x - Time.deltaTime * Speed;
        float y = oldPos.y - Mathf.Cos(x) * Phase;
        transform.position = new Vector3(x, y, oldPos.z);
    }

    /// <summary>
    /// 追逐影子
    /// </summary>
    void ChaseShadow()
    {
        if (ShadowManager.Instance.GetShadowsCount() > 0)
        {
            Vector3 shadowPos = ShadowManager.Instance.LastShadowPos();
            Vector3 dir = (shadowPos - transform.position).normalized;
            transform.position += dir * Time.deltaTime * ChaseSpeed;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
