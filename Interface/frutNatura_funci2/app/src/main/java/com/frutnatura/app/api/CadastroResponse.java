package com.frutnatura.app.api;

import com.google.gson.annotations.SerializedName;

public class CadastroResponse {

    @SerializedName("sucesso")
    public boolean sucesso;

    @SerializedName("mensagem")
    public String mensagem;

    public boolean isSucesso() {
        return sucesso;
    }

    public String getMensagem() {
        return mensagem != null ? mensagem : "";
    }
}
