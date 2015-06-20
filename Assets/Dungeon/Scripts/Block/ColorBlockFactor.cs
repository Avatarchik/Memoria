using UnityEngine;
using System.Collections;

public class ColorBlockFactor : BlockFactor
{
    public ColorBlockList colorBlockList { get; set; }

    // Use this for initialization
//    void Start()
//    {    
//    }
    
    // Update is called once per frame
//    void Update()
//    {    
//    }

    public override void OnPutBlock()
    {
        colorBlockList.OnPutBlock(this);
    }
}
