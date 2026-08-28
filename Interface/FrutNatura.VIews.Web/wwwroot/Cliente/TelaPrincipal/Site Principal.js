// ===================== CARRINHO =====================
const carrinho = [];
const ul = document.getElementById('itens-carrinho');
const totalSpan = document.getElementById('total');
const contador = document.getElementById('contador');
const modal = document.getElementById('modal');
const telefone = '5511956194625';

function atualizarCarrinho() {
    ul.innerHTML = '';
    let total = 0;
    carrinho.forEach(item => {
        ul.innerHTML += `
      <li>
        <img src="${item.img}" class="item-img" alt="${item.nome}">
        ${item.nome}
        <div style="display:flex;align-items:center;margin-left:5px;">
          <button class="btn-quantidade remove" onclick="removerCarrinho('${item.nome}')">-</button>
          <span class="quantidade-text">${item.quantidade}</span>
          <button class="btn-quantidade add" onclick="adicionarCarrinho('${item.nome}',${item.preco},'${item.img}')">+</button>
        </div>
        <span>R$ ${(item.preco * item.quantidade).toFixed(2)}</span>
      </li>`;
        total += item.preco * item.quantidade;
    });
    totalSpan.textContent = total.toFixed(2);
    contador.textContent = carrinho.reduce((sum, item) => sum + item.quantidade, 0);
}

function adicionarCarrinho(nome, preco, img) {
    const produto = carrinho.find(p => p.nome === nome);
    if (produto) { produto.quantidade++; }
    else { carrinho.push({ nome, preco, img, quantidade: 1 }); }
    atualizarCarrinho();
}

function removerCarrinho(nome) {
    const produto = carrinho.find(p => p.nome === nome);
    if (produto) {
        produto.quantidade--;
        if (produto.quantidade === 0) {
            const index = carrinho.findIndex(p => p.nome === nome);
            carrinho.splice(index, 1);
        }
        atualizarCarrinho();
    }
}

function abrirCarrinho() { modal.style.display = 'flex'; }
function fecharCarrinho() { modal.style.display = 'none'; }

// ===================== PAGAMENTO =====================
function mostrarPagamento(tipo) {
    document.querySelectorAll('.pagamento-form').forEach(div => div.style.display = 'none');
    document.getElementById(tipo).style.display = 'block';
}

function finalizarCompra(tipo) {
    if (carrinho.length === 0) { alert('Seu carrinho está vazio!'); return; }

    let mensagem = ` 💳 Olá! Gostaria de finalizar meu pedido no FrutNatura:\n\n`;
    carrinho.forEach(item => {
        mensagem += ` ${item.nome} x ${item.quantidade} → R$ ${(item.preco * item.quantidade).toFixed(2)}\n`;
    });

    mensagem += `\n Total: R$ ${parseFloat(totalSpan.textContent).toFixed(2)}\n`;

    if (tipo === 'cartao') {
        const nome = document.getElementById('nomeCartao').value;
        const numero = document.getElementById('numeroCartao').value;
        const validade = document.getElementById('validadeCartao').value;
        const cvv = document.getElementById('cvvCartao').value;
        if (!nome || !numero || !validade || !cvv) { alert('Preencha todos os campos do cartão.'); return; }
        mensagem += ` Pagamento: Cartão\nNome: ${nome}\nNúmero: ${numero}\nValidade: ${validade}\nCVV: ${cvv}`;
    } else if (tipo === 'pix') {
        mensagem += ` Pagamento: PIX`;
    } else if (tipo === 'dinheiro') {
        mensagem += `Pagamento: Dinheiro na entrega`;
    }

    mensagem += `\n\n Obrigado pelo pedido! Em breve entregaremos seus produtos fresquinhos!`;

    const url = `https://wa.me/${telefone}?text=${encodeURIComponent(mensagem)}`;
    window.open(url, '_blank');
    fecharCarrinho();
}

// ===================== MENU USUÁRIO =====================
const userIcon = document.getElementById('userIcon');
const dropdownMenu = document.getElementById('dropdownMenu');
userIcon.addEventListener('click', () => {
    dropdownMenu.style.display = dropdownMenu.style.display === 'block' ? 'none' : 'block';
});
document.addEventListener('click', (e) => {
    if (!userIcon.contains(e.target) && !dropdownMenu.contains(e.target)) {
        dropdownMenu.style.display = 'none';
    }
});

