using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.Model;

public class ToDoListRepository : INotifyPropertyChanged
{
    private ObservableCollection<ToDoListModel> toDoLists { get; set; }

    public ToDoListRepository()
    {
        try
        {
            ToDoLists = new ObservableCollection<ToDoListModel>();

            var todoList = DBR.ToToDoListTable();

            if (todoList.Count > 0)
            {
                foreach (var todo in todoList)
                {
                    var status = todo.ToDoStatus;
                    var isComplete = status.Contains("Not") == true ? false : true;

                    ToDoLists.Add(new ToDoListModel { Index = todo.ToDoIndex, Name = todo.ToDoName, Status = isComplete, ToDoStatus = status, TodoDateTime = todo.TodoDateTime });
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Error: " + ex.Message);

            ToDoLists = new ObservableCollection<ToDoListModel>();
        }
    }

    public ToDoListModel FindById(int id)
    {
        ToDoListModel TDLM = new ToDoListModel();

        if (ToDoLists.Count > 0)
        {
            foreach (var todo in ToDoLists.Where(w => w.Index == id))
            {
                TDLM = todo;
            }
        }

        return TDLM;
    }

    public ObservableCollection<ToDoListModel> GetAll(bool IsDiscanding = false)
    {
        List<ToDoListsTable> todoList = new List<ToDoListsTable>();

        ToDoLists = new ObservableCollection<ToDoListModel>();

        var _todoList = DBR.ToToDoListTable();

        if (IsDiscanding == true) todoList = _todoList.OrderByDescending(o => o.TodoDateTime).ToList();
        else todoList = _todoList.OrderBy(o => o.TodoDateTime).ToList();

        if (todoList.Count > 0)
        {
           
            foreach (var todo in todoList)
            {
                var status = todo.ToDoStatus;
                var isComplete = status.Contains("Not") == true ? false : true;

                ToDoLists.Add(new ToDoListModel { Index = todo.ToDoIndex, Name = todo.ToDoName, Status = isComplete, ToDoStatus = status, TodoDateTime = todo.TodoDateTime });
            }
        }

        return ToDoLists;
    }

    public ToDoListModel Update(ToDoListModel item)
    {
        ToDoLists.Add(item);

        return item;
    }

    public void Delete(ToDoListModel item)
    {
        ToDoLists.Remove(item);
    }

    public ObservableCollection<ToDoListModel> ToDoLists
    {
        get { return toDoLists; }
        set
        {
            this.toDoLists = value;
            this.RaisePropertyChanged("ToDoLists");
        }
    }

    #region INotifyPropertyChanged implementation
    public event PropertyChangedEventHandler PropertyChanged;
    private void RaisePropertyChanged(string propertyName)
    {
        if (this.PropertyChanged != null)
            this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
    }
    #endregion
}
