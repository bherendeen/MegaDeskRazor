using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MegaDeskRazor.Models
{
    public class Desk
    {
        // ID
        public int Id { get; set; }

        // CUSTOMER NAME
        [BindProperty]
        [Display(Name = "Name")] // Display name
        [Required] // Required
        [RegularExpression(@"^[A-Za-z'\-\s]+$", ErrorMessage = "Must only contain valid characters for a name.")] // Accept all common characters used for a name
        [StringLength(25, MinimumLength = 3, ErrorMessage = "Must be at least between 3 - 25 characters.")]
        public string? CustomerName { get; set; }

        // DESK WIDTH
        [BindProperty]
        [Display(Name = "Width")] // Display name
        [Required] // Required
        [RegularExpression(@"^\d+$", ErrorMessage = "Must only be numeric values.")] // Only accept numbers 0-9 and at least one digit
        [Range(24, 96, ErrorMessage = "Width must be between 24 and 96 inches.")] // Only accept number 24-96
        public int DeskWidth { get; set; }

        // DESK DEPTH
        [BindProperty]
        [Display(Name = "Depth")] // Display name
        [Required] // Required
        [RegularExpression(@"^\d+$", ErrorMessage = "Must only be numeric values.")] // Only accept numbers 0-9 and at least one digit
        [Range(12, 48, ErrorMessage = "Width must be between 12 and 48 inches.")] // Only accept number 12-48
        public int DeskDepth { get; set; }

        // NUMBER OF DRAWERS
        [BindProperty]
        [Display(Name = "Drawers")] // Display name
        [Required] // Required
        [RegularExpression(@"^\d+$", ErrorMessage = "Must only be numeric values.")] // Only accept numbers 0-9 and one digit only
        [Range(0, 7, ErrorMessage = "Number of drawers must be between 0 and 7.")] // Only accept number 0-7
        public int DeskNumDrawers { get; set; }

        // SURFACE MATERIAL INDEX
        [BindProperty]
        [Display(Name = "Material Index")] // Display name
        [Required] // Required
        public string? SurfaceMaterialIndex { get; set; }

        // SHIPPING TYPE INDEX
        [BindProperty]
        [Display(Name = "Shipping Index")] // Display name
        [Required] // Required
        public string? ShippingTypeIndex { get; set; }


        // [CALCULATED] DATE
        [Display(Name = "Date Created")] // Display name
        [DataType(DataType.Date)] // Accept date only
        public DateTime Date { get; set; }

        // [CALCULATED] TOTAL AREA
        [Display(Name = "Total Area")] // Display name.
        public int TotalArea { get; set; } // Calculated field for TotalArea.

        // [CALCULATED] TOTAL SURFACE COST
        [Display(Name = "Total Surface Cost")] // Display name
        [DisplayFormat(DataFormatString = "{0:C2}")] // Display as currency with two decimal places
        public decimal TotalSurfaceCost { get; set; } // Calculated field for TotalSurfaceCost

        // [CALCULATED] TOTAL DRAWER COST
        [Display(Name = "Total Drawer Cost")] // Display name
        [DisplayFormat(DataFormatString = "{0:C2}")] // Display as currency with two decimal places
        public decimal TotalDrawerCost { get; set; } // Calculated field for TotalDrawerCost

        // [CALCULATED] SURFACE MATERIAL TYPE
        [Display(Name = "Surface Material Type")] // Display name
        public string? SurfaceMaterialType { get; set; } // Calculated field for SurfaceMaterialType

        // [CALCULATED] TOTAL SURFACE MATERIAL COST
        [Display(Name = "Total Surface Material Cost")] // Display name
        [DisplayFormat(DataFormatString = "{0:C2}")] // Display as currency with two decimal places
        public decimal TotalSurfaceMaterialCost { get; set; } // Calculated field for TotalSurfaceMaterialCost 

        // [CALCULATED] SHIPPING TYPE
        [Display(Name = "Shipping Type")] // Display name
        public string? ShippingType { get; set; } // Calculated field for ShippingType

        // [CALCULATED] TOTAL SHIPPING COST
        [Display(Name = "Total Shipping Cost")] // Display name
        [DisplayFormat(DataFormatString = "{0:C2}")] // Display as currency with two decimal places
        public decimal TotalShippingCost { get; set; } // Calculated field for TotalShippingCost 

        // [CALCULATED] TOTAL COST
        [Display(Name = "Total")] // Display name
        [DisplayFormat(DataFormatString = "{0:C2}")] // Display as currency with two decimal places
        public decimal TotalCost { get; set; } // Calculated field for TotalCost

        // [CALCULATED] BASE PRICE
        [Display(Name = "Base Price")] // Display name
        [DisplayFormat(DataFormatString = "{0:C2}")] // Display as currency with two decimal places
        public decimal BasePrice { get; set; }
    }
}


