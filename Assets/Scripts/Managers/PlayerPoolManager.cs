using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

public enum PlayerClass
{
    Knight = 0,
    Thief = 1,
    Archer = 2,
    Priest = 3,
}

public class PlayerPoolManager : Singleton<PlayerPoolManager>
{
    [SerializeField]
    private GameObject[] prefabs;                                  // 오브젝트 프리팹
    [SerializeField]
    private const int initPoolSize = 4;                           // 초기 풀 사이즈 정의
    //Queue<GameObject> pool = new Queue<GameObject>();              // 아이템 풀로 이용할 큐
    GameObject[] pool = new GameObject[initPoolSize];

    private void Awake()
    {
        InitPool();
    }
    public void InitPool()
    {
        for (int i = 0; i < initPoolSize; i++)
            pool[i] = CreateObj((PlayerClass)i);
    }
    private GameObject CreateObj(PlayerClass playerClass)
    {
        var newObj = Instantiate(prefabs[(int)playerClass]);
        newObj.gameObject.SetActive(false);
        newObj.transform.SetParent(transform);
        return newObj;
    }
    public static GameObject GetFromPool(PlayerClass playerClass)
    {
        // 요청 시 풀에 있는 오브젝트를 할당해준다.
        if (Instance.pool[(int)playerClass] != null)
        {
            var obj = Instance.pool[(int)playerClass];
            obj.transform.SetParent(null);
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
            return null;
    }
    public static void ReturnToPool(GameObject obj)
    {
        // 오브젝트 비활성화시키고 다시 풀로 복귀시키기
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(Instance.transform);
        Instance.pool[(int)obj.GetComponent<Player>().playerClass] = obj;
    }
}
