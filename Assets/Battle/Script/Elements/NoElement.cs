
namespace Memoria.Battle
{
    public class NoElement : ElementType
    {

        public NoElement(Element elementType)
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