using ApplicationCore.Entities;
using ApplicationCore.Interfaces.Repositories;
using ApplicationCore.Statics;
using Infrastructure.Data;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Data.Repositories
{
    public class SignatureRepository : ISignatureRepository
    {
        private readonly AppDbContext _dbContext;

        public SignatureRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int GetMaxId(string field, RepeatAfterEnum repeatAfter = RepeatAfterEnum.NoRepeat, string companyId = "1", string siteId = "1")
        {
            var signatureEntity = GetSignature(field, companyId, siteId, repeatAfter);

            if (signatureEntity == null)
            {
                signatureEntity = new Signature
                {
                    Field = field,
                    Dates = DateTime.Today,
                    CompanyId = "1", 
                    SiteId = "1"
                };

                _dbContext.SignatureSet.Add(signatureEntity);
            }
            else
            {
                signatureEntity.LastNumber = ++signatureEntity.LastNumber;
                _dbContext.Entry(signatureEntity).State = EntityState.Modified;
            }

            _dbContext.SaveChanges();

            return Convert.ToInt32(signatureEntity.LastNumber);
        }

        public async Task<int> GetMaxIdAsync(string field, RepeatAfterEnum repeatAfter = RepeatAfterEnum.NoRepeat, string companyId = "1", string siteId = "1")
        {
            var signatureEntity = GetSignature(field, companyId, siteId, repeatAfter);

            if (signatureEntity == null)
            {
                signatureEntity = new Signature
                {
                    Field = field,
                    Dates = DateTime.Today,
                    CompanyId = "1",
                    SiteId = "1"
                };
                _dbContext.SignatureSet.Add(signatureEntity);
            }
            else
            {
                signatureEntity.LastNumber = ++signatureEntity.LastNumber;
                _dbContext.Entry(signatureEntity).State = EntityState.Modified;
            }

            await _dbContext.SaveChangesAsync();

            return Convert.ToInt32(signatureEntity.LastNumber);
        }

        public int GetMaxId(string field, int increment, RepeatAfterEnum repeatAfter = RepeatAfterEnum.NoRepeat, string companyId = "1", string siteId = "1")
        {
            var signatureEntity = GetSignature(field, companyId, siteId, repeatAfter);

            decimal maxId;
            if (signatureEntity == null)
            {
                signatureEntity = new Signature
                {
                    Field = field,
                    Dates = DateTime.Today,
                    CompanyId = "1",
                    SiteId = "1"
                };
                _dbContext.SignatureSet.Add(signatureEntity);

                maxId = signatureEntity.LastNumber;
                signatureEntity.LastNumber = increment;
            }
            else
            {
                maxId = signatureEntity.LastNumber;
                signatureEntity.LastNumber += increment;
                _dbContext.Entry(signatureEntity).State = EntityState.Modified;
            }

            _dbContext.SaveChanges();

            return Convert.ToInt32(++maxId);
        }

        public async Task<int> GetMaxIdAsync(string field, int increment, RepeatAfterEnum repeatAfter = RepeatAfterEnum.NoRepeat, string companyId = "1", string siteId = "1")
        {
            try
            {
                var signatureEntity = GetSignature(field, companyId, siteId, repeatAfter);

                decimal maxId;
                if (signatureEntity == null)
                {
                    signatureEntity = new Signature
                    {
                        Field = field,
                        Dates = DateTime.Today,
                        CompanyId = "1",
                        SiteId = "1"
                    };
                    _dbContext.SignatureSet.Add(signatureEntity);

                    maxId = signatureEntity.LastNumber;
                    signatureEntity.LastNumber = increment;
                }
                else
                {
                    maxId = signatureEntity.LastNumber;
                    signatureEntity.LastNumber += increment;
                    _dbContext.Entry(signatureEntity).State = EntityState.Modified;
                }

                await _dbContext.SaveChangesAsync();

                return Convert.ToInt32(++maxId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetMaxNo(string field, string padStr = "00000", RepeatAfterEnum repeatAfter = RepeatAfterEnum.NoRepeat, string companyId = "1", string siteId = "1")
        {
            var signatureEntity = GetSignature(field, companyId, siteId, repeatAfter);

            if (signatureEntity == null)
            {
                signatureEntity = new Signature
                {
                    Field = field,
                    Dates = DateTime.Today,
                    CompanyId = "1",
                    SiteId = "1"
                };

                _dbContext.SignatureSet.Add(signatureEntity);
            }
            else
            {
                signatureEntity.LastNumber = ++signatureEntity.LastNumber;
                _dbContext.Entry(signatureEntity).State = EntityState.Modified;
            }

            _dbContext.SaveChanges();

            var datePart = DateTime.Now.ToString("yyMMdd");
            var numberPart = signatureEntity.LastNumber.ToString(padStr);
            var maxNo = $@"{companyId}{datePart}{numberPart}";

            return maxNo;
        }

        public async Task<string> GetMaxNoAsync(string field, string padStr="00000", RepeatAfterEnum repeatAfter = RepeatAfterEnum.NoRepeat, string companyId = "1", string siteId = "1")
        {
            var signatureEntity = GetSignature(field, companyId, siteId, repeatAfter);

            if (signatureEntity == null)
            {
                signatureEntity = new Signature
                {
                    Field = field,
                    Dates = DateTime.Today,
                    CompanyId = "1",
                    SiteId = "1"
                };

                _dbContext.SignatureSet.Add(signatureEntity);
            }
            else
            {
                signatureEntity.LastNumber = ++signatureEntity.LastNumber;
                _dbContext.Entry(signatureEntity).State = EntityState.Modified;
            }

            await _dbContext.SaveChangesAsync();

            var datePart = DateTime.Now.ToString("yyMMdd");
            var numberPart = signatureEntity.LastNumber.ToString(padStr);
            var maxNo = $@"{companyId}{datePart}{numberPart}";

            return maxNo;
        }

        private Signature GetSignature(string field, string companyId, string siteId, RepeatAfterEnum repeatAfter)
        {
            var signatureEntity = new Signature();

            switch (repeatAfter)
            {
                case RepeatAfterEnum.NoRepeat:
                    signatureEntity = _dbContext.SignatureSet
                        .FirstOrDefault(x => x.Field == field && x.CompanyId == companyId && x.SiteId == siteId);
                    break;
                case RepeatAfterEnum.EveryYear:
                    signatureEntity = _dbContext.SignatureSet
                        .FirstOrDefault(x => x.Field == field && x.CompanyId == companyId && x.SiteId == siteId && x.Dates.Year == DateTime.Now.Year);
                    break;
                case RepeatAfterEnum.EveryMonth:
                    signatureEntity = _dbContext.SignatureSet
                        .FirstOrDefault(x => x.Field == field && x.CompanyId == companyId && x.SiteId == siteId && x.Dates.Month == DateTime.Now.Month);
                    break;
                case RepeatAfterEnum.EveryDay:
                    signatureEntity = _dbContext.SignatureSet
                        .FirstOrDefault(x => x.Field == field && x.CompanyId == companyId && x.SiteId == siteId && x.Dates.Date == DateTime.Today);
                    break;
                default:
                    break;
            }

            return signatureEntity;
        }
    }
}
