const API_BASE = "http://localhost:5146/api/ConfiguracionReserva";

document.addEventListener("DOMContentLoaded", () => {
    const btnCrear = document.getElementById("btnCrear");
    const btnModificar = document.getElementById("btnModificar");
    const btnObtener = document.getElementById("btnObtener");
    const mensaje = document.getElementById("mensaje");

    let configuracionActual = null;

    function obtenerToken() {
        const token = localStorage.getItem("token");
        if (!token) {
            mostrarMensaje(false, "No se encontró token. Haz login primero.");
            return null;
        }
        console.log("Token usado:", token);
        return token;
    }

    btnCrear.addEventListener("click", async (e) => {
        e.preventDefault();
        const token = obtenerToken();
        if (!token) return;

        const config = leerFormulario();
        if (!config) return;

        try {
            const res = await fetch(`${API_BASE}/Crear-configuracion`, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                    "Authorization": `Bearer ${token}`
                },
                body: JSON.stringify(config)
            });

            const text = await res.text();
            console.log("Respuesta Crear:", res.status, text);
            mostrarMensaje(res.ok, text);
            if(res.ok) configuracionActual = config;
        } catch (error) {
            mostrarMensaje(false, "Error al conectar con el servidor: " + error);
        }
    });

    btnModificar.addEventListener("click", async (e) => {
        e.preventDefault();
        const token = obtenerToken();
        if (!token) return;

        if(!configuracionActual){
            mostrarMensaje(false, "Primero debes obtener una configuración.");
            return;
        }

        const config = leerFormulario();
        if (!config) return;

        try {
            const res = await fetch(`${API_BASE}/Modificar-configuracion`, {
                method: "PUT",
                headers: {
                    "Content-Type": "application/json",
                    "Authorization": `Bearer ${token}`
                },
                body: JSON.stringify(config)
            });

            const text = await res.text();
            console.log("Respuesta Modificar:", res.status, text);
            mostrarMensaje(res.ok, text);
            if(res.ok) configuracionActual = config;
        } catch (error) {
            mostrarMensaje(false, "Error al conectar con el servidor: " + error);
        }
    });

    btnObtener.addEventListener("click", async (e) => {
        e.preventDefault();
        const token = obtenerToken();
        if (!token) return;

        const fechaInput = document.getElementById("fecha").value;
        const turnoInput = document.getElementById("turno").value;

        if (!fechaInput || !turnoInput) {
            mostrarMensaje(false, "Ingresa fecha y turno.");
            return;
        }

        try {
            const res = await fetch(`${API_BASE}/Obtener-configuracion?fecha=${fechaInput}&turno=${turnoInput}`, {
                headers: {
                    "Authorization": `Bearer ${token}`,
                    "Content-Type": "application/json"
                }
            });

            const text = await res.text();
            console.log("Respuesta Obtener:", res.status, text);

            if (!res.ok) {
                mostrarMensaje(false, `Error HTTP ${res.status}: ${text}`);
                return;
            }

            let config;
            try {
                config = JSON.parse(text);
            } catch (e) {
                mostrarMensaje(false, `Error al parsear JSON: ${e.message}`);
                return;
            }

            if (!config || Object.keys(config).length === 0) {
                mostrarMensaje(false, "La configuración está vacía o no existe para la fecha/turno indicado.");
                return;
            }

            document.getElementById("fecha").value = config.fecha ? formatDate(config.fecha) : "";
            document.getElementById("turno").value = config.turno || "";
            document.getElementById("horaInicio").value = config.horaInicio ? formatTime(config.horaInicio) : "";
            document.getElementById("horaFin").value = config.horaFin ? formatTime(config.horaFin) : "";
            document.getElementById("duracionCitas").value = config.duracionCitas || "";
            document.getElementById("cantidadEstaciones").value = config.cantidadEstaciones || "";

            configuracionActual = config;
            mostrarMensaje(true, "Configuración cargada correctamente.");
        } catch (error) {
            mostrarMensaje(false, "Error de conexión o fetch: " + error);
        }
    });

    function leerFormulario(){
        const fecha = document.getElementById("fecha").value;
        const turno = document.getElementById("turno").value;
        const horaInicio = document.getElementById("horaInicio").value;
        const horaFin = document.getElementById("horaFin").value;
        const duracionCitas = document.getElementById("duracionCitas").value;
        const cantidadEstaciones = document.getElementById("cantidadEstaciones").value;

        if (!fecha || !turno || !horaInicio || !horaFin || !duracionCitas || !cantidadEstaciones){
            mostrarMensaje(false, "Completa todos los campos.");
            return null;
        }

        return {
            fecha,
            turno,
            horaInicio,
            horaFin,
            duracionCitas: parseInt(duracionCitas),
            cantidadEstaciones: parseInt(cantidadEstaciones)
        };
    }

    function mostrarMensaje(exito, texto){
        mensaje.textContent = texto;
        mensaje.style.color = exito ? "green" : "red";
    }

    function formatDate(fecha){
        if(!fecha) return "";
        const d = new Date(fecha);
        const month = String(d.getMonth()+1).padStart(2,"0");
        const day = String(d.getDate()).padStart(2,"0");
        return `${d.getFullYear()}-${month}-${day}`;
    }

    function formatTime(hora){
        if(!hora) return "";
        if(hora.includes("T")) hora = hora.split("T")[1];
        const [h, m] = hora.split(":");
        return `${h.padStart(2,"0")}:${m.padStart(2,"0")}`;
    }
});
