namespace Memoria.Battle
{
    public class ElementWind : ElementType {

        public ElementWind(Element elementType)
            : base(elementType)
        {
        }
        override protected StrengthType GetResultWithFire()
        {
            return StrengthType.WEAK;
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
            return StrengthType.STRONG;
        }
    }
}
