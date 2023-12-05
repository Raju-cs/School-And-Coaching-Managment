using System;
using IqraCommerce.Services.ScheduleArea;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using IqraCommerce.Entities.BatchArea;
using IqraCommerce.Models.BatchArea;
using System.Linq;
using IqraCommerce.Helpers;

namespace IqraCommerce.Controllers.ScheduleArea
{
    public class BatchController: AppDropDownController<Batch, BatchModel>
    {
        BatchService ___service;

        public BatchController()
        {
            service = __service = ___service = new BatchService();
        }

        public async Task<JsonResult> BasicInfo([FromQuery] Guid id)
        {
            return Json(await ___service.BasicInfo(id));
        }

        public ActionResult DeleteBatch([FromBody] BatchModel recordToCreate)
        {
            var batchFormDB = ___service.GetEntity<Batch>().FirstOrDefault(exp => exp.Id == recordToCreate.Id
                                                                                        && exp.IsDeleted == false);



            if (batchFormDB != null)
            {
                batchFormDB.IsDeleted = true;
            }

            ___service.SaveChange();

            return Ok(new Response(batchFormDB.Id, batchFormDB, false, "Success"));
        }

    }
}
