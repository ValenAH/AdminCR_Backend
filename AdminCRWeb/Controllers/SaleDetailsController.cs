using Domain.Contracts;
using Domain.Contracts.DTO;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace AdminCRWeb.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class SaleDetailsController : ControllerBase
    {
        public IConfiguration _config;
        public ISaleDetailsService _service;
        public ISaleService _saleService;

        public SaleDetailsController(IConfiguration config, ISaleDetailsService service, ISaleService saleService)
        {
            _config = config;
            _service = service;
            _saleService = saleService;
        }

        [HttpPost]
        [Route("GetSaleDetails")]

        public async Task<IActionResult> ListSaleDetails(SaleDetailsDTO req)
        {
            var response = new Response<List<SaleDetailsDTO>>();
            try
            {
                response.Data = await _service.ListSaleDetails(req.SaleId);
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
        [Route("UpdateSaleDetail")]
        public async Task<IActionResult> UpdateSaveDetail(SaleDetailsDTO req)
        {
            var response = new Response<bool>();
            try
            {
                response.Data = await _service.UpdateSaleDetail(req);
                response.Header.Message = response.Data ? "El producto se ha actualizado con éxito" : "No se actualizó el producto";
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
        [Route("SaveSaleDetails")]
        public async Task<IActionResult> SaveSaleDetails(List<SaleDetailsDTO> req)
        {
            var response = new Response<bool>();
            try
            {
                response.Data = await _service.SaveSaleDetails(req);
                response.Header.Message = response.Data ? "Los detalles de la venta se han guardado con éxito" : "No se guardaron los detalles de la venta";
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Header.Code = 500;
                response.Header.Message = ex.ToString();
                return BadRequest(response);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteSaleDetail(int id)
        {
            var response = new Response<bool>();
            try
            {
                await _service.DeleteSaleDetail(id);
                response.Data = true;
                response.Header.Message = response.Data ? "El producto se ha eliminado con éxito" : "No se pudo eliminar el producto";

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
