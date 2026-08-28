package com.frutnatura.app.api;

import com.google.gson.annotations.SerializedName;

public class LoginResponse {
    @SerializedName("success")
    public boolean sucesso;
    @SerializedName("error")
    public String mensagem;
    @SerializedName("accessToken")
    public String token;
    @SerializedName("usuarioId")
    public String usuarioId;
    @SerializedName("name")
    public String nome;
    @SerializedName("role")
    public String role;
}
