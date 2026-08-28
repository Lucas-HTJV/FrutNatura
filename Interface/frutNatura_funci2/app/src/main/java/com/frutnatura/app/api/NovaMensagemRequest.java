package com.frutnatura.app.api;

import com.google.gson.annotations.SerializedName;

public class NovaMensagemRequest {
    @SerializedName("texto")
    public String texto;

    public NovaMensagemRequest(String texto) {
        this.texto = texto;
    }}
