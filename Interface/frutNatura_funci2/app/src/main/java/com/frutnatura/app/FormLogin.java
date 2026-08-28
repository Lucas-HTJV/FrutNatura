package com.frutnatura.app;

import android.content.Intent;
import android.content.SharedPreferences;
import android.graphics.Color;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;

import androidx.activity.EdgeToEdge;
import androidx.appcompat.app.AppCompatActivity;
import androidx.core.graphics.Insets;
import androidx.core.view.ViewCompat;
import androidx.core.view.WindowInsetsCompat;

import com.frutnatura.app.api.ApiService;
import com.frutnatura.app.api.LoginRequest;
import com.frutnatura.app.api.LoginResponse;
import com.frutnatura.app.api.RetrofitClient;   // 👈 USANDO O RetrofitClient

import com.google.android.material.snackbar.Snackbar;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class FormLogin extends AppCompatActivity {

    private EditText edit_email, edit_senha;
    private TextView text_tela_cadastro;
    private Button btn_entrar;
    String[] mensagens = {"Preencha todos os campos", "Login realizado com sucesso"};

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        EdgeToEdge.enable(this);
        setContentView(R.layout.activity_form_login);

        ViewCompat.setOnApplyWindowInsetsListener(findViewById(R.id.main), (v, insets) -> {
            Insets systemBars = insets.getInsets(WindowInsetsCompat.Type.systemBars());
            v.setPadding(systemBars.left, systemBars.top, systemBars.right, systemBars.bottom);
            return insets;
        });

        IniciarComponentes();

        text_tela_cadastro.setOnClickListener(v -> {
            Intent intent = new Intent(FormLogin.this, form_cadastro.class);
            startActivity(intent);
        });

        btn_entrar.setOnClickListener(v -> {
            String email = edit_email.getText().toString();
            String senha = edit_senha.getText().toString();

            if (email.isEmpty() || senha.isEmpty()) {
                mostrarMensagem(v, mensagens[0]);
            } else {
                loginViaApi(v, email, senha);
            }
        });
    }

    private void loginViaApi(View v, String email, String senha) {

        // ❗️ANTES: ApiClient.getClient()
        // AGORA: RetrofitClient, que já tem o AuthInterceptor com o token
        ApiService apiService = RetrofitClient.getApiService(FormLogin.this);
        LoginRequest request = new LoginRequest(email, senha);

        apiService.login(request).enqueue(new Callback<LoginResponse>() {
            @Override
            public void onResponse(Call<LoginResponse> call, Response<LoginResponse> response) {

                if (response.isSuccessful() && response.body() != null) {

                    LoginResponse resp = response.body();

                    Log.e("LOGIN_DEBUG", "sucesso: " + resp.sucesso);
                    Log.e("LOGIN_DEBUG", "mensagem: " + resp.mensagem);
                    Log.e("LOGIN_DEBUG", "Token recebido da API: " + resp.token);

                    if (resp.sucesso) {
                        mostrarMensagem(v, mensagens[1]);

                        String idUsuario = resp.usuarioId;
                        String token = resp.token;

                        // 🔥 SALVA ID E TOKEN PARA AS PRÓXIMAS TELAS
                        SharedPreferences prefs = getSharedPreferences("APP_DATA", MODE_PRIVATE);
                        prefs.edit()
                                .putString("token", resp.token)
                                .putString("usuarioId", resp.usuarioId)
                                .apply();

                        Intent intent = new Intent(FormLogin.this, ecommerce.class);
                        startActivity(intent);
                        finish();
                    } else {
                        mostrarMensagem(v, resp.mensagem);
                    }

                } else if (response.code() == 401) {
                    mostrarMensagem(v, "Email ou senha inválidos");
                } else {
                    mostrarMensagem(v, "Erro na API: " + response.code());
                }
            }

            @Override
            public void onFailure(Call<LoginResponse> call, Throwable t) {
                mostrarMensagem(v, "Falha de conexão: " + t.getMessage());
            }
        });
    }

    private void mostrarMensagem(View v, String texto) {
        Snackbar snackbar = Snackbar.make(v, texto, Snackbar.LENGTH_SHORT);
        snackbar.setBackgroundTint(Color.WHITE);
        snackbar.setTextColor(Color.BLACK);
        snackbar.show();
    }

    private void IniciarComponentes() {
        text_tela_cadastro = findViewById(R.id.text_cadastre_se);
        edit_email = findViewById(R.id.edit_email);
        edit_senha = findViewById(R.id.edit_senha);
        btn_entrar = findViewById(R.id.btn_entrar);
    }
}
