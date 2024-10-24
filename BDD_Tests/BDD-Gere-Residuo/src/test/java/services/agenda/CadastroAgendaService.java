package services.agenda;

import com.fasterxml.jackson.databind.ObjectMapper;
import com.google.gson.Gson;
import com.google.gson.GsonBuilder;
import com.networknt.schema.ValidationMessage;
import deserializer.ErrorModelDeserializer;
import hook.agenda.AgendaHook;
import io.restassured.http.ContentType;
import io.restassured.response.Response;
import lombok.Getter;
import lombok.Setter;
import model.agenda.AgendaModel;
import model.enums.StatusColetaDeLixo;
import model.enums.TipoResiduo;
import model.error.ErrorModel;
import org.json.JSONObject;
import services.notificacao.CadastroNotificacaoService;
import utils.JSONSchemaUtils;

import java.io.IOException;
import java.util.Set;

import static io.restassured.RestAssured.given;

public class CadastroAgendaService {
    private final AgendaModel agendaModel = new AgendaModel();
    @Getter
    private final Gson gson = new GsonBuilder()
            .excludeFieldsWithoutExposeAnnotation()
            .registerTypeAdapter(ErrorModel.class, new ErrorModelDeserializer())
            .create();
    @Getter
    @Setter
    private Response response;
    private String agendaId;
    @Getter
    @Setter
    private static int bairroId;
    @Getter
    @Setter
    private static int caminhaoId;
    @Getter
    @Setter
    private static int motoristaId;
    @Getter
    @Setter
    private static String tokenJwt;
    private final String schemasPath = "src/test/resources/schemas/agenda/";
    private JSONObject jsonSchema;
    private final ObjectMapper mapper = new ObjectMapper();
    private final String baseUrl = String.format("http://%s:%s", System.getenv("SERVER_IP"), System.getenv("SERVER_PORT"));

    public <T> void setAtributoAgenda(String atributo, T valor) {

        switch(atributo) {
            case "agendaId" -> agendaModel.setAgendaId(Integer.parseInt(valor.toString()));
            case "caminhaoId" -> agendaModel.setCaminhaoId(Integer.parseInt(valor.toString()));
            case "motoristaId" -> agendaModel.setMotoristaId(Integer.parseInt(valor.toString()));
            case "bairroId" -> agendaModel.setBairroId(Integer.parseInt(valor.toString()));
            case "diaColetaDeLixo" -> agendaModel.setDiaColetaDeLixo((String) valor);
            case "tipoResiduo" -> agendaModel.setTipoResiduo(TipoResiduo.valueOf((String) valor));
            case "statusColetaDeLixoAgendada" -> agendaModel.setStatusColetaDeLixoAgendada(StatusColetaDeLixo.valueOf((String) valor));
            case "pesoColetadoDeLixoKg" -> agendaModel.setPesoColetadoDeLixoKg(Integer.parseInt(valor.toString()));
            default -> throw new IllegalStateException("Atributo incorreto: " + valor);
        }
    }

    public void setAtributoBairroId() {
        setAtributoAgenda("bairroId", getBairroId());
    }

    public void setAtributoBairroIdInvalido() {
        setAtributoAgenda("bairroId", 0);
    }

    public void setAtributoCaminhaoId() {
        setAtributoAgenda("caminhaoId", getCaminhaoId());
    }

    public void setAtributoMotoristaId() {
        setAtributoAgenda("motoristaId", getMotoristaId());
    }

    public void createAgenda(String endpoint) {
        String url = baseUrl + endpoint;
        String bodyToSend = gson.toJson(agendaModel);

        response = given()
                .header("Authorization", "Bearer " + tokenJwt)
                .contentType(ContentType.JSON)
                .accept(ContentType.JSON)
                .body(bodyToSend)
                .when()
                .post(url)
                .then()
                .extract()
                .response();

        if (response.getStatusCode() == 201) {
            String id = String.valueOf(gson.fromJson(response.jsonPath().prettify(), AgendaModel.class).getAgendaId());
            AgendaHook.setAgendaCriadaId(id);
            CadastroNotificacaoService.setAgendaId(Integer.parseInt(id));
        }
    }

    public void setAgendaId() {
        if (agendaId == null) {
            agendaId = String.valueOf(gson.fromJson(response.jsonPath().prettify(), AgendaModel.class).getAgendaId());
        }
    }

    public void setAgendaIdInvalido() {
        agendaId = "0";
    }

    public void deleteAgenda(String endpoint) {
        String url = String.format("%s%s/%s", baseUrl, endpoint, agendaId);
        response = given()
                .header("Authorization", "Bearer " + tokenJwt)
                .accept(ContentType.JSON)
                .when()
                .delete(url)
                .then()
                .extract()
                .response();
    }

    public void deleteAgenda(String endpoint, String token, String id) {
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

    // JSON Schema Validator
    public void setArquivoJsonSchema(String contract) throws IOException {
        switch (contract) {
            case "Cadastro de agenda bem-sucedido" -> jsonSchema = JSONSchemaUtils.getArquivoJsonSchema(schemasPath + "cadastro-de-agenda-bem-sucedido.json");
            default -> throw new IllegalStateException("Arquivo JSON Schema não encontrado: " + contract);
        }
    }

    public Set<ValidationMessage> validateResponseContraJsonSchema() throws IOException
    {
        return JSONSchemaUtils.validateResponseContraJsonSchema(response, jsonSchema, mapper);
    }
}
