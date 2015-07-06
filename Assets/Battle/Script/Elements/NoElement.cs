
namespace Memoria.Battle
{
    public class NoElement : ElementType
    {

        public NoElement(Element elementType)
            : base(elementType)
        {
        }
        override protected StrengthType GetResultWithFire()
        {
            return StrengthType.NORMAL;
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
            return StrengthType.NORMAL;
        }

    }
}