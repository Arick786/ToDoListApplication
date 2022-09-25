using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class ToDoMaxId
{
    [JsonProperty("max")]
    public int? MaxId { get; set; }
}

public class ToDoListsTable
{
    [JsonProperty("todoindex")]
    public int ToDoIndex { get; set; }

    [JsonProperty("todoname")]
    public string ToDoName { get; set; }

    [JsonProperty("todostatus")]
    public string ToDoStatus { get; set; }

    [JsonProperty("tododatetime")]
    public DateTime TodoDateTime { get; set; }

    [JsonProperty("insertdatetime")]
    public DateTime InsertDateTime { get; set; }

    [JsonProperty("updatedatetime")]
    public DateTime UpdateDateTime { get; set; } 
}