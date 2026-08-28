const themeSelect = document.getElementById("theme");
const lightBtn = document.getElementById("lightBtn");
const darkBtn = document.getElementById("darkBtn");
const body = document.body;
const paymentInput = document.getElementById("paymentInput");
const paymentList = document.getElementById("paymentList");
let payments = [];

// Troca de tema via select (compatibilidade)
themeSelect.addEventListener("change", () => {
  if (themeSelect.value === "dark") {
    body.classList.add("dark-mode");
  } else {
    body.classList.remove("dark-mode");
  }
});

// Troca de tema via botões visuais
lightBtn.addEventListener("click", () => {
  themeSelect.value = "light";
  themeSelect.dispatchEvent(new Event('change'));
});

darkBtn.addEventListener("click", () => {
  themeSelect.value = "dark";
  themeSelect.dispatchEvent(new Event('change'));
});

// Adicionar forma de pagamento
document.querySelector(".add-payment").addEventListener("click", () => {
  const value = paymentInput.value.trim();
  if (value && !payments.includes(value)) {
    payments.push(value);
    renderPayments();
    paymentInput.value = "";
  }
});

// Remover forma de pagamento
function removePayment(method) {
  payments = payments.filter(p => p !== method);
  renderPayments();
}

// Renderizar lista de pagamentos
function renderPayments() {
  paymentList.innerHTML = "";
  payments.forEach(p => {
    const li = document.createElement("li");
    li.textContent = p;
    const btn = document.createElement("button");
    btn.textContent = "Remover";
    btn.onclick = () => removePayment(p);
    li.appendChild(btn);
    paymentList.appendChild(li);
  });
}

// Salvar configurações no localStorage
function saveSettings() {
  const settings = {
    theme: themeSelect.value,
    language: document.getElementById("language").value,
    currency: document.getElementById("currency").value,
    unit: document.getElementById("unit").value,
    showCents: document.getElementById("showCents").checked,
    stockAlerts: document.getElementById("stockAlerts").checked,
    payments: payments
  };

    sessionStorage.setItem("hortfruitSettings", JSON.stringify(settings));
  alert("✅ Configurações salvas!");
}

// Carregar configurações salvas
window.onload = () => {
    const saved = sessionStorage.getItem("hortfruitSettings");
  if (saved) {
    const settings = JSON.parse(saved);

    themeSelect.value = settings.theme;
    document.getElementById("language").value = settings.language;
    document.getElementById("currency").value = settings.currency;
    document.getElementById("unit").value = settings.unit;
    document.getElementById("showCents").checked = settings.showCents;
    document.getElementById("stockAlerts").checked = settings.stockAlerts;
    payments = settings.payments || [];
    renderPayments();

    if (settings.theme === "dark") {
      body.classList.add("dark-mode");
    }
  }
};
