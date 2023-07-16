﻿using Domain.Contracts;
using Domain.Contracts.DTO;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace AdminCRWeb.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class PaymentController : ControllerBase
    {
        IConfiguration _config;
        IPaymentService _service;

        public PaymentController(IConfiguration config, IPaymentService service)
        {
            _config = config;
            _service = service;
        }
        [HttpGet]
        [Route("GetPaymentBySale")]
        public async Task<IActionResult> ListPaymentBySale(PaymentDTO req)
        {
            var response = new Response<List<PaymentDTO>>();
            try
            {
                response.Data = await _service.ListPaymentBySale(req.SaleId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Header.Code = 500;
                response.Header.Message = ex.ToString();
                return BadRequest(response);
            }
        }
        
    }
}