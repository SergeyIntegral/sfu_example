using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Example.Web.Models
{
    public enum BreadCrumbTypes
    {
        Text,
        Link,
        MvcAction
    }

    public class BreadCrumbItem
    {
        public string Text { get; set; }
        public string Url { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public BreadCrumbTypes Type { get; set; }

        public BreadCrumbItem(string text)
        {
            Text = text;
            Type = BreadCrumbTypes.Text;
        }

        public BreadCrumbItem(string text, string url)
        {
            Text = text;
            Url = url;
            Type = BreadCrumbTypes.Link;
        }

        public BreadCrumbItem(string text, string controller, string action)
        {
            Text = text;
            Controller = controller;
            Action = action;
            Type = BreadCrumbTypes.MvcAction;
        }
    }
}