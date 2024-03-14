using System;
using System.Collections.Generic;
using System.Globalization;
using BlastPuzzle.Scripts.Interfaces;
using Newtonsoft.Json;
using UnityEngine;

namespace BlastPuzzle.Scripts.Models
{
    public static class PrefsManager
    {
        #region Config
        
        public static Dictionary<string, BaseStat> Prefs = new Dictionary<string, BaseStat>();

        public static void InitializeData(Dictionary<string, BaseStat> prefs)
        {
            InitializePlayerPrefs(prefs);
        }

        private static void InitializePlayerPrefs(Dictionary<string, BaseStat> prefs)
        {
            foreach (var stat in prefs)
            {
                Prefs.Add(stat.Key, stat.Value);
                if (HasSavedBefore(stat.Key)) continue;
                
                var type = stat.Value.GetStatType();
                SetByType(stat.Key, type);
            }
        }

        #endregion

        private static void SetByType(string key, Type type)
        {
            if (type == typeof(int))
            {
                var value = Prefs[key].Get<int>();

                SetInt(key, value);
            }
            else if (type == typeof(float))
            {
                var value = Prefs[key].Get<float>();

                SetFloat(key, value);
            }
            else if (type == typeof(long))
            {
                var value = Prefs[key].Get<long>();

                SetLong(key, value);
            }
            else if (type == typeof(bool))
            {
                var value = Prefs[key].Get<bool>();

                SetBool(key, value);
            }
            else if (type == typeof(string))
            {
                var value = Prefs[key].Get<string>();

                SetString(key, value);
            }
            else if (type == typeof(Vector2))
            {
                var value = Prefs[key].Get<Vector2>();

                SetVector2(key, value);
            }
            else if (type == typeof(Vector3))
            {
                var value = Prefs[key].Get<Vector3>();

                SetVector3(key, value);
            }
            else if (type == typeof(DateTime))
            {
                var value = Prefs[key].Get<DateTime>();

                SetDate(key, value);
            }
            else if (type == typeof(IObservableModel))
            {
                var value = Prefs[key].Get<string>();

                SetString(key, value);
            }
            else
            {
                var value = Prefs[key].Get<string>();

                SetString(key, value);
            }
        }

        private static bool SetData<T>(string key, T value)
        {
            if(!Prefs.TryGetValue(key, out BaseStat baseStat)) return false;

            baseStat.Set(value);
            return true;
        }

        private static T GetData<T>(string key)
        {
            return Prefs[key].Get<T>();
        }

        public static bool HasSavedBefore(string key)
        {
            return PlayerPrefs.HasKey(key);
        }

        #region Int
        public static void SetInt(string prefKey, int value = 0)
        {
            PlayerPrefs.SetInt(prefKey, value);
            SetData(prefKey, value);
        }

        public static int GetInt(string prefKey, int defaultValue = 0)
        {
            return PlayerPrefs.GetInt(prefKey, defaultValue);
        }

        #endregion

        #region Float
        public static void SetFloat(string prefKey, float value)
        {
            PlayerPrefs.SetFloat(prefKey, value);
            SetData(prefKey, value);
        }

        public static float GetFloat(string prefKey, float defaultValue = 0f)
        {
            return PlayerPrefs.GetFloat(prefKey, defaultValue);
        }

        #endregion

        #region Long
        public static void SetLong(string prefKey, long value)
        {
            PlayerPrefs.SetString(prefKey, value.ToString());
            SetData(prefKey, value);
        }

        public static long GetLong(string prefKey, long defaultValue = 0)
        {
            return long.Parse(PlayerPrefs.GetString(prefKey, defaultValue.ToString(CultureInfo.InvariantCulture)));
        }

        #endregion

        #region Bool
        public static void SetBool(string prefKey, bool value)
        {
            PlayerPrefs.SetInt(prefKey, value ? 1 : 0);
            SetData(prefKey, value);
        }

        public static bool GetBool(string prefKey, bool defaultValue = false)
        {
            return PlayerPrefs.GetInt(prefKey, defaultValue ? 1 : 0) == 1;
        }

