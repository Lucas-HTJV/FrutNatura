package com.frutnatura.app;

import android.content.Intent;
import android.graphics.Color;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;

import androidx.activity.EdgeToEdge;
import androidx.appcompat.app.AppCompatActivity;
import androidx.core.graphics.Insets;
import androidx.core.view.ViewCompat;
import androidx.core.view.WindowInsetsCompat;

import com.frutnatura.app.api.ApiService;
import com.frutnatura.app.api.CadastroRequest;
import com.frutnatura.app.api.CadastroResponse;
import com.frutnatura.app.api.RetrofitClient;   // 👈 USA O MESMO CLIENTE DO LOGIN/CHAMADO

import com.google.android.material.snackbar.Snackbar;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class form_cadastro extends AppCompatActivity {

    private EditText edit_nomecadastro, edit_emailcadastro, edit_senhacadastro;
    private Button btn_cadastrar;
    String[] mensagens = {"Preencha todos os campos", "Cadastro realizado com sucesso!"};

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        EdgeToEdge.enable(this);
        setContentView(R.layout.activity_form_cadastro);

        ViewCompat.setOnApplyWindowInsetsListener(findViewById(R.id.main), (v, insets) -> {
            Insets systemBars = insets.getInsets(WindowInsetsCompat.Type.systemBars());
            v.setPadding(systemBars.left, systemBars.top, systemBars.right, systemBars.bottom);
            return insets;
        });

        IniciarComponentes();

        btn_cadastrar.setOnClickListener(v -> {
            String nome = edit_nomecadastro.getText().toString().trim();
            String email = edit_emailcadastro.getText().toString().trim();
            String senha = edit_senhacadastro.getText().toString().trim();

            if (nome.isEmpty() || email.isEmpty() || senha.isEmpty()) {
                mostrarSnackbar(v, mensagens[0]);
            } else {
                cadastroViaApi(v, nome, email, senha);
            }
        });
    }

    private void cadastroViaApi(View v, String nome, String email, String senha) {

        ApiService apiService = RetrofitClient.getApiService(form_cadastro.this);
        CadastroRequest request = new CadastroRequest(nome, email, senha);

        apiService.cadastro(request).enqueue(new Callback<CadastroResponse>() {
            @Override
            public void onResponse(Call<CadastroResponse> call, Response<CadastroResponse> response) {

                // ✅ SE A RESPOSTA VOLTOU COM SUCESSO (2xx)
                if (response.isSuccessful()) {

                    CadastroResponse resp = response.body();

                    // Se a API retornar um objeto com sucesso = true
                    if (resp != null && resp.sucesso) {

                        // Mensagem de sucesso
                        mostrarSnackbar(v, "Cadastro realizado com sucesso!");

                        // Limpa os campos
                        edit_nomecadastro.setText("");
                        edit_emailcadastro.setText("");
                        edit_senhacadastro.setText("");

                        // Aguarda um pouquinho para o usuário ver a mensagem
                        new android.os.Handler().postDelayed(() -> {
                            Intent intent = new Intent(form_cadastro.this, FormLogin.class);
                            startActivity(intent);
                            finish(); // fecha a tela de cadastro
                        }, 1500);

                    } else if (resp != null && resp.getMensagem() != null && !resp.getMensagem().isEmpty()) {
                        // Caso a API responda com erro conhecido
                        mostrarSnackbar(v, resp.getMensagem());
                    } else {
                        // Caso raro: resposta 2xx mas sem body ou sem campos
                        mostrarSnackbar(v, "Cadastro concluído, mas a resposta da API veio vazia.");
                    }

                } else {
                    // ❌ STATUS 4xx / 5xx
                    mostrarSnackbar(v, "Erro na API: " + response.code());
                }
            }

            @Override
            public void onFailure(Call<CadastroResponse> call, Throwable t) {
                mostrarSnackbar(v, "Falha: " + t.getMessage());
            }
        });
    }

    private void mostrarSnackbar(View v, String texto) {
        Snackbar snackbar = Snackbar.make(v, texto, Snackbar.LENGTH_LONG);
        snackbar.setBackgroundTint(Color.WHITE);
        snackbar.setTextColor(Color.BLACK);
        snackbar.show();
    }

    private void IniciarComponentes() {
        edit_nomecadastro = findViewById(R.id.edit_nomecadastro);
        edit_emailcadastro = findViewById(R.id.edit_emailcadastro);
        edit_senhacadastro = findViewById(R.id.edit_senhacadastro);
        btn_cadastrar = findViewById(R.id.btn_cadastrar);
    }
}
