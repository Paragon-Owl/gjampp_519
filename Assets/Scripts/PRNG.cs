

using System;

public class PRNG
{
    private static long _seed;
    private static long _oldValue;

    private static long _m = (long) (Math.Pow(2,31)-1);
    private static long _a = 16807;
    private static long _q;
    private static long _r;
    private static bool _isInit = false;
    
    public static void init (long seed)
    {
        _seed = seed;
        _oldValue = seed;
        _q = _m / _a;
        _r = _m % _a;
        _isInit = true;
    }

    public static long getNextValue()
    {
        if (_isInit)
        {
            _seed = _a*(_oldValue%_q) - _r*(_oldValue/_q);
            if (_seed <= 0)
            {
                _seed = _seed + _m;
            }
            _oldValue = _seed;
            return _seed;
        }

        return -1;
    }
}