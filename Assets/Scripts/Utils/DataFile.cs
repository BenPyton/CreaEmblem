using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataFile
{
    static string endl = "\r\n";

    // Access to data of the file
    private List<Data> m_data;
    private bool m_allowDuplicates = true;

    public Data this[string _name]
    {
        get { return GetDataWithName(_name); }
    }

    public Data this[int _index]
    {
        get {
            if(_index >= 0 && _index < m_data.Count)
            {
                return m_data[_index];
            }
            else
            {
                return null;
            }
        }
    }


    public DataFile(bool allowDuplicates = true)
    {
        m_data = new List<Data>();
        m_allowDuplicates = allowDuplicates;
    }


    // Read data in the file and store it in field "data"
    public bool Read(string fileName)
    {
        bool success = false;
        m_data = new List<Data>();

        FileStream file = null;
        StreamReader sr = null;

        try
        {
            if (File.Exists(fileName))
            {
                file = new FileStream(fileName, FileMode.Open);
                sr = new StreamReader(file);

                string line;
                Data d;
                while ((line = sr.ReadLine()) != null)
                {
                    // try to read data
                    if(ReadData(line, out d))
                    {
                        Set(d, true);
                    }
                }
            }

            else
            {
                file = new FileStream(fileName, FileMode.Create);
                file.Dispose();
                Write(fileName);
            }

            success = true;
        }
        catch (Exception e)
        {
            Debug.LogWarning("Error : " + e.Message);
            success = false;
        }
        finally
        {
            if (sr != null)
            {
                sr.Dispose();
                sr.Close();
            }
            if (file != null)
            {
                file.Dispose();
                file.Close();
            }
        }

        return success;
    }


    public bool Write(string fileName)
    {
        bool success = false;

        FileStream file = null;
        StreamWriter sw = null;

        try
        {
            file = new FileStream(fileName, FileMode.Truncate);
            sw = new StreamWriter(file);

            string str = "";

            for(int i = 0; i < m_data.Count; i++)
            {
                if (m_data[i].type != Data.DataType.None)
                {
                    str += m_data[i];
                    if (i != m_data.Count - 1)
                    {
                        str += endl;
                    }
                }
            }

            sw.Write(str);
            success = true;
        }
        catch (Exception e)
        {
            Debug.LogWarning("Error : " + e.Message);
            success = false;
        }
        finally
        {
            if (sw != null)
            {
                sw.Dispose();
                sw.Close();
            }
            if (file != null)
            {
                file.Dispose();
                file.Close();
            }
        }


        return success;
    }

    // Try to get the first data with the name
    // Return a data with type NONE if not in datafile
    public Data GetDataWithName(string _name)
    {
        Data d = new Data(_name);
        bool exists = false;

        // Try to get an existing data in the list
        for(int i = 0; i < m_data.Count; i++)
        {
            if(m_data[i].name == _name)
            {
                d = m_data[i];
                exists = true;
            }
        }

        // if not exists then append the new data and return it
        if(!exists)
        {
            m_data.Add(d);
        }

        return d;
    }

    public Data[] GetData()
    {
        return m_data.ToArray();
    }

    // Warning : erase all existing data
    public void SetData(Data[] _data)
    {
        m_data.Clear();
        m_data.AddRange(_data);
    }

    // Try to set data in datafile
    // return false if it does not exists
    // return true if overwrited another
    public bool Set(Data _data, bool _append = false)
    {
        bool exists = false;

        if (!m_allowDuplicates)
        {
            // Try to overwrite an existing data in the list
            for (int i = 0; i < m_data.Count; i++)
            {
                if (m_data[i].name == _data.name)
                {
                    m_data[i] = _data;
                    exists = true;
                }
            }
        }

        // if not exists then append the new data
        if (!exists && _append)
        {
            m_data.Add(_data);
        }

        return exists;
    }


    // Try to add data in datafile
    // return true if appended
    // return false if exists already in allowDuplicate=false and not appended
    public bool Add(Data _data, bool _replace = false)
    {
        bool exists = false;

        if (!m_allowDuplicates)
        {
            for (int i = 0; i < m_data.Count; i++)
            {
                if (m_data[i].name == _data.name)
                {
                    if(_replace)
                    {
                        m_data[i] = _data;
                    }
                    exists = true;
                }
            }
        }

        // if not exists then append the new data
        if (!exists)
        {
            m_data.Add(_data);
        }

        return !exists;
    }

    // Return true if data is read
    // return false if data is not in format name:value
    private static bool ReadData(string _line, out Data _data)
    {
        bool success = false;
        _data = new Data();

        string[] splitted = _line.Split(':');
        if (splitted.Length != 2) return success;

        _data.name = splitted[0];
        
        float f = 0.0f;
        int i = 0;
        bool b = false;
        
        if (bool.TryParse(splitted[1], out b))
        {
            _data.SetBool(b);
            success = true;
        }

        if (!success && int.TryParse(splitted[1], out i))
        {
            _data.SetInt(i);
            success = true;
        }

        if (!success && float.TryParse(splitted[1], out f))
        {
            _data.SetFloat(f);
            success = true;
        }

        if(!success)
        {
            _data.SetString(splitted[1]);
            success = true;
        }
        
        return success;
    }


    public override string ToString()
    {
        string str = "";
        for(int i = 0; i < m_data.Count; i++)
        {
            str += m_data[i] + endl;
        }
        return str;
    }
}


public class Data
{
    public enum DataType { None, String, Int, Float, Bool };

    public string name;
    public DataType type;

    string stringValue;
    int intValue;
    float floatValue;
    bool boolValue;

    public Data(string _name = "")
    {
        name = _name;
        stringValue = "";
        intValue = 0;
        floatValue = 0.0f;
        boolValue = false;
        type = DataType.None;
    }


    /// ///////////////////////////////////////
    /// SET
    /// //////////////////////////////////////

    public void SetString(string _value)
    {
        type = DataType.String;
        stringValue = _value;
    }


    public void SetInt(int _value)
    {
        type = DataType.Int;
        intValue = _value;
    }


    public void SetFloat(float _value)
    {
        type = DataType.Float;
        floatValue = _value;
    }


    public void SetBool(bool _value)
    {
        type = DataType.Bool;
        boolValue = _value;
    }


    /// ///////////////////////////////////////
    /// GET
    /// //////////////////////////////////////

    public string GetString()
    {
        return stringValue;
    }
    
    public int GetInt()
    {
        return intValue;
    }


    public float GetFloat()
    {
        return floatValue;
    }


    public bool GetBool()
    {
        return boolValue;
    }

    public override string ToString()
    {
        string str = name + ":";


        switch(type)
        {
            case DataType.None:
                str += "NULL";
                break;
            case DataType.String:
                str += stringValue;
                break;
            case DataType.Int:
                str += intValue.ToString();
                break;
            case DataType.Float:
                str += floatValue.ToString("0.0######");
                break;
            case DataType.Bool:
                str += (boolValue) ? Boolean.TrueString : Boolean.FalseString;
                break;
        }

        //str += " (" + type + ")";

        return str;
    }
}
