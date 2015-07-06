
namespace Memoria.Battle
{
    abstract public class ElementType
    {
        protected ElementType(Element elementType)
        {
            Type = elementType;
        }

        public Element Type { get; set; }
        public StrengthType CheckElements(ElementType elementType)
        {
            switch(elementType.Type)
            {
                case Element.FIRE:
                    return GetResultWithFire;
                case Element.THUNDER:
                    return GetResultWithThunder;
                case Element.WATER:
                    return GetResultWithWater;
                case Element.WIND:
                    return GetResultWithWind;
            }
            return GetResultWithNormal;
        }

        abstract public StrengthType GetResultWithFire { get; }
        abstract public StrengthType GetResultWithWater { get; }
        abstract public StrengthType GetResultWithWind { get; }
        abstract public StrengthType GetResultWithThunder { get; }
        abstract public StrengthType GetResultWithNormal { get; }
    }

}
