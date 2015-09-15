namespace Memoria.Dungeon.BlockComponent
{
    public struct BlockData
    {
        public Vector2Int location { get; set; }

        public ShapeData shapeData { get; set; }

        public BlockType blockType { get; set; }

        public BlockData(Vector2Int location, ShapeData shapeData, BlockType blockType)
        {
            this.location = location;
            this.shapeData = shapeData;
            this.blockType = blockType;
        }
    }
}