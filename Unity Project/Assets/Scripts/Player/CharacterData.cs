using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 存放玩家数据
/// </summary>
[RequireComponent (typeof(Rigidbody2D))]
[RequireComponent (typeof(Collider2D))]
public class CharacterData : MonoBehaviour {
    public Animator _Animator;
    public CameraShake CamShake;
    public List<ShadowData> ShadowDatas;

    [SerializeField]
	private float JumpForce = 300;
	[SerializeField]
	private float MoveSpeed = 20;
	[SerializeField]
	private Transform Foot;
	[SerializeField]
	private float Radius=0.01f;

	private Rigidbody2D _Rigid;
	private float _Move = 0;
	private bool _IsGrounded=false;
    private float _HighestPosY = 0;
    
	void Start () {
		_Rigid = GetComponent<Rigidbody2D>();
		if(Foot == null)
		{
			Foot = this.transform.Find("Foot");
		}

        ShadowDatas = new List<ShadowData>();
	}
    
    void Update()
    {
        AddShadowData();
    }
   
	void FixedUpdate () {
        //记录在空中的最高高度
        if(_IsGrounded == false && transform.position.y > _HighestPosY)
        {
            _HighestPosY = transform.position.y;
        }

        bool oldIsGrounded = _IsGrounded;

        //检测是否踩在地板上
        Collider2D[] colliders = Physics2D.OverlapCircleAll(Foot.transform.position, Radius);
        _IsGrounded = false;
        if (colliders != null)
		{
			for(int i=0; i < colliders.Length; i++)
			{
				if(colliders[i].gameObject.layer == LayerMask.NameToLayer("Ground"))
				{
					_IsGrounded = true;
				}
			}
        }

        _Animator.SetBool("Jump", !_IsGrounded);
        _Animator.SetFloat("VerticalSpeed", _Rigid.velocity.y);

        //根据玩家跳跃高度来决定摄像机的震动幅度
        if(oldIsGrounded == false && _IsGrounded == true)
        {
            float jumpDistance = _HighestPosY - transform.position.y;
            CamShake.Shake(jumpDistance);
            _HighestPosY = transform.position.y;
        }
    }

	public void Move(bool jump, float horizontalInput)
	{
		if(jump && _IsGrounded)
		{
			_Rigid.AddForce (new Vector2(0, JumpForce));
            _Animator.SetBool("Jump", true);
		}

		_Move = horizontalInput*MoveSpeed;
        _Rigid.velocity = new Vector2(_Move, _Rigid.velocity.y);
        _Animator.SetFloat("Speed", Mathf.Abs(_Move));

        //决定面向
        if(_Move != 0)
        {
            Vector3 oldScale = transform.localScale;
            float scaleX = _Move > 0 ? 1 : -1;
            transform.localScale = new Vector3(scaleX, oldScale.y, oldScale.z);
        }
    }

    //记录阴影数据
    public void AddShadowData()
    {
        Vector3 pos = transform.position;
        Vector3 scale = transform.localScale;
        float speed = _Animator.GetFloat("Speed");
        float verticalSpeed = _Animator.GetFloat("VerticalSpeed");
        bool isJump = _Animator.GetBool("Jump");

        ShadowData data = new ShadowData(pos, scale, speed, verticalSpeed, isJump);
        ShadowDatas.Add(data);
    }
}
