using UnityEngine;


public class DataManager : Singleton<DataManager>
{
    private const string LOCATION_KEY = "DataManager.Data";
    private IData _data;
    private bool isDirty;
    private bool isLoaded;

    #region DataFunctions

    public void SaveData<T>(string key, T value)
    {
        _data.UpdateData(key, value);
        isDirty = true;
    }
    public T GetData<T>(string key)
    {
        if(!isLoaded) LoadData();
        return _data.GetData<T>(key);
    }

    public void DeleteData()
    {
        if (CheckHasKey(LOCATION_KEY)) return;
        PlayerPrefs.DeleteKey(LOCATION_KEY);
        isDirty = true;
    }
    #endregion

    #region InitFunctions
    //DataManager.Instance.SetData(new GameData())
    public void SetData(IData iData)
    {
        _data = iData;
    }
    #endregion

    private void LoadData()
    {
        SetData(new GameData());
        if (CheckHasKey(LOCATION_KEY))
        {
            _data = Json.ConvertFromJson<IData>(PlayerPrefs.GetString(LOCATION_KEY), _data.GetDataType());
        }else
        {
            _data = new GameData();
            isDirty = true;
        }
        isLoaded = true;
    }

    private bool CheckHasKey(string key)
    {
        return PlayerPrefs.HasKey(key);
    }

    private void LateUpdate()
    {
        if (!isDirty) return;
        PlayerPrefs.SetString(LOCATION_KEY, Json.ConvertToJson(_data));
        PlayerPrefs.Save();
        isDirty = false;
    }
}