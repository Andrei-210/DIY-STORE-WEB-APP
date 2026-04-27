// Cart utility - Add to Cart via AJAX (POST to /Cart/Add)
// This script handles .add-to-cart-btn buttons.
// It uses a single DOMContentLoaded listener and checks if already initialized
// to prevent double-registration when multiple scripts include cart.js.

(function () {
    if (window.__cartJsInit) return;
    window.__cartJsInit = true;

    // Toast container - created once
    function getOrCreateToastContainer() {
        let c = document.getElementById("toast-container");
        if (!c) {
            c = document.createElement("div");
            c.id = "toast-container";
            c.style.cssText = "position:fixed;bottom:20px;right:20px;z-index:9999;display:flex;flex-direction:column;gap:8px;";
            document.body.appendChild(c);
        }
        return c;
    }

    function showToast(message, type = "success") {
        const toastContainer = getOrCreateToastContainer();
        const toast = document.createElement("div");
        toast.className = `alert alert-${type} shadow-lg mb-0`;
        toast.style.cssText = "min-width:280px;animation:fadeInUp .3s ease;";
        toast.innerHTML = `<strong>${type === "success" ? "✅" : "❌"}</strong> ${message}`;
        toastContainer.appendChild(toast);
        setTimeout(() => { toast.style.opacity = "0"; toast.style.transition = "opacity .4s"; }, 2500);
        setTimeout(() => toast.remove(), 3000);
    }

    function getToken() {
        const el = document.querySelector('input[name="__RequestVerificationToken"]');
        return el ? el.value : "";
    }

    async function updateCartCount() {
        try {
            const resp = await fetch("/Cart/Count");
            if (resp.ok) {
                const data = await resp.json();
                const badge = document.getElementById("cart-count");
                if (badge) badge.textContent = data.count;
            }
        } catch {}
    }

    function attachCartButtons() {
        document.querySelectorAll(".add-to-cart-btn:not([data-cart-bound])").forEach(btn => {
            btn.setAttribute("data-cart-bound", "1");
            btn.addEventListener("click", async function () {
                const productId = this.dataset.productId;
                const productName = this.dataset.productName;
                if (!productId) return;

                const token = getToken();
                if (!token) {
                    showToast("Session expired. Please refresh the page.", "danger");
                    return;
                }

                try {
                    const resp = await fetch("/Cart/Add", {
                        method: "POST",
                        headers: {
                            "Content-Type": "application/x-www-form-urlencoded",
                            "Accept": "application/json"
                        },
                        body: `productId=${productId}&quantity=1&__RequestVerificationToken=${encodeURIComponent(token)}`
                    });

                    if (resp.ok) {
                        showToast(`"${productName}" a fost adăugat în coș! 🛒`, "success");
                        updateCartCount();
                    } else {
                        showToast("Eroare la adăugarea în coș.", "danger");
                    }
                } catch (e) {
                    showToast("Eroare de conexiune.", "danger");
                }
            });
        });
    }

    document.addEventListener("DOMContentLoaded", attachCartButtons);

    // CSS animation
    const style = document.createElement("style");
    style.textContent = `@keyframes fadeInUp { from { opacity:0; transform:translateY(20px); } to { opacity:1; transform:translateY(0); } }`;
    document.head.appendChild(style);
})();
