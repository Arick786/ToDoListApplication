using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

public static class Accesssqldb 
{
    public static string ConnectionString
    {
        get
        {
            var workingDirectory = Environment.CurrentDirectory;

            var projectDirectory = Directory.GetCurrentDirectory();

            var accessPath = Path.Combine(workingDirectory, "App_Data");

            if (Directory.Exists(accessPath) == false) accessPath = Path.Combine(projectDirectory, "App_Data");

            var accessFile = Path.Combine(accessPath, "ToDoList.accdb");

            var conStr = "Provider=Microsoft.Ace.OLEDB.12.0; Data Source=" + accessFile + "; Persist Security Info=True";

            return conStr;
        }
    }

    public static string ToDataJson(string queryStr)
    {
        var json = string.Empty;

        try
        {
            json = ADRWD.ToReadAccessAsJson(ConnectionString, queryStr);
        }
        catch (Exception e) { json = string.Empty; e.ToErrorException(); }

        return json;
    }

    public static bool ToDataInsert(string table, string[] FLD, string[] STR)
    {
        var saved = false;

        try
        {
            var query = "SELECT * FROM " + table;

            var dataReader = ADRWD.ToGetFieldTypeJson(ConnectionString, query);

            saved = ADRWD.InsertOrUpdateSQLByArray(ConnectionString, dataReader, table, FLD, STR);
        }
        catch (Exception e) { saved = e.ToErrorException(); }

        return saved;
    }

    public static bool ToDataUpdate(string table, string where, string[] FLD, string[] STR)
    {
        var saved = false;

        try
        {
            var query = "SELECT * FROM " + table;

            var dataReader = ADRWD.ToGetFieldTypeJson(ConnectionString, query);

            saved = ADRWD.InsertOrUpdateSQLByArray(ConnectionString, dataReader, table, FLD, STR, where);
        }
        catch (Exception e) { saved = e.ToErrorException(); }

        return saved;
    }

    public static bool ToDataDelete(string table, string where)
    {
        var deleted = false;

        try
        {
            deleted = ADRWD.DeleteFromAccess(ConnectionString, table, where);
        }
        catch (Exception e) { deleted = e.ToErrorException(); }

        return deleted;
    }

    public static bool ToDataExist(string queryStr)
    {
        var exist = false;

        try
        {
            exist = ADRWD.IsExistsRecord(ConnectionString, queryStr);
        }
        catch (Exception e) { exist = e.ToErrorException(); }

        return exist;
    }
}