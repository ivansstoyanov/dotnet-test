using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ShishaWeb.Services.Interfaces;
using ShishaWeb.Models;
using ShishaWeb.Services;
using ShishaWeb.Filters;

namespace ShishaWeb.Controllers
{
    [Authorize("Bearer")]
    [Route("api/[controller]")]
    [QrCodeManagementExceptionFilter]
    public class QrCodeController : BaseController
    {
        private readonly IQrCodeService qrCodeService;
        private readonly ITabaccoService tabaccosService;

        public QrCodeController(ITabaccoService tabaccosService, IQrCodeService qrCodeService)
        {
            this.qrCodeService = qrCodeService;
            this.tabaccosService = tabaccosService;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody]string qrValue)
        {
            var result = Mapper.ToDto(await this.qrCodeService.Add(qrValue));

            return Created($"{Request.Path}/{result.Id}", result);
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> Validate([FromBody]QrCodeDTO qrCode)
        {
            await this.qrCodeService.Validate(qrCode.Id, qrCode.Value);

            return this.Ok();
        }
    }
}
