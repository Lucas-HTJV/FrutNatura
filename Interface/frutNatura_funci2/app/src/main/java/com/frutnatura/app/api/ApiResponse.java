package com.frutnatura.app.api;

import com.google.gson.annotations.SerializedName;

public class ApiResponse<T> {
    @SerializedName("items")
    public T items;

    @SerializedName("totalCount")
    public int totalCount;

    @SerializedName("page")
    public int page;

    @SerializedName("pageSize")
    public int pageSize;
}
