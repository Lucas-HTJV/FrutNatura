package com.frutnatura.app;

import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.TextView;

import androidx.appcompat.app.AppCompatActivity;
import androidx.core.graphics.Insets;
import androidx.core.view.ViewCompat;
import androidx.core.view.WindowInsetsCompat;
import androidx.viewpager2.widget.ViewPager2;

import java.util.Arrays;
import java.util.List;

public class ecommerce extends AppCompatActivity {

    private ViewPager2 viewPager;
    private TextView tela_ajuda;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_ecommerce); // confira o nome do seu layout

        ViewCompat.setOnApplyWindowInsetsListener(findViewById(R.id.main), (v, insets) -> {
            Insets systemBars = insets.getInsets(WindowInsetsCompat.Type.systemBars());
            v.setPadding(systemBars.left, systemBars.top, systemBars.right, systemBars.bottom);
            return insets;
        });

        // 🔥 RECEBE O ID DO USUÁRIO QUE VEIO DO LOGIN
        int idUsuario = getIntent().getIntExtra("idUsuario", -1);

        IniciarComponentes();

        tela_ajuda.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {

                // 🔥 ENVIA O ID PARA A PRÓXIMA TELA
                Intent intent = new Intent(ecommerce.this, inicio_chamado.class);
                intent.putExtra("idUsuario", idUsuario);
                startActivity(intent);
            }
        });

        viewPager = findViewById(R.id.viewPagerBanner);

        // Lista com 3 itens (use os drawables que você tem no res/drawable)
        List<BannerItem> listaBanner = Arrays.asList(
                new BannerItem(R.drawable.maca_, "Maçã"),
                new BannerItem(R.drawable.banana_, "Banana"),
                new BannerItem(R.drawable.alface_, "Alface")
        );

        BannerAdapter adapter = new BannerAdapter(this, listaBanner);
        viewPager.setAdapter(adapter);

        // Page transformer suave (opcional)
        viewPager.setPageTransformer((page, position) -> {
            page.setAlpha(0.3f + (1 - Math.abs(position)));
        });
    }

    private void IniciarComponentes() {
        tela_ajuda = findViewById(R.id.ajuda);
    }
}
