package com.frutnatura.app;

import android.content.Intent;
import android.os.Bundle;
import android.os.Handler;
import android.util.Log;
import android.view.View;
import android.widget.LinearLayout;
import android.widget.ProgressBar;
import android.widget.TextView;
import android.widget.Toast;

import androidx.appcompat.app.AppCompatActivity;

import com.frutnatura.app.api.ApiResponse;
import com.frutnatura.app.api.ChamadoListaItem;
import com.frutnatura.app.api.ApiService;
import com.frutnatura.app.api.RetrofitClient;

import java.util.List;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class inicio_chamado extends AppCompatActivity {

    private LinearLayout listaChamadosLayout;
    private ProgressBar progressBar;
    private ApiService apiService;
    private TextView txtNovoChamado;

    private Handler handler = new Handler();
    private Runnable refreshTask;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_inicio_chamado);

        txtNovoChamado = findViewById(R.id.NovoChamado);
        txtNovoChamado.setOnClickListener(v -> {
            Intent intent = new Intent(inicio_chamado.this, tela_suporte.class);
            startActivity(intent);
        });

        listaChamadosLayout = findViewById(R.id.lista_chamados_layout);
        progressBar = findViewById(R.id.progress_bar_chamados);

        apiService = RetrofitClient.getApiService(this);

        carregarChamadosRecentes();

        // 🔄 Atualização automática a cada 3s
        refreshTask = new Runnable() {
            @Override
            public void run() {
                carregarChamadosRecentes();
                handler.postDelayed(this, 3000);
            }
        };
    }

    @Override
    protected void onResume() {
        super.onResume();
        handler.postDelayed(refreshTask, 3000);
    }

    @Override
    protected void onPause() {
        super.onPause();
        handler.removeCallbacks(refreshTask);
    }


    private void carregarChamadosRecentes() {
        mostrarCarregando(true);

        Call<ApiResponse<List<ChamadoListaItem>>> call =
                apiService.getMeusChamados(1, 20);

        call.enqueue(new Callback<ApiResponse<List<ChamadoListaItem>>>() {
            @Override
            public void onResponse(Call<ApiResponse<List<ChamadoListaItem>>> call,
                                   Response<ApiResponse<List<ChamadoListaItem>>> response) {

                mostrarCarregando(false);

                if (!response.isSuccessful()) {
                    Toast.makeText(inicio_chamado.this,
                            "Erro da API: " + response.code(), Toast.LENGTH_SHORT).show();
                    return;
                }

                ApiResponse<List<ChamadoListaItem>> apiResp = response.body();
                if (apiResp == null || apiResp.items == null || apiResp.items.isEmpty()) {
                    adicionarMensagem("Nenhum chamado recente encontrado.");
                    return;
                }

                exibirChamados(apiResp.items);
            }

            @Override
            public void onFailure(Call<ApiResponse<List<ChamadoListaItem>>> call, Throwable t) {
                mostrarCarregando(false);
                Toast.makeText(inicio_chamado.this,
                        "Falha ao carregar chamados: " + t.getMessage(),
                        Toast.LENGTH_LONG).show();
            }
        });
    }


    private void exibirChamados(List<ChamadoListaItem> lista) {
        listaChamadosLayout.removeAllViews();

        for (ChamadoListaItem item : lista) {

            String texto =
                    "Título: " + item.titulo + "\n" +
                            "Status: " + item.status + "\n" +
                            "Prioridade: " + item.prioridade + "\n" +
                            "Criado em: " + item.criadoEmUtc + "\n";

            TextView tv = new TextView(this);
            tv.setText(texto);
            tv.setTextSize(15);
            tv.setTextColor(android.graphics.Color.parseColor("#222222"));
            tv.setBackgroundResource(R.drawable.bg_chamado_card);

            int pad = (int) (16 * getResources().getDisplayMetrics().density);
            tv.setPadding(pad, pad, pad, pad);

            LinearLayout.LayoutParams params = new LinearLayout.LayoutParams(
                    LinearLayout.LayoutParams.MATCH_PARENT,
                    LinearLayout.LayoutParams.WRAP_CONTENT
            );
            params.setMargins(0, 0, 0, pad);
            tv.setLayoutParams(params);

            // 👉 CLICAR NO CHAMADO
            tv.setOnClickListener(v -> abrirChatChamado(item));

            listaChamadosLayout.addView(tv);
        }
    }


    private void abrirChatChamado(ChamadoListaItem item) {
        Intent i = new Intent(inicio_chamado.this, chat_suport.class);
        i.putExtra("chamadoId", String.valueOf(item.id));
        startActivity(i);
    }


    private void adicionarMensagem(String msg) {
        listaChamadosLayout.removeAllViews();

        TextView tv = new TextView(this);
        tv.setText(msg);
        tv.setTextSize(16);
        tv.setPadding(32, 32, 32, 32);

        listaChamadosLayout.addView(tv);
    }

    private void mostrarCarregando(boolean carregando) {
        if (progressBar != null) {
            progressBar.setVisibility(carregando ? View.VISIBLE : View.GONE);
        }
    }
}
