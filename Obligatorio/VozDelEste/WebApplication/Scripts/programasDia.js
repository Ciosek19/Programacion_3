document.addEventListener('DOMContentLoaded', function () {
    const botones = document.querySelectorAll('.link-dia');
    const programas = document.querySelectorAll('.programacion-item');

    // Mapeo del orden de los días para obtener el índice (0 = Domingo, etc.)
    const diasSemana = ['Domingo', 'Lunes', 'Martes', 'Miercoles', 'Jueves', 'Viernes', 'Sabado'];

    botones.forEach((boton, index) => {
        boton.addEventListener('click', function (e) {
            e.preventDefault();

            // Oculta todos los programas
            programas.forEach(p => p.style.display = 'none');

            // Muestra los programas correspondientes al día seleccionado
            programas.forEach(p => {
                if (p.dataset.dia == index.toString()) {
                    p.style.display = 'block';
                }
            });

            // Resalta el botón activo (opcional)
            botones.forEach(b => b.classList.remove('activo'));
            boton.classList.add('activo');
        });
    });

    // Mostrar los programas del día actual al cargar
    const hoy = new Date().getDay();
    programas.forEach(p => {
        if (p.dataset.dia == hoy.toString()) {
            p.style.display = 'block';
        } else {
            p.style.display = 'none';
        }
    });
});
