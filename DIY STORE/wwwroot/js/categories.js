document.addEventListener("DOMContentLoaded", function () {

    const params = new URLSearchParams(window.location.search);
    const categorie = params.get("cat");

    const titlu = document.getElementById("titluCategorie");
    const container = document.getElementById("listaSubcategorii");

    const date = {
        "power-tools": {
            titlu: "Power Tools",
            subcategorii: [
                { nume: "Drills", imagine: "images/drill.webp" },
                { nume: "Grinders", imagine: "images/grinders.webp" },
                { nume: "Saws", imagine: "images/saws.jpg" },
                { nume: "Sanders", imagine: "images/Sanders.jpg" }
            ]
        },
        "hand-tools": {
            titlu: "Hand Tools",
            subcategorii: [
                { nume: "Hammers", imagine: "images/hammer.jpg" },
                { nume: "Screwdrivers", imagine: "images/screwdriver.jpg" },
                { nume: "Wrenches", imagine: "images/wrenche.webp" },
                { nume: "Pliers", imagine: "images/plier.webp" }
            ]
        },
        "garden": {
            titlu: "Garden & Outdoor",
            subcategorii: [
                { nume: "Shovels", imagine: "images/shovel.webp" },
                { nume: "Seeds", imagine: "images/seed.webp" },
                { nume: "Pots", imagine: "images/pot.jpg" },
                { nume: "Hoses", imagine: "images/hoses.jpg" }
            ]
        },
        "paint": {
            titlu: "Paint & Finishes",
            subcategorii: [
                { nume: "Wall Paint", imagine: "images/wall paint.webp" },
                { nume: "Varnish", imagine: "images/varnish.webp" },
                { nume: "Brushes", imagine: "images/brush.webp" },
                { nume: "Rollers", imagine: "images/roller.jpg" }
            ]
        },
        "plumbing-electric": {
            titlu: "Plumbing & Electrical",
            subcategorii: [
                { nume: "Pipes", imagine: "images/pipe.jpg" },
                { nume: "Faucets", imagine: "images/chiuveta.webp" },
                { nume: "Cables", imagine: "images/cable.webp" },
                { nume: "Switches", imagine: "images/switch.webp" }
            ]
        },
        "building-materials": {
            titlu: "Building Materials",
            subcategorii: [
                { nume: "Bricks", imagine: "images/brick.webp" },
                { nume: "Cement", imagine: "images/cement.jpg" },
                { nume: "Wood", imagine: "images/wood.jpg" },
                { nume: "Insulation", imagine: "images/insulation.jpg" }
            ]
        }
    };

    if (date[categorie]) {
        titlu.textContent = date[categorie].titlu;

        date[categorie].subcategorii.forEach(sub => {
            container.innerHTML += `
                <div class="col-md-4">
                    <div class="card shadow-sm mb-3 h-100 d-flex flex-column">
                        <img src="${sub.imagine}" class="card-img-top" style="object-fit: contain; max-height: 354px;" alt="${sub.nume}">
                
                        <!-- Card body flex-column -->
                        <div class="card-body d-flex flex-column text-center">
                            <h5 class="mb-3 mt-auto">${sub.nume}</h5>
                            <a href="products.html?cat=${categorie}&sub=${sub.nume}" class="btn btn-warning">
                                View products
                            </a>
                        </div>
                    </div>
                </div>
            `;
        });
    } else {
        titlu.textContent = "Category not found";
    }

});