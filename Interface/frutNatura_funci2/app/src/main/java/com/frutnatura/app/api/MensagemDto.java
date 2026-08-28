package com.frutnatura.app.api;

import com.google.gson.annotations.SerializedName;

public class MensagemDto {
    @SerializedName("id")
    public String id;

    @SerializedName("autorId")
    public String autorId;

    @SerializedName("texto")
    public String texto;

    @SerializedName("criadoEmUtc")
    public String criadoEmUtc;
}
