const productMenu = document.getElementById("product-menu"); 
const searchOverlay = document.getElementById("search-overlay"); 

// overlay buttons
function openMenu() {
    productMenu.style.width = "100%"; 
}

function closeMenu() {
    productMenu.style.width = "0%"; 
}

function OpenSearch() {
    searchOverlay.style.height = "50%"; 
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

