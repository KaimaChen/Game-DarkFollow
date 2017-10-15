using UnityEngine;
using System.Collections;

/// <summary>
/// 震动相机（根据玩家降落的高度）
/// </summary>
public class CameraShake : MonoBehaviour {
    public Camera MainCamera;

    [SerializeField]
    private Vector3 _OldPos; //保存相机原来位置
    private const float MIN_DELTA = 0.03f; //最小振幅
    private const float MAX_DELTA = 0.1f; //最大振幅
    private const float HEIGHT = 10; //单位振幅对应的长度
    
	void Start () {
        _OldPos = MainCamera.transform.position;
	}
	
    /// <summary>
    /// 根据玩家降落的距离来决定震动幅度
    /// </summary>
    public void Shake(float jumpDistance)
    {
        StartCoroutine("ShakeCoroutine", jumpDistance);
    }

    /// <summary>
    /// 摄像机上移、下移再回到初始位置来造成震动效果
    /// </summary>
    IEnumerator ShakeCoroutine(float jumpDistance)
    {
        float t = Mathf.Clamp(jumpDistance / HEIGHT, 0, 1);
        float delta = Mathf.Lerp(MIN_DELTA, MAX_DELTA, t);
        MainCamera.transform.position = new Vector3(_OldPos.x, _OldPos.y + delta, _OldPos.z); //上
        yield return new WaitForSeconds(0.1f);
        MainCamera.transform.position = new Vector3(_OldPos.x, _OldPos.y - delta, _OldPos.z); //下
        yield return new WaitForSeconds(0.1f);
        MainCamera.transform.position = new Vector3(_OldPos.x, _OldPos.y, _OldPos.z); //回到原来位置
    }
}
