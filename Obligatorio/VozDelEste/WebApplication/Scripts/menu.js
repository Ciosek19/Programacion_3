const hamburgerBtn = document.getElementById("hamburgerBtn");
const menuModal = document.getElementById("menuModal");
const closeModal = document.getElementById("closeModal");
const adminToggle = document.getElementById("adminToggle");
const adminSubmenu = document.getElementById("adminSubmenu");

hamburgerBtn.addEventListener("click", () => {
    menuModal.style.display = "block";
    document.body.classList.add("no-scroll");  // <-- Bloquear scroll aquí
});

closeModal.addEventListener("click", () => {
    menuModal.style.display = "none";
    document.body.classList.remove("no-scroll");  // <-- Desbloquear scroll aquí
});

adminToggle.addEventListener("click", () => {
    const isVisible = adminSubmenu.style.display === "block";
    adminSubmenu.style.display = isVisible ? "none" : "block";
    adminToggle.textContent = isVisible ? "Administración ⌄" : "Administración ⌃";
});

// Cierra el modal si se hace clic fuera del contenido
window.addEventListener("click", (e) => {
    if (e.target == menuModal) {
        menuModal.style.display = "none";
        document.body.classList.remove("no-scroll"); // Desbloquear scroll también aquí
    }
});
