using BillsClientApi.Services;
using BillsClientApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace BillsClientApi.Controllers;
[ApiController]
[Route("api/[controller]")]
public class BillsController : ControllerBase
    {
        private readonly BillsService _billsService;

        public BillsController(BillsService billsService) =>
            _billsService = billsService;

        [HttpGet]
        public async Task<List<Models.Bills>> Get() =>
            await _billsService.GetAsync();

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

