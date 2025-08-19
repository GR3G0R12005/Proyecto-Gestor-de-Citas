const API_URL = "http://localhost:5146/api/ConfiguracionReserva/Obtener-todas";

const calendarBody = document.getElementById("calendarBody");
const monthYear = document.getElementById("monthYear");
const prevMonthBtn = document.getElementById("prevMonth");
const nextMonthBtn = document.getElementById("nextMonth");

let currentDate = new Date();
let configs = []; // Guardaremos los datos de la API

// ----------------------
// Crear calendario (solo las celdas una vez)
// ----------------------
function createCalendarCells() {
  if (calendarBody.childElementCount > 0) return; // Ya creado

  for (let i = 0; i < 42; i++) { // 6 semanas x 7 días
    const cell = document.createElement("div");
    cell.classList.add("day");

    const dayNumber = document.createElement("div");
    dayNumber.classList.add("day-number");
    cell.appendChild(dayNumber);

    const tagsContainer = document.createElement("div");
    tagsContainer.classList.add("tags-container");
    cell.appendChild(tagsContainer);

    calendarBody.appendChild(cell);
  }
}

// ----------------------
// Renderizar calendario (población de datos)
// ----------------------
function renderCalendar() {
  const year = currentDate.getFullYear();
  const month = currentDate.getMonth();

  const monthNames = [
    "Enero","Febrero","Marzo","Abril","Mayo","Junio",
    "Julio","Agosto","Septiembre","Octubre","Noviembre","Diciembre"
  ];
  monthYear.textContent = `${monthNames[month]} ${year}`;

  const firstDay = new Date(year, month, 1).getDay();
  const startingDay = (firstDay === 0 ? 6 : firstDay - 1); // lunes = 0
  const daysInMonth = new Date(year, month + 1, 0).getDate();
  const daysInPrevMonth = new Date(year, month, 0).getDate();

  const cells = calendarBody.querySelectorAll(".day");

  cells.forEach((cell, i) => {
    const dayNum = cell.querySelector(".day-number");
    const tagsContainer = cell.querySelector(".tags-container");

    tagsContainer.innerHTML = "";
    cell.classList.remove("inactive-day", "today");

    let day;
    let cellMonth = month;
    let cellYear = year;

    // Mes anterior
    if (i < startingDay) {
      day = daysInPrevMonth - (startingDay - i - 1);
      cellMonth = month - 1;
      if (cellMonth < 0) {
        cellMonth = 11;
        cellYear--;
      }
      cell.classList.add("inactive-day");
    }
    // Mes siguiente
    else if (i >= startingDay + daysInMonth) {
      day = i - (startingDay + daysInMonth) + 1;
      cellMonth = month + 1;
      if (cellMonth > 11) {
        cellMonth = 0;
        cellYear++;
      }
      cell.classList.add("inactive-day");
    }
    // Mes actual
    else {
      day = i - startingDay + 1;
    }

    dayNum.textContent = day;

    // Marcar hoy
    const today = new Date();
    if (
      day === today.getDate() &&
      cellMonth === today.getMonth() &&
      cellYear === today.getFullYear()
    ) {
      cell.classList.add("today");
    }

    // Construir fecha YYYY-MM-DD
    const dateStr = `${cellYear}-${String(cellMonth + 1).padStart(2, "0")}-${String(day).padStart(2, "0")}`;

    // Pintar turnos si existen configs
    const dayConfigs = configs.filter(c => {
      const fechaSolo = c.fecha.split('T')[0]; // YYYY-MM-DD
      return fechaSolo === dateStr;
    });

    dayConfigs.forEach(config => {
      const tag = document.createElement("span");
      tag.classList.add("tag");
      if (config.turno.toLowerCase() === "matutino") {
        tag.classList.add("tag-matutino");
        tag.textContent = "Matutino";
      } else if (config.turno.toLowerCase() === "vespertino") {
        tag.classList.add("tag-vespertino");
        tag.textContent = "Vespertino";
      }
      tagsContainer.appendChild(tag);
    });
  });
}

// ----------------------
// Obtener configuraciones desde la API
// ----------------------
async function fetchConfigs() {
  try {
    const response = await fetch(API_URL);
    if (!response.ok) throw new Error(`HTTP ${response.status}`);
    const data = await response.json();
    configs = data; // Guardar globalmente
    console.log("Datos del backend:", configs);
  } catch (error) {
    console.error("No se pudo cargar la API:", error);
    // Puedes mostrar un mensaje de error en la UI si quieres
    configs = []; // Evitar que sea undefined
  } finally {
    renderCalendar(); // Renderizar siempre, aunque falle la API
  }
}

// ----------------------
// Navegar entre meses
// ----------------------
prevMonthBtn.addEventListener("click", () => {
  currentDate.setMonth(currentDate.getMonth() - 1);
  renderCalendar();
});

nextMonthBtn.addEventListener("click", () => {
  currentDate.setMonth(currentDate.getMonth() + 1);
  renderCalendar();
});

// Inicializar
createCalendarCells();
fetchConfigs(); // Solo aquí hacemos el fetch
