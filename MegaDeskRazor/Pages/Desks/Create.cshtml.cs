using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MegaDeskRazor.Data;
using MegaDeskRazor.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace MegaDeskRazor.Pages.Desks
{
    public class CreateModel : PageModel
    {
        private readonly MegaDeskRazorContext _context;
        private readonly IWebHostEnvironment _env;

        // Constants for calculation
        private const decimal BasePrice = 200.00m;
        private const decimal CostPerDrawer = 50.00m;
        private const int NonChargeableArea = 1000;

        public CreateModel(MegaDeskRazorContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Desk Desk { get; set; } = default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {

            if (!ModelState.IsValid || _context.Desk == null || Desk == null)
            {
                return Page();
            }

            // System Calculate Date
            Desk.Date = DateTime.Now;

            // System Calculate TotalArea
            Desk.TotalArea = Desk.DeskWidth * Desk.DeskDepth;

            // System Calculate TotalSurfaceCost
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

            // System Calculate TotalDrawerCost
            Desk.TotalDrawerCost = Desk.DeskNumDrawers * CostPerDrawer;

            // System Calculate TotalSurfaceMaterialCost
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

            // System Calculate TotalShippingCost
            string rushOrderPriceFile = Path.Combine(_env.ContentRootPath, "Data", "rushOrderPrices.txt");
            string[] prices = System.IO.File.ReadAllLines(rushOrderPriceFile);

            Console.WriteLine(prices);
            Console.WriteLine(prices[0]);

            string[,] rushOrderPrices = new string[3, 3];
            int row = 0;
            int column = 0;
            int priceIndex = 0;

            for (; row < 3; row++)
            {
                for (; column < 3; column++)
                {
                    rushOrderPrices[row, column] = prices[priceIndex].Trim();
                    priceIndex++;
                }
                column = 0;
            }

            int shippingCost = 0;
            int shippingTypeIndex = Int32.Parse(Desk.ShippingTypeIndex);
            string shippingType = "";
            int totalArea = Desk.TotalArea;

            switch (shippingTypeIndex)
            {
                case 3:
                    shippingType = "3 Days";
                    if (totalArea > 2000)
                    {
                        shippingCost = Int32.Parse(rushOrderPrices[0, 2]);
                    }
                    else if (totalArea >= 1000)
                    {
                        shippingCost = Int32.Parse(rushOrderPrices[0, 1]);
                    }
                    else
                    {
                        shippingCost = Int32.Parse(rushOrderPrices[1, 2]);
                    }
                    break;
                case 5:
                    shippingType = "5 Days";
                    if (totalArea > 2000)
                    {
                        shippingCost = Int32.Parse(rushOrderPrices[1, 2]);
                    }
                    else if (totalArea >= 1000)
                    {
                        shippingCost = Int32.Parse(rushOrderPrices[1, 1]);
                    }
                    else
                    {
                        shippingCost = Int32.Parse(rushOrderPrices[1, 0]);
                    }
                    break;
                case 7:
                    shippingType = "7 Days";
                    if (totalArea > 2000)
                    {
                        shippingCost = Int32.Parse(rushOrderPrices[2, 2]);
                    }
                    else if (totalArea >= 1000)
                    {
                        shippingCost = Int32.Parse(rushOrderPrices[2, 1]);
                    }
                    else
                    {
                        shippingCost = Int32.Parse(rushOrderPrices[2, 0]);
                    }
                    break;
                default:
                    shippingType = "14 Days";
                    shippingCost = 0;
                    break;
            }

            Desk.ShippingType = shippingType;
            Desk.TotalShippingCost = shippingCost;

            // System Set BasePrice
            decimal BaseCost = BasePrice;
            Desk.BasePrice = BaseCost;

            // System Calculate TotalCost
            decimal totalCost = BasePrice + Desk.TotalSurfaceMaterialCost + Desk.TotalDrawerCost + Desk.TotalSurfaceCost + Desk.TotalShippingCost;
            Desk.TotalCost = totalCost;

            _context.Desk.Add(Desk);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

    }
}
