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
        // ��û �� Ǯ�� �ִ� ������Ʈ�� �Ҵ����ش�.
        if (Instance.pool.Count > 0)
        {
            var obj = Instance.pool.Dequeue();
            obj.transform.SetParent(null);
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            // ���� Ǯ���� �� �ʿ��ϸ�, Ǯ�� �÷� ���� �����Ͽ� �̿�
            var newObj = Instance.CreateObj();
            newObj.gameObject.SetActive(true);
            newObj.transform.SetParent(null);
            return newObj;
        }
    }
    private static void ReturnToPool(GameObject obj)
    {
        // ������Ʈ ��Ȱ��ȭ��Ű�� �ٽ� Ǯ�� ���ͽ�Ű��
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
