using System;
using System.Collections.Generic;

namespace Presentation.Models
{
    public class TSLLabellingMasterBindingModel : BaseViewModel
    {
        public TSLLabellingMasterBindingModel()
        {
            Childs = new List<TSLLabellingChildBindingModel>();
            OrderDate = DateTime.Now;
        }

        public string OrderNo { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public int OrderForId { get; set; }
        public string OrderFor { get; set; }
        public int CustomerId { get; set; }
        public bool Acknowledge { get; set; }
        public bool Reject { get; set; }
        public bool Printing { get; set; }
        public bool ReadyForDelivery { get; set; }
        public bool Shipped { get; set; }
        public bool Delivered { get; set; }
        public List<TSLLabellingChildBindingModel> Childs { get; set; }
    }

    public class TSLLabellingChildBindingModel : BaseViewModel
    {
        public TSLLabellingChildBindingModel()
        {
            HasError = true;
        }

        public int OrderId { get; set; }
        public string BarcodeNo { get; set; }
        public string ShortDesc { get; set; }
        public string TPND { get; set; }
        public decimal Qty { get; set; }
        public bool HasError { get; set; }
    }
}