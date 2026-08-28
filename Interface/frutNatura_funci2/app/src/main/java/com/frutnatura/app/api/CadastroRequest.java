package com.frutnatura.app.api;

import com.google.gson.annotations.SerializedName;

public class CadastroRequest {
    @SerializedName("nome")
    public String nome;

    @SerializedName("email")
    public String email;

    @SerializedName("hashPassword")
    public String senha;

    public CadastroRequest(String nome, String email, String senha) {
        this.nome = nome;
        this.email = email;
        this.senha = senha;
    }
}

