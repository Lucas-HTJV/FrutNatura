package com.frutnatura.app;

import android.os.Bundle;
import android.os.Handler;
import android.os.Looper;
import android.text.method.ScrollingMovementMethod;
import android.view.KeyEvent;
import android.view.View;
import android.graphics.Color;
import android.view.inputmethod.EditorInfo;
import android.widget.EditText;
import android.widget.LinearLayout;
import android.widget.ScrollView;
import android.widget.TextView;
import android.widget.Toast;
import android.view.Gravity;


import androidx.activity.EdgeToEdge;
import androidx.appcompat.app.AppCompatActivity;
import androidx.core.graphics.Insets;
import androidx.core.view.ViewCompat;
import androidx.core.view.WindowInsetsCompat;

import com.frutnatura.app.api.ApiService;
import com.frutnatura.app.api.MensagemDto;
import com.frutnatura.app.api.NovaMensagemRequest;
import com.frutnatura.app.api.RetrofitClient;

import java.util.List;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class chat_suport extends AppCompatActivity {

    private LinearLayout chatContainer;
    private EditText input;
    private ScrollView scrollView;

    private ApiService apiService;
    private String chamadoId;

    private Handler handler = new Handler(Looper.getMainLooper());
    private final int INTERVALO_REFRESH_MS = 5000;

    private final Runnable refreshRunnable = new Runnable() {
        @Override
        public void run() {
            carregarMensagens(false);
            handler.postDelayed(this, INTERVALO_REFRESH_MS);
        }
    };

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        EdgeToEdge.enable(this);
        setContentView(R.layout.activity_chat_suport);

        // insets
        ViewCompat.setOnApplyWindowInsetsListener(findViewById(R.id.main), (v, insets) -> {
            Insets systemBars = insets.getInsets(WindowInsetsCompat.Type.systemBars());
            v.setPadding(systemBars.left, systemBars.top, systemBars.right, systemBars.bottom);
            return insets;
        });

        // 🔥 pega o id do chamado que veio da tela anterior
        chamadoId = getIntent().getStringExtra("chamadoId");
        if (chamadoId == null) {
            Toast.makeText(this, "Chamado não encontrado.", Toast.LENGTH_LONG).show();
            finish();
            return;
        }

        chatContainer = findViewById(R.id.chatContainer);
        input = findViewById(R.id.chat);
        scrollView = findViewById(R.id.scrollChat);

        input.setMovementMethod(new ScrollingMovementMethod());
        input.setVerticalScrollBarEnabled(true);

        apiService = RetrofitClient.getApiService(this);


        input.setOnEditorActionListener((v, actionId, event) -> {

            boolean isEnterKey =
                    (event != null
                            && event.getKeyCode() == KeyEvent.KEYCODE_ENTER
                            && event.getAction() == KeyEvent.ACTION_DOWN);

            boolean isSendAction =
                    (actionId == EditorInfo.IME_ACTION_SEND
                            || actionId == EditorInfo.IME_ACTION_DONE);

            if (isEnterKey || isSendAction) {
                String message = input.getText().toString().trim();
                if (!message.isEmpty()) {
                    enviarMensagem(message);
                }
                return true; // consumimos o evento
            }

            return false; // deixa passar outros eventos
        });


        // Carrega mensagens iniciais
        carregarMensagens(true);
    }

    @Override
    protected void onResume() {
        super.onResume();
        handler.postDelayed(refreshRunnable, INTERVALO_REFRESH_MS);
    }

    @Override
    protected void onPause() {
        super.onPause();
        handler.removeCallbacks(refreshRunnable);
    }

    // ----- API -----

    private void carregarMensagens(boolean scrollNoFinal) {
        apiService.listarMensagens(chamadoId).enqueue(new Callback<List<MensagemDto>>() {
            @Override
            public void onResponse(Call<List<MensagemDto>> call, Response<List<MensagemDto>> response) {
                if (!response.isSuccessful() || response.body() == null) {
                    Toast.makeText(chat_suport.this,
                            "Erro ao carregar mensagens: " + response.code(),
                            Toast.LENGTH_SHORT).show();
                    return;
                }

                List<MensagemDto> mensagens = response.body();
                exibirMensagens(mensagens);

                if (scrollNoFinal) {
                    rolarParaFim();
                }
            }

            @Override
            public void onFailure(Call<List<MensagemDto>> call, Throwable t) {
                Toast.makeText(chat_suport.this,
                        "Falha ao carregar mensagens: " + t.getMessage(),
                        Toast.LENGTH_SHORT).show();
            }
        });
    }

    private void enviarMensagem(String texto) {
        NovaMensagemRequest request = new NovaMensagemRequest(texto);

        apiService.enviarMensagem(chamadoId, request).enqueue(new Callback<Void>() {
            @Override
            public void onResponse(Call<Void> call, Response<Void> response) {
                if (!response.isSuccessful()) {
                    Toast.makeText(chat_suport.this,
                            "Erro ao enviar mensagem: " + response.code(),
                            Toast.LENGTH_SHORT).show();
                    return;
                }

                // Limpa o campo
                input.setText("");


                carregarMensagens(true);
            }

            @Override
            public void onFailure(Call<Void> call, Throwable t) {
                Toast.makeText(chat_suport.this,
                        "Falha ao enviar mensagem: " + t.getMessage(),
                        Toast.LENGTH_SHORT).show();
            }
        });
    }


    // ----- UI -----

    private void exibirMensagens(List<MensagemDto> mensagens) {
        chatContainer.removeAllViews();

        for (MensagemDto msg : mensagens) {
            // ajuste o nome do campo se no seu DTO não for "texto"
            adicionarMensagemNaTela(msg.texto, /*enviadoPeloCliente=*/ false);
        }

        rolarParaFim();
    }

    private void adicionarMensagemNaTela(String texto, boolean enviadoPeloCliente) {
        // Container da linha (para alinhar esquerda/direita)
        LinearLayout linha = new LinearLayout(this);
        linha.setOrientation(LinearLayout.VERTICAL);
        LinearLayout.LayoutParams linhaParams = new LinearLayout.LayoutParams(
                LinearLayout.LayoutParams.MATCH_PARENT,
                LinearLayout.LayoutParams.WRAP_CONTENT
        );
        linhaParams.setMargins(8, 4, 8, 4);
        linha.setLayoutParams(linhaParams);
        linha.setGravity(enviadoPeloCliente ? Gravity.END : Gravity.START);

        // A bolha em si
        TextView textView = new TextView(this);
        textView.setText(texto);
        textView.setTextSize(15);
        textView.setTextColor(Color.WHITE);
        textView.setPadding(0, 0, 0, 0); // padding já está no drawable

        if (enviadoPeloCliente) {
            textView.setBackgroundResource(R.drawable.bg_msg_cliente);
        } else {
            textView.setBackgroundResource(R.drawable.bg_msg_suporte);
        }

        LinearLayout.LayoutParams bubbleParams = new LinearLayout.LayoutParams(
                LinearLayout.LayoutParams.WRAP_CONTENT,
                LinearLayout.LayoutParams.WRAP_CONTENT
        );
        bubbleParams.setMargins(12, 4, 12, 4);
        textView.setLayoutParams(bubbleParams);

        linha.addView(textView);
        chatContainer.addView(linha);

        rolarParaFim();
    }


    private void rolarParaFim() {
        if (scrollView != null) {
            scrollView.post(() -> scrollView.fullScroll(View.FOCUS_DOWN));
        }
    }
}
