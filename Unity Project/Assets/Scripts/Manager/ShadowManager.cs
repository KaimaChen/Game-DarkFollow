using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShadowManager : MonoBehaviour {
    public static ShadowManager Instance = null;
    public List<Shadow> Shadows;
    public CharacterData Player;

    private const int GAP = 20; //影子之间的距离（数据链表里的索引距离）
    private float _DelayTimeToCreateShadow = 0.5f; //等待多久才产生影子

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start () {
        Shadows = new List<Shadow>();
	}
	
	void Update () {
        if (GameManager.IsPause)
            return;
        
        for (int i = 0; i < Shadows.Count; i++)
        {
            Shadows[i].Refresh(Player);
        }

        //使数据链表保持一定长度
        if (Player.ShadowDatas.Count > (Shadows.Count*GAP + Shadow.GAP_TO_PLAYER) && Shadows.Count > 0)
            Player.ShadowDatas.RemoveAt(0);
	}
    
    public void CreateShadow()
    {
        StartCoroutine(DelayCreateShadow());
    }

    IEnumerator DelayCreateShadow()
    {
        yield return new WaitForSeconds(_DelayTimeToCreateShadow);

        GameObject shadowGo = (GameObject)Resources.Load("Shadow");
        shadowGo = Instantiate(shadowGo);
        shadowGo.transform.parent = this.transform;
        Shadow shadow = shadowGo.GetComponent<Shadow>();
        shadow.Index = Shadows.Count * GAP; //保证每个阴影之间有一定距离
        Shadows.Add(shadow);
    }

    public void RemoveShadow()
    {
        if (Shadows.Count <= 0)
            return;

        Shadow shadow = Shadows[Shadows.Count - 1];
        Shadows.Remove(shadow);
        Destroy(shadow.gameObject);
        shadow = null;
    }

    public int GetShadowsCount()
    {
        return Shadows.Count;
    }

    public Vector3 LastShadowPos()
    {
        if(Shadows.Count > 0)
        {
            return Shadows[Shadows.Count - 1].transform.position;
        }
        else
        {
            return Vector3.zero;
        }
    }
}
