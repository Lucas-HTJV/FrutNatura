// ============================
// script.js - Tela de Chamados
// ============================

// 1) API base
const api = (path) => `http://localhost:5000/api${path}`;


// 2) Helpers
const GUID_RX = /^[0-9a-f]{8}-[0-9a-f]{4}-[1-5][0-9a-f]{3}-[89ab][0-9a-f]{3}-[0-9a-f]{12}$/i;
const EMAIL_RX = /^[^\s@]+@[^\s@]+\.[^\s@]+$/i;

function toast(msg, type = "info", ms = 3800) {
    const wrap = document.createElement("div");
    wrap.innerHTML = `
    <div class="alert alert-${type} alert-dismissible fade show position-fixed"
         style="top:16px; right:16px; z-index:1060; min-width:300px" role="alert">
      ${msg}
      <button class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
    </div>`;
    const el = wrap.firstElementChild;
    document.body.appendChild(el);
    setTimeout(() => { try { el.classList.remove("show"); } catch { } }, ms);
    setTimeout(() => { try { el.remove(); } catch { } }, ms + 320);
}

function authHeaders() {
    const token = sessionStorage.getItem("token"); // <- usa o token salvo no login
    const h = { "Content-Type": "application/json" };
    if (token) h["Authorization"] = `Bearer ${token}`;
    return h;
}

async function fetchJson(url, init = {}) {
    const resp = await fetch(url, init);
    const data = await resp.json().catch(() => ({}));
    if (!resp.ok) throw new Error((data && (data.detail || data.title || data.message)) || `Erro HTTP ${resp.status}`);
    return data;
}

// ID do chamado atualmente exibido no modal
let chamadoAtualId = null;
// Timer para autoatualizar mensagens
let mensagensIntervalId = null;

// Para atualizar apenas o conteúdo do chamado atual
async function atualizarChamadoAtual() {
    if (!chamadoAtualId) return;

    const statusResult = document.getElementById("statusResult");
    if (!statusResult) return;

    try {
        const d = await fetchJson(
            api(`/clientes/chamados/${encodeURIComponent(chamadoAtualId)}`),
            { headers: authHeaders() }
        );

        statusResult.innerHTML = `
            ${renderChamadoBox(d.chamado)}
            <h6 class="mt-3">Mensagens</h6>
            ${renderMensagens(d.mensagens)}
        `;
    } catch (err) {
        console.error("Erro ao atualizar chamado atual:", err);
        // aqui não dou toast pra não ficar piscando erro toda vez
    }
}

function iniciarAutoRefreshMensagens(intervalMs = 3000) {
    // sempre zera o anterior
    pararAutoRefreshMensagens();

    if (!chamadoAtualId) return;

    // chama uma vez de imediato
    atualizarChamadoAtual();

    // e depois de X em X segundos
    mensagensIntervalId = setInterval(() => {
        atualizarChamadoAtual();
    }, intervalMs);
}

function pararAutoRefreshMensagens() {
    if (mensagensIntervalId) {
        clearInterval(mensagensIntervalId);
        mensagensIntervalId = null;
    }
}


// 3) Render helpers
function renderChamadoBox(c) {
    if (!c) return `<div class="alert alert-warning">Chamado não encontrado.</div>`;
    return `
  <div class="border rounded p-3 mb-3">
    <div class="d-flex justify-content-between flex-wrap">
      <div>
        <div class="fw-bold"># ${c.id}</div>
        <div class="text-muted small">Cliente: ${c.clienteId}</div>
      </div>
      <span class="badge bg-secondary align-self-start">Status: ${c.status}</span>
    </div>
    <div class="mt-2">
      <div class="fw-semibold">${escapeHtml(c.titulo || "")}</div>
      <div class="text-muted small">Prioridade: ${escapeHtml(String(c.prioridade ?? ""))}</div>
      <p class="mt-2 mb-1">${escapeHtml(c.descricao || "")}</p>
      <div class="text-muted small">Criado: ${formatDate(c.criadoEmUtc)}</div>
      ${c.fechadoEmUtc ? `<div class="text-muted small">Fechado: ${formatDate(c.fechadoEmUtc)}</div>` : ""}
    </div>
  </div>`;
}

