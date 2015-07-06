namespace Memoria.Battle
{
    public class ElementThunder : ElementType {

        public ElementThunder(Element elementType)
            : base(elementType)
        {
        }
        override public StrengthType GetResultWithFire
        {
            get
            {
                return StrengthType.NORMAL;
            }
        }
        override public StrengthType GetResultWithWater
        {
            get
            {
                return StrengthType.STRONG;
            }
        }
        override public StrengthType GetResultWithWind
        {
            get
            {
                return StrengthType.WEAK;
            }
        }
        override public StrengthType GetResultWithThunder
        {
            get
            {
                return StrengthType.NORMAL;
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
