using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BonusManager : MonoBehaviour {
    public static BonusManager Instance = null;
    public List<Bonus> Bonuses;

    public RemoveBonus RemoveBonus;
    public Transform RemoveBonusPos;

    public Bonus ChaseBonus;
    public Transform ChaseEnemyPos;

    private int[] BonusTypeGroup = { 0, 1, 0, 0, 0 }; //0是普通，1是带阴影
    private int _TypeIndex = 0;
    private int _PreviousIndex = -1;

    // ChaseBonus
    private float _TimeToCreateChaseBonus = 15;
    private float _ChaseBonusTimer = 0;
    private int _ChaseBonusScore = 4;
    private bool _IsFirstTimeToCreateChaseBonus = true;
    
    // RemoveBonus
    private float _TimeToCreateRemoveBonus = 20;
    private float _RemoveBonusTimer = 0;

    void Awake()
    {
        Instance = this;
    }
    
	void Start () {
        RandomCreateBonus();
	}

    void Update()
    {
        UpdateChaseTimer();
        UpdateRemoveBonusTimer();
    }

    void UpdateChaseTimer()
    {
        GameObject chaseEnemy = GameObject.FindGameObjectWithTag("ChaseEnemy");
        if (chaseEnemy != null) //场景中有跟踪型敌人就不再产生
            return;

        _ChaseBonusTimer += Time.deltaTime;

        float timeToCreate = _TimeToCreateChaseBonus;
        if(_IsFirstTimeToCreateChaseBonus)
        {
            timeToCreate *= 2;
        }
        
        if (_ChaseBonusTimer >= timeToCreate)
        {
            _ChaseBonusTimer = 0;
            ChaseBonus.CreateBonus(BonusType.Chase);
        }
    }

    void UpdateRemoveBonusTimer()
    {
        if (RemoveBonus.gameObject.activeSelf)
            return;

        _RemoveBonusTimer += Time.deltaTime;
        if(_RemoveBonusTimer >= _TimeToCreateRemoveBonus && ShadowManager.Instance.GetShadowsCount() > 0)
        {
            _RemoveBonusTimer = 0;
            float deltaY = Random.Range(-2f, 2f);
            Vector3 pos = new Vector3(RemoveBonusPos.position.x, RemoveBonusPos.position.y + deltaY, 0);
            RemoveBonus.CreateAt(pos);
        }
    }
	
    public void RandomCreateBonus()
    {
        int index = Random.Range(0, Bonuses.Count);
        while(index == _PreviousIndex) //保证不生成同一位置
        {
            index = Random.Range(0, Bonuses.Count);
        }
        _PreviousIndex = index;
        Bonuses[index].CreateBonus(IsNormal());
    }

    BonusType IsNormal()
    {
        bool isNormal = BonusTypeGroup[_TypeIndex] == 0;
        _TypeIndex++;
        if (_TypeIndex >= BonusTypeGroup.Length)
            _TypeIndex = 0;

        if (isNormal)
            return BonusType.Normal;
        else
            return BonusType.Shadow;
    }

    public void CreateChaseEnemy()
    {
        GameObject go = Instantiate(Resources.Load("ChaseEnemy")) as GameObject;
        go.transform.position = ChaseEnemyPos.position;
    }

    public void AddScore(BonusType type)
    {
        if(type == BonusType.Chase)
        {
            UIManager.Instance.AddScore(_ChaseBonusScore);
            _ChaseBonusScore *= 2;
        }
        else if(type == BonusType.Normal || type == BonusType.Shadow)
        {
            UIManager.Instance.AddScore(1);
        }
    }
}
