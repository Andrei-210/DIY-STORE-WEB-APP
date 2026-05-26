// Cart utility - Add to Cart via AJAX (POST to /Cart/Add)
// Handles .add-to-cart-btn buttons on any page.

(function () {
    if (window.__cartJsInit) return;
    window.__cartJsInit = true;

    // ── Toast (corner notification) ──────────────────────────────────────────
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

    // ── Cart count badge ─────────────────────────────────────────────────────
    function updateCartCount(count) {
        const badge = document.getElementById("cart-count");
        if (!badge) return;
        if (count > 0) {
            badge.textContent = count;
            badge.style.display = "";
        } else {
            badge.style.display = "none";
        }
    }

    async function fetchAndUpdateCartCount() {
        try {
            const resp = await fetch("/Cart/Count");
            if (resp.ok) {
                const data = await resp.json();
                updateCartCount(data.count);
            }
        } catch {}
    }

    // ── Modal ─────────────────────────────────────────────────────────────────
    function injectModalStyles() {
        if (document.getElementById("cart-modal-styles")) return;
        const style = document.createElement("style");
        style.id = "cart-modal-styles";
        style.textContent = `
            #cart-modal-overlay {
                position: fixed; inset: 0;
                background: rgba(0,0,0,0.55);
                z-index: 10000;
                display: flex; align-items: center; justify-content: center;
                animation: cmFadeIn .2s ease;
            }
            #cart-modal-box {
                background: #fff;
                border-radius: 16px;
                box-shadow: 0 24px 80px rgba(0,0,0,0.35);
                max-width: 420px; width: 92%;
                overflow: hidden;
                animation: cmSlideUp .25s ease;
            }
            #cart-modal-header {
                background: linear-gradient(135deg,#f59e0b,#d97706);
                color:#fff; padding:1rem 1.25rem;
                display:flex; align-items:center; justify-content:space-between;
            }
            #cart-modal-header h5 { margin:0; font-weight:700; font-size:1rem; }
            #cart-modal-close {
                background:none; border:none; color:#fff;
                font-size:1.4rem; line-height:1; cursor:pointer; padding:0 4px;
            }
            #cart-modal-body { padding:1.25rem; display:flex; gap:1rem; align-items:flex-start; }
            #cart-modal-img {
                width:80px; height:80px; object-fit:contain;
                border-radius:8px; border:1px solid #e5e7eb; flex-shrink:0;
            }
            #cart-modal-info { flex:1; }
            #cart-modal-name { font-weight:600; font-size:0.95rem; margin-bottom:4px; }
            #cart-modal-price { color:#d97706; font-weight:700; font-size:1.05rem; margin-bottom:6px; }
            #cart-modal-footer { padding:.75rem 1.25rem 1.25rem; display:flex; gap:.5rem; }
            #cart-modal-footer a, #cart-modal-footer button {
                flex:1; padding:.55rem; font-weight:600; font-size:.9rem;
                border-radius:8px; text-align:center; text-decoration:none;
            }
            @keyframes cmFadeIn { from{opacity:0} to{opacity:1} }
            @keyframes cmSlideUp { from{opacity:0;transform:translateY(30px)} to{opacity:1;transform:translateY(0)} }
        `;
        document.head.appendChild(style);
    }

    function showCartModal(name, price, imageUrl) {
        const existing = document.getElementById("cart-modal-overlay");
        if (existing) existing.remove();

        injectModalStyles();

        const cleanImage = imageUrl
            ? (imageUrl.startsWith("/") || imageUrl.startsWith("http") ? imageUrl : "/" + imageUrl)
            : "/images/placeholder.webp";

        const overlay = document.createElement("div");
        overlay.id = "cart-modal-overlay";
        overlay.innerHTML = `
            <div id="cart-modal-box">
                <div id="cart-modal-header">
                    <h5>🛒 Added to cart!</h5>
                    <button id="cart-modal-close" title="Close">×</button>
                </div>
                <div id="cart-modal-body">
                    <img id="cart-modal-img" src="${cleanImage}" alt="${name}" onerror="this.src='/images/placeholder.webp'">
                    <div id="cart-modal-info">
                        <div id="cart-modal-name">${name}</div>
                        <div id="cart-modal-price">${price}</div>
                        <div style="font-size:.8rem;color:#6b7280;">Successfully added to your cart.</div>
                    </div>
                </div>
                <div id="cart-modal-footer">
                    <button onclick="document.getElementById('cart-modal-overlay').remove()"
                            style="background:#f3f4f6;border:1.5px solid #e5e7eb;color:#374151;">
                        Continue shopping
                    </button>
                    <a href="/Cart/Index"
                       style="background:linear-gradient(135deg,#f59e0b,#d97706);color:#fff;border:none;display:flex;align-items:center;justify-content:center;">
                        View cart →
                    </a>
                </div>
            </div>
        `;

        overlay.addEventListener("click", function (e) {
            if (e.target === overlay) overlay.remove();
        });

        document.body.appendChild(overlay);
        document.getElementById("cart-modal-close").addEventListener("click", () => overlay.remove());

        setTimeout(() => { if (overlay.parentNode) overlay.remove(); }, 8000);
    }

    // Expose showCartModal globally (used by Details.cshtml inline script)
    window.showCartModal = showCartModal;

    // ── CSRF token ───────────────────────────────────────────────────────────
    function getToken() {
        const el = document.querySelector('input[name="__RequestVerificationToken"]');
        return el ? el.value : "";
    }

    // ── Disable out-of-stock buttons on page load ─────────────────────────────
    function disableOutOfStockButtons() {
        document.querySelectorAll(".add-to-cart-btn").forEach(btn => {
            const stock = parseInt(btn.dataset.stock ?? "1");
            if (stock <= 0) {
                btn.disabled = true;
                btn.classList.remove("btn-warning");
                btn.classList.add("btn-secondary");
                btn.style.cursor = "not-allowed";
                btn.style.opacity = "0.65";
                btn.title = "Out of stock";
                btn.innerHTML = "Out of Stock";
            }
        });
    }

    // ── Bind buttons ─────────────────────────────────────────────────────────
    function attachCartButtons() {
        disableOutOfStockButtons();

        document.querySelectorAll(".add-to-cart-btn:not([data-cart-bound]):not([disabled])").forEach(btn => {
            btn.setAttribute("data-cart-bound", "1");
            btn.addEventListener("click", async function () {
                const productId    = this.dataset.productId;
                const productName  = this.dataset.productName || "Product";
                const productPrice = this.dataset.productPrice
                    ? parseFloat(this.dataset.productPrice).toFixed(2) + " RON"
                    : "";
                const productImage = this.dataset.productImage || "";
                const stock        = parseInt(this.dataset.stock ?? "1");

                if (!productId) return;

                // Client-side stock guard (belt-and-suspenders)
                if (stock <= 0) {
                    this.disabled = true;
                    this.classList.remove("btn-warning");
                    this.classList.add("btn-secondary");
                    this.innerHTML = "Out of Stock";
                    return;
                }

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

                    const data = await resp.json();

                    if (data.success) {
                        updateCartCount(data.count ?? 0);
                        showToast(`"${productName}" a fost adăugat în coș! 🛒`, "success");
                        showCartModal(
                            productName,
                            productPrice,
                            productImage
                        );
                    } else {
                        // Server says out of stock — disable button immediately
                        this.disabled = true;
                        this.classList.remove("btn-warning");
                        this.classList.add("btn-secondary");
                        this.style.cursor = "not-allowed";
                        this.style.opacity = "0.65";
                        this.innerHTML = "Out of Stock";
                        // Show a small toast, NO modal
                        showToast(data.message || "Produs indisponibil.", "danger");
                    }
                } catch (e) {
                    showToast("Eroare de conexiune.", "danger");
                }
            });
        });
    }

    document.addEventListener("DOMContentLoaded", () => {
        attachCartButtons();
        fetchAndUpdateCartCount();
    });

    window.attachCartButtons = attachCartButtons;

    // CSS animation
    const style = document.createElement("style");
    style.textContent = `@keyframes fadeInUp { from { opacity:0; transform:translateY(20px); } to { opacity:1; transform:translateY(0); } }`;
    document.head.appendChild(style);
})();
