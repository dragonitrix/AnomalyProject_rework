using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EZcsv
{
    public int maxcol;
    public List<List<string>> datas = new List<List<string>>();

    public EZcsv(CSVTemplate CSVTemplate)
    {
        AddRows(CSVTemplate.GetDatas());
        maxcol = CSVTemplate.GetCol();
    }
    public EZcsv(int col)
    {
        this.maxcol = col;
    }

    public void SetData(string data,int col,int row)
    {
        try
        {
            datas[row][col] = data;
        }
        catch (System.Exception)
        {
            Debug.Log("invalid data position");
        }
    }

    public void AddRow(List<string> row)
    {
        List<string> toAdd = new List<string>();
        toAdd.AddRange(row.GetRange(0, maxcol));

        if (toAdd.Count < maxcol)
        {
            for (int i = toAdd.Count; i < maxcol; i++)
            {
                toAdd.Add("");
            }
        }

        datas.Add(row);
    }

    public void AddRows(List<List<string>> rows)
    {
        foreach (var item in rows)
        {
            AddRow(item);
        }
    }

    public string GetCSVStrings()
    {
        var csvString = "";
        for (int i = 0; i < datas.Count; i++)
        {
            var row = datas[i];
            var rowString = Row2String(row);

            csvString += rowString;

            if (i < datas.Count - 1)
            {
                csvString += "\n";
            }

        }
        return csvString;
    }

    public string Row2String(List<string> row)
    {
        var rowstring = "";

        for (int i = 0; i < row.Count; i++)
        {
            rowstring += row[i];
            if (i < row.Count - 1) rowstring += ",";
        }

        return rowstring;
    }

}
