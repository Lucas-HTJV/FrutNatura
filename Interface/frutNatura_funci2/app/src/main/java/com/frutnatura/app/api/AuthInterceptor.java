package com.frutnatura.app.api;

import android.content.Context;
import android.content.SharedPreferences;
import android.util.Log;

import java.io.IOException;

import okhttp3.Interceptor;
import okhttp3.Request;
import okhttp3.Response;

public class AuthInterceptor implements Interceptor {

    private final Context context;

    public AuthInterceptor(Context context) {
        this.context = context;
    }

    @Override
    public Response intercept(Chain chain) throws IOException {

        SharedPreferences prefs =
                context.getSharedPreferences("APP_DATA", Context.MODE_PRIVATE);

        String token = prefs.getString("token", null);

        // Log para depuração
        Log.e("TOKEN_DEBUG", "Token enviado pelo interceptor: " + token);

        Request original = chain.request();
        Request.Builder builder = original.newBuilder()
                .header("Content-Type", "application/json");

        if (token != null && !token.isEmpty()) {
            builder.header("Authorization", "Bearer " + token);
        }

        Request request = builder.build();
        return chain.proceed(request);
    }
}
