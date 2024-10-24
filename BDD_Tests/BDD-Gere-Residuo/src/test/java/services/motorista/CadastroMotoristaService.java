package services.motorista;

import com.fasterxml.jackson.databind.ObjectMapper;
import com.google.gson.Gson;
import com.google.gson.GsonBuilder;
import com.networknt.schema.ValidationMessage;
import deserializer.ErrorModelDeserializer;
import hook.motorista.MotoristaHook;
import io.restassured.http.ContentType;
import io.restassured.response.Response;
import lombok.Getter;
import lombok.Setter;
import model.error.ErrorModel;
import model.motorista.MotoristaModel;
import org.json.JSONObject;
import org.junit.Assert;
import services.agenda.CadastroAgendaService;
import utils.JSONSchemaUtils;

import java.io.IOException;
import java.util.Set;

import static io.restassured.RestAssured.given;

public class CadastroMotoristaService {

    private final MotoristaModel motoristaModel = new MotoristaModel();
    @Getter
    private final Gson gson = new GsonBuilder()
            .excludeFieldsWithoutExposeAnnotation()
            .registerTypeAdapter(ErrorModel.class, new ErrorModelDeserializer())
            .create();
    @Getter
    @Setter
    private Response response;
    private String motoristaId;
    @Getter
    @Setter
    private static String tokenJwt;
    private final String schemasPath = "src/test/resources/schemas/motorista/";
    private JSONObject jsonSchema;
    private final ObjectMapper mapper = new ObjectMapper();
    private final String baseUrl = String.format("http://%s:%s", System.getenv("SERVER_IP"), System.getenv("SERVER_PORT"));

    public <T> void setAtributoMotorista(String atributo, T valor) {
        switch(atributo) {
            case "motoristaId" -> motoristaModel.setMotoristaId(Integer.parseInt(valor.toString()));
            case "motoristaNome" -> motoristaModel.setMotoristaNome((String) valor);
            case "motoristaCpf" -> motoristaModel.setMotoristaCpf((String) valor);
            case "motoristaNrCelular" -> motoristaModel.setMotoristaNrCelular((String) valor);
            case "motoristaNrCelularDdd" -> motoristaModel.setMotoristaNrCelularDdd((String) valor);
            case "motoristaNrCelularDdi" -> motoristaModel.setMotoristaNrCelularDdi((String) valor);
            case "motoristaEstaDisponivel" -> motoristaModel.setMotoristaEstaDisponivel((boolean) valor);
            default -> throw new IllegalStateException("Atributo incorreto: " + valor);
        }
    }

    public void validateMotoristaEstaDisponivel(String condition) {
        switch(condition) {
            case "False" -> Assert.assertFalse(
                    gson.fromJson(
                                    response.jsonPath().prettify(), MotoristaModel.class)
                            .isMotoristaEstaDisponivel()
            );
            case "True" -> Assert.assertTrue(
                    gson.fromJson(
                                    response.jsonPath().prettify(), MotoristaModel.class)
                            .isMotoristaEstaDisponivel()
            );
            default -> throw new IllegalStateException("Condição incorreta: " + condition);
        }
    }

    public void createMotorista(String endpoint) {
        String url = baseUrl + endpoint;
        String bodyToSend = gson.toJson(motoristaModel);

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
            String id = String.valueOf(gson.fromJson(response.jsonPath().prettify(), MotoristaModel.class).getMotoristaId());
            MotoristaHook.setMotoristaCriadoId(id);
            CadastroAgendaService.setMotoristaId(Integer.parseInt(id));
        }
    }

    public void getMotorista(String endpoint) {
        String url = String.format("%s%s/%s", baseUrl, endpoint, motoristaId);
        response = given()
                .header("Authorization", "Bearer " + tokenJwt)
                .accept(ContentType.JSON)
                .when()
                .get(url)
                .then()
                .extract()
                .response();
    }

    public void setMotoristaId() {
        if (motoristaId == null) {
            motoristaId = String.valueOf(gson.fromJson(response.jsonPath().prettify(), MotoristaModel.class).getMotoristaId());
        }
    }

    public void setMotoristaIdInvalido(int id) {
        motoristaId = String.valueOf(id);
    }

    public void deleteMotorista(String endpoint) {
        String url = String.format("%s%s/%s", baseUrl, endpoint, motoristaId);
        response = given()
                .header("Authorization", "Bearer " + tokenJwt)
                .accept(ContentType.JSON)
                .when()
                .delete(url)
                .then()
                .extract()
                .response();
    }

    public void deleteMotorista(String endpoint, String token, String id) {
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
            case "Cadastro de motorista bem-sucedido" -> jsonSchema = JSONSchemaUtils.getArquivoJsonSchema(schemasPath + "cadastro-de-motorista-bem-sucedido.json");
            default -> throw new IllegalStateException("Arquivo JSON Schema não encontrado: " + contract);
        }
    }

    public Set<ValidationMessage> validateResponseContraJsonSchema() throws IOException
    {
        return JSONSchemaUtils.validateResponseContraJsonSchema(response, jsonSchema, mapper);
    }
}
