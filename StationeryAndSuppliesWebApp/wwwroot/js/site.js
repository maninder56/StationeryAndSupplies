// Menu overlay buttons
const productMenu = document.getElementById("product-menu-overlay");

function openMenu() {
    productMenu.style.width = "100%";
}

function closeMenu() {
    productMenu.style.width = "0%";
}


// Search overlay buttons

const searchOverlay = document.getElementById("search-overlay");

function OpenSearch() {
    let currentHeight = window.innerHeight; 

    //if (currentHeight > 500) {
    //    searchOverlay.style.height = "50%";
    //}
    //else {
    //    searchOverlay.style.height = "300px";
    //}

    //searchOverlay.style.height = "300px";
    searchOverlay.style.height = "100%";
    
}

function CloseSearch() {
    searchOverlay.style.height = "0%";
}


let eightHundredPixelOrMoreMediaQuery = window.matchMedia("(min-width: 800px)");

eightHundredPixelOrMoreMediaQuery.addEventListener("change", () => {
    if (eightHundredPixelOrMoreMediaQuery.matches) {
        productMenu.style.removeProperty("width");
        searchOverlay.style.removeProperty("height");
    }
})



// Home page scrollable cards

const categoryContainer = document.getElementById("categories-container");

function leftScroll() {
    categoryContainer.scrollBy({
        left: - categoryContainer.clientWidth,
        behavior: "smooth"
    });
}

function rightScroll() {
    categoryContainer.scrollBy({
        left: categoryContainer.clientWidth,
        behavior: "smooth"
    });
}



// Order by Drop down on product list page

const orderByDropDownForm = document.getElementById("order-by-drop-down-form"); 
const orderbyDorpDown = document.getElementById("order-by-drop-down"); 

if (orderbyDorpDown !== null) {
    orderbyDorpDown.onchange = function () {
        orderByDropDownForm.submit();
    }
}




// To calculate when price when quantity changes on Single product page

const originalProductPrice = document.getElementById("original-product-price");
const currentProductPrice = document.getElementById("current-product-price"); 
const quantityDropDownOptions = document.getElementById("quantity-drop-down-options"); 


if (quantityDropDownOptions !== null) {
    quantityDropDownOptions.onchange = () => {
        let originalPrice = parseFloat(originalProductPrice.innerText);
        let quantityNumber = Number.parseInt(quantityDropDownOptions.value);

        if (!(Number.isNaN(originalPrice) || Number.isNaN(quantityNumber))) {
            currentProductPrice.textContent = `£${(originalPrice * quantityNumber).toFixed(2)}`;
        }
    }
}








