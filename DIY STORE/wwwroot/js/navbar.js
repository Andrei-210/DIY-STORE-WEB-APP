document.addEventListener("DOMContentLoaded", function () {

    const navbarContainer = document.getElementById("navbar-placeholder");
    if (!navbarContainer) return;

    // Incarca navbar-ul
    fetch("navbar.html")
        .then(res => res.text())
        .then(html => {navbarContainer.innerHTML = html;
        })
        .catch(err => console.error("Navbar error:", err));
});