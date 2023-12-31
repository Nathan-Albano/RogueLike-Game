﻿using ConsoleApp1.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.System
{
    public class SchedulingSystem
    {
        private int _time;
        private readonly SortedDictionary<int, List<IScheduleable>> _scheduleables;

        public SchedulingSystem()
        {
            _time = 0;
            _scheduleables = new SortedDictionary<int, List<IScheduleable>>();

        }

        public void Add(IScheduleable scheduleable)
        {
            int key = _time + scheduleable.Time;
            if(!_scheduleables.ContainsKey(key))
            {
                _scheduleables.Add(key, new List<IScheduleable>());
            }
            _scheduleables[key].Add(scheduleable);
        }

        public void Remove(IScheduleable scheduleable)
        {
            KeyValuePair<int, List<IScheduleable>> scheduleableListFound = new KeyValuePair<int, List<IScheduleable>>(-1, null);
            foreach(var scheduleableList in _scheduleables)
            {
                if(scheduleableList.Value.Contains(scheduleable))
                {
                    scheduleableListFound = scheduleableList;
                    break;
                }
            }
            if(scheduleableListFound.Value != null)
            {
                scheduleableListFound.Value.Remove(scheduleable);
                if(scheduleableListFound.Value.Count <= 0)
                {
                    _scheduleables.Remove(scheduleableListFound.Key);
                }
            }
        }

        public IScheduleable Get()
        {
            var firstScheduleableGroup = _scheduleables.First();
            var firstScheduleable = firstScheduleableGroup.Value.First();
            Remove(firstScheduleable);
            _time = firstScheduleableGroup.Key;
            return firstScheduleable;
        }

        public int GetTime()
        {
            return _time;
        }

        public void Clear()
        {
            _time = 0;
            _scheduleables.Clear();
        }
    }
}