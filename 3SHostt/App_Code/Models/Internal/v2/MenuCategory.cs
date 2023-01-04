using System;
using System.Activities.Expressions;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for MenuCategory
/// </summary>
public class MenuCategory
{
    public string GroupCode { get; set; }
    public List<MenuItems> Items { get; set; } = new List<MenuItems>();
}