using Nancy.Json;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ToDoList.Model;

public static class Appendage
{
    private static string ExceptionTable = "exceptiontbl";

    public static CultureInfo Japanese
    {
        get { return new CultureInfo("ja-JP"); }
    }

    public static CultureInfo English
    {
        get { return new CultureInfo("en-EN"); }
    }

    /// <summary>
    /// Exception to DataBase
    /// </summary>
    /// <param name="Exception"></param>
    /// <param name="callerFilePath"></param>
    /// <param name="callerMemberName"></param>
    /// <param name="callerLineNumber"></param>
    /// <returns></returns>
    public static bool ToErrorException(this Exception Exception, [CallerFilePath] string callerFilePath = null, [CallerMemberName] string callerMemberName = null, [CallerLineNumber] int callerLineNumber = 0)
    {
        string writtingTxt = string.Empty;
        string filePath = string.Empty;
        String ErrorlineNo, Errormsg, Extype, ErrorLocation;
        bool txtWrite = false;

        try
        {
            StackFrame frame = new StackFrame(1);
            MethodBase method = frame.GetMethod();
            var name = method.DeclaringType.Name;

            var maxId = ToExceptionMaxId() + 1;
            var errorTime = Postgre.StrDateTime(DateTime.Now);

            var table = ExceptionTable;
            var query = "SELECT * FROM " + table;
            var where = " WHERE errorindex=" + maxId;
            var sqlStr = query + where;

            writtingTxt += "=======================================Start=======================================" + Environment.NewLine;
            writtingTxt += "Date: " + DateTime.Now.ToString(Japanese) + Environment.NewLine;
            writtingTxt += Environment.NewLine + Environment.NewLine;

            if (Exception != null)
            {
                ErrorlineNo = Exception.StackTrace.Substring(Exception.StackTrace.Length - 7, 7);
                Errormsg = Exception.GetType().Name.ToString();
                Extype = Exception.GetType().ToString();
                ErrorLocation = Exception.Message.ToString();

                writtingTxt += Exception.GetType().FullName + Environment.NewLine;
                writtingTxt += "Error Source: " + Exception.Source + Environment.NewLine;
                writtingTxt += "Error Message: " + Errormsg + Environment.NewLine;
                writtingTxt += "Error Line No: " + ErrorlineNo + Environment.NewLine;
                writtingTxt += "Exception Type: " + Extype + Environment.NewLine;
                writtingTxt += "Error Location: " + ErrorLocation + Environment.NewLine;
                writtingTxt += "Error StackTrace: " + Exception.StackTrace + Environment.NewLine;
                writtingTxt += "InnerException: " + Exception.InnerException?.Message + Environment.NewLine;
            }
            else
            {
                writtingTxt += "Error" + Environment.NewLine;
            }

            writtingTxt += "Caller File Path: " + callerFilePath + Environment.NewLine;
            writtingTxt += "Caller Line Number: " + callerLineNumber + Environment.NewLine;
            writtingTxt += "Caller Member Name: " + method.DeclaringType.FullName + Environment.NewLine;

            writtingTxt += "=======================================*End*=======================================" + Environment.NewLine;
            writtingTxt += Environment.NewLine;

            var isExist = Postgresqldb.ToDataExist(sqlStr);
            //var isExist = Accesssqldb.ToDataExist(sqlStr);

            if (isExist == false)
            {
                string[] FLD = new string[]
                {
                    "errorindex",
                    "errordatetime",
                    "callername",
                    "errormessage",
                    "insertdatetime",
                    "updatedatetime"
                };
                string[] STR = new string[]
                {
                    maxId.ToString(),
                    errorTime,
                    name,
                    writtingTxt,
                    Postgre.InsertDateTime,
                    Postgre.UpdateDateTime
                };

                txtWrite = Postgresqldb.ToDataInsert(table, FLD, STR);
                //txtWrite = Accesssqldb.ToDataInsert(table, FLD, STR);
            }

            return txtWrite;
        }
        catch (Exception ex) { Debug.WriteLine("Error: " + ex.Message); return false; }
    }

