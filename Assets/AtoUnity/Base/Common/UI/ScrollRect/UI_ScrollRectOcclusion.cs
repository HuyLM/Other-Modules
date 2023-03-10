using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using AtoGame.Base.Helper;
/// <summary>
/// ScrollRectOcclusion - disables the objects outside of the scrollrect viewport. 
/// Usefull for scrolls with lots of content, reduces geometry and drawcalls (if content is not batched)
/// 
/// Fields
/// - InitByUSer - in case your scrollrect is populated from code, you can explicitly Initialize the infinite scroll after your scroll is ready
/// by callin Init() method
/// 
/// Notes
/// - In some cases it might create a bit of spikes, especially if you have lots of UI.Text objects in the childs. In that case consider to Add 
/// CanvasGroup to your childs and instead of calling setActive on game object change CanvasGroup.alpha value. At 0 it is not being rendered hence will 
/// also optimize the prformance. 
/// - works for both vertical and horizontal scrolls, even at the same time (grid layout)
/// - in order to work it disables layout components and size fitter if present (automatically)
/// </summary>
namespace AtoGame.Base.UI
{

    public class UI_ScrollRectOcclusion : MonoBehaviour
    {

        //if true user will need to call Init() method manually (in case the contend of the scrollview is generated from code or requires special initialization)
        public bool InitByUser = false;
        private ScrollRect _scrollRect;
        private ContentSizeFitter _contentSizeFitter;
        private VerticalLayoutGroup _verticalLayoutGroup;
        private HorizontalLayoutGroup _horizontalLayoutGroup;
        private GridLayoutGroup _gridLayoutGroup;
        private bool _isVertical = false;
        private bool _isHorizontal = false;
        private float _disableMarginX = 0;
        private float _disableMarginY = 0;
        private bool hasDisabledGridComponents = false;
        private List<RectTransform> items = new List<RectTransform>();

        private int canScroll = 0;

        void Awake()
        {
            if (InitByUser)
                return;

            Init();

        }
        public void Init()
        {
            canScroll = 0;
            if (GetComponent<ScrollRect>() != null)
            {
                hasDisabledGridComponents = false;

                _scrollRect = GetComponent<ScrollRect>();
                _scrollRect.onValueChanged.AddListener(OnScroll);

                _isHorizontal = _scrollRect.horizontal;
                _isVertical = _scrollRect.vertical;

                for (int i = 0; i < _scrollRect.content.childCount; i++)
                {
                    items.Add(_scrollRect.content.GetChild(i).GetComponent<RectTransform>());
                }


                if (_scrollRect.content.GetComponent<VerticalLayoutGroup>() != null)
                {
                    _verticalLayoutGroup = _scrollRect.content.GetComponent<VerticalLayoutGroup>();
                }
                if (_scrollRect.content.GetComponent<HorizontalLayoutGroup>() != null)
                {
                    _horizontalLayoutGroup = _scrollRect.content.GetComponent<HorizontalLayoutGroup>();
                }
                if (_scrollRect.content.GetComponent<GridLayoutGroup>() != null)
                {
                    _gridLayoutGroup = _scrollRect.content.GetComponent<GridLayoutGroup>();
                }
                if (_scrollRect.content.GetComponent<ContentSizeFitter>() != null)
                {
                    _contentSizeFitter = _scrollRect.content.GetComponent<ContentSizeFitter>();
                }

            }
            else
            {
                Debug.LogError("UI_ScrollRectOcclusion => No ScrollRect component found");
            }
        }

