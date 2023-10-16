using Domain.Contracts;
using Domain.Contracts.DTO;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Web;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static iTextSharp.text.pdf.AcroFields;

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

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GeneratePDF(int id)
        {
            var sale = await _service.GetSaleById(id);
            MemoryStream ms = new MemoryStream();
            Document document = new Document(PageSize.LETTER, 10, 10, 12, 12);
            PdfWriter pw = PdfWriter.GetInstance(document, ms);

            document.Open();

            //Title and Author
            document.AddAuthor("Valen");
            document.AddTitle("Factura " + sale.Consecutive);

            //Definir la fuente
            Font standardFont = new Font(Font.FontFamily.HELVETICA, 12, Font.NORMAL, BaseColor.BLACK);
            //Encabezado
            document.Add(new Paragraph("FACTURA DE VENTA"));
            document.Add(new Paragraph("Liliana María Hincapié Noreña \n " +
                                        "NIT:43588603-1 \n " +
                                        "Carrera 52 N 2 sur 10 \n " +
                                        "Teléfono: 255 26 20 \n" +
                                        "E-mail: colchonescristorey@gmail.com"));
            document.Add(new Paragraph("N° de factura: " + sale.Consecutive));
            document.Add(new Paragraph("Fecha de factura: " + sale.SaleDate));
            document.Add(Chunk.NEWLINE);
            document.Add(new Paragraph("Datos del cliente"));

            //Información del cliente
            PdfPTable customerInformation = new PdfPTable(4);
            customerInformation.WidthPercentage = 100;
            PdfPCell customerFirstColumn = new(new Phrase("Identificación", standardFont))
            {
                BorderWidth = 0
            };
            PdfPCell customerSecondColumn = new(new Phrase(sale.Customer.IdentificationNumber, standardFont))
            {
                BorderWidth = 0
            };
            PdfPCell customerThirdColumn = new(new Phrase("Teléfono", standardFont))
            {
                BorderWidth = 0
            };
            PdfPCell customerFourthColumn = new(new Phrase(sale.Customer.Telephone, standardFont))
            {
                BorderWidth = 0
            };
            customerInformation.AddCell(customerFirstColumn);
            customerInformation.AddCell(customerSecondColumn);
            customerInformation.AddCell(customerThirdColumn);
            customerInformation.AddCell(customerFourthColumn);

            customerFirstColumn = new(new Phrase("Nombre", standardFont))
            {
                BorderWidth = 0
            };
            customerSecondColumn = new(new Phrase(sale.Customer.Name, standardFont))
            {
                BorderWidth = 0
            };
            customerThirdColumn = new(new Phrase("Dirección", standardFont))
            {
                BorderWidth = 0
            };
            customerFourthColumn = new(new Phrase(sale.Customer.Address, standardFont))
            {
                BorderWidth = 0
            };

            customerInformation.AddCell(customerFirstColumn);
            customerInformation.AddCell(customerSecondColumn);
            customerInformation.AddCell(customerThirdColumn);
            customerInformation.AddCell(customerFourthColumn);

            customerFirstColumn = new(new Phrase("Email", standardFont))
            {
                BorderWidth = 0
            };
            customerSecondColumn = new(new Phrase(sale.Customer.Email, standardFont))
            {
                BorderWidth = 0
            };
            customerThirdColumn = new(new Phrase("", standardFont))
            {
                BorderWidth = 0
            };
            customerFourthColumn = new(new Phrase("", standardFont))
            {
                BorderWidth = 0
            };

            customerInformation.AddCell(customerFirstColumn);
            customerInformation.AddCell(customerSecondColumn);
            customerInformation.AddCell(customerThirdColumn);
            customerInformation.AddCell(customerFourthColumn);

            document.Add(customerInformation);

            //Espacio
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
                    BorderWidth = 0,
                    HorizontalAlignment = Element.ALIGN_CENTER
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

            byte[] bytesSTream = ms.ToArray();

            ms = new MemoryStream();
            ms.Write(bytesSTream, 0, bytesSTream.Length);
            ms.Position = 0;

            return new FileStreamResult(ms, "application/pdf");
        }
    }
}
