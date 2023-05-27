using BillsClientApi.Services;
using BillsClientApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;

namespace BillsClientApi.Controllers;
[ApiController]
[Route("api/[controller]")]
public class BillsController : ControllerBase
    {
        private readonly BillsService _billsService;

        public BillsController(BillsService billsService) =>
            _billsService = billsService;
   
    [HttpGet]
    public async Task<List<Models.Bills>> Get()
    {
        try
        {
            var bills = await _billsService.GetAsync();

            var filteredBills = bills.Where(b => b.Estado == "primerrecordatorio" || b.Estado == "segundorecordatorio").ToList();

            foreach (var bill in filteredBills)
            {
                if (bill.Estado == "primerrecordatorio")
                {
                    try
                    {
                        SendEmail(bill.Correo, "Ha pasado a Segundo recordatorio");

                        await _billsService.UpdateAsync(bill.Id, new Models.Bills { Id= bill.Id,InvoiceCode = bill.InvoiceCode, Cliente= bill.Cliente, Ciudad= bill.Ciudad, NIT= bill.NIT, TotalInvoice = bill.TotalInvoice, SubTotal= bill.SubTotal, Iva= bill.Iva, Retencion= bill.Retencion, CreationDate = bill.CreationDate, Estado=  "segundorecordatorio", Pagada= bill.Pagada, PaymentDate = "", Correo= bill.Correo });
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error al enviar el correo electrónico: " + ex.Message);
                       
                    }
                }
                else if (bill.Estado == "segundorecordatorio")
                {
                    try
                    {
                        SendEmail(bill.Correo, "Va a ser desactivado");

                        await _billsService.UpdateAsync(bill.Id, new Models.Bills { Id = bill.Id, InvoiceCode = bill.InvoiceCode, Cliente = bill.Cliente, Ciudad = bill.Ciudad, NIT = bill.NIT, TotalInvoice = bill.TotalInvoice, SubTotal = bill.SubTotal, Iva = bill.Iva, Retencion = bill.Retencion, CreationDate = bill.CreationDate, Estado = "desactivado", Pagada = bill.Pagada, PaymentDate = "", Correo = bill.Correo });
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error al enviar el correo electrónico: " + ex.Message);
                        
                    }
                }
            }

            var updatedBills = await _billsService.GetAsync();

            return updatedBills;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error al procesar las facturas: " + ex.Message);
            return null;
        }
    }

    private async Task SendEmail(string emailAddress, string subject)
    {
        using (var client = new SmtpClient("smtp.gmail.com", 587))
        {
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("empresac575@gmail.com", "rxrakkabnpwtaobs");

            var message = new MailMessage("empresac575@gmail.com", emailAddress, subject, "Cordialmente.");

            try
            {
                await client.SendMailAsync(message);
                //Console.WriteLine("Correo electrónico enviado correctamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al enviar el correo electrónico: " + ex.Message);
                
            }
        }
    }

    //[HttpGet]
    //  public async Task<List<Models.Bills>> Get() =>
    //    await _billsService.GetAsync();

    [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Models.Bills>> Get(string id)
        {
            var bills = await _billsService.GetAsync(id);

            if (bills is null)
            {
                return NotFound();
            }

            return bills;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Models.Bills newBills)
        {
            await _billsService.CreateAsync(newBills);

            return CreatedAtAction(nameof(Get), new { id = newBills.Id }, newBills);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, Models.Bills updatedBills)
        {
            var bills = await _billsService.GetAsync(id);

            if (bills is null)
            {
                return NotFound();
            }

            updatedBills.Id = bills.Id;

            await _billsService.UpdateAsync(id, updatedBills);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var bills = await _billsService.GetAsync(id);

            if (bills is null)
            {
                return NotFound();
            }

            await _billsService.RemoveAsync(id);

            return NoContent();
        }
    }

