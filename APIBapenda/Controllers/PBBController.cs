using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MonPDLib.General;
using Swashbuckle.AspNetCore.Annotations;
using static APIBapenda.Helper;
using APIBapenda.Models;
using MonPDLib;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace APIBapenda.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PBBController : ControllerBase
    {

        [HttpPost("GetPBBByNOP")]
        [SwaggerOperation(Summary = "Get PBB By NOP", Description = "Ambil Data PBB by NOP")]
        [Authorize]
        public async Task<APIResponse<List<MonPDLib.EF.DatapbbSatupetum>>> GetPBBInfoAsync(PBBVM.PBBReq req)
        {
            var response = new APIResponse<List<MonPDLib.EF.DatapbbSatupetum>> { Code = 0, Message = "Sukses" };

            try
            {
                // Bungkus pemanggilan synchronous ke dalam Task.Run jika tidak tersedia versi async                
                var _context= DBClass.GetContext();
                response.Data = await Task.Run(() => _context.DatapbbSatupeta.Where(x=>x.Nop==req.PBB).ToListAsync());                              
            }
            catch (Exception ex)
            {
                response.Code = 500;
                response.Message = ex.InnerException?.Message ?? ex.Message;
            }

            return response;
        }

        [HttpPost("GetPBBByIdPersil")]
        [SwaggerOperation(Summary = "Get PBB By NOP", Description = "Ambil Data PBB by NOP")]
        [Authorize]
        public async Task<APIResponse<List<MonPDLib.EF.DatapbbSatupetum>>> GetPBBInfoAsync(PBBVM.IdPersilReq req)
        {
            var response = new APIResponse<List<MonPDLib.EF.DatapbbSatupetum>> { Code = 0, Message = "Sukses" };

            try
            {
                // Bungkus pemanggilan synchronous ke dalam Task.Run jika tidak tersedia versi async                
                var _context = DBClass.GetContext();
                response.Data = await Task.Run(() => _context.DatapbbSatupeta.Where(x => x.IdPersil == req.Persil).ToListAsync());
            }
            catch (Exception ex)
            {
                response.Code = 500;
                response.Message = ex.InnerException?.Message ?? ex.Message;
            }

            return response;
        }
    }
}
