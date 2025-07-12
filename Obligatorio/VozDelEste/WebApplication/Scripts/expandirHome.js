document.addEventListener("DOMContentLoaded", function () {
   const boton = document.getElementById("botonExpandir");
   const programas = document.querySelectorAll(".hidden-programa");
   let expandido = false;

   boton.addEventListener("click", function () {
      expandido = !expandido;
      programas.forEach(p => {
         p.style.display = expandido ? "flex" : "none";
      });
      boton.textContent = expandido ? "Mostrar menos" : "Expandir más";
   });

   // Ocultar los programas ocultos al inicio
   programas.forEach(p => p.style.display = "none");
});
