using ApplicationCore.Statics;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.Repositories
{
    /// <summary>
    /// ISignatureRepository
    /// </summary>
    public interface ISignatureRepository
    {
        /// <summary>
        /// Get Max Id
        /// </summary>
        /// <param name="field">Field or Table name</param>
        /// <param name="repeatAfter"><see cref="RepeatAfterEnum"/> Repeat After</param>
        /// <returns></returns>
        int GetMaxId(string field, RepeatAfterEnum repeatAfter = RepeatAfterEnum.NoRepeat, string companyId = "1", string siteId = "1");

        /// <summary>
        /// Get Max Id Async
        /// </summary>
        /// <param name="field">Field or Table name</param>
        /// <param name="repeatAfter"><see cref="RepeatAfterEnum"/> Repeat After</param>
        /// <returns></returns>
        Task<int> GetMaxIdAsync(string field, RepeatAfterEnum repeatAfter = RepeatAfterEnum.NoRepeat, string companyId = "1", string siteId = "1");

        /// <summary>
        /// Get Max Id
        /// </summary>
        /// <param name="field">Field or Table name</param>
        /// <param name="increment">Increment</param>
        /// <param name="repeatAfter"><see cref="RepeatAfterEnum"/>Repeat After</param>
        /// <returns></returns>
        int GetMaxId(string field, int increment, RepeatAfterEnum repeatAfter = RepeatAfterEnum.NoRepeat, string companyId = "1", string siteId = "1");

        /// <summary>
        /// Get Max Id Async
        /// </summary>
        /// <param name="field">Field or Table name</param>
        /// <param name="increment">Increment</param>
        /// <param name="repeatAfter"><see cref="RepeatAfterEnum"/>Repeat After</param>
        /// <returns></returns>
        Task<int> GetMaxIdAsync(string field, int increment, RepeatAfterEnum repeatAfter = RepeatAfterEnum.NoRepeat, string companyId = "1", string siteId = "1");

        string GetMaxNo(string field, string padStr = "00000", RepeatAfterEnum repeatAfter = RepeatAfterEnum.NoRepeat, string companyId = "1", string siteId = "1");

        Task<string> GetMaxNoAsync(string field, string padStr = "00000", RepeatAfterEnum repeatAfter = RepeatAfterEnum.NoRepeat, string companyId = "1", string siteId = "1");
    }
}
