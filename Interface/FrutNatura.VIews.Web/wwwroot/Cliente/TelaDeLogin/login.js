document.addEventListener("DOMContentLoaded", () => {

    // ======== CONFIG ========
    const API = "http://localhost:5000";
    // Agora estamos direcionando para a página correta no site principal
    const REDIRECT_URL = `${window.location.origin}/Cliente/TelaPrincipal/Naturefruit.html`; 

    // ======== UTIL ========
    function setMsg(el, text, ok = false) {
        if (!el) return;
        el.textContent = text || "";
        el.style.color = ok ? "#00a86b" : "#ff6b6b";
    }
    async function tryJson(resp) { try { return await resp.json(); } catch { return null; } }

    // ======== REGISTRO ========
    const regForm = document.getElementById("registroForm");
    const regNome = document.getElementById("regNome");
    const regUsuario = document.getElementById("regUsuario");
    const regSenha = document.getElementById("regSenha");
    const regConf = document.getElementById("regConfirmaSenha");
    const regMsg = document.getElementById("registroMensagem");

    regForm?.addEventListener("submit", async (e) => {
        e.preventDefault();
        setMsg(regMsg, "Registrando...");

        const nome = regNome.value.trim();
        const email = regUsuario.value.trim();
        const senha = regSenha.value;
        const confirma = regConf.value;

        if (!nome || !email || !senha) return setMsg(regMsg, "Preencha todos os campos.");
        if (senha !== confirma) return setMsg(regMsg, "As senhas não conferem.");

        try {
            const resp = await fetch(`${API}/api/auth/register`, {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                // seu backend espera 'hashPassword' no register
                body: JSON.stringify({ nome, email, hashPassword: senha })
            });

            if (!resp.ok) {
                const data = await tryJson(resp);
                const msg = data?.error || data?.message || await resp.text();
                return setMsg(regMsg, msg || "Falha no registro");
            }

            setMsg(regMsg, "✅ Registrado com sucesso! Vá para o login.", true);
            regForm.reset();
            setTimeout(() => toggleLogin(), 1200);
        } catch {
            setMsg(regMsg, "❌ Erro de rede. Verifique se a API está em HTTP.");
        }
    });

    // ======== LOGIN ========
    const loginForm = document.getElementById("loginForm");
    const loginUsuario = document.getElementById("loginUsuario");
    const loginSenha = document.getElementById("loginSenha");
    const loginMsg = document.getElementById("loginMensagem");

    loginForm?.addEventListener("submit", async (e) => {
        e.preventDefault();
        setMsg(loginMsg, "Verificando credenciais...");

        const email = loginUsuario.value.trim();
        const password = loginSenha.value;
        if (!email || !password) return setMsg(loginMsg, "Informe e-mail e senha.");

        try {
            const resp = await fetch(`${API}/api/auth/login`, {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                // seu endpoint está aceitando 'hashPassword' no body do login
                body: JSON.stringify({ email, hashPassword: password })
            });

            if (!resp.ok) {
                const data = await tryJson(resp);
                const msg = data?.error || data?.message || await resp.text();
                return setMsg(loginMsg, msg || "Falha no login");
            }

            const data = await resp.json();

            // sua API retorna 'accessToken'
            const token =
                data.accessToken ||
                data.token || data.Token ||
                data.AccessToken || data.jwt || null;

            if (!token) {
                console.warn("Resposta da API sem token:", data);
                return setMsg(loginMsg, "Token não retornado.");
            }

            sessionStorage.setItem("token", token);

            if (data.usuario || data.Usuario) {
                const userInfo = data.usuario || data.Usuario;
                sessionStorage.setItem("usuario", JSON.stringify(userInfo));
            }

            setMsg(loginMsg, "✅ Login realizado! Redirecionando...", true);
            setTimeout(() => window.location.href = REDIRECT_URL, 900); // Garante o redirecionamento para o site principal
        } catch {
            setMsg(loginMsg, "❌ Erro de rede. Verifique se a API está em HTTP.");
        }
    });

    // ======== ALTERNÂNCIA COM ANIMAÇÃO ========
    const container = document.getElementById("container");
    const loginform = document.querySelector(".loginform");
    const registroform = document.querySelector(".registroform");
    const linkRegistro = document.querySelector(".registrolink");
    const linkLogin = document.querySelector(".loginlink");

    function toggleRegistro() {
        container.classList.add("registro-ativo");
        loginform.classList.add("active");
        registroform.classList.add("active");
    }
    function toggleLogin() {
        container.classList.remove("registro-ativo");
        loginform.classList.remove("active");
        registroform.classList.remove("active");
    }

    linkRegistro?.addEventListener("click", (e) => { e.preventDefault(); toggleRegistro(); });
    linkLogin?.addEventListener("click", (e) => { e.preventDefault(); toggleLogin(); });
});
