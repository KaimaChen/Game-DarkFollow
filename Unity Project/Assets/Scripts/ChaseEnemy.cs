using UnityEngine;
using System.Collections;

/// <summary>
/// 追踪型敌人
/// </summary>
public class ChaseEnemy : MonoBehaviour {
    public float AliveTime = 10; //存活时间
    public float Speed = 4; //移动速度

    private Transform _Player;
    private float _Timer = 0;
    private SpriteRenderer _SpriteRenderer;

    private float _FadeTime = 2; //闪烁时长
    private bool _IsFade = true; //是否正在消隐
    private float _FadeDelta = 0.05f; //每帧alpha变化值

	void Start () {
        _Player = GameObject.FindGameObjectWithTag("Player").transform;
        _SpriteRenderer = transform.FindChild("Sprite").GetComponent<SpriteRenderer>();
	}
	
	void Update () {
        UpdateTime();

        MoveToTarget();
	}

    void UpdateTime()
    {
        _Timer += Time.deltaTime;
        if (_Timer >= AliveTime) //时间都就销毁
        {
            Destroy(this.gameObject);
        }
        else if(_Timer > AliveTime - _FadeTime) //否则就变化alpha值来实现闪烁
        {
            Color oldColor = _SpriteRenderer.color;
            float oldAlpha = _SpriteRenderer.color.a;
            float newAlpha = oldAlpha + (_IsFade ? -_FadeDelta : _FadeDelta);
            if(newAlpha <=0 || newAlpha >= 1)
            {
                _IsFade = !_IsFade;
            }
            _SpriteRenderer.color = new Color(oldColor.r, oldColor.g, oldColor.b, newAlpha);
        }
    }
    
    void MoveToTarget()
    {
        Vector3 dir = (_Player.position - transform.position).normalized;
        transform.position += dir * Time.deltaTime * Speed;
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            GameManager.GameOver();
        }
    }
}
