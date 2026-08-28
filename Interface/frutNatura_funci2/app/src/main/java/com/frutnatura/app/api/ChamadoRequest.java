
package com.frutnatura.app.api;

import com.google.gson.annotations.SerializedName;

public class ChamadoRequest {

    @SerializedName("titulo")
    public String titulo;

    @SerializedName("descricao")
    public String descricao;

    @SerializedName("prioridade")
    public String prioridade;

    public ChamadoRequest(String titulo, String descricao, String prioridade) {
        this.titulo = titulo;
        this.descricao = descricao;
        this.prioridade = prioridade;
    }

}

