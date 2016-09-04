using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ara2;
using Ara2.Components;

namespace Ara2.Grid
{
    [Serializable]
    public class AraGridColGroups
    {
        AraGridSearchLinq _Grid;

        public AraGridColGroups(AraGridSearchLinq vGrid)
        {
            _Grid = vGrid;
            _ColsGroup = new List<AraGridColGroup>();
        }

        List<AraGridColGroup> _ColsGroup;

        private bool JaAddLoadColsBySql = false;

        public void Add(AraGridColGroup Group)
        {
            _ColsGroup.Add(Group);
        }

        public void Add(AraGridColGroup[] Group)
        {
            _ColsGroup.AddRange(Group);
        }

        public void Del(AraGridColGroup Group)
        {
            _ColsGroup.Remove(Group);
        }

        public int Count
        {
            get
            {
                return _ColsGroup.Count;
            }
        }

        public void Clear()
        {
            _ColsGroup.Clear();
        }

        public AraGridColGroup[] ToArray()
        {
            return _ColsGroup.ToArray();
        }

        public AraGridColGroup this[string vName]
        {
            get
            {
                foreach (AraGridColGroup Col in _ColsGroup.ToArray())
                {
                    if (Col.Col.Name.ToUpper() == vName.ToUpper())
                        return Col;
                }

                return null;
            }
        }

        public void Commit()
        {
            if (_Grid.Grid.IsCommit)
            {
                _Grid.LoadColsBySql();
                _Grid.Grid.Commit();
            }
        }
    }
}