        public void Reload()
        {
            canScroll++;
            items.Clear();
            for (int i = 0; i < _scrollRect.content.childCount; i++)
            {
                items.Add(_scrollRect.content.GetChild(i).GetComponent<RectTransform>());
                items[i].gameObject.SetActive(true);
            }

            if (_isVertical)
                _disableMarginY = _scrollRect.GetComponent<RectTransform>().rect.height / 2 + items[0].sizeDelta.y;

            if (_isHorizontal)
                _disableMarginX = _scrollRect.GetComponent<RectTransform>().rect.width / 2 + items[0].sizeDelta.x;


            if (_verticalLayoutGroup)
            {
                _verticalLayoutGroup.enabled = true;
            }
            if (_horizontalLayoutGroup)
            {
                _horizontalLayoutGroup.enabled = true;
            }
            if (_contentSizeFitter)
            {
                _contentSizeFitter.enabled = true;
            }
            if (_gridLayoutGroup)
            {
                _gridLayoutGroup.enabled = true;
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate(_scrollRect.content);
            //DisableGridComponents();
            this.InvokeExtension(EndReload, 0.05f);
        }

        public void Reload(int numberItem)
        {
            canScroll++;
            items.Clear();
            for (int i = 0; i < numberItem; i++)
            {
                items.Add(_scrollRect.content.GetChild(i).GetComponent<RectTransform>());
                items[i].gameObject.SetActive(true);
            }
            if (numberItem > 0)
            {
                if (_isVertical)
                    _disableMarginY = _scrollRect.GetComponent<RectTransform>().rect.height / 2 + items[0].sizeDelta.y;

                if (_isHorizontal)
                    _disableMarginX = _scrollRect.GetComponent<RectTransform>().rect.width / 2 + items[0].sizeDelta.x;

            }
            if (_verticalLayoutGroup)
            {
                _verticalLayoutGroup.enabled = true;
            }
            if (_horizontalLayoutGroup)
            {
                _horizontalLayoutGroup.enabled = true;
            }
            if (_contentSizeFitter)
            {
                _contentSizeFitter.enabled = true;
            }
            if (_gridLayoutGroup)
            {
                _gridLayoutGroup.enabled = true;
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate(_scrollRect.content);
            //DisableGridComponents();
            this.InvokeExtension(EndReload, 0.05f);
        }

        private void EndReload()
        {
            DisableGridComponents();
            canScroll--;
            if (canScroll < 0)
            {
                canScroll = 0;
            }
            CalculatingOcclusion(_scrollRect.normalizedPosition);
        }


        void DisableGridComponents()
        {
            if (items != null && items.Count > 0)
            {
                if (_isVertical)
                    _disableMarginY = _scrollRect.GetComponent<RectTransform>().rect.height / 2 + items[0].sizeDelta.y;

                if (_isHorizontal)
                    _disableMarginX = _scrollRect.GetComponent<RectTransform>().rect.width / 2 + items[0].sizeDelta.x;
            }
            if (_verticalLayoutGroup)
            {
                _verticalLayoutGroup.enabled = false;
            }
            if (_horizontalLayoutGroup)
            {
                _horizontalLayoutGroup.enabled = false;
            }
            if (_contentSizeFitter)
            {
                _contentSizeFitter.enabled = false;
            }
            if (_gridLayoutGroup)
            {
                _gridLayoutGroup.enabled = false;
            }
            hasDisabledGridComponents = true;
        }

        public void OnScroll(Vector2 pos)
        {

            if (!hasDisabledGridComponents)
                DisableGridComponents();

            if (canScroll > 0)
            {
                return;
            }

            CalculatingOcclusion(pos);
        }

        private void CalculatingOcclusion(Vector2 pos)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (_isVertical && _isHorizontal)
                {
                    if (_scrollRect.transform.InverseTransformPoint(items[i].position).y < -_disableMarginY || _scrollRect.transform.InverseTransformPoint(items[i].position).y > _disableMarginY
                    || _scrollRect.transform.InverseTransformPoint(items[i].position).x < -_disableMarginX || _scrollRect.transform.InverseTransformPoint(items[i].position).x > _disableMarginX)
                    {
                        items[i].gameObject.SetActive(false);
                    }
                    else
                    {
                        items[i].gameObject.SetActive(true);
                    }
                }
                else
                {
                    if (_isVertical)
                    {
                        if (_scrollRect.transform.InverseTransformPoint(items[i].position).y < -_disableMarginY || _scrollRect.transform.InverseTransformPoint(items[i].position).y > _disableMarginY)
                        {
                            items[i].gameObject.SetActive(false);
                        }
                        else
                        {
                            items[i].gameObject.SetActive(true);
                        }
                    }

                    if (_isHorizontal)
                    {
                        if (_scrollRect.transform.InverseTransformPoint(items[i].position).x < -_disableMarginX || _scrollRect.transform.InverseTransformPoint(items[i].position).x > _disableMarginX)
                        {
                            items[i].gameObject.SetActive(false);
                        }
                        else
                        {
                            items[i].gameObject.SetActive(true);
                        }
                    }
                }
            }
        }
    }
}
