using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MonPDLib.General;
using Swashbuckle.AspNetCore.Annotations;
using static APIBapenda.Helper;
using APIBapenda.Models;

namespace APIBapenda.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PBBController : ControllerBase
    {
        [HttpGet("GetPBBByNOP")]
        [SwaggerOperation(Summary = "Get PBB By NOP", Description = "Ambil Data PBB by NOP")]
        public async Task<APIResponse<List<PBBVM.TanahPBB>>> GetPBBInfoAsync(PBBVM.PBBReq req)
        {
            var response = new APIResponse<List<PBBVM.TanahPBB>> { Code = 0, Message = "Sukses" };

            try
            {
                // Bungkus pemanggilan synchronous ke dalam Task.Run jika tidak tersedia versi async
                var aa = await Task.Run(() => VrmLib.Transaksi.SPKL.SpklInfo.GetSpklInfoList(req.IdNumber));

                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("id-ID");

                var a = aa.Where(x => x.StatusSPKL == EnumFactory.EDokumenState.FinishAccepted);
                var tglspklList = a.Select(x => x.Tanggal).Distinct();

                var data = new List<Notification>();

                foreach (var item in tglspklList)
                {
                    var row = new Notification
                    {
                        dates = item.ToString("dd MMMM yyyy"),
                        notification = new List<NotificationDetail>()
                    };

                    var detList = a.Where(x => x.Tanggal == item).ToList();

                    foreach (var detitem in detList)
                    {
                        var rowdet = new NotificationDetail
                        {
                            title = "Notif SPKL " + detitem.JenisLembur.GetDescription(),
                            description = "Kamu telah menerima notifikasi terbaru pada hari ini",
                            time = detitem.JamAwalLembur.ToString("HH:mm"),
                            planningTime = detitem.JamAwalLembur.ToString("HH:mm") + " - " + detitem.JamAkhirLembur.ToString("HH:mm"),
                            statusNotif = "Approve",
                            isRead = false
                        };

                        row.notification.Add(rowdet);
                    }

                    data.Add(row);
                }

                response.Data = data;
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
