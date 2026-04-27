document.addEventListener("DOMContentLoaded", function () {

    const buttons = document.querySelectorAll(".profile-sidebar .btn");

    function setActive(section) {
        buttons.forEach(b => b.classList.remove("active"));
        const activeBtn = document.querySelector(`[data-section="${section}"]`);
        if (activeBtn) activeBtn.classList.add("active");
    }

    function showSection(section) {
        setActive(section);
        loadSection(section);
        window.location.hash = section;
    }

    // incarcare initiala
    const initialHash = window.location.hash.replace("#", "") || "account";
    showSection(initialHash);

    // click pe bara laterala
    buttons.forEach(btn => {
        btn.addEventListener("click", function () {
            showSection(this.dataset.section);
        });
    });

    window.addEventListener("hashchange", function () {
        const newHash = window.location.hash.replace("#", "");
        if (newHash) showSection(newHash);
    });

});

function loadSection(section) {
    if (section === "account") loadAccount();
    if (section === "orders") loadOrders();
    if (section === "favorites") loadFavorites();
}

// --- Cont static ---
function loadAccount() {
    const content = document.getElementById("profile-content");

    content.innerHTML = `
        <h4 class="mb-4">Informații cont</h4>
        <form id="accountForm">
            <div class="mb-3">
                <label class="form-label">Username</label>
                <input type="text" class="form-control" id="username" value="DemoUser">
            </div>
            <div class="mb-3">
                <label class="form-label">Email</label>
                <input type="email" class="form-control" id="email" value="demo@example.com">
            </div>
            <div class="mb-3">
                <label class="form-label">Adresă</label>
                <input type="text" class="form-control" id="address" value="Str. Demo 1, București">
            </div>
            <div class="mb-3">
                <label class="form-label">Parolă nouă</label>
                <input type="password" class="form-control" id="password">
            </div>
            <button class="btn btn-primary">Salvează modificările</button>
        </form>
    `;

    document.getElementById("accountForm").addEventListener("submit", function (e) {
        e.preventDefault();
        showAlert("Datele au fost actualizate!", "success");
    });
}

// --- Istoric comenzi static ---
function loadOrders() {
    const content = document.getElementById("profile-content");

    content.innerHTML = `
        <h4 class="mb-4">Istoric comenzi</h4>
        <div class="card mb-3 border-0 shadow-sm">
            <div class="card-body">
                <h6>Comanda #1234</h6>
                <p class="mb-1"><strong>Data:</strong> 01/03/2026</p>
                <p class="mb-1"><strong>Total:</strong> 399 RON</p>
            </div>
        </div>
        <div class="card mb-3 border-0 shadow-sm">
            <div class="card-body">
                <h6>Comanda #1235</h6>
                <p class="mb-1"><strong>Data:</strong> 28/02/2026</p>
                <p class="mb-1"><strong>Total:</strong> 549 RON</p>
            </div>
        </div>
    `;
}

// --- Favorite static ---
function loadFavorites() {
    const content = document.getElementById("profile-content");

    content.innerHTML = `
        <h4 class="mb-4">Produse favorite</h4>
        <ul class="list-group">
            <li class="list-group-item d-flex justify-content-between align-items-center">
                <div>
                    <strong>Drill 2000</strong>
                    <div class="text-muted">399 RON</div>
                </div>
                <button class="btn btn-sm btn-danger">Șterge</button>
            </li>
            <li class="list-group-item d-flex justify-content-between align-items-center">
                <div>
                    <strong>Electric Sander</strong>
                    <div class="text-muted">299 RON</div>
                </div>
                <button class="btn btn-sm btn-danger">Șterge</button>
            </li>
        </ul>
    `;
}

// --- Alert vizual ---
function showAlert(message, type) {
    const content = document.getElementById("profile-content");
    const alert = document.createElement("div");
    alert.className = `alert alert-${type} mt-3`;
    alert.innerText = message;
    content.prepend(alert);
    setTimeout(() => alert.remove(), 3000);
}