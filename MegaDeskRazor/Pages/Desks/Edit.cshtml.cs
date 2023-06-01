using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MegaDeskRazor.Data;
using MegaDeskRazor.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace MegaDeskRazor.Pages.Desks
{
    public class EditModel : PageModel
    {
        private readonly MegaDeskRazorContext _context;
        private readonly IWebHostEnvironment _env;

        // Constants for calculation
        private const decimal BasePrice = 200.00m;
        private const decimal CostPerDrawer = 50.00m;
        private const int NonChargeableArea = 1000;

        public EditModel(MegaDeskRazorContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [BindProperty]
        public Desk Desk { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Desk == null)
            {
                return NotFound();
            }

            var desk = await _context.Desk.FirstOrDefaultAsync(m => m.Id == id);
            if (desk == null)
            {
                return NotFound();
            }
            Desk = desk;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Calculate Date
            Desk.Date = DateTime.Now;

            // Calculate TotalArea
            Desk.TotalArea = Desk.DeskWidth * Desk.DeskDepth;

            // Calculate TotalSurfaceCost
            int chargeableArea;
            if (Desk.TotalArea > NonChargeableArea)
            {
                chargeableArea = Desk.TotalArea - NonChargeableArea;
            }
            else
            {
                chargeableArea = 0;
            }
            Desk.TotalSurfaceCost = chargeableArea * 1.00m;

            // Calculate TotalDrawerCost
            Desk.TotalDrawerCost = Desk.DeskNumDrawers * CostPerDrawer;

            // Calculate TotalSurfaceMaterialCost
            string surfaceMaterialType = "";
            int materialCost;
            switch (Desk.SurfaceMaterialIndex)
            {
                case "1":
                    surfaceMaterialType = "Laminate";
                    materialCost = 100;
                    break;
                case "2":
                    surfaceMaterialType = "Oak";
                    materialCost = 200;
                    break;
                case "3":
                    surfaceMaterialType = "Pine";
                    materialCost = 50;
                    break;
                case "4":
                    surfaceMaterialType = "Rosewood";
                    materialCost = 300;
                    break;
                case "5":
                    surfaceMaterialType = "Veneer";
                    materialCost = 125;
                    break;
                default:
                    materialCost = 0;
                    break;
            }
            Desk.SurfaceMaterialType = surfaceMaterialType;
            Desk.TotalSurfaceMaterialCost = materialCost;

            // Calculate TotalShippingCost
            string rushOrderPriceFile = Path.Combine(_env.ContentRootPath, "Data", "rushOrderPrices.txt");
            string[] prices = System.IO.File.ReadAllLines(rushOrderPriceFile);

            string[,] rushOrderPrices = new string[3, 3];
            int row = 0;
            int column = 0;
            int priceIndex = 0;

            for (; row < 3; row++)
            {
                for (; column < 3; column++)
                {
                    rushOrderPrices[row, column] = prices[priceIndex];
                    priceIndex++;
                }
                column = 0;
            }

            int shippingCost = 0;
            int shippingTypeIndex = int.Parse(Desk.ShippingTypeIndex);
            string shippingType = "";
            int totalArea = Desk.TotalArea;

            switch (shippingTypeIndex)
            {
                case 3:
                    shippingType = "3 Days";
                    if (totalArea > 2000)
                    {
                        shippingCost = int.Parse(rushOrderPrices[0, 2]);
                    }
                    else if (totalArea >= 1000)
                    {
                        shippingCost = int.Parse(rushOrderPrices[0, 1]);
                    }
                    else
                    {
                        shippingCost = int.Parse(rushOrderPrices[0, 0]);
                    }
                    break;
                case 5:
                    shippingType = "5 Days";
                    if (totalArea > 2000)
                    {
                        shippingCost = int.Parse(rushOrderPrices[1, 2]);
                    }
                    else if (totalArea >= 1000)
                    {
                        shippingCost = int.Parse(rushOrderPrices[1, 1]);
                    }
                    else
                    {
                        shippingCost = int.Parse(rushOrderPrices[1, 0]);
                    }
                    break;
                case 7:
                    shippingType = "7 Days";
                    if (totalArea > 2000)
                    {
                        shippingCost = int.Parse(rushOrderPrices[2, 2]);
                    }
                    else if (totalArea >= 1000)
                    {
                        shippingCost = int.Parse(rushOrderPrices[2, 1]);
                    }
                    else
                    {
                        shippingCost = int.Parse(rushOrderPrices[2, 0]);
                    }
                    break;
                default:
                    shippingType = "14 Days";
                    shippingCost = 0;
                    break;
            }

            Desk.ShippingType = shippingType;
            Desk.TotalShippingCost = shippingCost;

            // Set BasePrice
            decimal BaseCost = BasePrice;
            Desk.BasePrice = BaseCost;

            // Calculate TotalCost
            decimal totalCost = BasePrice + Desk.TotalSurfaceMaterialCost + Desk.TotalDrawerCost + Desk.TotalSurfaceCost + Desk.TotalShippingCost;
            Desk.TotalCost = totalCost;

            _context.Attach(Desk).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DeskExists(Desk.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool DeskExists(int id)
        {
            return (_context.Desk?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
