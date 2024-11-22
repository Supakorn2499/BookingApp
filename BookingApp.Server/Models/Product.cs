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
        public int umid { get; set; } = default;
        public int? categoryid { get; set; } = default;
        public int? brandid { get; set; } = default;
        public int? formatid { get; set; } = default;
        public int? designid { get; set; } = default;
        public int? gradeid { get; set; } = default;
        public int? modelid { get; set; } = default;
        public int? sizeid { get; set; } = default;
        public int? colorid { get; set; } = default;
        public int? weightid { get; set; } = default;
        public int? segmentid { get; set; } = default;
        public int? accbuycashid { get; set; } = default;
        public int? accbuycreditid { get; set; } = default;
        public int? accsalecashid { get; set; } = default;
        public int? accsalecreditid { get; set; } = default;
        public int? acccostid { get; set; } = default;
        public int? accstockid { get; set; } = default;
        public int? AccWorkInProcessId { get; set; } = default;
        public int? accsuppliedusedid { get; set; } = default;
        public int? accmaterialrequistionid { get; set; } = default;
        public int? budgetgroupid { get; set; } = default;
        public int? acccngoodid { get; set; } = default;
        public int? acccnpriceid { get; set; } = default;
        public int? accincreasedebtreturnincreaseid { get; set; } = default;
        public int? accincreasedebtpriceincreaseid { get; set; } = default;
        public string? code { get; set; } = string.Empty;
        public string? status { get; set; } = "A";
        public string? active { get; set; }
        public DateTime? inactivedate { get; set; } = null;
        public string? name1 { get; set; } = string.Empty;
        public string? sname1 { get; set; } = string.Empty;
        public string? name2 { get; set; } = string.Empty;
        public string? sname2 { get; set; } = string.Empty;
        public string? prodtype { get; set; } = string.Empty;
        public string? taxrate { get; set; } = string.Empty;
        public string? inputtax { get; set; } = string.Empty;
        public string? salestax { get; set; } = string.Empty;
        public string? stockcounting { get; set; }
        public string? showinventories { get; set; } = "Y";
        public string? purchaseprice { get; set; } = string.Empty;
        public string? saleprice { get; set; }
        public decimal stdcost { get; set; } = 0M;
        public decimal stdprice { get; set; } = 0M;
        public decimal minprice { get; set; } = 0M;
        public string? remark1 { get; set; } = string.Empty;
        public string? remark2 { get; set; } = string.Empty;
        public string? remark3 { get; set; } = string.Empty;
        public string? remark4 { get; set; } = string.Empty;
        public string? remark5 { get; set; } = string.Empty;
        public string? remark6 { get; set; } = string.Empty;
        public string? remark7 { get; set; } = string.Empty;
        public string? remark8 { get; set; } = string.Empty;
        public string? remark9 { get; set; } = string.Empty;
        public string? remark10 { get; set; } = string.Empty;
        public string? vattype { get; set; }
        public string? commision { get; set; } = "N";
        public string? cardfee { get; set; } = "N";
        public string? createby { get; set; } = string.Empty;
        public DateTime? createatutc { get; set; }
        public string? updateby { get; set; } = string.Empty;
        public DateTime? updateatutc { get; set; }
    }

}
