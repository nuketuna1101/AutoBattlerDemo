using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    /// <summary>
    /// ��Ʋ �Ŵ����� ������� Player�� ���� UI�� ����������.
    /// </summary>
    public Sprite[] sprites = new Sprite[4];
    private Transform CharacterSlotsParent;
    private Coroutine updateHPbarCoroutine;

    private void Awake()
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        CharacterSlotsParent = canvas.transform.GetChild(0).GetChild(1).transform;
        for(int i = 0; i < 4; i++)
        {
            Transform slot = CharacterSlotsParent.GetChild(i).transform;
            slot.GetChild(1).GetComponent<Image>().sprite = sprites[i];
            slot.GetChild(2).GetChild(0).GetComponent<TMP_Text>().text = string.Format("{0}", (i + 1));
            Slider slider = slot.GetChild(3).GetComponent<Slider>();
            slider.maxValue = BattleManager.Instance.Players[i].maxHealth;
            slider.value = BattleManager.Instance.Players[i].health;
            Debug.Log("BattleManager.Instance.Players[i] : " + (BattleManager.Instance.Players[i] == null));
        }
    }

    private IEnumerator UpdateHPbarRoutine()
    {
        // ü�¹� HP UI�� ������Ʈ ����ٴϴ� �ڷ�ƾ. �׸��� ���� ü�¿� ���� �����̴� ������Ʈ
        while (true)
        {
            yield return null;
            for (int i = 0; i < 4; i++)
            {
                //if (BattleManager.Instance.players)

                Transform slot = CharacterSlotsParent.GetChild(i).transform;
                Slider slider = slot.GetChild(3).GetComponent<Slider>();
                //slider.value = 
            }
        }
    }
}
