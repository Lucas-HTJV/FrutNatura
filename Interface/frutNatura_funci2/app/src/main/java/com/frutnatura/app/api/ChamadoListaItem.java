package com.frutnatura.app.api;

import com.google.gson.annotations.SerializedName;

public class ChamadoListaItem {

    @SerializedName("id")
    public String id;

    @SerializedName("clienteId")
    public String clienteId;

    @SerializedName("titulo")
    public String titulo;

    @SerializedName("descricao")
    public String descricao;

    @SerializedName("status")
    public String status;

    @SerializedName("prioridade")
    public String prioridade;

    @SerializedName("criadoEmUtc")
    public String criadoEmUtc;
}