// ===================== CARROSSEL =====================
const slide = document.getElementById('carrosselSlide');
const indicadoresContainer = document.getElementById('indicadores');
const slides = document.querySelectorAll('.carrossel-item');
let index = 0;

slides.forEach((_, i) => {
    const dot = document.createElement('div');
    dot.classList.add('indicador');
    if (i === 0) dot.classList.add('active');
    dot.addEventListener('click', () => { index = i; atualizarSlide(); });
    indicadoresContainer.appendChild(dot);
});

function atualizarSlide() {
    slide.style.transform = `translateX(-${index * 100}%)`;
    document.querySelectorAll('.indicador').forEach((dot, i) => {
        dot.classList.toggle('active', i === index);
    });
}

function mudarSlide(n) {
    index += n;
    if (index < 0) index = slides.length - 1;
    if (index >= slides.length) index = 0;
    atualizarSlide();
}
setInterval(() => { mudarSlide(1); }, 5000);

// ===================== FAQ & DEPOIMENTOS =====================
document.querySelectorAll('.faq-question').forEach(button => {
    button.addEventListener('click', () => button.parentElement.classList.toggle('active'));
});
const form = document.getElementById('form-depoimento');
const lista = document.getElementById('depoimentos-lista');
if (form && lista) {
    form.addEventListener('submit', (e) => {
        e.preventDefault();
        const nome = document.getElementById('nome').value;
        const mensagem = document.getElementById('mensagem').value;
        const novo = document.createElement('div');
        novo.classList.add('depoimento');
        novo.innerHTML = `<strong>${nome}</strong><p>${mensagem}</p>`;
        lista.prepend(novo);
        form.reset();
    });
}

// ===================== CHAT =====================
const chatToggle = document.getElementById("chat-toggle");
const chatWindow = document.getElementById("chat-window");
const chatClose = document.getElementById("chat-close");
const chatSend = document.getElementById("chat-send");
const chatInput = document.getElementById("chat-input");
const chatBody = document.getElementById("chat-body");

const CHAT_API_URL = "http://localhost:5000/api/chat";


if (chatToggle && chatWindow && chatClose) {
    chatToggle.onclick = () => chatWindow.classList.toggle("hidden");
    chatClose.onclick = () => chatWindow.classList.add("hidden");
}

function addMessage(text, sender = "bot") {
    const msg = document.createElement("div");
    msg.classList.add("message", sender);
    msg.textContent = text;
    chatBody.appendChild(msg);
    chatBody.scrollTop = chatBody.scrollHeight;
}

async function sendMessage() {
    const text = chatInput.value.trim();
    if (!text) return;

    addMessage(text, "user");
    chatInput.value = "";

    const loadingMsg = document.createElement("div");
    loadingMsg.classList.add("message", "bot");
    loadingMsg.textContent = "Digitando...";
    chatBody.appendChild(loadingMsg);
    chatBody.scrollTop = chatBody.scrollHeight;

    try {
        const response = await fetch(CHAT_API_URL, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ message: text })
        });
        const data = await response.json().catch(() => ({}));
        loadingMsg.remove();
        addMessage(data.reply || "Não obtive resposta da IA 🤔", "bot");
    } catch (error) {
        console.error(error);
        loadingMsg.remove();
        addMessage("Erro ao conectar com o suporte 😢", "bot");
    }
}

if (chatSend && chatInput) {
    chatSend.onclick = sendMessage;
    chatInput.addEventListener("keypress", e => {
        if (e.key === "Enter") sendMessage();
    });
}
// ===================== LOGOUT =====================
const logoutLink = document.querySelector('a[href="/Cliente/TelaDeLogin/Login.html"]');

logoutLink?.addEventListener('click', (e) => {
    e.preventDefault();

    sessionStorage.removeItem("token");
    sessionStorage.removeItem("usuario");

    window.location.href = "/Cliente/TelaDeLogin/Login.html";
});

// ===================== SUPORTE =====================
const suporteLink = document.querySelector('a[href="/Cliente/TelaDeChamado/Criar chamado.html"]');

suporteLink?.addEventListener('click', (e) => {
    e.preventDefault();

    
    window.location.href = "/Cliente/TelaDeChamado/Criar chamado.html";
});
