using System;

namespace JigiJumper.Utils
{
    public class LazyValue<T>
    {
        private T _value;
        private bool _initialized = false;
        private Func<T> _initializer;

        public LazyValue(Func<T> initializer)
        {
            _initializer = initializer;
        }
        
        public T value
        {
            get
            {
                ForceInit();
                return _value;
            }
            private set
            {
                _initialized = true;
                _value = value;
            }
        }

        public void ForceInit()
        {
            if (!_initialized)
            {
                _value = _initializer();
                _initialized = true;
            }
        }
    }
}