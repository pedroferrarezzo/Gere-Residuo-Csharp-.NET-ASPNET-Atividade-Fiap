package services.notificacao;

import com.google.gson.Gson;
import com.google.gson.GsonBuilder;
import com.google.gson.reflect.TypeToken;
import deserializer.ErrorModelDeserializer;
import io.restassured.http.ContentType;
import io.restassured.response.Response;
import model.error.ErrorModel;
import model.notificacao.NotificacaoModel;

import java.lang.reflect.Type;
import java.util.List;
import java.util.Optional;

import static io.restassured.RestAssured.given;

public class NotificacaoService {
    private final String baseUrl = "http://52.170.197.27:80";
    private Response response;
    private final Gson gson = new GsonBuilder()
            .excludeFieldsWithoutExposeAnnotation()
            .registerTypeAdapter(ErrorModel.class, new ErrorModelDeserializer())
            .create();


    public NotificacaoModel getNotificacaoDaAgenda(String endpoint, String token, int agendaCriadaId) {
        String url = String.format("%s%s", baseUrl, endpoint);
        response = given()
                .header("Authorization", "Bearer " + token)
                .accept(ContentType.JSON)
                .when()
                .get(url)
                .then()
                .extract()
                .response();

        Type listNotificacaoType = new TypeToken<List<NotificacaoModel>>() {}.getType();
        List<NotificacaoModel> notificacoes = gson.fromJson(response.jsonPath().prettify(), listNotificacaoType);

        Optional<NotificacaoModel> notificacaoDaAgenda = notificacoes.stream()
                .filter(notificacao -> notificacao.getAgendaDaNotificacao().getAgendaId() == agendaCriadaId)
                .findFirst();

        if (notificacaoDaAgenda.isPresent()) {
            return notificacaoDaAgenda.get();
        }

        throw new IllegalStateException("Agenda de ID: " + agendaCriadaId + " não gerou notificações!");
    }

    public void deleteNotificacao(String endpoint, String token, String id) {
        String url = String.format("%s%s/%s", baseUrl, endpoint, id);
        response = given()
                .header("Authorization", "Bearer " + token)
                .accept(ContentType.JSON)
                .when()
                .delete(url)
                .then()
                .extract()
                .response();
    }
}
