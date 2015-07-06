namespace Memoria.Battle
{
    public class ElementWater : ElementType {

        public ElementWater(Element elementType)
            : base(elementType)
        {
        }
        override public StrengthType GetResultWithFire
        {
            get
            {
                return StrengthType.STRONG;
            }
        }
        override public StrengthType GetResultWithWater
        {
            get
            {
                return StrengthType.NORMAL;
            }
        }
        override public StrengthType GetResultWithWind
        {
            get
            {
                return StrengthType.NORMAL;
            }
        }
        override public StrengthType GetResultWithThunder
        {
            get
            {
                return StrengthType.WEAK;
            }
        }
        override public StrengthType GetResultWithNormal
        {
            get
            {
                return StrengthType.NORMAL;
            }
        }
    }
}
