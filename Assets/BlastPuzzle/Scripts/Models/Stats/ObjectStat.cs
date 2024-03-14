using System;
using BlastPuzzle.Scripts.Interfaces;
using Newtonsoft.Json;
using UnityEngine;

namespace Mek.Models.Stats
{
    public class ObjectStat<TT> : StringStat where TT : class, IObservableModel, new ()
    {
        private TT _objectModel;

        private TT ObjectModel
        {
            get => _objectModel;
            set
            {
                if (_objectModel != null)
                {
                    _objectModel.PropertyChanged -= OnPropertyChanged;
                }

                _objectModel = value;
                _objectModel.PropertyChanged += OnPropertyChanged;
            }
        }

        public ObjectStat(string initial) : base(initial)
        {
            ObjectModel = string.IsNullOrEmpty(initial) ? new TT() : JsonConvert.DeserializeObject<TT>(CurrentValue);
        }

        public ObjectStat(TT initial)
        {
            var json = JsonConvert.SerializeObject(initial);
            ObjectModel = string.IsNullOrEmpty(json) ? new TT() : initial;
            CurrentValue = json;
        }

        ~ObjectStat()
        {
            ObjectModel.PropertyChanged -= OnPropertyChanged;
        }

        public override void Reconstruct(string value)
        {
            base.Reconstruct(value);

            CurrentValue = value;
            
            ObjectModel = string.IsNullOrEmpty(CurrentValue) ? new TT() : JsonConvert.DeserializeObject<TT>(CurrentValue);
        }

        private void OnPropertyChanged()
        {
            CurrentValue = JsonConvert.SerializeObject(ObjectModel);
            IsDirty = true;
        }

        public override T Get<T>()
        {
            if (typeof(T) != typeof(TT))
            {
                Reconstruct(CurrentValue);
                if (typeof(T) != typeof(string))
                {
                    Debug.LogError($"Cannot get typeof {typeof(T).Name} to type {typeof(TT).Name}");
                    throw new InvalidCastException();
                }
            }

            return (T)Convert.ChangeType(CurrentValue, typeof(T));
        }

        public override bool Set<T>(T value)
        {
            if (typeof(T) == typeof(TT))
            {
                return SerializeObject(value as TT);
            }
            
            if (typeof(T) == typeof(string))
            {
                return ParseString(value as string);
            }

            throw new InvalidCastException(
                $"Cannot cast value of type {value.GetType().Name} to type {typeof(TT).Name}");
        }

        private bool SerializeObject(TT obj)
        {
            if (Equals(obj, ObjectModel) || ReferenceEquals(obj, ObjectModel)) return false;

            CurrentValue = JsonUtility.ToJson(obj);

            ObjectModel = obj;

            IsDirty = true;

            return true;
        }

        private bool ParseString(string value)
        {
            string newValue = Convert.ToString(value);

            if (newValue == CurrentValue) return false;

            CurrentValue = newValue;

            ObjectModel = JsonUtility.FromJson<TT>(CurrentValue);

            IsDirty = true;

            return true;
        }

        public override Type GetStatType()
        {
            return typeof(IObservableModel);
        }
    }
}
