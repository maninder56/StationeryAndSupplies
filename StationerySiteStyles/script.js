const productMenu = document.getElementById("product-menu"); 

// overlay buttons
function openMenu() {
    productMenu.style.width = "100%"; 
}

function closeMenu() {
    productMenu.style.width = "0%"; 
}


let eightHundredPixelOrMoreMediaQuery = window.matchMedia("(min-width: 800px)"); 

eightHundredPixelOrMoreMediaQuery.addEventListener("change", () => {
    if (eightHundredPixelOrMoreMediaQuery.matches) {
        productMenu.style.removeProperty("width"); 
    } 
})

