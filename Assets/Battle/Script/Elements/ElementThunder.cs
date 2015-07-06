namespace Memoria.Battle
{
    public class ElementThunder : ElementType {

        public ElementThunder(Element elementType)
            : base(elementType)
        {
        }
        override protected StrengthType GetResultWithFire()
        {
            return StrengthType.NORMAL;
        }
        override protected StrengthType GetResultWithWater()
        {
            return StrengthType.STRONG;
        }
        override protected StrengthType GetResultWithWind()
        {
            return StrengthType.WEAK;
        }
        override protected StrengthType GetResultWithThunder()
        {
            return StrengthType.NORMAL;
        }
    }
}
