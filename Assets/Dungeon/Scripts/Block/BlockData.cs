using UnityEngine;
using System.Collections;

public struct BlockData
{
    public Location location { get; set; }
    public BlockShape shape { get; set; }
    public BlockType type { get; set; }

    public bool hasEvent { get; set; }

    public BlockData(Location location, BlockShape shape, BlockType type, bool hasEvent)
    {
        this.location = location;
        this.shape = shape;
        this.type = type;
        this.hasEvent = hasEvent;
    }
}