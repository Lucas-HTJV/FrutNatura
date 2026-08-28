package com.frutnatura.app.api;

import com.google.gson.annotations.SerializedName;

public class ChamadoResponse {
    @SerializedName("sucesso")
    public boolean sucesso;
    @SerializedName("mensagem")
    public String mensagem;
    @SerializedName("chamado")
    public Chamado chamado;
    public static class Chamado {
        @SerializedName("idChamado")
        public int idChamado;
        @SerializedName("idUsuario")
        public int idUsuario;
        @SerializedName("assunto")
        public String assunto;
        @SerializedName("descricao")
        public String descricao;
        @SerializedName("status")
        public String status;
        @SerializedName("dataCriacao")
        public String dataCriacao;
        @SerializedName("categoria")
        public String categoria;
        @SerializedName("urgencia")
        public String urgencia;
        @SerializedName("anexo")
        public String anexo;
    }
}
