using System;
using System.Collections.Generic;

namespace Presentation.Models
{
    public class UkAndCeLabellingMasterBindingModel : BaseViewModel
    {
        public UkAndCeLabellingMasterBindingModel()
        {
            Childs = new List<UkAndCeLabellingChildBindingModel>();
            OrderDate = DateTime.Now;
        }

        ///<summary>
        /// OrderNo (length: 100)
        ///</summary>
        public string OrderNo { get; set; }

        ///<summary>
        /// OrderDate
        ///</summary>
        public DateTime OrderDate { get; set; }

        ///<summary>
        /// DeliveryDate
        ///</summary>
        public DateTime? DeliveryDate { get; set; }

        ///<summary>
        /// OrderForId
        ///</summary>
        public int OrderForId { get; set; }

        ///<summary>
        /// OrderFor
        ///</summary>
        public string OrderFor { get; set; }

        ///<summary>
        /// CustomerID
        ///</summary>
        public int CustomerId { get; set; }

        ///<summary>
        /// Acknowledge
        ///</summary>
        public bool Acknowledge { get; set; }

        ///<summary>
        /// Reject
        ///</summary>
        public bool Reject { get; set; }

        ///<summary>
        /// RejectRason (length: 200)
        ///</summary>
        public string RejectRason { get; set; }

        ///<summary>
        /// Cancel
        ///</summary>
        public bool Cancel { get; set; }

        ///<summary>
        /// CancelRason (length: 200)
        ///</summary>
        public string CancelRason { get; set; }

        ///<summary>
        /// Printing
        ///</summary>
        public bool Printing { get; set; }

        ///<summary>
        /// ReadyForDelivery
        ///</summary>
        public bool ReadyForDelivery { get; set; }

        ///<summary>
        /// Shipped
        ///</summary>
        public bool Shipped { get; set; }

        ///<summary>
        /// Delivered
        ///</summary>
        public bool Delivered { get; set; }

        ///<summary>
        /// UserIP (length: 50)
        ///</summary>
        public string UserIp { get; set; }

        ///<summary>
        /// AddedBy
        ///</summary>
        public int AddedBy { get; set; }

        ///<summary>
        /// DateAdded
        ///</summary>
        public DateTime DateAdded { get; set; }

        ///<summary>
        /// UpdatedBy
        ///</summary>
        public int? UpdatedBy { get; set; }

        ///<summary>
        /// DateUpdated
        ///</summary>
        public DateTime? DateUpdated { get; set; }

        /// <summary>
        /// Child UkAndCeLabellingChilds where [UKAndCELabellingChild].[OrderID] point to this entity (FK_UKAndCELabellingChild_UKAndCELabellingMaster)
        /// </summary>
        public List<UkAndCeLabellingChildBindingModel> Childs { get; set; }
    }

    public class UkAndCeLabellingChildBindingModel : BaseViewModel
    {
        ///<summary>
        /// OrderID
        ///</summary>
        public int OrderId { get; set; }

        ///<summary>
        /// PackTypeId
        ///</summary>
        public int PackTypeId { get; set; }

        ///<summary>
        /// PackType
        ///</summary>
        public string PackType { get; set; }

        ///<summary>
        /// SeasonId
        ///</summary>
        public int SeasonId { get; set; }

        ///<summary>
        /// Season
        ///</summary>
        public string Season { get; set; }

        ///<summary>
        /// TPND (length: 13)
        ///</summary>
        public string TPND { get; set; }

        /// <summary>
        /// PO No
        /// </summary>
        public string PONo { get; set; }

        ///<summary>
        /// UKStyleRef (length: 10)
        ///</summary>
        public string UKStyleRef { get; set; }

        ///<summary>
        /// CEStyleRef (length: 10)
        ///</summary>
        public string CEStyleRef { get; set; }

        ///<summary>
        /// ShortDesc (length: 22)
        ///</summary>
        public string ShortDesc { get; set; }

        ///<summary>
        /// EQOSCode (length: 20)
        ///</summary>
        public string EQOSCode { get; set; }

        ///<summary>
        /// BarcodeNo (length: 14)
        ///</summary>
        public string BarcodeNo { get; set; }

        ///<summary>
        /// SupplierId
        ///</summary>
        public int SupplierId { get; set; }

        ///<summary>
        /// Supplier
        ///</summary>
        public string Supplier { get; set; }

        ///<summary>
        /// PackagingSupplierId
        ///</summary>
        public int PackagingSupplierId { get; set; }

        ///<summary>
        /// PackagingSupplier
        ///</summary>
        public string PackagingSupplier { get; set; }

        ///<summary>
        /// DeptId
        ///</summary>
        public int DeptId { get; set; }

        ///<summary>
        /// Dept
        ///</summary>
        public string Dept { get; set; }

        ///<summary>
        /// BrandId
        ///</summary>
        public int BrandId { get; set; }

        ///<summary>
        /// Brand
        ///</summary>
        public string Brand { get; set; }

        ///<summary>
        /// RFIDCompliant
        ///</summary>
        public bool RfidCompliant { get; set; }

        ///<summary>
        /// TagAtSource
        ///</summary>
        public bool TagAtSource { get; set; }

        ///<summary>
        /// NoofPiecesinRatio
        ///</summary>
        public decimal NoofPiecesinRatio { get; set; }

        ///<summary>
        /// NoofRationinOneCartoon
        ///</summary>
        public decimal NoofRationinOneCartoon { get; set; }

        ///<summary>
        /// NoofPiecesinOneRatio
        ///</summary>
        public decimal NoofPiecesinOneRatio { get; set; }

        ///<summary>
        /// NoofPiecesinCartoonPick
        ///</summary>
        public decimal NoofPiecesinCartoonPick { get; set; }

        ///<summary>
        /// CasesPerCartoon
        ///</summary>
        public decimal CasesPerCartoon { get; set; }

        ///<summary>
        /// UnitsPerCase
        ///</summary>
        public decimal UnitsPerCase { get; set; }

        ///<summary>
        /// PiecesPerShroud
        ///</summary>
        public decimal PiecesPerShroud { get; set; }

        ///<summary>
        /// ManufactureDate
        ///</summary>
        public string ManufactureDate { get; set; }

        ///<summary>
        /// Qty
        ///</summary>
        public decimal Qty { get; set; }

    }
}