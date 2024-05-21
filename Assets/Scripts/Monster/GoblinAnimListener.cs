using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GoblinAnimListener : MonoBehaviour
{
    /// <summary>
    /// ��� ������ �ִϸ��̼� �� ����� event call
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
