using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.SqlServer.Server;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Drawing;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http.HttpResults;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BookingApp.Server.Models
{
    public class Product
    {
        public int id { get; set; }
        public int? companyid { get; set; } = default;
        public int? productgroupid { get; set; } = default;        
        public string? code { get; set; } = string.Empty;
        public string? status { get; set; } = "A";
        public string? active { get; set; }
        public DateTime? inactivedate { get; set; } = null;
        public string? name1 { get; set; } = string.Empty;
        public string? name2 { get; set; } = string.Empty;
        public int? prodtype { get; set; } 
        public decimal? saleprice { get; set; } = 0M;
        public string? remark { get; set; } = string.Empty;        
        public int? vattype { get; set; }
        public string? unitname { get; set; } = string.Empty;
        public string? createby { get; set; } = string.Empty;
        public DateTime? createatutc { get; set; }
        public string? updateby { get; set; } = string.Empty;
        public DateTime? updateatutc { get; set; }
    }

}
