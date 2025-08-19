const API_URL = "http://localhost:5146/api/ConfiguracionReserva/Obtener-todas";

const calendarBody = document.getElementById("calendarBody");
const monthYear = document.getElementById("monthYear");
const prevMonthBtn = document.getElementById("prevMonth");
const nextMonthBtn = document.getElementById("nextMonth");

let currentDate = new Date();

async function renderCalendar() {
  const year = currentDate.getFullYear();
  const month = currentDate.getMonth();

  const monthNames = [
    "Enero","Febrero","Marzo","Abril","Mayo","Junio",
    "Julio","Agosto","Septiembre","Octubre","Noviembre","Diciembre"
  ];
  monthYear.textContent = `${monthNames[month]} ${year}`;

  const firstDay = new Date(year, month, 1).getDay();
  const startingDay = (firstDay === 0 ? 6 : firstDay - 1); 
  const daysInMonth = new Date(year, month + 1, 0).getDate();
  const daysInPrevMonth = new Date(year, month, 0).getDate();

  const configs = await fetchConfigs();

  if (calendarBody.childElementCount === 0) {
    for (let i = 0; i < 42; i++) {
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

  const cells = calendarBody.querySelectorAll(".day");
  cells.forEach((cell, i) => {
    const dayNum = cell.querySelector(".day-number");
    const tagsContainer = cell.querySelector(".tags-container");

    tagsContainer.innerHTML = "";
    cell.classList.remove("inactive-day", "today");

    let day;
    let cellMonth = month;
    let cellYear = year;

    if (i < startingDay) {
      day = daysInPrevMonth - (startingDay - i - 1);
      cellMonth = month - 1;
      if (cellMonth < 0) {
        cellMonth = 11;
        cellYear--;
      }
      cell.classList.add("inactive-day");
    }

    else if (i >= startingDay + daysInMonth) {
      day = i - (startingDay + daysInMonth) + 1;
      cellMonth = month + 1;
      if (cellMonth > 11) {
        cellMonth = 0;
        cellYear++;
      }
      cell.classList.add("inactive-day");
    }

    else {
      day = i - startingDay + 1;
    }

    dayNum.textContent = day;

    const dateStr = `${cellYear}-${String(cellMonth + 1).padStart(2, "0")}-${String(day).padStart(2, "0")}`;

    const today = new Date();
    if (
      day === today.getDate() &&
      cellMonth === today.getMonth() &&
      cellYear === today.getFullYear()
    ) {
      cell.classList.add("today");
    }

    if (configs.length === 0) return; 

    const dayConfigs = configs.filter(c => {
      const fechaSolo = c.fecha.split('T')[0];
      return fechaSolo === dateStr;
    });

    dayConfigs.forEach(config => {
      const tag = document.createElement("a");
      tag.classList.add("tag");

      if (config.turno.toLowerCase() === "matutino") {
        tag.classList.add("tag-matutino");
        tag.textContent = "Matutino";
      } else if (config.turno.toLowerCase() === "vespertino") {
        tag.classList.add("tag-vespertino");
        tag.textContent = "Vespertino";
      }

      // 🔹 Redirigir con parámetros dinámicos
      tag.href = `reservar.html?fecha=${config.fecha.split("T")[0]}&turno=${config.turno}`;

      tagsContainer.appendChild(tag);
    });
  });
}

async function fetchConfigs() {
  try {
    const response = await fetch(API_URL);

    if (!response.ok) {
      console.error("No se pudo obtener la API, status:", response.status);
      mostrarErrorEnCalendario("No se pudo cargar la agenda desde la API");
      return [];
    }

    const data = await response.json();
    console.log("Datos del backend:", data);

    return data.map(item => ({
      fecha: item.fecha,
      turno: item.turno,
      horaInicio: item.horaInicio,
      horaFin: item.horaFin
    }));
  } catch (error) {
    console.error("Error en fetchConfigs:", error);
    mostrarErrorEnCalendario("No se pudo conectar con la API");
    return [];
  }
}

function mostrarErrorEnCalendario(mensaje) {
  calendarBody.innerHTML = `<div class="error-fetch">${mensaje}</div>`;
}

prevMonthBtn.addEventListener("click", () => {
  currentDate.setMonth(currentDate.getMonth() - 1);
  renderCalendar();
});

nextMonthBtn.addEventListener("click", () => {
  currentDate.setMonth(currentDate.getMonth() + 1);
  renderCalendar();
});

renderCalendar();
