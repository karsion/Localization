using System;
using UnityEngine;

public class LanguageRectTransform : MonoBehaviourLanguage
{
    public bool isOverrideAnchoredPosition3D = true;
    public bool isOverrideAizeDelta;
    public RectTransform rtSelf; //控件
    [Serializable]
	public struct RectTransformData
    {
		public Vector3 anchoredPosition3D;
        public Vector2 sizeDelta;
	}

    public RectTransformData[] rectTransformDatas = new RectTransformData[0];
	public override void SwitchLanguage(int nLanguage)
    {
        if (rectTransformDatas.Length == 0 || rectTransformDatas.Length <= nLanguage)
        {
            Debug.Log(this.Debug(" 切换语言错误：" + nLanguage));
            return;
        }

        RectTransformData rectTransformData = rectTransformDatas[nLanguage];
        if (isOverrideAnchoredPosition3D)
        {
            rtSelf.anchoredPosition3D = rectTransformData.anchoredPosition3D; //如果数组元素不对，故意留着报错
        }

        if (isOverrideAizeDelta)
        {
            rtSelf.sizeDelta = rectTransformData.sizeDelta; //如果数组元素不对，故意留着报错
        }
    }

#if UNITY_EDITOR
    private void Reset() { Setup(); }

    protected override void Setup()
    {
        if (!rtSelf)
        {
            rtSelf = GetComponent<RectTransform>();
        }

        if (rtSelf)
        {
            rectTransformDatas = new RectTransformData[LanguageManager.nLanguageCount]; //如果数组元素不对，故意留着报错
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
