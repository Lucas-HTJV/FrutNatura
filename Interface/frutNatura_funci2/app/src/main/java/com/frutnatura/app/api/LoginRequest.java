package com.frutnatura.app.api;

import com.google.gson.annotations.SerializedName;

public class LoginRequest {

    @SerializedName("email")
    public String email;

    @SerializedName("hashPassword")
    public String senha;

    public LoginRequest(String email, String senha) {
        this.email = email;
        this.senha = senha;
    }
}
