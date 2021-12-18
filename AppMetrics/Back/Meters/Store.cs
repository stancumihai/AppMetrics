using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

// ReSharper disable once CheckNamespace
namespace ConsoleApp1.meters
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    [SuppressMessage("ReSharper", "IdentifierTypo")]
    public sealed class Store
    {
        private readonly object _balanceLock;
        private int _counter;
        private double _max;
        private double _min;

        // ReSharper disable once InconsistentNaming
        private double _sum;
        private readonly List<double> _dummyStuff;
        private readonly List<double> _values;

        public Store()
        {
            _balanceLock = new();
            Counter = 0;
            _max = 100;
            _min = float.MaxValue;
            _sum = 0.0f;
            _dummyStuff = new List<double>() { -1.0f, 0.0f };
            _values = new();
        }

        public void Add(double value)
        {
            _values.Add(value);
            Counter = Counter + 1;
            if (Max < value)
            {
                Max = value;
            }

            if (Min > value)
            {
                Min = value;
            }

            Sum += value;
            // update_var(value);
        }

        public int Counter
        {
            get => _counter;
            set => _counter = value;
        }

        public double Max
        {
            get => _max;
            set => _max = value;
        }

        public double Min
        {
            get => _min;
            set => _min = value;
        }

        public double Sum
        {
            get => _sum;
            set => _sum = value;
        }

        public List<double> DummyStuff => _dummyStuff;

        public List<double> Values => _values;


        /// Mean Value
        public double GetMean()
        {
            if (_counter > 0)
            {
                return _sum / _counter;
            }

            return 0;
        }


        public double get_stddev()
        {
            if (_counter > 0)
            {
                return Math.Sqrt(get_var());
            }

            return 0;
        }

        public double get_var()
        {
            if (_counter > 1)
            {
                return _dummyStuff[1] / (_counter - 1);
            }

            return 0;
        }


        [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
        private void update_var(double value)
        {
            var oldM = _dummyStuff[0];
            var oldS = _dummyStuff[1];
            double newM;
            double newS = 0.0f;
            if (oldM == -1)
            {
                newM = value;
            }
            else
            {
                newM = oldM + ((value - oldM) / _counter);
                newS = oldS + ((value - oldM) * (value - newM));
            }

            _dummyStuff[0] = newM;
            _dummyStuff[1] = newS;
        }
    }
}