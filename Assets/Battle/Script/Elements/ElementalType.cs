using System;
using System.Collections.Generic;

namespace Memoria.Battle
{
    abstract public class ElementType
    {
        protected Dictionary<Element, Func<StrengthType>> result;

        protected ElementType(Element elementType)
        {
            Type = elementType;

            result = new Dictionary<Element, Func<StrengthType>>
                {
                    { Element.FIRE,    () => GetResultWithFire()    },
                    { Element.THUNDER, () => GetResultWithThunder() },
                    { Element.WATER,   () => GetResultWithWater()   },
                    { Element.WIND,    () => GetResultWithWind()    },
                    { Element.NONE,    () => GetResultWithNormal()  }
                };
        }

        public Element Type { get; private set; }

        public StrengthType CheckAgainstElement(ElementType elementType)
        {
            return result[elementType.Type].Invoke();
        }
        abstract protected StrengthType GetResultWithFire();
        abstract protected StrengthType GetResultWithWater();
        abstract protected StrengthType GetResultWithWind();
        abstract protected StrengthType GetResultWithThunder();

        protected StrengthType GetResultWithNormal()
        {
            return StrengthType.NORMAL;
        }
    }
}
