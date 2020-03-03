using Ardalis.GuardClauses;
using AutoMapper;
using EPYSLACSCustomer.Core.DTOs;
using EPYSLACSCustomer.Core.Entities;
using EPYSLACSCustomer.Core.Interfaces.Repositories;
using EPYSLACSCustomer.Core.Interfaces.Services;
using EPYSLACSCustomer.Core.Statics;
using Presentation.Extends;
using Presentation.Extends.Filters;
using Presentation.Extends.Helpers;
using Presentation.Extends.Providers;
using Presentation.Models;
using Presentation.Models.Validation;
using ExcelDataReader;
using System;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Presentation.Controllers.Apis
{
    [CustomAuthorize(Roles ="SuperUser")]
    [RoutePrefix("api/labelling")]
    public class LabellingApiController : ApiBaseController
    {
        private readonly IEfRepository<TSLLabellingMaster> _tslLabellingRepository;
        private readonly IEfRepository<UKAndCELabellingMaster> _ukAndCELabellingRepository;
        private readonly IEfRepository<EntityType> _entityTypeRepository;
        private readonly IEfRepository<EntityTypeValue> _entityTypeValueRepository;
        private readonly ISignatureRepository _signatureRepository;
        private readonly ITSLLabellingService _tslLabellingService;
        private readonly IUKAndCELabellingService _ukAndCELabellingService;
        private readonly ISelect2Service _select2Service;
        private readonly ICommonHelpers _commonHelpers;
        private readonly IMapper _mapper;

        public LabellingApiController(IEfRepository<TSLLabellingMaster> tslLabellingRepository
            , IEfRepository<UKAndCELabellingMaster> ukAndCELabellingRepository
            , IEfRepository<EntityType> entityTypeRepository
            , IEfRepository<EntityTypeValue> entityTypeValueRepository
            , ISignatureRepository signatureRepository
            , ITSLLabellingService tslLabellingService
            , IUKAndCELabellingService ukAndCELabellingService
            , ISelect2Service select2Service
            , ICommonHelpers commonHelpers
            , IMapper mapper)
        {
            _tslLabellingRepository = tslLabellingRepository;
            _ukAndCELabellingRepository = ukAndCELabellingRepository;
            _entityTypeRepository = entityTypeRepository;
            _entityTypeValueRepository = entityTypeValueRepository;
            _signatureRepository = signatureRepository;
            _tslLabellingService = tslLabellingService;
            _ukAndCELabellingService = ukAndCELabellingService;
            _select2Service = select2Service;
            _commonHelpers = commonHelpers;
            _mapper = mapper;
        }

        #region TSL
        [Route("tsl")]
        [HttpGet]
        public IHttpActionResult GetTSL(string region, OrderStatus orderStatus, int offset = 0, int limit = 10, string filter = null, string sort = null, string order = null)
        {
            var filterBy = _commonHelpers.GetFilterBy(filter);
            var orderBy = string.IsNullOrEmpty(sort) ? "" : $"ORDER BY {sort} {order}";
            var records = _tslLabellingService.GetTSLLabellings(AppUser.IsSuperUser, AppUser.ContactId, region, orderStatus, offset, limit, filterBy, orderBy);
            return Ok(records);
        }

        [HttpGet]
        [Route("tsl-details/{id}")]
        public async Task<IHttpActionResult> GetTSLDetails(int id)
        {
            var entity = await _tslLabellingRepository.FindAsync(id);
            Guard.Against.NullEntity(id, entity);

            var record = _mapper.Map<TSLLabellingMasterDTO>(entity);
            record.OrderForList = _select2Service.GetEntityTypeValues(LabellingEntityTypes.LabellingOrderFor).FindAll(x => x.text.Contains(LabelTypes.TSL));
            record.RegionList = new System.Collections.Generic.List<Select2Option>
            {
                new Select2Option {id = "UK", text = "UK"},
                new Select2Option {id = "CE", text = "CE"},
            };

            record.Region = record.OrderForList.Find(x => x.id == entity.OrderForId.ToString()).text.Contains("CE") ? "CE" : "UK";
            if (record.Region == "UK")
                record.OrderForList = record.OrderForList.FindAll(x => !x.text.Contains("CE"));
            else
                record.OrderForList = record.OrderForList.FindAll(x => x.text.Contains("CE"));

            return Ok(record);
        }

        [Route("tsl-process-file")]
        [HttpPost]
        public async Task<IHttpActionResult> ProcessTSLFile()
        {
            if (!Request.Content.IsMimeMultipartContent())
                return BadRequest("Unsupported media type.");

            var provider = await Request.Content.ReadAsMultipartAsync(new InMemoryMultipartFormDataStreamProvider());
            if (!provider.Files.Any())
                return BadRequest("You must upload TSL data.");

            var formData = provider.FormData;
            var model = formData.ConvertToObject<TSLLabellingMasterBindingModel>();

            var originalFile = provider.Files[0];

            var inputStream = await originalFile.ReadAsStreamAsync();
            using (var reader = ExcelReaderFactory.CreateReader(inputStream))
            {
                var result = reader.AsDataSet();

                foreach (DataTable table in result.Tables)
                    model.Childs = table.ConvertToList<TSLLabellingChildBindingModel>();

                // Validate Data
                var validator = new TSLMasterBindingModelValidator(model.OrderFor);
                var validationResult = validator.Validate(model);
                if (!validationResult.IsValid)
                {
                    string messages = string.Join("<br>", validationResult.Errors.Select(x => x.PropertyName.Replace("Childs", "Row") + " : " + x.ErrorMessage));
                    return BadRequest(messages);
                }
            }

            return Ok(model.Childs);
        }

        [Route("tsl")]
        [HttpPost]
        public async Task<IHttpActionResult> UploadTSLLabelling()
        {
            if (AppUser.IsSuperUser)
                return BadRequest("You are not authorized to access this resource.");

            if (!Request.Content.IsMimeMultipartContent())
                return BadRequest("Unsupported media type.");

            var provider = await Request.Content.ReadAsMultipartAsync(new InMemoryMultipartFormDataStreamProvider());
            if (!provider.Files.Any())
                return BadRequest("You must upload TSL data.");

            var formData = provider.FormData;
            var model = formData.ConvertToObject<TSLLabellingMasterBindingModel>();

            var filePath = string.Empty;
            var originalFile = provider.Files[0];

            var inputStream = await originalFile.ReadAsStreamAsync();
            using (var reader = ExcelReaderFactory.CreateReader(inputStream))
            {
                var result = reader.AsDataSet();

                foreach (DataTable table in result.Tables)
                    model.Childs = table.ConvertToList<TSLLabellingChildBindingModel>();

                // Validate Data
                var validator = new TSLMasterBindingModelValidator(model.OrderFor);
                var validationResult = validator.Validate(model);
                if (!validationResult.IsValid)
                {
                    string messages = string.Join("<br>", validationResult.Errors.Select(x => x.PropertyName.Replace("Childs", "Row") + " : " + x.ErrorMessage));
                    return BadRequest(messages);
                }

                // Save file
                var fileName = string.Join(string.Empty, originalFile.Headers.ContentDisposition.FileName.Split(Path.GetInvalidFileNameChars()));
                var contentType = originalFile.Headers.ContentType.ToString();

                filePath = $"{UploadPaths.TSL}/{Guid.NewGuid()}_{fileName}";
                var savePath = HttpContext.Current.Server.MapPath(filePath);
                using (var fileStream = File.Create(savePath))
                {
                    inputStream.Seek(0, SeekOrigin.Begin);
                    await inputStream.CopyToAsync(fileStream);
                }
            }

            TSLLabellingMaster entity;
            if (model.IsModified())
            {
                entity = await _tslLabellingRepository.FindAsync(model.Id);
                Guard.Against.NullEntity(model.Id, entity);

                entity.OrderForId = model.OrderForId;
                entity.CustomerId = model.CustomerId;
                entity.UpdatedBy = UserId;
                entity.DateUpdated = DateTime.Now;

                foreach (var item in entity.Childs)
                    item.EntityState = EntityState.Unchanged;

                foreach (var item in model.Childs)
                {
                    var childEntity = entity.Childs.FirstOrDefault(x => x.BarcodeNo == item.BarcodeNo && x.ShortDesc == item.ShortDesc && x.TPND == item.TPND);
                    if (childEntity == null)
                    {
                        childEntity = _mapper.Map<TSLLabellingChild>(item);
                    }
                    else
                    {
                        childEntity.Qty = item.Qty;
                        childEntity.EntityState = EntityState.Modified;
                    }
                }
            }
            else
            {
                entity = _mapper.Map<TSLLabellingMaster>(model);
                entity.AddedBy = UserId;
            }
            entity.FilePath = filePath;
            entity.UserIP = Request.GetClientIpAddress();
            entity.CustomerId = AppUser.ContactId;

            await _tslLabellingService.SaveAsync(entity);

            return Ok();
        }

        [HttpPost]
        [Route("tsl-acknowledge")]
        public async Task<IHttpActionResult> TSLAcknowledge(int id, bool acknowledge)
        {
            var entity = await _tslLabellingRepository.FindAsync(id);
            Guard.Against.NullEntity(id, entity);

            entity.Acknowledge = acknowledge;
            entity.AcknowledgeBy = UserId;
            entity.AcknowledgeDate = DateTime.Now;
            entity.UpdatedBy = UserId;
            entity.DateUpdated = DateTime.Now;
            entity.EntityState = EntityState.Modified;

            await _tslLabellingRepository.UpdateAsync(entity);
            return Ok();
        }

        [HttpPost]
        [Route("tsl-reject")]
        public async Task<IHttpActionResult> TSLReject(int id, bool reject)
        {
            var entity = await _tslLabellingRepository.FindAsync(id);
            Guard.Against.NullEntity(id, entity);

            entity.Reject = reject;
            entity.RejectBy = UserId;
            entity.RejectDate = DateTime.Now;
            entity.UpdatedBy = UserId;
            entity.DateUpdated = DateTime.Now;
            entity.EntityState = EntityState.Modified;

            await _tslLabellingRepository.UpdateAsync(entity);
            return Ok();
        }

        [HttpPost]
        [Route("tsl-printing")]
        public async Task<IHttpActionResult> TSLPrinting(int id, bool printing)
        {
            var entity = await _tslLabellingRepository.FindAsync(id);
            Guard.Against.NullEntity(id, entity);

            entity.Printing = printing;
            entity.PrintedBy = UserId;
            entity.PrintedDate = DateTime.Now;
            entity.UpdatedBy = UserId;
            entity.DateUpdated = DateTime.Now;
            entity.EntityState = EntityState.Modified;

            await _tslLabellingRepository.UpdateAsync(entity);
            return Ok();
        }

        [HttpPost]
        [Route("tsl-ready-for-delivery")]
        public async Task<IHttpActionResult> TSLReadyForDelivery(int id, bool readyForDelivery)
        {
            var entity = await _tslLabellingRepository.FindAsync(id);
            Guard.Against.NullEntity(id, entity);

            entity.ReadyForDelivery = readyForDelivery;
            entity.ReadyForDeliveryBy = UserId;
            entity.ReadyForDeliveryDate = DateTime.Now;
            entity.UpdatedBy = UserId;
            entity.DateUpdated = DateTime.Now;
            entity.EntityState = EntityState.Modified;

            await _tslLabellingRepository.UpdateAsync(entity);
            return Ok();
        }

        [HttpPost]
        [Route("tsl-shipped")]
        public async Task<IHttpActionResult> TSLShipped(int id, bool shipped)
        {
            var entity = await _tslLabellingRepository.FindAsync(id);
            Guard.Against.NullEntity(id, entity);

            entity.ReadyForDelivery = shipped;
            entity.ShippedBy = UserId;
            entity.ShippedDate = DateTime.Now;
            entity.UpdatedBy = UserId;
            entity.DateUpdated = DateTime.Now;
            entity.EntityState = EntityState.Modified;

            await _tslLabellingRepository.UpdateAsync(entity);
            return Ok();
        }

        [HttpPost]
        [Route("tsl-delivered")]
        public async Task<IHttpActionResult> TSLDelivered(int id, bool delivered)
        {
            var entity = await _tslLabellingRepository.FindAsync(id);
            Guard.Against.NullEntity(id, entity);

            entity.ReadyForDelivery = delivered;
            entity.ShippedBy = UserId;
            entity.ShippedDate = DateTime.Now;
            entity.UpdatedBy = UserId;
            entity.DateUpdated = DateTime.Now;
            entity.EntityState = EntityState.Modified;

            await _tslLabellingRepository.UpdateAsync(entity);
            return Ok();
        }
        #endregion

        #region UK & CE Labelling
        [Route("uk-and-ce")]
        [HttpGet]
        public IHttpActionResult GetUKAndCELabelling(string labellingType, OrderStatus orderStatus, int offset = 0, int limit = 10, string filter = null, string sort = null, string order = null)
        {
            var filterBy = _commonHelpers.GetFilterBy(filter);
            var orderBy = string.IsNullOrEmpty(sort) ? "" : $"ORDER BY {sort} {order}";
            var records = _ukAndCELabellingService.GetUKAndCELabellings(AppUser.IsSuperUser, AppUser.ContactId, labellingType, orderStatus, offset, limit, filterBy, orderBy);
            return Ok(records);
        }

        [Route("uk-and-ce-details/{id}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetUKAndCELabellingDetails(int id)
        {
            var entity = await _ukAndCELabellingRepository.FindAsync(id);
            Guard.Against.NullEntity(id, entity);

            var childs = _ukAndCELabellingService.GetUKAndCELabellingChilds(id);
            var record = _mapper.Map<UkAndCeLabellingMasterDTO>(entity);
            record.OrderForList = _select2Service.GetEntityTypeValues(LabellingEntityTypes.LabellingOrderFor);
            record.Childs = childs;

            return Ok(record);
        }

        [Route("uk-and-ce/process-file")]
        [HttpPost]
        public async Task<IHttpActionResult> ProcessTCLAndTHL()
        {
            if (!Request.Content.IsMimeMultipartContent())
                return BadRequest("Unsupported media type.");

            var provider = await Request.Content.ReadAsMultipartAsync(new InMemoryMultipartFormDataStreamProvider());
            if (!provider.Files.Any())
                return BadRequest("You must upload data.");

            var formData = provider.FormData;
            var model = formData.ConvertToObject<UkAndCeLabellingMasterBindingModel>();

            var originalFile = provider.Files[0];

            Stream inputStream = await originalFile.ReadAsStreamAsync();
            using (var reader = ExcelReaderFactory.CreateReader(inputStream))
            {
                var result = reader.AsDataSet();

                foreach (DataTable table in result.Tables)
                    model.Childs = table.ConvertToList<UkAndCeLabellingChildBindingModel>();

                // Validate Data
                var validator = new UkAndCeLabellingMasterBindingModelValidator(model.OrderFor);
                var validationResult = validator.Validate(model);
                if (!validationResult.IsValid)
                {
                    string messages = string.Join("<br>", validationResult.Errors.Select(x => x.PropertyName.Replace("Childs", "Row") + " : " + x.ErrorMessage));
                    return BadRequest(messages);
                }
            }

            return Ok(model.Childs);
        }

        [Route("uk-and-ce")]
        [HttpPost]
        public async Task<IHttpActionResult> UploadUKAndCELabelling()
        {
            if (AppUser.IsSuperUser)
                return BadRequest("You are not authorized to access this resource.");

            if (!Request.Content.IsMimeMultipartContent())
                return BadRequest("Unsupported media type.");

            var provider = await Request.Content.ReadAsMultipartAsync(new InMemoryMultipartFormDataStreamProvider());
            if (!provider.Files.Any())
                return BadRequest("You must upload data.");

            var formData = provider.FormData;
            var model = formData.ConvertToObject<UkAndCeLabellingMasterBindingModel>();

            var filePath = string.Empty;
            var originalFile = provider.Files[0];

            Stream inputStream = await originalFile.ReadAsStreamAsync();
            using (var reader = ExcelReaderFactory.CreateReader(inputStream))
            {
                var result = reader.AsDataSet();

                foreach (DataTable table in result.Tables)
                    model.Childs = table.ConvertToList<UkAndCeLabellingChildBindingModel>();

                // Validate Data
                var validator = new UkAndCeLabellingMasterBindingModelValidator(model.OrderFor);
                var validationResult = validator.Validate(model);
                if (!validationResult.IsValid)
                {
                    string messages = string.Join("<br>", validationResult.Errors.Select(x => x.PropertyName.Replace("Childs", "Row") + " : " + x.ErrorMessage));
                    return BadRequest(messages);
                }

                // Save file
                var fileName = string.Join(string.Empty, originalFile.Headers.ContentDisposition.FileName.Split(Path.GetInvalidFileNameChars()));
                var contentType = originalFile.Headers.ContentType.ToString();

                var directory = model.OrderFor.Contains("THL") ? UploadPaths.THL : UploadPaths.TCL;
                filePath = $"{directory}/{Guid.NewGuid()}_{fileName}";
                var savePath = HttpContext.Current.Server.MapPath(filePath);
                using (var fileStream = File.Create(savePath))
                {
                    inputStream.Seek(0, SeekOrigin.Begin);
                    await inputStream.CopyToAsync(fileStream);
                }
            }

            // Set EntityTypeValue IDs
            model = await SetEntityTypeValueIdsAsync(model);

            UKAndCELabellingMaster entity;
            if (model.IsModified())
            {
                entity = await _ukAndCELabellingRepository.FindAsync(model.Id);
                Guard.Against.NullEntity(model.Id, entity);

                entity.OrderForId = model.OrderForId;
                entity.CustomerId = model.CustomerId;
                entity.UpdatedBy = UserId;
                entity.DateUpdated = DateTime.Now;

                foreach (var item in entity.Childs)
                    item.EntityState = EntityState.Unchanged;

                foreach (var item in model.Childs)
                {
                    var childEntity = entity.Childs.FirstOrDefault(x => x.IsEqual(item));
                    if (childEntity == null)
                    {
                        childEntity = _mapper.Map<UKAndCELabellingChild>(item);
                    }
                    else
                    {
                        childEntity.Qty = item.Qty;
                        childEntity.EntityState = EntityState.Modified;
                    }
                }
            }
            else
            {
                entity = _mapper.Map<UKAndCELabellingMaster>(model);
                entity.DateAdded = DateTime.Now;
                entity.AddedBy = UserId;
            }
            entity.FilePath = filePath;
            entity.UserIp = Request.GetClientIpAddress();
            entity.CustomerId = AppUser.ContactId;

            await _ukAndCELabellingService.SaveAsync(entity);

            return Ok();
        }

        [HttpPost]
        [Route("uk-and-ce-cancel")]
        public async Task<IHttpActionResult> UKAndCELabellingCancel(int id, bool cancel, string cancelReason)
        {
            var entity = await _ukAndCELabellingRepository.FindAsync(id);
            Guard.Against.NullEntity(id, entity);

            if (string.IsNullOrEmpty(cancelReason))
                return BadRequest("Cancel reason is required.");

            entity.Cancel = cancel;
            entity.CancelBy = UserId;
            entity.CancelDate = DateTime.Now;
            entity.UpdatedBy = UserId;
            entity.DateUpdated = DateTime.Now;
            entity.EntityState = EntityState.Modified;

            await _ukAndCELabellingRepository.UpdateAsync(entity);
            return Ok();
        }

        [HttpPost]
        [Route("uk-and-ce-acknowledge")]
        public async Task<IHttpActionResult> UKAndCELabellingAcknowledge(int id, bool acknowledge)
        {
            var entity = await _ukAndCELabellingRepository.FindAsync(id);
            Guard.Against.NullEntity(id, entity);

            entity.Acknowledge = acknowledge;
            entity.AcknowledgeBy = UserId;
            entity.AcknowledgeDate = DateTime.Now;
            entity.UpdatedBy = UserId;
            entity.DateUpdated = DateTime.Now;
            entity.EntityState = EntityState.Modified;

            await _ukAndCELabellingRepository.UpdateAsync(entity);
            return Ok();
        }

        [HttpPost]
        [Route("uk-and-ce-reject")]
        public async Task<IHttpActionResult> UKAndCELabellingReject(int id, bool reject)
        {
            var entity = await _ukAndCELabellingRepository.FindAsync(id);
            Guard.Against.NullEntity(id, entity);

            entity.Reject = reject;
            entity.RejectBy = UserId;
            entity.RejectDate = DateTime.Now;
            entity.UpdatedBy = UserId;
            entity.DateUpdated = DateTime.Now;
            entity.EntityState = EntityState.Modified;

            await _ukAndCELabellingRepository.UpdateAsync(entity);
            return Ok();
        }

        [HttpPost]
        [Route("uk-and-ce-printing")]
        public async Task<IHttpActionResult> UKAndCELabellingPrinting(int id, bool printing)
        {
            var entity = await _ukAndCELabellingRepository.FindAsync(id);
            Guard.Against.NullEntity(id, entity);

            entity.Printing = printing;
            entity.PrintedBy = UserId;
            entity.PrintedDate = DateTime.Now;
            entity.UpdatedBy = UserId;
            entity.DateUpdated = DateTime.Now;
            entity.EntityState = EntityState.Modified;

            await _ukAndCELabellingRepository.UpdateAsync(entity);
            return Ok();
        }

        [HttpPost]
        [Route("uk-and-ce-ready-for-delivery")]
        public async Task<IHttpActionResult> UKAndCELabellingReadyForDelivery(int id, bool readyForDelivery)
        {
            var entity = await _ukAndCELabellingRepository.FindAsync(id);
            Guard.Against.NullEntity(id, entity);

            entity.ReadyForDelivery = readyForDelivery;
            entity.ReadyForDeliveryBy = UserId;
            entity.ReadyForDeliveryDate = DateTime.Now;
            entity.UpdatedBy = UserId;
            entity.DateUpdated = DateTime.Now;
            entity.EntityState = EntityState.Modified;

            await _ukAndCELabellingRepository.UpdateAsync(entity);
            return Ok();
        }

        [HttpPost]
        [Route("uk-and-ce-shipped")]
        public async Task<IHttpActionResult> UKAndCELabellingShipped(int id, bool shipped)
        {
            var entity = await _ukAndCELabellingRepository.FindAsync(id);
            Guard.Against.NullEntity(id, entity);

            entity.ReadyForDelivery = shipped;
            entity.ShippedBy = UserId;
            entity.ShippedDate = DateTime.Now;
            entity.UpdatedBy = UserId;
            entity.DateUpdated = DateTime.Now;
            entity.EntityState = EntityState.Modified;

            await _ukAndCELabellingRepository.UpdateAsync(entity);
            return Ok();
        }

        [HttpPost]
        [Route("uk-and-ce-delivered")]
        public async Task<IHttpActionResult> UKAndCELabellingDelivered(int id, bool delivered)
        {
            var entity = await _ukAndCELabellingRepository.FindAsync(id);
            Guard.Against.NullEntity(id, entity);

            entity.ReadyForDelivery = delivered;
            entity.ShippedBy = UserId;
            entity.ShippedDate = DateTime.Now;
            entity.UpdatedBy = UserId;
            entity.DateUpdated = DateTime.Now;
            entity.EntityState = EntityState.Modified;

            await _ukAndCELabellingRepository.UpdateAsync(entity);
            return Ok();
        }
        #endregion

        #region Download Template
        [Route("download-template")]
        [HttpGet]
        public IHttpActionResult DownloadTemplate(string labelType)
        {
            var templatePath = labelType == LabelTypes.TSL ? TemplatePaths.TSL : labelType == LabelTypes.TCL? TemplatePaths.TCL : TemplatePaths.THL;
            var downloadFileInfo = new DownloadFileViewModel(templatePath);
            return new FileActionResult(downloadFileInfo);
        }

        [Route("download-attached")]
        [HttpGet]
        public IHttpActionResult DownloadAttached(string path)
        {
            var downloadFileInfo = new DownloadFileViewModel(path);

            if (!File.Exists(downloadFileInfo.FilePath))
                return BadRequest("File doesn't exists.");

            return new FileActionResult(downloadFileInfo);
        }
        #endregion

        #region Helpers
        private async Task<UkAndCeLabellingMasterBindingModel> SetEntityTypeValueIdsAsync(UkAndCeLabellingMasterBindingModel model)
        {
            #region Order For
            if(model.OrderForId <= 0)
            {
                var orderFor = await _entityTypeValueRepository.FindAsync(x => x.ValueName.Equals(model.OrderFor));
                if (orderFor == null)
                {
                    var orderForId = await _signatureRepository.GetMaxIdAsync(TableNames.ENTITY_TYPE);

                    orderFor = new EntityTypeValue
                    {
                        Id = orderForId,
                        ValueName = model.OrderFor,
                        EntityTypeId = LabellingEntityTypes.LabellingOrderFor,
                        AddedBy = UserId,
                        DateAdded = DateTime.Now
                    };

                    await _entityTypeValueRepository.AddAsync(orderFor);
                }
                else
                    model.OrderForId = orderFor.Id;
            }
            #endregion

            foreach (var item in model.Childs)
            {
                #region Pack Type
                if(item.PackTypeId == 0)
                {
                    var packType = await _entityTypeValueRepository.FindAsync(x => x.ValueName.Equals(item.PackType));
                    if (packType == null)
                    {
                        var packTypeId = await _signatureRepository.GetMaxIdAsync(TableNames.ENTITY_TYPE_VALUE);

                        packType = new EntityTypeValue
                        {
                            Id = packTypeId,
                            ValueName = item.PackType,
                            EntityTypeId = LabellingEntityTypes.LabellingPackType,
                            AddedBy = UserId,
                            DateAdded = DateTime.Now
                        };

                        await _entityTypeValueRepository.AddAsync(packType);
                    }

                    item.PackTypeId = packType.Id;
                }
                #endregion

                #region Season
                if (item.SeasonId == 0)
                {
                    var season = await _entityTypeValueRepository.FindAsync(x => x.ValueName.Equals(item.Season));
                    if (season == null)
                    {
                        var seasonId = await _signatureRepository.GetMaxIdAsync(TableNames.ENTITY_TYPE_VALUE);

                        season = new EntityTypeValue
                        {
                            Id = seasonId,
                            ValueName = item.Season,
                            EntityTypeId = LabellingEntityTypes.LabellingSeason,
                            AddedBy = UserId,
                            DateAdded = DateTime.Now
                        };

                        await _entityTypeValueRepository.AddAsync(season);
                    }

                    item.SeasonId = season.Id;
                }
                #endregion

                #region Supplier
                if (item.SupplierId == 0)
                {
                    var supplier = await _entityTypeValueRepository.FindAsync(x => x.ValueName.Equals(item.Supplier));
                    if (supplier == null)
                    {
                        var supplierId = await _signatureRepository.GetMaxIdAsync(TableNames.ENTITY_TYPE_VALUE);

                        supplier = new EntityTypeValue
                        {
                            Id = supplierId,
                            ValueName = item.Supplier,
                            EntityTypeId = LabellingEntityTypes.LabellingSupplier,
                            AddedBy = UserId,
                            DateAdded = DateTime.Now
                        };

                        await _entityTypeValueRepository.AddAsync(supplier);
                    }

                    item.SupplierId = supplier.Id;
                }
                #endregion

                #region Packaging Supplier
                if (item.PackagingSupplierId == 0)
                {
                    var packagingSupplier = await _entityTypeValueRepository.FindAsync(x => x.ValueName.Equals(item.PackagingSupplier));
                    if (packagingSupplier == null)
                    {
                        var packagingSupplierId = await _signatureRepository.GetMaxIdAsync(TableNames.ENTITY_TYPE_VALUE);

                        packagingSupplier = new EntityTypeValue
                        {
                            Id = packagingSupplierId,
                            ValueName = item.PackagingSupplier,
                            EntityTypeId = LabellingEntityTypes.LabellingPackagingSupplier,
                            AddedBy = UserId,
                            DateAdded = DateTime.Now
                        };

                        await _entityTypeValueRepository.AddAsync(packagingSupplier);
                    }

                    item.PackagingSupplierId = packagingSupplier.Id;
                }
                #endregion

                #region Dept
                if (item.DeptId == 0)
                {
                    var dept = await _entityTypeValueRepository.FindAsync(x => x.ValueName.Equals(item.Dept));
                    if (dept == null)
                    {
                        var deptId = await _signatureRepository.GetMaxIdAsync(TableNames.ENTITY_TYPE_VALUE);

                        dept = new EntityTypeValue
                        {
                            Id = deptId,
                            ValueName = item.Dept,
                            EntityTypeId = LabellingEntityTypes.LabellingDept,
                            AddedBy = UserId,
                            DateAdded = DateTime.Now
                        };

                        await _entityTypeValueRepository.AddAsync(dept);
                    }

                    item.DeptId = dept.Id;
                }
                #endregion

                #region Brand
                if (item.BrandId == 0)
                {
                    var brand = await _entityTypeValueRepository.FindAsync(x => x.ValueName.Equals(item.Brand));
                    if (brand == null)
                    {
                        var brandId = await _signatureRepository.GetMaxIdAsync(TableNames.ENTITY_TYPE_VALUE);

                        brand = new EntityTypeValue
                        {
                            Id = brandId,
                            ValueName = item.Brand,
                            EntityTypeId = LabellingEntityTypes.LabellinBrand,
                            AddedBy = UserId,
                            DateAdded = DateTime.Now
                        };

                        await _entityTypeValueRepository.AddAsync(brand);
                    }

                    item.BrandId = brand.Id;
                }
                #endregion
            }

            return model;
        }
        #endregion
    }
}
