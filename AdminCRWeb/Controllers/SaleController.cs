using Domain.Contracts;
using Domain.Contracts.DTO;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace AdminCRWeb.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class SaleController : ControllerBase
    {
        public IConfiguration _config;
        public ISaleService _service;

        public SaleController(IConfiguration config, ISaleService service)
        {
            _config = config;
            _service = service;
        }

        [HttpGet]
        [Route("GetSales")]
        public async Task<IActionResult> GetSales()
        {
            var response = new Response<List<SaleDTO>>();
            try
            {
                response.Data = await _service.ListSales();
                return Ok(response);
            }
            catch(Exception ex)
            {
                response.Header.Code = 500;
                response.Header.Message = ex.ToString();
                return BadRequest(response);
            }
        }

        [HttpPost]
        [Route("GetSaleById")]
        public async Task<IActionResult> GetSaleById(SaleDTO req)
        {
            var response = new Response<SaleDTO>();
            try
            {
                response.Data = await _service.GetSaleById(req.Id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Header.Code=500;
                response.Header.Message = ex.ToString();
                return BadRequest(response);
            }
        }

        [HttpPost]
        [Route("UpdateSale")]
        public async Task<IActionResult> UpdateSale(SaleDTO sale)
        {
            var response = new Response<bool>();
            try
            {
                response.Data = await _service.UpdateSale(sale);
                response.Header.Message = response.Data ? "La venta se ha actualizado con éxito" : "No se actualizó la venta";
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Header.Code = 500;
                response.Header.Message = ex.ToString();
                return BadRequest(response);
            }

        }

        [HttpPost]
        [Route("SaveSale")]
        public async Task<IActionResult> SaveSale(SaleDTO sale)
        {
            var response = new Response<bool>();
            try
            {
                response.Data = await _service.SaveSale(sale);
                response.Header.Message = response.Data ? "La venta se ha creado con éxito" : "No se guardó la venta";
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
