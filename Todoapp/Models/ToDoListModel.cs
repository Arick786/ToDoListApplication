using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.ModelView;

namespace ToDoList.Model
{
    public class ToDoListModel : BaseViewModel
    {
        #region Private Variables
        private int todoIndex;
        private string todoName;
        private bool todoStatus;
        private string todoCompleted;
        private DateTime todoDateTime;
        #endregion

        #region Public Properties
        public ToDoListModel()
        {
            Title = "To-Do List";
        }
        
        public int Index
        {
            get { return todoIndex; }
            set { SetProperty(ref todoIndex, value); }
        }

        public string Name
        {
            get { return todoName; }
            set { SetProperty(ref todoName, value); }
        }

        public bool Status
        {
            get { return todoStatus; }
            set { SetProperty(ref todoStatus, value); }
        }

        public string ToDoStatus
        {
            get { return todoCompleted; }
            set { SetProperty(ref todoCompleted, value); }
        }

        public DateTime TodoDateTime
        {
            get { return todoDateTime; }
            set { SetProperty(ref todoDateTime, value); }
        }
        #endregion
    }
}
