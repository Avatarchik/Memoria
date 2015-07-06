namespace Memoria.Battle
{
    public class ElementFire : ElementType {

        public ElementFire(Element elementType)
            : base(elementType)
        {
        }

        override protected StrengthType GetResultWithFire()
        {
            return StrengthType.NORMAL;
        }
        override protected StrengthType GetResultWithWater()
        {
            return StrengthType.WEAK;
        }
        override protected StrengthType GetResultWithWind()
        {
            return StrengthType.STRONG;
        }
        override protected StrengthType GetResultWithThunder()
        {
            return StrengthType.NORMAL;
        }
    }
}
