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

        [HttpGet]
        [Route("GetPendingSales")]
        public async Task<IActionResult> GetPendingSales()
        {
            var response = new Response<List<SaleDTO>>();
            try
            {
                response.Data = await _service.ListPendingSales();
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
                sale.SaleDate = NormalizeDateTimeUtc(sale.SaleDate);
                sale.DeliveryDate = NormalizeDateTimeUtc(sale.DeliveryDate);

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
                sale.SaleDate = NormalizeDateTimeUtc(sale.SaleDate);
                sale.DeliveryDate = NormalizeDateTimeUtc(sale.DeliveryDate);

                var saleDetailsToSave = sale.SaleDetails ?? new List<SaleDetailsDTO>();
                sale.SaleDetails = null;

                sale.Consecutive = await _service.GetConsecutive();
                var saleId = await _service.SaveSale(sale);
                response.Data = saleId;

                if (saleId != 0)
                {
                    if (saleDetailsToSave.Any())
                    {
                        foreach (var detail in saleDetailsToSave)
                        {
                            detail.SaleId = saleId;
                        }

                        var detailsSaved = await _serviceDetails.SaveSaleDetails(saleDetailsToSave);
                        if (!detailsSaved)
                        {
                            response.Header.Code = 500;
                            response.Header.Message = "La venta se creó, pero no se guardaron los detalles.";
                            return BadRequest(response);
                        }
                    }

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
            try
            {
                var sale = await _service.GetSaleById(id);
                if (sale == null)
                {
                    return NotFound(new { message = "Venta no encontrada." });
                }

                var pdfBytes = BuildInvoicePdf(sale);
                var fileName = string.IsNullOrWhiteSpace(sale.Consecutive) ? $"factura-{id}.pdf" : $"factura-{sale.Consecutive}.pdf";

                Response.Headers.Add("Content-Disposition", $"inline; filename*=UTF-8''{Uri.EscapeDataString(fileName)}");
                Response.Headers.Add("Cache-Control", "no-store");
                return File(pdfBytes, "application/pdf");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error al generar la factura PDF.",
                    detail = ex.Message,
                    inner = ex.InnerException?.Message
                });
            }
        }

        private byte[] BuildInvoicePdf(SaleDTO sale)
        {
            var saleDetails = sale.SaleDetails ?? new List<SaleDetailsDTO>();
            var customer = sale.Customer ?? new CustomerDTO();
            var saleStatus = sale.SaleStatus ?? new SaleStatusDTO();
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
            document.AddTitle("Factura " + (string.IsNullOrWhiteSpace(sale.Consecutive) ? "N/A" : sale.Consecutive));

            var titleFont = new Font(Font.FontFamily.TIMES_ROMAN, 20, Font.BOLD, companyColor);
            var headerFont = new Font(Font.FontFamily.TIMES_ROMAN, 14, Font.BOLD, BaseColor.BLACK);
            var labelFont = new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD, textColor);
            var normalFont = new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL, textColor);
            var smallFont = new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL, mutedColor);
            var accentBold = new Font(Font.FontFamily.TIMES_ROMAN, 13, Font.BOLD, accentColor);
            var headerFill = new BaseColor(238, 243, 249);

            var invoiceTitleBackground = new BaseColor(238, 243, 249);
            var titleParagraph = new Paragraph("FACTURA DE VENTA", new Font(Font.FontFamily.TIMES_ROMAN, 20, Font.BOLD, BaseColor.BLACK))
            {
                Alignment = Element.ALIGN_CENTER,
                SpacingBefore = 0f,
                SpacingAfter = 0f,
                Leading = 20f
            };

            var titleCell = new PdfPCell(titleParagraph)
            {
                Colspan = 1,
                Border = Rectangle.NO_BORDER,
                BackgroundColor = invoiceTitleBackground,
                PaddingTop = 10f,
                PaddingBottom = 10f,
                HorizontalAlignment = Element.ALIGN_CENTER
            };

            var titleBlock = new PdfPTable(1);
            titleBlock.WidthPercentage = 100;
            titleBlock.AddCell(titleCell);
            document.Add(titleBlock);
            document.Add(new Paragraph(" ") { SpacingAfter = 12f });

            var headerTable = new PdfPTable(3);
            headerTable.WidthPercentage = 100;
            headerTable.SetWidths(new float[] { 35f, 40f, 25f });
            headerTable.SpacingAfter = 10f;

            var logoCell = new PdfPCell
            {
                Border = Rectangle.NO_BORDER,
                Padding = 10f,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                HorizontalAlignment = Element.ALIGN_CENTER
            };

            var logo = TryLoadLogo();
            if (logo != null)
            {
                logo.ScaleToFit(110f, 70f);
                logo.Alignment = Element.ALIGN_CENTER;
                logoCell.AddElement(logo);
            }
            else
            {
                logoCell.AddElement(new Paragraph("LOGO", new Font(Font.FontFamily.TIMES_ROMAN, 16, Font.BOLD, companyColor)));
            }

            var companyCell = new PdfPCell
            {
                Border = Rectangle.NO_BORDER,
                Padding = 10f,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                HorizontalAlignment = Element.ALIGN_CENTER
            };
            companyCell.AddElement(new Paragraph("Liliana María Hincapié Noreña", new Font(Font.FontFamily.TIMES_ROMAN, 15, Font.NORMAL, textColor)));
            companyCell.AddElement(new Paragraph("NIT: 43588603-1", normalFont));
            companyCell.AddElement(new Paragraph("Carrera 52 N 2 sur 10", normalFont));
            companyCell.AddElement(new Paragraph("Teléfono: 255 26 20", normalFont));
            companyCell.AddElement(new Paragraph("E-mail: colchonescristorey@gmail.com", normalFont));

            var invoiceCell = new PdfPCell
            {
                Border = Rectangle.NO_BORDER,
                Padding = 8f,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                HorizontalAlignment = Element.ALIGN_CENTER
            };

            var labelTable = new PdfPTable(2);
            labelTable.WidthPercentage = 100;
            labelTable.SetWidths(new float[] { 60f, 40f });

            var noFacturaLabel = new PdfPCell(new Phrase("N° Factura", labelFont))
            {
                Border = Rectangle.BOX,
                Padding = 6f,
                BackgroundColor = headerFill,
                BorderColor = new BaseColor(220, 224, 232),
                HorizontalAlignment = Element.ALIGN_CENTER
            };
            var noFacturaValue = new PdfPCell(new Phrase((string.IsNullOrWhiteSpace(sale.Consecutive) ? "N/A" : sale.Consecutive), normalFont))
            {
                Border = Rectangle.BOX,
                Padding = 6f,
                BorderColor = new BaseColor(220, 224, 232),
                HorizontalAlignment = Element.ALIGN_CENTER
            };
            var fechaLabel = new PdfPCell(new Phrase("Fecha", labelFont))
            {
                Border = Rectangle.BOX,
                Padding = 6f,
                BackgroundColor = headerFill,
                BorderColor = new BaseColor(220, 224, 232),
                HorizontalAlignment = Element.ALIGN_CENTER
            };
            var fechaValue = new PdfPCell(new Phrase(sale.SaleDate.ToString("dd/MM/yyyy"), normalFont))
            {
                Border = Rectangle.BOX,
                Padding = 6f,
                BorderColor = new BaseColor(220, 224, 232),
                HorizontalAlignment = Element.ALIGN_CENTER
            };

            labelTable.AddCell(noFacturaLabel);
            labelTable.AddCell(noFacturaValue);
            labelTable.AddCell(fechaLabel);
            labelTable.AddCell(fechaValue);

            invoiceCell.AddElement(labelTable);
            invoiceCell.AddElement(new Paragraph(" ")
            {
                SpacingBefore = 6f,
                SpacingAfter = 6f
            });
            invoiceCell.AddElement(new Paragraph("Régimen Simplificado", normalFont));
            invoiceCell.AddElement(new Paragraph("Medellín", normalFont));

            headerTable.AddCell(logoCell);
            headerTable.AddCell(companyCell);
            headerTable.AddCell(invoiceCell);
            document.Add(headerTable);

            document.Add(new Paragraph(" ")
            {
                SpacingBefore = 6f,
                SpacingAfter = 10f
            });

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
                BorderWidth = 0.5f,
                BorderColor = new BaseColor(220, 224, 232),
                Padding = 4f,
                BackgroundColor = headerFill,
                HorizontalAlignment = Element.ALIGN_CENTER
            };
            var customerInfoValue = new PdfPCell(new Phrase(string.IsNullOrWhiteSpace(customer.IdentificationNumber) ? "N/A" : customer.IdentificationNumber, normalFont))
            {
                BorderWidth = 0.5f,
                BorderColor = new BaseColor(220, 224, 232),
                Padding = 4f,
                BackgroundColor = BaseColor.WHITE,
                HorizontalAlignment = Element.ALIGN_CENTER
            };

            customerTable.AddCell(customerInfoLabel);
            customerTable.AddCell(customerInfoValue);

            customerInfoLabel = new PdfPCell(new Phrase("Nombre", labelFont))
            {
                BorderWidth = 0.5f,
                BorderColor = new BaseColor(220, 224, 232),
                Padding = 4f,
                BackgroundColor = headerFill,
                HorizontalAlignment = Element.ALIGN_CENTER
            };
            customerInfoValue = new PdfPCell(new Phrase(string.IsNullOrWhiteSpace(customer.Name) ? "Cliente no registrado" : customer.Name, normalFont))
            {
                BorderWidth = 0.5f,
                BorderColor = new BaseColor(220, 224, 232),
                Padding = 4f,
                BackgroundColor = BaseColor.WHITE,
                HorizontalAlignment = Element.ALIGN_CENTER
            };
            customerTable.AddCell(customerInfoLabel);
            customerTable.AddCell(customerInfoValue);

            customerInfoLabel = new PdfPCell(new Phrase("Teléfono", labelFont))
            {
                BorderWidth = 0.5f,
                BorderColor = new BaseColor(220, 224, 232),
                Padding = 4f,
                BackgroundColor = headerFill,
                HorizontalAlignment = Element.ALIGN_CENTER
            };
            customerInfoValue = new PdfPCell(new Phrase(string.IsNullOrWhiteSpace(customer.Telephone) ? "N/A" : customer.Telephone, normalFont))
            {
                BorderWidth = 0.5f,
                BorderColor = new BaseColor(220, 224, 232),
                Padding = 4f,
                BackgroundColor = BaseColor.WHITE,
                HorizontalAlignment = Element.ALIGN_CENTER
            };
            customerTable.AddCell(customerInfoLabel);
            customerTable.AddCell(customerInfoValue);

            customerInfoLabel = new PdfPCell(new Phrase("Dirección", labelFont))
            {
                BorderWidth = 0.5f,
                BorderColor = new BaseColor(220, 224, 232),
                Padding = 4f,
                BackgroundColor = headerFill,
                HorizontalAlignment = Element.ALIGN_CENTER
            };
            customerInfoValue = new PdfPCell(new Phrase(string.IsNullOrWhiteSpace(customer.Address) ? "Sin dirección" : customer.Address, normalFont))
            {
                BorderWidth = 0.5f,
                BorderColor = new BaseColor(220, 224, 232),
                Padding = 4f,
                BackgroundColor = BaseColor.WHITE,
                HorizontalAlignment = Element.ALIGN_CENTER
            };
            customerTable.AddCell(customerInfoLabel);
            customerTable.AddCell(customerInfoValue);

            customerInfoLabel = new PdfPCell(new Phrase("Correo", labelFont))
            {
                BorderWidth = 0.5f,
                BorderColor = new BaseColor(220, 224, 232),
                Padding = 4f,
                BackgroundColor = headerFill,
                HorizontalAlignment = Element.ALIGN_CENTER
            };
            customerInfoValue = new PdfPCell(new Phrase(string.IsNullOrWhiteSpace(customer.Email) ? "Sin email" : customer.Email, normalFont))
            {
                BorderWidth = 0.5f,
                BorderColor = new BaseColor(220, 224, 232),
                Padding = 4f,
                BackgroundColor = BaseColor.WHITE,
                HorizontalAlignment = Element.ALIGN_CENTER
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

            if (!saleDetails.Any())
            {
                table.AddCell(new PdfPCell(new Phrase("Sin productos", normalFont))
                {
                    Colspan = 5,
                    BorderWidth = 0.5f,
                    BorderColor = new BaseColor(220, 224, 232),
                    Padding = 8f,
                    HorizontalAlignment = Element.ALIGN_CENTER
                });
            }
            else
            {
                foreach (var item in saleDetails)
                {
                    var product = item.Product ?? new ProductDTO();

                    table.AddCell(new PdfPCell(new Phrase(string.IsNullOrWhiteSpace(product.Name) ? "Producto" : product.Name, normalFont))
                    {
                        BorderWidth = 0.5f,
                        BorderColor = new BaseColor(220, 224, 232),
                        Padding = 6f
                    });

                    table.AddCell(new PdfPCell(new Phrase(string.IsNullOrWhiteSpace(product.Description) ? "Sin descripción" : product.Description, smallFont))
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
            return ms.ToArray();
        }

        private static DateTime NormalizeDateTimeUtc(DateTime value)
        {
            if (value == default)
            {
                return value;
            }

            return value.Kind switch
            {
                DateTimeKind.Utc => value,
                DateTimeKind.Local => value.ToUniversalTime(),
                _ => DateTime.SpecifyKind(value, DateTimeKind.Local).ToUniversalTime()
            };
        }

        private Image? TryLoadLogo()
        {
            var logoValue = _config["Company:LogoUrl"];
            var contentRoot = Directory.GetCurrentDirectory();
            var baseDirectory = AppContext.BaseDirectory;
            var exactCandidates = new List<string>
            {
                logoValue,
                "/app/assets/Logo_CR.png",
                "/app/assets/logo.png",
                "/workspaces/AdminCR_Backend/AdminCRWeb/assets/Logo_CR.png",
                "/workspaces/AdminCR_Backend/AdminCRWeb/assets/logo.png",
                Path.Combine(contentRoot, "assets", "Logo_CR.png"),
                Path.Combine(contentRoot, "assets", "logo.png"),
                Path.Combine(contentRoot, "AdminCRWeb", "assets", "Logo_CR.png"),
                Path.Combine(contentRoot, "AdminCRWeb", "assets", "logo.png"),
                Path.Combine(baseDirectory, "assets", "Logo_CR.png"),
                Path.Combine(baseDirectory, "assets", "logo.png")
            };

            foreach (var candidate in exactCandidates.Distinct())
            {
                if (string.IsNullOrWhiteSpace(candidate)) continue;

                try
                {
                    var resolvedPath = candidate;
                    if (!Path.IsPathRooted(candidate))
                    {
                        resolvedPath = Path.GetFullPath(Path.Combine(contentRoot, candidate));
                    }

                    if (System.IO.File.Exists(resolvedPath))
                    {
                        return Image.GetInstance(resolvedPath);
                    }
                }
                catch
                {
                    // Ignore invalid file candidates and continue.
                }
            }

            try
            {
                if (!string.IsNullOrWhiteSpace(logoValue) && Uri.TryCreate(logoValue, UriKind.Absolute, out var uri))
                {
                    using var httpClient = new HttpClient();
                    var bytes = httpClient.GetByteArrayAsync(uri).GetAwaiter().GetResult();
                    return Image.GetInstance(bytes);
                }
            }
            catch
            {
                return null;
            }

            return null;
        }
    }
}
