using Domain.Contracts;
using Domain.Contracts.DTO;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Globalization;
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
        [HttpGet]
        [Route("GetCreditSales")]
        public async Task<IActionResult> GetCreditSales()
        {
            var response = new Response<List<SaleDTO>>();
            try
            {
                response.Data = await _service.ListCreditSales();
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

            if (sale == null)
            {
                return NotFound();
            }

            var saleDetails = sale.SaleDetails ?? new List<SaleDetailsDTO>();
            var subtotal = saleDetails.Sum(item => item.Quantity * item.Amount);
            var total = sale.TotalAmount > 0 ? sale.TotalAmount : subtotal;
            var culture = new CultureInfo("es-CO");

            string FormatCurrency(decimal value)
            {
                return value.ToString("C0", culture);
            }

            var companyColor = new BaseColor(23, 72, 123);
            var accentColor = new BaseColor(35, 118, 188);
            var textColor = new BaseColor(45, 55, 65);
            var mutedColor = new BaseColor(102, 112, 123);

            using var ms = new MemoryStream();
            var document = new Document(PageSize.LETTER, 36, 36, 36, 36);
            PdfWriter.GetInstance(document, ms);
            document.Open();

            document.AddAuthor("AdminCR");
            document.AddTitle("Factura " + sale.Consecutive);

            var titleFont = new Font(Font.FontFamily.HELVETICA, 18, Font.BOLD, companyColor);
            var headerFont = new Font(Font.FontFamily.HELVETICA, 14, Font.BOLD, BaseColor.BLACK);
            var labelFont = new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD, textColor);
            var normalFont = new Font(Font.FontFamily.HELVETICA, 10, Font.NORMAL, textColor);
            var smallFont = new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL, mutedColor);

            document.Add(new Paragraph("FACTURA DE VENTA", titleFont));
            document.Add(Chunk.NEWLINE);

            var headerTable = new PdfPTable(2);
            headerTable.WidthPercentage = 100;
            headerTable.SetWidths(new float[] { 60f, 40f });

            var companyCell = new PdfPCell
            {
                Border = Rectangle.NO_BORDER,
                Padding = 6f,
                PaddingBottom = 10f
            };
            companyCell.AddElement(new Paragraph("Liliana María Hincapié Noreña", new Font(Font.FontFamily.HELVETICA, 15, Font.BOLD, textColor)));
            companyCell.AddElement(new Paragraph("NIT: 43588603-1", normalFont));
            companyCell.AddElement(new Paragraph("Carrera 52 N 2 sur 10", normalFont));
            companyCell.AddElement(new Paragraph("Teléfono: 255 26 20", normalFont));
            companyCell.AddElement(new Paragraph("E-mail: colchonescristorey@gmail.com", normalFont));

            var invoiceCell = new PdfPCell
            {
                Border = Rectangle.NO_BORDER,
                Padding = 6f,
                HorizontalAlignment = Element.ALIGN_RIGHT
            };
            invoiceCell.AddElement(new Paragraph("FACTURA", new Font(Font.FontFamily.HELVETICA, 16, Font.BOLD, accentColor)));
            invoiceCell.AddElement(new Paragraph("No. " + (sale.Consecutive ?? "N/A"), labelFont));
            invoiceCell.AddElement(new Paragraph("Fecha: " + sale.SaleDate.ToString("dd/MM/yyyy"), labelFont));
            invoiceCell.AddElement(new Paragraph("Estado: " + (sale.SaleStatus?.Status ?? "Pendiente"), labelFont));

            headerTable.AddCell(companyCell);
            headerTable.AddCell(invoiceCell);
            document.Add(headerTable);

            document.Add(new Paragraph(" ")
            {
                SpacingBefore = 6f,
                SpacingAfter = 10f
            });
            document.Add(new iTextSharp.text.pdf.draw.LineSeparator(0.75f, 100f, companyColor, Element.ALIGN_CENTER, 0));

            var customerTable = new PdfPTable(2);
            customerTable.WidthPercentage = 100;
            customerTable.SetWidths(new float[] { 50f, 50f });

            var customerTitle = new PdfPCell(new Phrase("Datos del cliente", headerFont))
            {
                Colspan = 2,
                Border = Rectangle.NO_BORDER,
                PaddingBottom = 6f,
                PaddingTop = 4f
            };
            customerTable.AddCell(customerTitle);

            var customerInfoLabel = new PdfPCell(new Phrase("Identificación", labelFont))
            {
                BorderWidth = 0,
                Padding = 4f,
                BackgroundColor = new BaseColor(245, 247, 250)
            };
            var customerInfoValue = new PdfPCell(new Phrase(sale.Customer?.IdentificationNumber ?? "N/A", normalFont))
            {
                BorderWidth = 0,
                Padding = 4f,
                BackgroundColor = new BaseColor(245, 247, 250)
            };

            customerTable.AddCell(customerInfoLabel);
            customerTable.AddCell(customerInfoValue);

            customerInfoLabel = new PdfPCell(new Phrase("Teléfono", labelFont))
            {
                BorderWidth = 0,
                Padding = 4f
            };
            customerInfoValue = new PdfPCell(new Phrase(sale.Customer?.Telephone ?? "N/A", normalFont))
            {
                BorderWidth = 0,
                Padding = 4f
            };
            customerTable.AddCell(customerInfoLabel);
            customerTable.AddCell(customerInfoValue);

            customerInfoLabel = new PdfPCell(new Phrase("Nombre", labelFont))
            {
                BorderWidth = 0,
                Padding = 4f,
                BackgroundColor = new BaseColor(245, 247, 250)
            };
            customerInfoValue = new PdfPCell(new Phrase(sale.Customer?.Name ?? "Cliente no registrado", normalFont))
            {
                BorderWidth = 0,
                Padding = 4f,
                BackgroundColor = new BaseColor(245, 247, 250)
            };
            customerTable.AddCell(customerInfoLabel);
            customerTable.AddCell(customerInfoValue);

            customerInfoLabel = new PdfPCell(new Phrase("Dirección", labelFont))
            {
                BorderWidth = 0,
                Padding = 4f
            };
            customerInfoValue = new PdfPCell(new Phrase(sale.Customer?.Address ?? "Sin dirección", normalFont))
            {
                BorderWidth = 0,
                Padding = 4f
            };
            customerTable.AddCell(customerInfoLabel);
            customerTable.AddCell(customerInfoValue);

            customerInfoLabel = new PdfPCell(new Phrase("Email", labelFont))
            {
                BorderWidth = 0,
                Padding = 4f,
                BackgroundColor = new BaseColor(245, 247, 250)
            };
            customerInfoValue = new PdfPCell(new Phrase(sale.Customer?.Email ?? "Sin email", normalFont))
            {
                BorderWidth = 0,
                Padding = 4f,
                BackgroundColor = new BaseColor(245, 247, 250)
            };
            customerTable.AddCell(customerInfoLabel);
            customerTable.AddCell(customerInfoValue);

            document.Add(customerTable);
            document.Add(Chunk.NEWLINE);

            var table = new PdfPTable(5);
            table.WidthPercentage = 100;
            table.SetWidths(new float[] { 28f, 30f, 12f, 15f, 15f });

            var headers = new[] { "Producto", "Descripción", "Cantidad", "Precio", "Total" };
            foreach (var header in headers)
            {
                var cell = new PdfPCell(new Phrase(header, labelFont))
                {
                    BackgroundColor = new BaseColor(238, 243, 249),
                    BorderWidth = 0.5f,
                    BorderColor = new BaseColor(220, 224, 232),
                    Padding = 7f,
                    HorizontalAlignment = Element.ALIGN_CENTER
                };
                table.AddCell(cell);
            }

            foreach (var item in saleDetails)
            {
                table.AddCell(new PdfPCell(new Phrase(item.Product?.Name ?? "Producto", normalFont))
                {
                    BorderWidth = 0.5f,
                    BorderColor = new BaseColor(220, 224, 232),
                    Padding = 6f
                });

                table.AddCell(new PdfPCell(new Phrase(item.Product?.Description ?? "Sin descripción", smallFont))
                {
                    BorderWidth = 0.5f,
                    BorderColor = new BaseColor(220, 224, 232),
                    Padding = 6f
                });

                table.AddCell(new PdfPCell(new Phrase(item.Quantity.ToString(), normalFont))
                {
                    BorderWidth = 0.5f,
                    BorderColor = new BaseColor(220, 224, 232),
                    Padding = 6f,
                    HorizontalAlignment = Element.ALIGN_CENTER
                });

                table.AddCell(new PdfPCell(new Phrase(FormatCurrency(item.Amount), normalFont))
                {
                    BorderWidth = 0.5f,
                    BorderColor = new BaseColor(220, 224, 232),
                    Padding = 6f,
                    HorizontalAlignment = Element.ALIGN_RIGHT
                });

                table.AddCell(new PdfPCell(new Phrase(FormatCurrency(item.Quantity * item.Amount), normalFont))
                {
                    BorderWidth = 0.5f,
                    BorderColor = new BaseColor(220, 224, 232),
                    Padding = 6f,
                    HorizontalAlignment = Element.ALIGN_RIGHT
                });
            }

            document.Add(table);
            document.Add(Chunk.NEWLINE);

            var totalsTable = new PdfPTable(2);
            totalsTable.WidthPercentage = 38;
            totalsTable.HorizontalAlignment = Element.ALIGN_RIGHT;
            totalsTable.SetWidths(new float[] { 55f, 45f });

            totalsTable.AddCell(new PdfPCell(new Phrase("Subtotal", labelFont))
            {
                Border = Rectangle.NO_BORDER,
                PaddingTop = 4f,
                PaddingBottom = 4f,
                HorizontalAlignment = Element.ALIGN_LEFT
            });
            totalsTable.AddCell(new PdfPCell(new Phrase(FormatCurrency(subtotal), normalFont))
            {
                Border = Rectangle.NO_BORDER,
                PaddingTop = 4f,
                PaddingBottom = 4f,
                HorizontalAlignment = Element.ALIGN_RIGHT
            });

            totalsTable.AddCell(new PdfPCell(new Phrase("IVA", labelFont))
            {
                Border = Rectangle.NO_BORDER,
                PaddingTop = 4f,
                PaddingBottom = 4f,
                HorizontalAlignment = Element.ALIGN_LEFT
            });
            totalsTable.AddCell(new PdfPCell(new Phrase("$0", normalFont))
            {
                Border = Rectangle.NO_BORDER,
                PaddingTop = 4f,
                PaddingBottom = 4f,
                HorizontalAlignment = Element.ALIGN_RIGHT
            });

            totalsTable.AddCell(new PdfPCell(new Phrase("TOTAL", new Font(Font.FontFamily.HELVETICA, 11, Font.BOLD, textColor)))
            {
                Border = Rectangle.NO_BORDER,
                PaddingTop = 8f,
                PaddingBottom = 4f,
                HorizontalAlignment = Element.ALIGN_LEFT
            });
            totalsTable.AddCell(new PdfPCell(new Phrase(FormatCurrency(total), new Font(Font.FontFamily.HELVETICA, 11, Font.BOLD, accentColor)))
            {
                Border = Rectangle.NO_BORDER,
                PaddingTop = 8f,
                PaddingBottom = 4f,
                HorizontalAlignment = Element.ALIGN_RIGHT
            });

            document.Add(totalsTable);

            document.Add(Chunk.NEWLINE);
            var note = new Paragraph("Gracias por su compra. Estamos para servirle.", smallFont)
            {
                Alignment = Element.ALIGN_CENTER
            };
            document.Add(note);

            document.Close();

            var bytesStream = ms.ToArray();
            ms.Position = 0;

            return new FileStreamResult(ms, "application/pdf");
        }
    }
}