function renderMensagens(msgs) {
    if (!msgs || !msgs.length) {
        return `<div class="alert alert-light">Sem mensagens ainda.</div>`;
    }
    return `
  <ul class="list-group">
    ${msgs.map(m => `
      <li class="list-group-item">
        <div class="d-flex justify-content-between">
          <div class="fw-semibold">${m.visivelParaCliente ?? true ? "📣 Público" : "🔒 Interno"}</div>
          <div class="text-muted small">${formatDate(m.criadoEmUtc)}</div>
        </div>
        ${m.autorId ? `<div class="small text-muted">Autor: ${escapeHtml(String(m.autorId))}</div>` : ""}
        <div class="mt-1">${escapeHtml(m.texto || "")}</div>
      </li>
    `).join("")}
  </ul>`;
}

function renderListaChamados(paged) {
    const items = paged?.items || paged?.data || [];
    if (!items.length) return `<div class="alert alert-warning">Nenhum chamado encontrado para este cliente.</div>`;
    return `
    <div class="list-group">
      ${items.map(c => `
        <a class="list-group-item list-group-item-action" href="javascript:void(0)" 
           onclick="window.consultarChamadoPorId('${c.id}')">
          <div class="d-flex w-100 justify-content-between">
            <h6 class="mb-1">#${c.id} — ${escapeHtml(c.titulo || "")}</h6>
            <small class="text-muted">${formatDate(c.criadoEmUtc)}</small>
          </div>
          <small class="text-muted">Status: ${escapeHtml(c.status || "")} • Prioridade: ${escapeHtml(String(c.prioridade ?? ""))}</small>
        </a>
      `).join("")}
    </div>`;
}

function escapeHtml(s) {
    return String(s).replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
}
function formatDate(d) {
    try {
        if (!d) return "-";
        const dt = new Date(d);
        return isNaN(dt.getTime()) ? String(d) : dt.toLocaleString();
    } catch { return String(d || "-"); }
}

// 4) Abertura de chamado
function readPriorityNumber(form) {
    let prioridade = 1; // padrão: Média
    const rBaixa = form.querySelector("#prioBaixa");
    const rMedia = form.querySelector("#prioMedia");
    const rAlta = form.querySelector("#prioAlta");
    const sel = form.querySelector("#priority");

    if (rBaixa || rMedia || rAlta) {
        if (rBaixa?.checked) prioridade = 0;
        else if (rAlta?.checked) prioridade = 2;
        else prioridade = 1;
    } else if (sel) {
        const v = String(sel.value || "").toLowerCase();
        if (v === "0" || v === "baixa") prioridade = 0;
        else if (v === "2" || v === "alta") prioridade = 2;
        else prioridade = 1;
    }
    return prioridade;
}

function resetAberturaForm(form) {
    if (!form) return;
    form.reset();
    form.querySelector("#prioMedia")?.click(); // se existir o radio média
}

