using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColorBlockList : BlockList
{
    [SerializeField]
    private GameObject colorBlockFactorPrefab;
    private float offset = 118;
    private float space = 188;

    private float minHeight = 800;

    private RectTransform _rectTransform;
    private RectTransform rectTransform
    {
        get
        {
            if (!_rectTransform)
            {
                _rectTransform = GetComponent<RectTransform>();
            }

            return _rectTransform;
        }

        set
        {
            _rectTransform = value;
        }
    }

    // Use this for initialization
    protected override void Start()
    {
        Initialize();
    }
    
    // Update is called once per frame
    void Update()
    {    
    }

    public void CreateColorBlockList(List<BlockData> blockDatas)
    {
        float y = offset;

        blockDatas.ForEach((blockData) => {
            ColorBlockFactor colorBlockFactor = CreateInColorBlockFactor();

            Vector3 position = colorBlockFactor.transform.localPosition;
            position.y = y;
            colorBlockFactor.transform.localPosition = position;
            y += space;

            int shapeType = blockData.shape.type;
            BlockType type = blockData.type;
            colorBlockFactor.CreateBlock(shapeType, type);

            blockFactors.Add(colorBlockFactor);
        });

        UpdateHeight();
    }

    private ColorBlockFactor CreateInColorBlockFactor()
    {
        GameObject colorBlockFactorObject = Instantiate<GameObject>(colorBlockFactorPrefab);
        ColorBlockFactor colorBlockFactor = colorBlockFactorObject.GetComponent<ColorBlockFactor>();;
        colorBlockFactor.colorBlockList = this;
        colorBlockFactor.transform.SetParent(transform);
        colorBlockFactor.transform.localPosition = Vector3.zero;
        colorBlockFactor.transform.localScale = Vector3.one;
        
        return colorBlockFactor;
    }

    public void OnPutBlock(ColorBlockFactor colorBlockFactor)
    {
        int index = blockFactors.FindIndex(blockFactor => blockFactor.Equals(colorBlockFactor));
        for (int i = index; i < blockFactors.Count; i++)
        {
            Vector3 position = blockFactors[i].transform.localPosition;
            position.y -= space;
            blockFactors[i].transform.localPosition = position;
        }

        blockFactors.Remove(colorBlockFactor);
        Destroy(colorBlockFactor.gameObject);

        UpdateHeight();
        MoveInRange();

        paramaterManager.parameter.sp -= 1;
    }

    private void UpdateHeight()
    {        
        float height = space * (blockFactors.Count - 1) + 2 * offset;
        Vector2 size = rectTransform.sizeDelta;
        size.y = Mathf.Max(height, minHeight);
        rectTransform.sizeDelta = size;
    }

    private void MoveInRange()
    {
        Vector2 position = rectTransform.anchoredPosition;
        float height = rectTransform.sizeDelta.y;
        position.y = Mathf.Max(position.y, 800 - height);
        rectTransform.anchoredPosition = position;
    }
}