const productMenu = document.getElementById("product-menu-overlay");
const searchOverlay = document.getElementById("search-overlay");

// overlay buttons
function openMenu() {
    productMenu.style.width = "100%";
}

function closeMenu() {
    productMenu.style.width = "0%";
}

function OpenSearch() {
    let currentHeight = window.innerHeight; 

    if (currentHeight > 500) {
        searchOverlay.style.height = "50%";
    }
    else {
        searchOverlay.style.height = "300px";
    }
    
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
const numberOfpixelsToScroll = 400; 

function leftScroll() {
    categoryContainer.scrollBy({
        left: - numberOfpixelsToScroll,
        behavior: "smooth"
    });
}

function rightScroll() {
    categoryContainer.scrollBy({
        left: numberOfpixelsToScroll,
        behavior: "smooth"
    });
}


