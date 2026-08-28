package com.frutnatura.app;

import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageView;
import android.widget.TextView;

import androidx.annotation.NonNull;
import androidx.recyclerview.widget.RecyclerView;

import java.util.List;

public class BannerAdapter extends RecyclerView.Adapter<BannerAdapter.BannerViewHolder> {

    private Context context;
    private List<BannerItem> lista;

    public BannerAdapter(Context context, List<BannerItem> lista) {
        this.context = context;
        this.lista = lista;
    }

    @NonNull
    @Override
    public BannerViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        View view = LayoutInflater.from(context).inflate(R.layout.item_banner, parent, false);
        return new BannerViewHolder(view);
    }

    @Override
    public void onBindViewHolder(@NonNull BannerViewHolder holder, int position) {
        BannerItem item = lista.get(position);
        holder.img.setImageResource(item.imagem);
        holder.nome.setText(item.nome);
    }

    @Override
    public int getItemCount() {
        return lista != null ? lista.size() : 0;
    }

    static class BannerViewHolder extends RecyclerView.ViewHolder {
        ImageView img;
        TextView nome;

        BannerViewHolder(View itemView) {
            super(itemView);
            img = itemView.findViewById(R.id.imgBanner);
            nome = itemView.findViewById(R.id.nomeBanner);
        }
    }
}
