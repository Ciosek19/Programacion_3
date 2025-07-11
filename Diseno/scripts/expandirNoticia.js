document.addEventListener("DOMContentLoaded", function () {
  const botonesExpandir = document.querySelectorAll(".noticia-btnExpandir");

  botonesExpandir.forEach((boton) => {
    boton.addEventListener("click", function () {
      const descripcion = this.previousElementSibling;

      if (descripcion.classList.contains("expandida")) {
        descripcion.classList.remove("expandida");
        this.textContent = "Expandir";
      } else {
        descripcion.classList.add("expandida");
        this.textContent = "Reducir";
      }
    });
  });
});
