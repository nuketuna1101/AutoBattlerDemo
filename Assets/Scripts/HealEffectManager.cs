using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HealEffectManager : Singleton<HealEffectManager>
{
    [SerializeField]
    private GameObject healEfxPrefab;
    Queue<GameObject> pool = new Queue<GameObject>();
    static Coroutine healEfxCoroutine = null;
    private void Awake()
    {
        pool.Enqueue(CreateObj());
    }
    private GameObject CreateObj()
    {
        var newObj = Instantiate(healEfxPrefab);
        newObj.gameObject.SetActive(false);
        newObj.transform.SetParent(transform);
        return newObj;
    }
    private static GameObject GetFromPool()
    {
        // 요청 시 풀에 있는 오브젝트를 할당해준다.
        if (Instance.pool.Count > 0)
        {
            var obj = Instance.pool.Dequeue();
            obj.transform.SetParent(null);
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            // 가진 풀보다 더 필요하면, 풀을 늘려 새로 생성하여 이용
            var newObj = Instance.CreateObj();
            newObj.gameObject.SetActive(true);
            newObj.transform.SetParent(null);
            return newObj;
        }
    }
    private static void ReturnToPool(GameObject obj)
    {
        // 오브젝트 비활성화시키고 다시 풀로 복귀시키기
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(Instance.transform);
        Instance.pool.Enqueue(obj);
    }


    public static void ShowHealEfx(Transform targetTransform)
    {
        GameObject obj = GetFromPool();
        obj.transform.position = targetTransform.position;
        CoroutineHelper.RestartCor(Instance, ref healEfxCoroutine, HealEfxRoutine(obj));
    }

    private static IEnumerator HealEfxRoutine(GameObject obj)
    {
        yield return new WaitForSeconds(1.0f);
        ReturnToPool(obj);
    }
}
