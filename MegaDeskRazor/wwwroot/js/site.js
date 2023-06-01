// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

const materialSelect = document.getElementById("materialSelect");
const materialImage = document.getElementById("materialImage");

function updateMaterialImage() {
    const selectedValue = materialSelect.value;

    // Update the image source based on the selected value
    switch (selectedValue) {
        case "1":
            materialImage.src = "../images/laminate.jpg";
            break;
        case "2":
            materialImage.src = "../images/oak.jpg";
            break;
        case "3":
            materialImage.src = "../images/pine.jpg";
            break;
        case "4":
            materialImage.src = "../images/rosewood.jpg";
            break;
        case "5":
            materialImage.src = "../images/veneer.jpg";
            break;
        default:
            materialImage.src = "../images/default-image.jpg"; // Set a default image source if needed
            break;
    }
}

// Call the updateMaterialImage function on page load
updateMaterialImage();

// Add change event listener to materialSelect
materialSelect.addEventListener("change", updateMaterialImage);


