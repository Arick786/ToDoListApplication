using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
/// Data Base Read
/// </summary>
public static class DBR
{
    public static string ToDoListTable = "todolisttable";
    
    public static int ToDoMaxId()
    {
        var where = string.Empty;
        var json = string.Empty;
        var query = "SELECT MAX(todoindex) FROM " + ToDoListTable;
        List<ToDoMaxId> todoList = new List<ToDoMaxId>();

        var maxId = 0;

        try
        {
            where = "";

            var sqlStr = query + where;

            json = Postgresqldb.ToDataJson(sqlStr);
            //json = Accesssqldb.ToDataJson(sqlStr);

            if (!string.IsNullOrEmpty(json))
            {
                if (json != "[]") todoList = json.ToJsonList<ToDoMaxId>().ToList();
                else todoList = new List<ToDoMaxId>();
            }
            else todoList = new List<ToDoMaxId>();
        }
        catch (Exception e)
        {
            todoList = new List<ToDoMaxId>(); e.ToErrorException();
        }

        if (todoList.Count == 1)
        {
            foreach (var todo in todoList)
            {
                maxId = todo.MaxId == null ? 0 : todo.MaxId.ToObjInt();
            }
        }
        else maxId = 0;

        return maxId + 1;
    }

    public static List<ToDoListsTable> ToToDoListTable(int index = -1)
    {
        var where = string.Empty;
        var json = string.Empty;
        var query = "SELECT * FROM " + ToDoListTable;
        List<ToDoListsTable> todoList = new List<ToDoListsTable>();

        try
        {
            if (index > 0) where = " WHERE todoindex=" + index;
            else where = "";

            var sqlStr = query + where;

            json = Postgresqldb.ToDataJson(sqlStr);
            //json = Accesssqldb.ToDataJson(sqlStr);

            if (!string.IsNullOrEmpty(json))
            {
                if (json != "[]") todoList = json.ToJsonList<ToDoListsTable>().OrderBy(o => o.ToDoIndex).ThenBy(t => t.TodoDateTime).ToList();
                else todoList = new List<ToDoListsTable>();
            }
            else todoList = new List<ToDoListsTable>();
        }
        catch (Exception e)
        {
            todoList = new List<ToDoListsTable>(); e.ToErrorException();
        }

        return todoList;
    }
}