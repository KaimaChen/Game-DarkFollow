using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class Bonus : MonoBehaviour {
    public Sprite NormalBonus; //加分
    public Sprite ShadowBonus; //能加分，但同时会产生一个影子
    public Sprite ChaseBonus; //能累计加分，但同时产生追逐玩家的敌人
    
    private SpriteRenderer _Renderer;
    private BonusType _Type = BonusType.Normal;
    
	void Awake () {
        _Renderer = this.GetComponent<SpriteRenderer>();
        this.gameObject.SetActive(false);
	}
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            this.gameObject.SetActive(false);

            BonusManager.Instance.AddScore(_Type);
            
            //额外效果
            if(_Type == BonusType.Shadow)
            {
                ShadowManager.Instance.CreateShadow();
            }
            else if(_Type == BonusType.Chase)
            {
                BonusManager.Instance.CreateChaseEnemy();
            }

            //只有在拿到normal和shadow才产生下一个奖励
            if(_Type == BonusType.Normal || _Type == BonusType.Shadow)
            {
                BonusManager.Instance.RandomCreateBonus();
            }
        }
    }

    public void CreateBonus(BonusType type)
    {
        _Type = type;
        if(_Type == BonusType.Normal)
        {
            _Renderer.sprite = NormalBonus;
        }
        else if(_Type == BonusType.Shadow)
        {
            _Renderer.sprite = ShadowBonus;
        }
        else if(_Type == BonusType.Chase)
        {
            _Renderer.sprite = ChaseBonus;
        }

        this.gameObject.SetActive(true);
    }

    public void CreateBonus(BonusType type, Vector3 pos)
    {
        CreateBonus(type);
        transform.position = pos;
    }
}
