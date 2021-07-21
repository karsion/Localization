using System;
using UnityEngine;

[RequireComponent(typeof(LanguageRectTransform))]
public class LanguageRectTransform : MonoBehaviourLanguage
{
    public bool isOverrideAnchoredPosition3D = true;
    public bool isOverrideAizeDelta;
    public RectTransform rtSelf; //�ؼ�

    [Serializable]
    public struct RectTransformData
    {
        public Vector3 anchoredPosition3D;
        public Vector2 sizeDelta;
    }

    public RectTransformData[] rectTransformDatas = new RectTransformData[0];

    public override void SwitchLanguage(int nLanguageIndex)
    {
        if (rectTransformDatas.Length == 0 || rectTransformDatas.Length <= nLanguageIndex)
        {
            Debug.Log(this.Debug(" �л����Դ���" + nLanguageIndex));
            return;
        }

        RectTransformData rectTransformData = rectTransformDatas[nLanguageIndex];
        if (isOverrideAnchoredPosition3D)
        {
            rtSelf.anchoredPosition3D = rectTransformData.anchoredPosition3D; //�������Ԫ�ز��ԣ��������ű���
        }

        if (isOverrideAizeDelta)
        {
            rtSelf.sizeDelta = rectTransformData.sizeDelta; //�������Ԫ�ز��ԣ��������ű���
        }
    }

#if UNITY_EDITOR
    private void Reset() { Setup(); }

    protected override void Setup()
    {
        if (!rtSelf) { rtSelf = GetComponent<RectTransform>(); }

        if (rtSelf)
        {
            rectTransformDatas = new RectTransformData[LanguageManager.nLanguageCount]; //�������Ԫ�ز��ԣ��������ű���
            for (int i = 0; i < rectTransformDatas.Length; i++)
            {
                RectTransform rectTransform = transform as RectTransform;
                rectTransformDatas[i] = new RectTransformData();
                rectTransformDatas[i].sizeDelta = rectTransform.sizeDelta;
                rectTransformDatas[i].anchoredPosition3D = rectTransform.anchoredPosition3D;
            }

            base.Setup();
        }
    }
#endif
}