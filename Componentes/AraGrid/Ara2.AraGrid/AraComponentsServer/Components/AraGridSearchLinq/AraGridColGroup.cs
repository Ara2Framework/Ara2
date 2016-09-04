using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ara2;
using Ara2.Components;

namespace Ara2.Grid
{
    [Serializable]
    public class AraGridColGroup
    {
        public AraGridCol Col;
        public EOperator Operator;

        [Serializable]
        public enum EOperator
        {
            nothing = 1,
            Group = 2,
            Sum = 3,
            Avg = 4,
            Max = 5,
            Min = 6,
            Count = 7,
        }

        public AraGridColGroup(AraGridCol vCol)
        {
            Col = vCol;
            Operator = EOperator.Group;
        }


        public AraGridColGroup(AraGridCol vCol, EOperator vOperator) :
            this(vCol)
        {
            Operator = vOperator;
        }

    }
}
