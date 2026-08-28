package com.frutnatura.app.api;

import java.util.List;

import retrofit2.Call;
import retrofit2.http.Body;
import retrofit2.http.GET;
import retrofit2.http.POST;
import retrofit2.http.Path;
import retrofit2.http.Query;

public interface ApiService {

    // LOGIN
    @POST("api/auth/login")
    Call<LoginResponse> login(@Body LoginRequest request);

    // CADASTRO
    @POST("api/auth/register")
    Call<CadastroResponse> cadastro(@Body CadastroRequest request);

    // ABRIR CHAMADO
    @POST("api/clientes/chamados")
    Call<Void> criarChamado(@Body ChamadoRequest request);

    // LISTAR CHAMADOS DO CLIENTE

    @GET("api/clientes/chamados")
    Call<ApiResponse<java.util.List<ChamadoListaItem>>> getMeusChamados(
            @Query("page") int page,
            @Query("pageSize") int pageSize
    );


    // LISTAR MENSAGENS DO CHAMADO
    @GET("api/chamados/{id}/mensagens")
    Call<List<MensagemDto>> listarMensagens(@Path("id") String chamadoId);

    // ENVIAR MENSAGEM DO CLIENTE
    @POST("api/clientes/chamados/{id}/mensagens")
    Call<Void> enviarMensagem(@Path("id") String chamadoId,
                              @Body NovaMensagemRequest request);
}
