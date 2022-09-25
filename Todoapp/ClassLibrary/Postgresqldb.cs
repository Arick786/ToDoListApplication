using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class Postgre
{
    public static string InsertDateTime
    {
        get { return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); }
    }

    public static string UpdateDateTime
    {
        get { return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); }
    }

    public static string StrDateTime(DateTime dateTime)
    {
        return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
    }

    public static string StrDate(DateTime dateTime)
    {
        return dateTime.ToString("yyyy-MM-dd 00:00:00");
    }

    public static string StrTimeSpan(TimeSpan timeSpan)
    {
        return timeSpan.ToString(@"dd\.hh\:mm");
    }
}

public static class Postgresqldb
{
    private static string ConnectionString
    {
        get
        {
            return PSQDataBase.ToConnectionString;
        }
    }

    public static string ToDataJson(string queryStr)
    {
        var json = string.Empty;

        try
        {
            json = PSQDataBase.ReadDataBaseAsJson(ConnectionString, queryStr);
        }
        catch (Exception e) { json = string.Empty; e.ToErrorException(); }

        return json;
    }

    public static bool ToDataInsert(string table, string[] FLD, string[] STR)
    {
        var saved = false;

        try
        {
            var dataReader = PSQDataBase.ToJsonDataField(ConnectionString, table);

            saved = PSQDataBase.PostgresInsertOrUpdateByArray(ConnectionString, dataReader, table, FLD, STR);
        }
        catch (Exception e) { saved = e.ToErrorException(); }

        return saved;
    }

    public static bool ToDataUpdate(string table, string where, string[] FLD, string[] STR)
    {
        var saved = false;

        try
        {
            var dataReader = PSQDataBase.ToJsonDataField(ConnectionString, table);

            saved = PSQDataBase.PostgresInsertOrUpdateByArray(ConnectionString, dataReader, table, FLD, STR, where);
        }
        catch (Exception e) { saved = e.ToErrorException(); }

        return saved;
    }

    public static bool ToDataDelete(string table, string[] FLD, string[] STR)
    {
        var deleted = false;

        try
        {
            deleted = PSQDataBase.DeletePostgresRaw(ConnectionString, table, FLD, STR);
        }
        catch (Exception e) { deleted = e.ToErrorException(); }

        return deleted;
    }

    public static bool ToDataExist(string queryStr)
    {
        var exist = false;

        try
        {
            exist = PSQDataBase.PostgresExistRaw(ConnectionString, queryStr);
        }
        catch (Exception e) { exist = e.ToErrorException(); }

        return exist;
    }
}
