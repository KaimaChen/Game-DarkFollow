using UnityEngine;

/// <summary>
/// 实现一个影子需要的数据结构
/// </summary>
public class ShadowData
{
    public Vector3 Pos; //位置
    public Vector3 Scale; //缩放（用于决定面向）
    //以下都是动画状态机中的变量
    public float Speed; 
    public float VerticalSpeed;
    public bool IsJump;

    public ShadowData(Vector3 pos, Vector3 scale, float speed, float verticalSpeed, bool isJump)
    {
        Pos = pos;
        Scale = scale;
        Speed = speed;
        VerticalSpeed = verticalSpeed;
        IsJump = isJump;
    }
}

public class Shadow : MonoBehaviour {
    public int Index = 0; //要读取的玩家数据集中的倒数index
    public Animator _Animator;

    public const int GAP_TO_PLAYER = 80; //第一个影子距离玩家的距离（即这数字之后的数据才能被读）

    private SpriteRenderer _Renderer;

    //以下是实现闪烁需要
    private bool _IsBlink = true;
    private float _BlinkTime = 2f;
    private float _BlinkTimer = 0;
    private bool _IsFading = true;
    private float _FadeDelta = 0.1f;
    
    void Awake()
    {
        _Renderer = transform.FindChild("Sprite").GetComponent<SpriteRenderer>();
    }
    
    void Update()
    {
        if (GameManager.IsPause)
            return;

        if (_IsBlink)
        {
            _BlinkTimer += Time.deltaTime;
            if(_BlinkTimer >= _BlinkTime)
            {
                _IsBlink = false;
                _Renderer.color = new Color(1, 1, 1, 1);
            }
            else
            {
                Color oldColor = _Renderer.color;
                float oldAlpha = _Renderer.color.a;
                float newAlpha = oldAlpha + (_IsFading ? -_FadeDelta : _FadeDelta);
                if (newAlpha <= 0 || newAlpha >= 1)
                {
                    _IsFading = !_IsFading;
                }
                _Renderer.color = new Color(oldColor.r, oldColor.g, oldColor.b, newAlpha);
            }
        }
    }

	public void Refresh (CharacterData player) {
        int minSize = Index + GAP_TO_PLAYER; 
	    if(player.ShadowDatas.Count > minSize)
        {
            this.gameObject.SetActive(true);
            int index = player.ShadowDatas.Count - 1 - GAP_TO_PLAYER - Index; //读倒数的数据
            ShadowData data = player.ShadowDatas[index];
            RefreshStateBy(data);
        }
        else
        {
            this.gameObject.SetActive(false); //防止没数据时，影子傻乎乎地站在初始位置（正常来说不会发生这种情况）
        }
	}

    void RefreshStateBy(ShadowData data)
    {
        transform.position = data.Pos;
        transform.localScale = data.Scale;
        _Animator.SetBool("Jump", data.IsJump);
        _Animator.SetFloat("Speed", data.Speed);
        _Animator.SetFloat("VerticalSpeed", data.VerticalSpeed);
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        IfTouchPlayer(coll);
    }

    void OnTriggerStay2D(Collider2D coll)
    {
        IfTouchPlayer(coll);
    }

    void IfTouchPlayer(Collider2D coll)
    {
        if (coll.gameObject.layer == LayerMask.NameToLayer("Player") && _IsBlink == false)
        {
            Destroy(gameObject);
            GameManager.GameOver();
        }
    }
}
