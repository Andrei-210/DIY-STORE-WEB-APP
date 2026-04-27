document.addEventListener("DOMContentLoaded", function () {

    // PRODUS STATIC
    const product = {
        name: "Drill 2000",
        price: "399 RON",
        oldPrice: "499 RON",
        discount: "-20%",
        description: "Ciocan electric pentru mesteri",
        images: ["images/drill.webp", "images/saws.jpg"],
        manufacturer: { name: "Bosch", logo: "images/bosch_logo.webp" },
        specifications: ["500W", "Mandrina 13mm"]
    };

    // Titlu ai pret
    document.getElementById("productTitle").textContent = product.name;
    document.getElementById("productPrice").textContent = product.price;
    document.getElementById("oldPrice").textContent = product.oldPrice;
    document.getElementById("discountPercent").textContent = product.discount;
    document.getElementById("productDescription").textContent = product.description;

    // Carousel imagini
    const carouselInner = document.getElementById("productImages");
    carouselInner.innerHTML = product.images.map((img, i) => `
        <div class="carousel-item ${i === 0 ? "active" : ""}">
            <img src="${img}" class="d-block w-100" style="height:300px; object-fit:contain;">
        </div>
    `).join('');

    // Producator
    document.getElementById("manufacturerLogo").src = product.manufacturer.logo;
    document.getElementById("manufacturerName").textContent = product.manufacturer.name;

    // Buton Add to Cart
    document.getElementById("addToCartBtn").addEventListener("click", () => {
        alert(`Ai adaugat ${product.name} in cos!`);
    });

    // Specifications
    const specsContainer = document.getElementById("productSpecs");
    specsContainer.innerHTML = product.specifications.map(spec => `<li>${spec}</li>`).join('');

    // Produse similare (STATIC)
    const similarContainer = document.getElementById("similarProductsContainer");
    const similarProducts = [
        { name: "Electric Sander", price: "549 RON", image: "images/screwdriver.jpg" },
        { name: "Electric Ceva", price: "678 RON", image: "images/hammer.jpg" }
    ];
    similarContainer.innerHTML = "";
    similarProducts.forEach(p => {
        const card = document.createElement("div");
        card.className = "col-md-3";
        card.innerHTML = `
            <div class="card h-100">
                <img src="${p.image}" class="card-img-top" style="height:150px; object-fit:contain;">
                <div class="card-body text-center">
                    <h6>${p.name}</h6>
                    <p class="fw-bold text-warning">${p.price}</p>
                </div>
            </div>
        `;
        similarContainer.appendChild(card);
    });

});