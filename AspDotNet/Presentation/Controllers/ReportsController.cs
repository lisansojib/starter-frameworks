using Ardalis.GuardClauses;
using AutoMapper;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces.Repositories;
using EPYSLACSCustomer.Service.Reporting;
using Presentation.Models;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Presentation.Controllers
{
    public class ReportsController : BaseController
    {
        private readonly IReportSuiteRepository _reportSuiteSqlRepository;
        private readonly IEfRepository<ReportSuite> _reportSuiteRepository;
        private readonly IEfRepository<ReportSuiteExternalSetup> _reportSuiteExternalSetupRepository;
        private readonly IEfRepository<ReportSuiteColumnValue> _reportSuiteColumnValueRepository;
        private readonly IMapper _mapper;
        private readonly RDLReportDocument _reportDocument;
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public ReportsController(IReportSuiteRepository reportSuiteSqlRepository
            , IEfRepository<ReportSuite> reportSuiteRepository
            , IEfRepository<ReportSuiteExternalSetup> reportSuiteExternalSetupRepository
            , IEfRepository<ReportSuiteColumnValue> reportSuiteColumnValueRepository
            , RDLReportDocument reportDocument
            , IMapper mapper)
        {
            _reportSuiteSqlRepository = reportSuiteSqlRepository;
            _reportSuiteRepository = reportSuiteRepository;
            _reportSuiteExternalSetupRepository = reportSuiteExternalSetupRepository;
            _reportSuiteColumnValueRepository = reportSuiteColumnValueRepository;
            _reportDocument = reportDocument;
            _mapper = mapper;
        }

        public ActionResult Index()
        {
            return View();
        }

        public async Task<JsonResult> GetMenus(int applicationId)
        {
            var records = await _reportSuiteSqlRepository.GetMenusAsync(UserId, applicationId, AppUser.CompanyId);
            return Json(records, JsonRequestBehavior.AllowGet);
        }

        #region Pdf Report View
        [HttpGet]
        public ActionResult PdfView()
        {
            int.TryParse(Request.QueryString["ReportId"], out int reportId);

            var reportEntity = _reportSuiteRepository.Find(reportId);
            Guard.Against.NullEntity(reportId, reportEntity);

            string reportPath = string.Format("{0}{1}", reportEntity.ReportPathName, reportEntity.ReportName);
            if (!System.IO.File.Exists(reportPath))
                throw new Exception("Can't load report file.");

            var filterSetList = JsonConvert.DeserializeObject<List<FilterSets>>(Request.QueryString["FilterSetList"]);

            _reportDocument.SetFields(UserId.ToString(), reportEntity.NodeText, reportEntity.ReportPathName);
            _reportDocument.Load(reportPath);
            _reportDocument.FilterSetList = filterSetList;
            _reportDocument.LoadSourceDataSet();
            _reportDocument.SetFilterValue();

            byte[] pdfByte = RDLReportExporter.GetExportedByte(ExportType.PDF, _reportDocument.DsSource, _reportDocument.ReportPath, _reportDocument.Parameters, _reportDocument.SubReportList);
            Response.Clear();
            Response.ContentType = "Application/pdf";
            string FileName = "";
            if (Request.QueryString["FileName"] != "" && Request.QueryString["FileName"] != null)
            {
                FileName = Request.QueryString["FileName"].Trim();
                Response.AddHeader("Content-Disposition", "inline; filename=" + FileName + ".pdf;");
            }

            Response.Buffer = false;
            Response.OutputStream.Write(pdfByte, 0, pdfByte.Length);
            Response.Flush();
            Response.End();
            return View();
        }

        public async Task<ActionResult> InlinePdfView()
        {
            int.TryParse(Request.QueryString["ReportId"], out int reportId);
            var reportEntity = await _reportSuiteRepository.FindAsync(reportId);
            Guard.Against.NullEntity(reportId, reportEntity);

            var reportName = reportEntity.ReportName;
            var reportPathName = reportEntity.ReportPathName;

            bool.TryParse(Request.QueryString["HasExternalReport"], out bool hasExternalReport);

            if (hasExternalReport)
            {
                int.TryParse(Request.QueryString["ExternalId"], out int externalId);

                var reportSuiteExternalSetup = _reportSuiteExternalSetupRepository.Find(x => x.Reportid == reportId && x.ExternalId == externalId);
                if (reportSuiteExternalSetup != null)
                {
                    reportName = reportSuiteExternalSetup.ReportName;
                    reportPathName = reportSuiteExternalSetup.ReportPathName;
                }
            }

            string reportPath = string.Format("{0}{1}", reportPathName, reportName);
            if (!System.IO.File.Exists(reportPath))
                throw new Exception("Can't load report file.");

            var parameters = new List<CustomeParameter>();
            foreach (var key in Request.QueryString.AllKeys)
            {
                if (key.Equals("ReportId", StringComparison.OrdinalIgnoreCase) || key.Equals("HasExternalReport", StringComparison.OrdinalIgnoreCase) || key.Equals("ExternalId", StringComparison.OrdinalIgnoreCase))
                    continue;

                var param = new CustomeParameter { ParameterName = key, ParameterValue = Request.QueryString[key] };
                parameters.Add(param);
            }

            var parameterValues = await _reportSuiteSqlRepository.LoadReportParameterInfoAsync(reportEntity.Id);
            parameterValues.Tables[0].TableName = "ParameterValues";

            var reportColumnList = new List<ReportSuiteColumnValueViewModel>();
            _reportDocument.SetFields(UserId.ToString(), reportEntity.NodeText, reportEntity.ReportPathName);
            _reportDocument.Load(reportPath);
            _reportDocument.LoadFilterTable(parameterValues.Tables["ParameterValues"].Columns, reportColumnList);
            _reportDocument.SetParameterValue(parameters);
            _reportDocument.LoadSourceDataSet();
            _reportDocument.SetFilterValue();

            byte[] pdfByte = RDLReportExporter.GetExportedByte(ExportType.PDF, _reportDocument.DsSource, _reportDocument.ReportPath, _reportDocument.Parameters, _reportDocument.SubReportList);
            Response.Clear();
            Response.ContentType = "Application/pdf";
            string FileName = "";
            if (Request.QueryString["FileName"] != "" && Request.QueryString["FileName"] != null)
            {
                FileName = Request.QueryString["FileName"].Trim();
                Response.AddHeader("Content-Disposition", "inline; filename=" + FileName + ".pdf;");
            }

            Response.Buffer = false;
            Response.OutputStream.Write(pdfByte, 0, pdfByte.Length);
            Response.Flush();
            Response.End();
            return View();
        }
        #endregion

        public async Task<ActionResult> GetReportInformation(int reportId, bool hasExternalReport, int? buyerId)
        {
            var reportEntity = await _reportSuiteRepository.FindAsync(reportId);
            Guard.Against.NullEntity(reportId, reportEntity);

            var columnValueOptions = new List<dynamic>();
            if (!string.IsNullOrEmpty(reportEntity.ReportSql))
                columnValueOptions = _reportSuiteSqlRepository.GetDynamicData(reportEntity.ReportSql);

            var reportName = reportEntity.ReportName;
            var reportPathName = reportEntity.ReportPathName;

            if (hasExternalReport)
            {
                var reportSuiteExternalSetup = _reportSuiteExternalSetupRepository.Find(x => x.Reportid == reportId && x.ExternalId == buyerId);
                if (reportSuiteExternalSetup != null)
                {
                    reportName = reportSuiteExternalSetup.ReportName;
                    reportPathName = reportSuiteExternalSetup.ReportPathName;
                }
            }

            var reportPhysicalPath = Path.Combine(reportPathName, reportName);
            if (!System.IO.File.Exists(reportPhysicalPath))
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            var records = _reportSuiteColumnValueRepository.ListAll(x => x.ReportId == reportId);
            var columnValues = _mapper.Map<List<ReportSuiteColumnValueViewModel>>(records);

            foreach (var item in columnValues.FindAll(x => !string.IsNullOrEmpty(x.DefaultValueMethodName) && !string.IsNullOrEmpty(x.DefaultValueColumnId)))
                item.DefaultValue = await _reportSuiteSqlRepository.GetStringValueAsync(item.DefaultValueMethodName);

            var parameterValues = await _reportSuiteSqlRepository.LoadReportParameterInfoAsync(reportId);
            parameterValues.Tables[0].TableName = "ParameterValues";
            _reportDocument.SetFields(UserId.ToString(), reportEntity.NodeText, reportPathName);
            _reportDocument.Load(reportPhysicalPath);
            _reportDocument.LoadFilterTable(parameterValues.Tables["ParameterValues"].Columns, columnValues);

            if (hasExternalReport)
            {
                var buyerFilterSet = _reportDocument.FilterSetList.Find(x => x.ColumnName.Equals("BuyerID", System.StringComparison.OrdinalIgnoreCase));
                if (buyerFilterSet != null)
                {
                    buyerFilterSet.ColumnValue = buyerId.ToString();
                    buyerFilterSet.DefaultValue = buyerId.ToString();
                }
            }

            foreach (var item in _reportDocument.FilterSetList)
                item.ColumnValue = string.IsNullOrEmpty(item.DefaultValue) ? "" : item.DefaultValue;


            return Json(new { _reportDocument.FilterSetList, ColumnValueOptions = columnValueOptions }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetFilterColumnOptions()
        {
            var parameters = new List<SqlParameter>();
            foreach (var key in Request.QueryString.AllKeys)
            {
                if (key.Equals("ReportId") || key.Equals("MethodName"))
                    continue;

                var param = new SqlParameter($"@{key}", Request.QueryString[key]);
                parameters.Add(param);
            }

            var data = _reportSuiteSqlRepository.GetDynamicData(Request.QueryString["MethodName"], parameters.ToArray());

            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}