document.addEventListener("DOMContentLoaded", () => {
    const ticketModalEl = document.getElementById("ticketModal");
    const ticketModal = ticketModalEl ? new bootstrap.Modal(ticketModalEl) : null;

    document.getElementById("btnAbrirChamado")?.addEventListener("click", (e) => {
        e.preventDefault();
        resetAberturaForm(document.getElementById("ticketForm"));
        ticketModal?.show();
    });

    document.getElementById("menuAbrirChamado")?.addEventListener("click", (e) => {
        e.preventDefault();
        resetAberturaForm(document.getElementById("ticketForm"));
        ticketModal?.show();
    });

    // Submit abrir chamado
    const ticketForm = document.getElementById("ticketForm");
    if (ticketForm) {
        ticketForm.addEventListener("submit", async (e) => {
            e.preventDefault();
            const titulo = (ticketForm.querySelector("#subject")?.value || "").trim();
            const descricao = (ticketForm.querySelector("#description")?.value || "").trim();
            const prioridade = readPriorityNumber(ticketForm); // número 0/1/2

            if (titulo.length < 3) return toast("Informe um título (mín. 3).", "warning");
            if (descricao.length < 10) return toast("Descreva melhor o problema (mín. 10).", "warning");

            try {
                const data = await fetchJson(
                    api('/clientes/chamados'),
                    {
                        method: "POST",
                        headers: authHeaders(),
                        body: JSON.stringify({ titulo, descricao, prioridade })
                    }
                );

                const chamadoId = typeof data === "string"
                    ? data
                    : (data.id || data.chamadoId || "");

                ticketModal?.hide();
                ticketForm.reset();
                toast(`Chamado criado com sucesso! Nº: <b>${chamadoId}</b>`, "success");
            } catch (err) {
                console.error(err);
                toast(String(err.message || "Falha ao abrir chamado."), "danger");
            }
        });
    }

    const statusForm = document.getElementById("statusForm");
    const statusModalEl = document.getElementById("statusModal");
    const statusModal = statusModalEl ? new bootstrap.Modal(statusModalEl) : null;
    const statusResult = document.getElementById("statusResult");

    
    if (statusModalEl) {
        statusModalEl.addEventListener("hidden.bs.modal", () => {
            pararAutoRefreshMensagens();
            chamadoAtualId = null;
        });
    }


    statusForm?.addEventListener("submit", async (e) => {
        e.preventDefault();
        if (!statusResult) return;

        const q = String(document.getElementById("queryInput")?.value || "").trim();
        if (!q) return;

        try {
            if (GUID_RX.test(q)) {
                const d = await fetchJson(
                    api(`/clientes/chamados/${encodeURIComponent(q)}`),
                    { headers: authHeaders() }
                );

                chamadoAtualId = d.chamado?.id || q;

                statusResult.innerHTML = `
                    ${renderChamadoBox(d.chamado)}
                    <h6 class="mt-3">Mensagens</h6>
                    ${renderMensagens(d.mensagens)}
                `;
                statusModal?.show();
                iniciarAutoRefreshMensagens();
                return;
            }

            // aqui você ainda poderia filtrar por e-mail (EMAIL_RX), se quiser
            const lista = await fetchJson(
                api('/clientes/chamados?page=1&pageSize=20'),
                { headers: authHeaders() }
            );
            statusResult.innerHTML = renderListaChamados(lista);
            statusModal?.show();

        } catch (err) {
            console.error(err);
            toast("Erro inesperado ao consultar.", "danger");
        }
    });
});

// Detalhar chamado ao clicar na lista
window.consultarChamadoPorId = async function (id) {
    const statusResult = document.getElementById("statusResult");
    const statusModalEl = document.getElementById("statusModal");
    const statusModal = statusModalEl ? new bootstrap.Modal(statusModalEl) : null;

    if (!statusResult) return;

    try {
        const d = await fetchJson(
            api(`/clientes/chamados/${encodeURIComponent(id)}`),
            { headers: authHeaders() }
        );

        chamadoAtualId = d.chamado?.id || id;

        statusResult.innerHTML = `
            ${renderChamadoBox(d.chamado)}
            <h6 class="mt-3">Mensagens</h6>
            ${renderMensagens(d.mensagens)}
        `;
        statusModal?.show();
        iniciarAutoRefreshMensagens();
    } catch (err) {
        console.error(err);
        toast("Falha ao carregar detalhes do chamado.", "danger");
    }
};

// Enviar mensagem de resposta do cliente
window.enviarMensagem = async function () {
    const input = document.getElementById("replyInput");
    const texto = (input?.value || "").trim();

    if (!chamadoAtualId) {
        toast("Nenhum chamado selecionado.", "warning");
        return;
    }

    if (!texto) {
        toast("Digite uma mensagem antes de enviar.", "warning");
        return;
    }

    try {
        await fetchJson(
            api(`/clientes/chamados/${encodeURIComponent(chamadoAtualId)}/mensagens`),
            {
                method: "POST",
                headers: authHeaders(),
                body: JSON.stringify({
                    texto
                   
                })
            }
        );

        input.value = "";
        toast("Mensagem enviada com sucesso!", "success");

        await window.consultarChamadoPorId(chamadoAtualId);
    } catch (err) {
        console.error(err);
        toast("Erro ao enviar mensagem.", "danger");
    }
};
