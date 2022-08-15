using CISM_PJ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CISM_PJ.Areas.Setup.Models
{
    public class ExchangeRateModel
    {
        public System.Guid exchange_rate_id { get; set; }
        public int to_currency_id { get; set; }
        public int from_currency_id { get; set; }
        public System.DateTime date { get; set; }
        public decimal rate { get; set; }
        public System.DateTime createddate { get; set; }
        public System.DateTime modifieddate { get; set; }
        public int createduser { get; set; }
        public int modifieduser { get; set; }
        public ModelState state { get; set; }
        public string sfrom_cur_code { get; set; }
        public string sto_cur_code { get; set; }
        public string sdate { get; set; }
        public static implicit operator ExchangeRateModel(ExchangeRate data)
        {
            return new ExchangeRateModel
            {
                exchange_rate_id = data.exchange_rate_id,
                to_currency_id = data.to_currency_id,
                from_currency_id = data.from_currency_id,
                sdate = data.date.ToString("dd/MM/yyyy"),
                date = data.date,
                rate = data.rate,
                sto_cur_code = data.Currency.currency_code,
                sfrom_cur_code = data.Currency1.currency_code,
                createddate = data.createddate,
                modifieddate = data.modifieddate,
                createduser = data.createduser,
                modifieduser = data.modifieduser
            };
        }
    }
}