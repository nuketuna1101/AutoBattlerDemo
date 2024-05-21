using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GoblinAnimListener : MonoBehaviour
{
    /// <summary>
    /// 고블린 몬스터의 애니메이션 중 연결된 event call
    /// </summary>
    private void GoblinAttack()
    {
        this.transform.parent.GetComponent<Monster>().BasicAttack();
    }
    private void GoblinDeath()
    {
        this.transform.parent.GetComponent<Monster>().ReturnToPool();
    }
}
