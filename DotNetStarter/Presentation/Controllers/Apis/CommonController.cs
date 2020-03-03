using EPYSLACSCustomer.Core.DTOs;
using EPYSLACSCustomer.Core.Interfaces.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Presentation.Controllers
{
    [RoutePrefix("api")]
    public class CommonController : ApiBaseController
    {
        private readonly IMenuRepository _menuSqlRepository;

        public CommonController(IMenuRepository menuSqlRepository)
        {
            _menuSqlRepository = menuSqlRepository;
        }

        [Authorize]
        [Route("getmenus/{applicationId}")]
        public IHttpActionResult GetMenus(int applicationId)
        {
            var records = _menuSqlRepository.GetMenus(UserId, applicationId, AppUser.CompanyId);

            var menuList = new List<MenuDto>();
            foreach (var item in records)
            {
                var childs = item.Childs.OrderBy(x => x.SeqNo).ToList();
                item.Childs.RemoveAll(x => x.MenuId > 0);
                item.Childs.AddRange(childs);
                menuList.Add(item);
            }

            return Ok(menuList);
        }

        //[Route("commoninterface")]
        //public IHttpActionResult GetCommonInterface(int menuId)
        //{
        //    var spec = new CommonInterfaceSpecification(menuId);
        //    var entity = _commonInterfaceMasterRepository.GetSingleBySpec(spec);
        //    Guard.Against.NullEntity(menuId, entity);

        //    entity.CommonInterfaceChilds = entity.CommonInterfaceChilds.OrderBy(x => x.Seq).ToList();
        //    foreach (var item in entity.CommonInterfaceChildGrids)
        //        item.CommonInterfaceChildGridColumns = item.CommonInterfaceChildGridColumns.OrderBy(x => x.Seq).ToList();

        //    var response = _mapper.Map<CommonInterfaceMaster, CommonInterfaceMasterViewModel>(entity);
        //    return Ok(response);
        //}
    }
}