    /// <summary>
    /// Max Id From DataBase
    /// </summary>
    /// <returns></returns>
    private static int ToExceptionMaxId()
    {
        var where = string.Empty;
        var json = string.Empty;
        var query = "SELECT MAX(errorindex) FROM " + ExceptionTable;
        List<ExceptionMaxId> exceptionList = new List<ExceptionMaxId>();

        var maxId = 0;

        try
        {
            where = "";

            var sqlStr = query + where;

            json = Postgresqldb.ToDataJson(sqlStr);
            //json = Accesssqldb.ToDataJson(sqlStr);

            if (!string.IsNullOrEmpty(json))
            {
                if (json != "[]") exceptionList = json.ToJsonList<ExceptionMaxId>().ToList();
                else exceptionList = new List<ExceptionMaxId>();
            }
            else exceptionList = new List<ExceptionMaxId>();
        }
        catch (Exception e)
        {
            exceptionList = new List<ExceptionMaxId>(); e.ToErrorException();
        }

        if (exceptionList.Count == 1)
        {
            foreach (var error in exceptionList)
            {
                maxId = error.MaxId == null ? 0 : error.MaxId.ToObjInt();
            }
        }
        else maxId = 0;

        return maxId;
    }

    public class ExceptionMaxId
    {
        [JsonProperty("max")]
        public int? MaxId { get; set; }
    }

    /// <summary>
    /// Save/Update to DataBase
    /// </summary>
    /// <param name="todo"></param>
    /// <returns></returns>
    public static bool ToSaveUpdate(ToDoListModel todo)
    {
        var table = DBR.ToDoListTable;
        var query = "SELECT * FROM " + table;
        var where = " WHERE todoindex=" + todo.Index;
        var sqlStr = query + where;
        var saved = false;

        var status = todo.Status;
        var isComplete = status == true ? "Completed" : "Not Copleted";

        var isExist = Postgresqldb.ToDataExist(sqlStr);
        //var isExist = Accesssqldb.ToDataExist(sqlStr);

        if (isExist == true)
        {
            string[] FLD = new string[]
            {
                "todoname",
                "todostatus",
                "updatedatetime"
            };
            string[] STR = new string[]
            {
                todo.Name,
                isComplete,
                Postgre.UpdateDateTime
            };

            saved = Postgresqldb.ToDataUpdate(table, where, FLD, STR);
            //saved = Accesssqldb.ToDataUpdate(table, where, FLD, STR);
        }
        else
        {
            string[] FLD = new string[]
            {
                "todoindex",
                "todoname",
                "todostatus",
                "tododatetime",
                "insertdatetime",
                "updatedatetime"
            };
            string[] STR = new string[]
            {
                todo.Index.ToString(),
                todo.Name,
                isComplete,
                Postgre.StrDateTime(todo.TodoDateTime),
                Postgre.InsertDateTime,
                Postgre.UpdateDateTime
            };

            saved = Postgresqldb.ToDataInsert(table, FLD, STR);
            //saved = Accesssqldb.ToDataInsert(table, FLD, STR);
        }

        return saved;
    }

    /// <summary>
    /// Delete from DataBase
    /// </summary>
    /// <param name="todo"></param>
    /// <returns></returns>
    public static bool ToDelete(ToDoListModel todo)
    {
        var table = DBR.ToDoListTable;
        var query = "SELECT * FROM " + table;
        var where = " WHERE todoindex=" + todo.Index;
        var sqlStr = query + where;
        var delete = false;

        var isExist = Postgresqldb.ToDataExist(sqlStr);
        //var isExist = Accesssqldb.ToDataExist(sqlStr);

        if (isExist == true)
        {
            string[] FLD = new string[]
            {
                "todoindex"
            };
            string[] STR = new string[]
            {
                todo.Index.ToString()
            };

            delete = Postgresqldb.ToDataDelete(table, FLD, STR);
            //delete = Accesssqldb.ToDataDelete(table, where);
        }

        return delete;
    }
}