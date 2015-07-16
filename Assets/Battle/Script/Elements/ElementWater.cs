namespace Memoria.Battle
{
    public class ElementWater : ElementType {

        public ElementWater(Element elementType)
            : base(elementType)
        {
        }
        override protected StrengthType GetResultWithFire()
        {
            return StrengthType.STRONG;
        }
        override protected StrengthType GetResultWithWater()
        {
            return StrengthType.NORMAL;
        }
        override protected StrengthType GetResultWithWind()
        {
            return StrengthType.NORMAL;
        }
        override protected StrengthType GetResultWithThunder()
        {
            return StrengthType.WEAK;
        }
    }
}
