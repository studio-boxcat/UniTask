using System;
using System.Collections.Generic;
using UnityEngine.LowLevel;

namespace Cysharp.Threading.Tasks
{
    public class PlayerLoopBuilder
    {
        PlayerLoopSystem _target;
        List<PlayerLoopSystem> _subSystems;

        public PlayerLoopBuilder(PlayerLoopSystem target)
        {
            _target = target;
        }

        public Type type => _target.type;

        public int IndexOf(Type type)
        {
            CopySubSystems();

            for (var i = 0; i < _subSystems.Count; i++)
            {
                if (_subSystems[i].type == type)
                    return i;
            }

            return -1;
        }

        public void Add(PlayerLoopSystem yieldLoop, PlayerLoopSystem runnerLoop)
        {
            CopySubSystems();
            _subSystems.Add(yieldLoop);
            _subSystems.Add(runnerLoop);
        }

        public void InsertFront(PlayerLoopSystem yieldLoop, PlayerLoopSystem runnerLoop)
        {
            CopySubSystems();
            // Insert in reverse order.
            _subSystems.Insert(0, runnerLoop);
            _subSystems.Insert(0, yieldLoop);
        }

        public void Insert(int index, PlayerLoopSystem loop)
        {
            CopySubSystems();
            _subSystems.Insert(index, loop);
        }

        public void Remove(Type type)
        {
            CopySubSystems();

            for (var i = _subSystems.Count - 1; i >= 0; i--)
            {
                var ls = _subSystems[i];
                if (ls.type == type)
                    _subSystems.RemoveAt(i);
            }
        }

        public void Remove(Type type1, Type type2)
        {
            CopySubSystems();

            for (var i = _subSystems.Count - 1; i >= 0; i--)
            {
                var ls = _subSystems[i];
                if (ls.type == type1 || ls.type == type2)
                    _subSystems.RemoveAt(i);
            }
        }

        public PlayerLoopSystem Build()
        {
            if (_subSystems == null)
                return _target;

            _target.subSystemList = _subSystems.ToArray();
            _subSystems = null;
            return _target;
        }

        void CopySubSystems()
        {
            if (_subSystems != null)
                return;

            var subSystems = _target.subSystemList;
            _subSystems = new List<PlayerLoopSystem>(subSystems);
        }

        public static PlayerLoopBuilder[] CreateSubSystemArray(PlayerLoopSystem ls)
        {
            var subSystems = ls.subSystemList;
            var subSystemArray = new PlayerLoopBuilder[subSystems.Length];
            for (var i = 0; i < subSystems.Length; i++)
                subSystemArray[i] = new PlayerLoopBuilder(subSystems[i]);
            return subSystemArray;
        }

        public static PlayerLoopSystem[] BuildSubSystemArray(PlayerLoopBuilder[] subSystemArray)
        {
            var subSystems = new PlayerLoopSystem[subSystemArray.Length];
            for (var i = 0; i < subSystemArray.Length; i++)
                subSystems[i] = subSystemArray[i].Build();
            return subSystems;
        }
    }
}