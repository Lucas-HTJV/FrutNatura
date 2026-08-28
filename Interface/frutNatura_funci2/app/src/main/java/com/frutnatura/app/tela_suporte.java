package com.frutnatura.app;

import android.content.SharedPreferences;
import android.os.Bundle;
import android.util.Log;
import android.widget.Button;
import android.widget.EditText;
import android.widget.Toast;

import androidx.activity.EdgeToEdge;
import androidx.appcompat.app.AppCompatActivity;
import androidx.core.graphics.Insets;
import androidx.core.view.ViewCompat;
import androidx.core.view.WindowInsetsCompat;

import com.frutnatura.app.api.ApiService;
import com.frutnatura.app.api.ChamadoRequest;
import com.frutnatura.app.api.RetrofitClient;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class tela_suporte extends AppCompatActivity {

    // só o que realmente existe no XML
    private EditText campoAssunto, campoDescricao;
    private Button botaoEnviar;

    private int idUsuario; // só p/ log

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        EdgeToEdge.enable(this);
        setContentView(R.layout.activity_tela_suporte);

        ViewCompat.setOnApplyWindowInsetsListener(findViewById(R.id.main), (v, insets) -> {
            Insets systemBars = insets.getInsets(WindowInsetsCompat.Type.systemBars());
            v.setPadding(systemBars.left, systemBars.top, systemBars.right, systemBars.bottom);
            return insets;
        });

        // ID do usuário só para log (backend usa o token)
        SharedPreferences prefs = getSharedPreferences("APP_DATA", MODE_PRIVATE);
        String idUsuarioStr = prefs.getString("usuarioId", null);
        try {
            idUsuario = idUsuarioStr != null ? Integer.parseInt(idUsuarioStr) : -1;
        } catch (NumberFormatException e) {
            idUsuario = -1;
        }
        Log.d("API_DEBUG", "ID carregado do SharedPreferences: " + idUsuario);

        iniciarComponentes();

        botaoEnviar.setOnClickListener(v -> enviarChamado());
    }

    private void iniciarComponentes() {
        campoAssunto  = findViewById(R.id.CAMPO_assunto);
        campoDescricao = findViewById(R.id.CAMPO_descricao);
        botaoEnviar   = findViewById(R.id.button_enviar_chamado);
    }

    private void enviarChamado() {
        // Pega os valores da tela
        String titulo    = campoAssunto.getText().toString().trim();
        String descricao = campoDescricao.getText().toString().trim();
        // por enquanto prioridade fixa; depois você pode colocar um Spinner se quiser
        String prioridade = "Normal";

        if (titulo.isEmpty() || descricao.isEmpty()) {
            Toast.makeText(this, "Preencha título e descrição.", Toast.LENGTH_SHORT).show();
            return;
        }

        ChamadoRequest request = new ChamadoRequest(
                titulo,
                descricao,
                prioridade
        );

        ApiService apiService = RetrofitClient.getApiService(tela_suporte.this);
        Call<Void> call = apiService.criarChamado(request);

        call.enqueue(new Callback<Void>() {
            @Override
            public void onResponse(Call<Void> call, Response<Void> response) {
                if (!response.isSuccessful()) {
                    Toast.makeText(tela_suporte.this,
                            "Erro API: " + response.code(),
                            Toast.LENGTH_SHORT).show();
                    return;
                }

                Toast.makeText(tela_suporte.this,
                        "Chamado enviado com sucesso!",
                        Toast.LENGTH_LONG).show();

                finish(); // volta para a tela de chamados
            }

            @Override
            public void onFailure(Call<Void> call, Throwable t) {
                Toast.makeText(tela_suporte.this,
                        "Falha: " + t.getMessage(),
                        Toast.LENGTH_LONG).show();
            }
        });
    }
}
