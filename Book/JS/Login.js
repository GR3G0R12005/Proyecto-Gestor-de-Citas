async function logearse() {
    const correo = document.getElementById("loginCorreo").value;
    const contraseña = document.getElementById("loginContraseña").value;
    const loginMessage = document.getElementById("loginMessage");

    if (!correo || !contraseña) {
        loginMessage.textContent = "Por favor completa todos los campos.";
        loginMessage.style.color = "red";
        return;
    }

    try {
        const response = await fetch('http://localhost:5146/api/RegistroUsuario/Logearse', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ correo, contraseña })
        });

        const data = await response.json();

        if (!data.success) {
            loginMessage.textContent = data.message || "Error en la solicitud";
            loginMessage.style.color = "red";
            return;
        }

        localStorage.setItem("token", data.token);
        loginMessage.textContent = `Bienvenido ${data.usuario.nombre}`;
        loginMessage.style.color = "green";

        if (data.usuario.rol === false) {
            window.location.href = "IndexUsuario.html"; 
        } else {
            window.location.href = "Configuracion.html"; 
        }

    } catch (error) {
        console.error(error);
        loginMessage.textContent = "Error al conectar con la API";
        loginMessage.style.color = "red";
    }
}
