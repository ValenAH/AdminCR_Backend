using Domain.Contracts;
using Domain.Contracts.DTO;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace AdminCRWeb.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class SaleController : ControllerBase
    {
        public IConfiguration _config;
        public ISaleService _service;
        public ISaleDetailsService _serviceDetails;

        public SaleController(IConfiguration config, ISaleService service, ISaleDetailsService saleDetailsService)
        {
            _config = config;
            _service = service;
            _serviceDetails = saleDetailsService;
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
            var response = new Response<int>();
            try
            {
                sale.Consecutive = await _service.GetConsecutive();
                response.Data = await _service.SaveSale(sale);
                 
                if(response.Data != 0)
                {
                    foreach(SaleDetailsDTO detail in sale.SaleDetails)
                    {
                        detail.SaleId = response.Data;
                    }
                    await _serviceDetails.SaveSaleDetails(sale.SaleDetails);
                    response.Header.Message = "La venta se ha creado con éxito";
                }
                else
                {
                    response.Header.Message = "No se guardó la venta";
                }
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
        [Route("GetSalesByCustomerId")]
        public async Task<IActionResult> GetSalesByCustomerId(SaleDTO req)
        {
            var response = new Response<List<SaleDTO>>();
            try
            {
                response.Data = await _service.GetSalesByCustomerId(req.CustomerId);
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
        [Route("GeneratePDF")]
        public void GeneratePDF(SaleDTO sale)
        {
            FileStream fs = new FileStream("Invoices\\Factura_"+sale.Consecutive +".pdf", FileMode.Create);
            Document document = new Document(PageSize.LETTER, 5, 5, 7, 7);
            PdfWriter pw = PdfWriter.GetInstance(document, fs);

            document.Open();

            //Title and Author
            document.AddAuthor("Valen");
            document.AddTitle("Factura");

            //Definir la fuente
            Font standardFont = new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK);
            //Encabezado
            document.Add(new Paragraph("Título"));
            document.Add(Chunk.NEWLINE);

            //Encabezado columnas
            PdfPTable table = new PdfPTable(5);
            table.WidthPercentage = 100;

            //Títulos de las columnas
            PdfPCell product = new(new Phrase("Producto", standardFont))
            {
                BorderWidth = 0,
                BorderWidthBottom = 0.75f
            };
            PdfPCell productDescription = new(new Phrase("Descripción", standardFont))
            {
                BorderWidth = 0,
                BorderWidthBottom = 0.75f
            };
            PdfPCell quantity = new(new Phrase("Cantidad", standardFont))
            {
                BorderWidth = 0,
                BorderWidthBottom = 0.75f
            };
            PdfPCell amount = new(new Phrase("Precio", standardFont))
            {
                BorderWidth = 0,
                BorderWidthBottom = 0.75f
            };
            PdfPCell total = new(new Phrase("Total", standardFont))
            {
                BorderWidth = 0,
                BorderWidthBottom = 0.75f
            };

            table.AddCell(product);
            table.AddCell(productDescription);
            table.AddCell(quantity);
            table.AddCell(amount);
            table.AddCell(total);

            foreach (var item in sale.SaleDetails)
            {
                product = new(new Phrase(item.Product.Name, standardFont))
                {
                    BorderWidth = 0
                };
                productDescription = new(new Phrase(item.Product.Description, standardFont))
                {
                    BorderWidth = 0
                };
                quantity = new(new Phrase(item.Quantity.ToString(), standardFont))
                {
                    BorderWidth = 0
                };
                amount = new(new Phrase(item.Amount.ToString(), standardFont))
                {
                    BorderWidth = 0
                };
                total = new(new Phrase((item.Quantity * item.Amount).ToString(), standardFont))
                {
                    BorderWidth = 0
                };

                table.AddCell(product);
                table.AddCell(productDescription);
                table.AddCell(quantity);
                table.AddCell(amount);
                table.AddCell(total);
            }
            document.Add(table);

            document.Close();
            pw.Close();


        }
    }
}
