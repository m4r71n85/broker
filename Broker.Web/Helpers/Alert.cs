using System.Collections.Generic;
using System.Web.Mvc;

namespace Broker.Web.Helpers
{
    public static class Alerts
    {
        public static void Add(TempDataDictionary tempData, AlertProps alertProps)
        {
            if (!tempData.ContainsKey("alerts"))
            {
                tempData.Add("alerts", new List<AlertProps>());
            }
            ((List<AlertProps>)tempData["alerts"]).Add(alertProps);
        }
    }

    public class AlertProps
    {
        public string AlertType { get; set; }
        public string Message { get; set; }

        public AlertProps(string message)
            : this("alert-success", message)
        {
        }

        public AlertProps(string AlertType, string message)
        {
            this.AlertType = AlertType;
            Message = message;
        }
    }

    public static class AlertType
    {
        public const string Success = "alert-success";
        public const string Info = "alert-info";
        public const string Warning = "alert-warning";
        public const string Danger = "alert-danger";
    }
}