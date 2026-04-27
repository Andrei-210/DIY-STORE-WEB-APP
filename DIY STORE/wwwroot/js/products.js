document.addEventListener("DOMContentLoaded", function () {

    const container = document.getElementById("productsContainer");
    const paginationInfo = document.getElementById("paginationInfo");
    const paginationControls = document.getElementById("paginationControls");

    // Titlu categorie static
    document.getElementById("categoryTitle").textContent = "Power Tools Products";

    // Produse statice pentru demo vizual
    const products = [
        { name: "Drill 2000", price: "399 RON", images: ["images/drill.webp", "images/saws.jpg"] },
        { name: "Angle Grinder", price: "549 RON", images: ["images/grinder1.webp", "images/grinder2.webp"] },
        { name: "Circular Saw", price: "699 RON", images: ["images/saw1.webp", "images/saw2.webp"] },
        { name: "Electric Sander", price: "299 RON", images: ["images/sander1.webp", "images/sander2.webp"] }
    ];

    const productsPerPage = 4; // static
    let currentPage = 1;

    function renderProducts() {
        container.innerHTML = "";

        products.forEach((p, index) => {
            container.innerHTML += `
            <div class="col-md-3">
                <div class="card h-100 product-card clickable-card">
                    <div id="carousel${index}" class="carousel slide" data-bs-ride="carousel">
                        <div class="carousel-inner">
                            ${p.images.map((img, i) => `
                                <div class="carousel-item ${i === 0 ? "active" : ""}">
                                    <img src="${img}" class="d-block w-100" style="height:200px; object-fit:contain;">
                                </div>
                            `).join('')}
                        </div>
                        <button class="carousel-control-prev" type="button" data-bs-target="#carousel${index}" data-bs-slide="prev">
                            <span class="carousel-control-prev-icon"></span>
                        </button>
                        <button class="carousel-control-next" type="button" data-bs-target="#carousel${index}" data-bs-slide="next">
                            <span class="carousel-control-next-icon"></span>
                        </button>
                    </div>
                    <div class="card-body d-flex flex-column text-center">
                        <h6>${p.name}</h6>
                        <p class="fw-bold text-warning">${p.price}</p>
                        <div class="d-flex justify-content-between align-items-center mt-auto">
                            <button class="btn btn-warning add-to-cart-btn">Add to cart</button>
                            <button class="btn btn-outline-danger favorite-btn">❤</button>
                        </div>
                    </div>
                </div>
            </div>
            `;
        });

        // Buton Add to Cart
        document.querySelectorAll(".add-to-cart-btn").forEach((btn, i) => {
            btn.addEventListener("click", (e) => {
                e.stopPropagation();
                alert(`${products[i].name} has been added to the cart!`);
            });
        });

        // Favorite toggle vizual
        document.querySelectorAll(".favorite-btn").forEach(btn => {
            btn.addEventListener("click", (e) => {
                e.stopPropagation();
                btn.classList.toggle("btn-danger");
                btn.classList.toggle("btn-outline-danger");
                alert(`${products[i].name} has been added to the favorites!`);
            });
        });

        // Pagination info static
        paginationInfo.textContent = `Page 1 of 1`;
        paginationControls.innerHTML = ""; // nu există mai multe pagini
    }

    renderProducts();
});