        #endregion

        #region String
        public static void SetString(string prefKey, string value)
        {
            PlayerPrefs.SetString(prefKey, value);
            SetData(prefKey, value);
        }

        public static string GetString(string prefKey, string defaultValue = "")
        {
            return PlayerPrefs.GetString(prefKey, defaultValue);
        }

        #endregion

        #region Vector2
        public static void SetVector2(string prefKey, Vector2 value)
        {
            PlayerPrefs.SetFloat($"{prefKey}.X", value.x);
            PlayerPrefs.SetFloat($"{prefKey}.Y", value.y);
            SetData(prefKey, value);
        }

        public static Vector2 GetVector2(string prefKey)
        {
            var value = new Vector2(PlayerPrefs.GetFloat($"{prefKey}.X"), PlayerPrefs.GetFloat($"{prefKey}.Y"));
            return value;
        }

        #endregion

        #region Vector3
        public static void SetVector3(string prefKey, Vector3 value)
        {
            var json = JsonUtility.ToJson(value);
            PlayerPrefs.SetString(prefKey, json);
            SetData(prefKey, value);
        }

        public static Vector3 GetVector3(string prefKey, Vector3 defaultValue = default)
        {
            var json = PlayerPrefs.GetString(prefKey, JsonUtility.ToJson(defaultValue));
            var value = JsonUtility.FromJson<Vector3>(json);
            return value;
        }

        #endregion

        #region Date
        public static void SetDate(string prefKey, DateTime value)
        {
            PlayerPrefs.SetString(prefKey, DateTime.Now.ToBinary().ToString());
            SetData(prefKey, value);
        }

        public static DateTime GetDate(string prefKey, DateTime defaultValue = default)
        {
            long temp = Convert.ToInt64(PlayerPrefs.GetString(prefKey, defaultValue.ToBinary().ToString()));

            return DateTime.FromBinary(temp);
        }

        #endregion

        #region Object

        public static void SetObject<T>(string prefKey, T value) where T : class, IObservableModel, new ()
        {
            var json = JsonConvert.SerializeObject(value);
            PlayerPrefs.SetString(prefKey, json);
            SetData(prefKey, json);
        }

        public static T GetObject<T>(string prefKey, T defaultValue = default) where T : class, IObservableModel, new ()
        {
            if (defaultValue == null)
            {
                defaultValue = new T();
            }
            var defaultValueJson = JsonConvert.SerializeObject(defaultValue);
            var objFromJson = JsonConvert.DeserializeObject<T>(PlayerPrefs.GetString(prefKey, defaultValueJson));
            return objFromJson;
        }

        #endregion
        
        #region List

        public static void SetList<T>(string prefKey, List<T> value)
        {
            var json = JsonConvert.SerializeObject(value);
            PlayerPrefs.SetString(prefKey, json);
            SetData(prefKey, json);
        }

        public static List<T> GetList<T>(string prefKey, List<T> defaultValue = null)
        {
            var json = PlayerPrefs.GetString(prefKey);
            if (string.IsNullOrEmpty(json))
                return defaultValue ?? new List<T>();

            return JsonConvert.DeserializeObject<List<T>>(json) ?? defaultValue ?? new List<T>();
        }

        #endregion
        
        #region Dictionary

        public static void SetDictionary<T1,T2>(string prefKey, Dictionary<T1,T2> value)
        {
            var json = JsonConvert.SerializeObject(value);
            PlayerPrefs.SetString(prefKey, json);
            SetData(prefKey, json);
        }

        public static Dictionary<T1,T2> GetDictionary<T1,T2>(string prefKey, Dictionary<T1,T2> defaultValue = null)
        {
            var json = PlayerPrefs.GetString(prefKey);
            if (string.IsNullOrEmpty(json))
                return defaultValue ?? new Dictionary<T1,T2>();
            return JsonConvert.DeserializeObject<Dictionary<T1,T2>>(json) ?? defaultValue ?? new Dictionary<T1,T2>();
        }

        #endregion
    }
}