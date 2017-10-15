using UnityEngine;
using System.Collections;

/// <summary>
/// 玩家输入控制
/// </summary>
[RequireComponent (typeof(CharacterData))]
public class CharacterControl : MonoBehaviour {
	private CharacterData m_data;
	private bool _IsJump=false;
	private float _HorizontalInput;
    private float _LeftBorderX = -9;
    private float _RightBorderX = 9;
    
	void Start () {
		m_data = GetComponent<CharacterData>();
	}
	
	void Update () {
		if(Input.GetKeyDown(KeyCode.UpArrow))
		{
			_IsJump = true;
		}
		_HorizontalInput = Input.GetAxis("Horizontal");
	}

	void FixedUpdate()
	{
		m_data.Move (_IsJump, _HorizontalInput);
        _HorizontalInput = 0;
		_IsJump = false;
	}

    void OnCollisionEnter2D(Collision2D coll)
    {
        Vector2 oldPos = transform.position;
        string name = coll.gameObject.name;
        //让玩家能够从左边界直接到右边界（或者相反）
        if(name == "LeftBorder")
        {
            transform.position = new Vector2(_RightBorderX, oldPos.y);
        }
        else if(name == "RightBorder")
        {
            transform.position = new Vector2(_LeftBorderX, oldPos.y);
        }
    }

    public void MoveLeft()
    {
        _HorizontalInput = -1;
    }

    public void MoveRight()
    {
        _HorizontalInput = 1;
    }

    public void Jump()
    {
        _IsJump = true;
    }
}
