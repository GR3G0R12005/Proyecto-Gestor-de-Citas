async function registrarse() {
    const nombre = document.getElementById("registroNombre").value;
    const edad = document.getElementById("registroEdad").value;
    const correo = document.getElementById("registroCorreo").value;
    const contraseña = document.getElementById("registroContraseña").value;
    const dia = document.getElementById("registroDia").value;
    const mes = document.getElementById("registroMes").value;
    const año = document.getElementById("registroAño").value;
    const registroMessage = document.getElementById("registroMessage");
    const telefono = document.getElementById("registroTelefono").value;

    if (!nombre || !edad || !correo || !contraseña || !dia || !mes || !año) {
        registroMessage.textContent = "Por favor completa todos los campos obligatorios.";
        registroMessage.style.color = "red";
        return;
    }

    try {
        const response = await fetch("http://localhost:5146/api/RegistroUsuario/Registrarse", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({
                nombre,
                edad: parseInt(edad),
                telefono,
                correo,
                contraseña,
                dia: parseInt(dia),
                mes: parseInt(mes),
                año: parseInt(año)
            })
        });

        const data = await response.json();

        if (!response.ok || data.success === false) {
            registroMessage.textContent = data.message || "No se pudo registrar. Intenta nuevamente.";
            registroMessage.style.color = "red";
        } else {
            registroMessage.textContent = "Registro exitoso";
            registroMessage.style.color = "green";

            setTimeout(() => {
                window.location.href = "login.html";
            }, 1000);
        }

    } catch (error) {
        console.error("Error en registro:", error);
        registroMessage.textContent = "Error al conectar con la API.";
        registroMessage.style.color = "green";
    }
}
