package services.notificacao;

import com.google.gson.Gson;
import com.google.gson.GsonBuilder;
import com.google.gson.reflect.TypeToken;
import deserializer.ErrorModelDeserializer;
import hook.notificacao.NotificacaoHook;
import io.restassured.http.ContentType;
import io.restassured.response.Response;
import lombok.Getter;
import lombok.Setter;
import model.error.ErrorModel;
import model.notificacao.NotificacaoModel;

import java.lang.reflect.Type;
import java.util.List;
import java.util.Optional;

import static io.restassured.RestAssured.given;

public class CadastroNotificacaoService {
    private final NotificacaoModel notificacaoModel = new NotificacaoModel();

    @Getter
    private final Gson gson = new GsonBuilder()
            .excludeFieldsWithoutExposeAnnotation()
            .registerTypeAdapter(ErrorModel.class, new ErrorModelDeserializer())
            .create();
    @Getter
    @Setter
    private Response response;
    @Getter
    @Setter
    private static int agendaId;
    private int notificacaoId;
    private NotificacaoModel notificacaoDaAgenda;
    @Getter
    @Setter
    private static String tokenJwt;
    private final String baseUrl = "http://52.170.197.27:80";

    public void setNotificacaoId(String endpoint) {
        String url = String.format("%s%s", baseUrl, endpoint);

        response = given()
                .header("Authorization", "Bearer " + tokenJwt)
                .accept(ContentType.JSON)
                .when()
                .get(url)
                .then()
                .extract()
                .response();

        Type listNotificacaoType = new TypeToken<List<NotificacaoModel>>() {}.getType();
        List<NotificacaoModel> notificacoes = gson.fromJson(response.jsonPath().prettify(), listNotificacaoType);

        Optional<NotificacaoModel> notificacaoRecuperada = notificacoes.stream()
                .filter(notificacao -> notificacao.getAgendaDaNotificacao().getAgendaId() == agendaId)
                .findFirst();

        if (notificacaoRecuperada.isPresent()) {
            notificacaoDaAgenda = notificacaoRecuperada.get();
            notificacaoId = notificacaoDaAgenda.getNotificacaoId();
        }
        else {
            throw new IllegalStateException("Agenda de ID: " + agendaId + " não gerou notificações!");
        }
    }

    public void deleteNotificacao(String endpoint) {
        String url = String.format("%s%s/%s", baseUrl, endpoint, notificacaoId);
        response = given()
                .header("Authorization", "Bearer " + tokenJwt)
                .accept(ContentType.JSON)
                .when()
                .delete(url)
                .then()
                .extract()
                .response();
    }

    public void deleteNotificacao(String endpoint, String token) {
        String url = String.format("%s%s/%s", baseUrl, endpoint, notificacaoId);
        response = given()
                .header("Authorization", "Bearer " + token)
                .accept(ContentType.JSON)
                .when()
                .delete(url)
                .then()
                .extract()
                .response();
    }

    public void setNotificacaoIdInvalido(int id) {
        notificacaoId = id;
    }
}
