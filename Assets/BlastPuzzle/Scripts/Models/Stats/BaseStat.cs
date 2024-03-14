using System;


    public abstract class BaseStat
    {
        public event Action Changed;

        public abstract T Get<T>();
        public virtual bool Set<T>(T value)
        {
            Changed?.Invoke();

            return true;
        }

        public virtual bool Set()
        {
            Changed?.Invoke();

            return true;
        }

        private bool _isDirty;
        public bool IsDirty
        {
            get { return _isDirty; }
            set
            {
                _isDirty = value;

                if (!value) return;
                Changed?.Invoke();
            }
        }

        public abstract Type GetStatType();

        protected virtual void ValidateType(Type type)
        {
            var t = GetStatType();
            if (type != GetStatType())
            {
                throw new InvalidCastException();
            }
        }
    }
