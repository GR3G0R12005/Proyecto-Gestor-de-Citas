// Base API
const API_BASE = "http://localhost:5146/api/ConfiguracionReserva";

// Elementos DOM
const fechaInput = document.getElementById("fecha");
const turnoSelect = document.getElementById("turno");
const horaInicioInput = document.getElementById("horaInicio");
const horaFinInput = document.getElementById("horaFin");
const duracionInput = document.getElementById("duracionCitas");
const estacionesInput = document.getElementById("cantidadEstaciones");
const mensaje = document.getElementById("mensaje");
const adminContainer = document.getElementById("adminContainer");

// Función para decodificar JWT
function parseJwt(token) {
    try {
        return JSON.parse(atob(token.split('.')[1]));
    } catch (e) {
        return null;
    }
}

// Mostrar contenedor solo si el usuario es admin
document.addEventListener("DOMContentLoaded", () => {
    const token = localStorage.getItem("jwtToken");
    if (!token) {
        alert("Debes iniciar sesión");
        window.location.href = "../HTML/login.html";
        return;
    }

    const payload = parseJwt(token);
    if (!payload || !(payload.id === "1" || payload.admin)) {
        alert("No tienes permisos para acceder a esta sección");
        localStorage.removeItem("jwtToken");
        window.location.href = "../HTML/login.html";
        return;
    }

    adminContainer.style.display = "flex";
});

// Headers con token
function getHeaders() {
    const token = localStorage.getItem("jwtToken");
    return {
        "Content-Type": "application/json",
        "Authorization": `Bearer ${token}`
    };
}

// Convertir fecha a yyyy-MM-dd
function formatFecha(fecha) {
    if (!fecha) return "";
    const d = new Date(fecha);
    const month = String(d.getMonth() + 1).padStart(2, '0');
    const day = String(d.getDate()).padStart(2, '0');
    return `${d.getFullYear()}-${month}-${day}`;
}

// Crear Configuración
async function crearConfiguracion() {
    const data = {
        Fecha: formatFecha(fechaInput.value),
        Turno: turnoSelect.value,
        HoraInicio: horaInicioInput.value,
        HoraFin: horaFinInput.value,
        DuracionCitas: Number(duracionInput.value),
        CantidadEstaciones: Number(estacionesInput.value)
    };

    try {
        const res = await fetch(`${API_BASE}/Crear-configuracion`, {
            method: "POST",
            headers: getHeaders(),
            body: JSON.stringify(data)
        });

        const resultText = await res.text(); // Evita error de JSON

        if (res.ok) {
            mensaje.textContent = resultText;
            mensaje.style.color = "green";
        } else {
            mensaje.textContent = resultText;
            mensaje.style.color = "red";
        }
    } catch (err) {
        mensaje.textContent = "Error al conectar con la API";
        mensaje.style.color = "red";
        console.error(err);
    }
}

// Modificar Configuración
async function modificarConfiguracion() {
    const data = {
        Fecha: formatFecha(fechaInput.value),
        Turno: turnoSelect.value,
        HoraInicio: horaInicioInput.value,
        HoraFin: horaFinInput.value,
        DuracionCitas: Number(duracionInput.value),
        CantidadEstaciones: Number(estacionesInput.value)
    };

    try {
        const res = await fetch(`${API_BASE}/Modificar-configuracion`, {
            method: "PUT",
            headers: getHeaders(),
            body: JSON.stringify(data)
        });

        const resultText = await res.text(); // Evita error de JSON

        if (res.ok) {
            mensaje.textContent = resultText;
            mensaje.style.color = "green";
        } else {
            mensaje.textContent = resultText;
            mensaje.style.color = "red";
        }
    } catch (err) {
        mensaje.textContent = "Error al conectar con la API";
        mensaje.style.color = "red";
        console.error(err);
    }
}

// Obtener Configuración
async function obtenerConfiguracion() {
    const fecha = formatFecha(fechaInput.value);
    const turno = turnoSelect.value;

    try {
        const res = await fetch(`${API_BASE}/Obtener-configuracion?fecha=${fecha}&turno=${turno}`, {
            method: "GET",
            headers: getHeaders()
        });

        const resultText = await res.text(); // Evita error de JSON

        if (res.ok) {
            const config = JSON.parse(resultText); // Parseamos solo si es JSON válido
            horaInicioInput.value = config.horaInicio;
            horaFinInput.value = config.horaFin;
            duracionInput.value = config.duracionCitas;
            estacionesInput.value = config.cantidadEstaciones;

            mensaje.textContent = "Configuración cargada correctamente";
            mensaje.style.color = "green";
        } else {
            mensaje.textContent = resultText;
            mensaje.style.color = "red";
        }
    } catch (err) {
        mensaje.textContent = "Error al conectar con la API";
        mensaje.style.color = "red";
        console.error(err);
    }
}

// Eventos botones
document.getElementById("btnCrear").addEventListener("click", crearConfiguracion);
document.getElementById("btnModificar").addEventListener("click", modificarConfiguracion);
document.getElementById("btnObtener").addEventListener("click", obtenerConfiguracion);
