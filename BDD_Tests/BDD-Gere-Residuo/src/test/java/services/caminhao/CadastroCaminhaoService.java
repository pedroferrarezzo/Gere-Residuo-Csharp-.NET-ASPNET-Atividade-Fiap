package services.caminhao;

import com.fasterxml.jackson.databind.ObjectMapper;
import com.google.gson.Gson;
import com.google.gson.GsonBuilder;
import com.networknt.schema.ValidationMessage;
import deserializer.ErrorModelDeserializer;
import hook.caminhao.CaminhaoHook;
import io.restassured.http.ContentType;
import io.restassured.response.Response;
import lombok.Getter;
import lombok.Setter;
import model.caminhao.CaminhaoModel;
import model.error.ErrorModel;
import org.json.JSONObject;
import org.junit.Assert;
import services.agenda.CadastroAgendaService;
import utils.JSONSchemaUtils;

import java.io.IOException;
import java.util.Set;

import static io.restassured.RestAssured.given;

public class CadastroCaminhaoService {

    private final CaminhaoModel caminhaoModel = new CaminhaoModel();
    @Getter
    private final Gson gson = new GsonBuilder()
            .excludeFieldsWithoutExposeAnnotation()
            .registerTypeAdapter(ErrorModel.class, new ErrorModelDeserializer())
            .create();
    @Getter
    @Setter
    private Response response;
    private String caminhaoId;
    @Getter
    @Setter
    private static String tokenJwt;
    private final String schemasPath = "src/test/resources/schemas/caminhao/";
    private JSONObject jsonSchema;
    private final ObjectMapper mapper = new ObjectMapper();
    private final String baseUrl = "http://52.170.197.27:80";

    public <T> void setAtributoCaminhao(String atributo, T valor) {
        switch(atributo) {
            case "caminhaoId" -> caminhaoModel.setCaminhaoId(Integer.parseInt(valor.toString()));
            case "caminhaoPlaca" -> caminhaoModel.setCaminhaoPlaca((String) valor);
            case "dataFabricacao" -> caminhaoModel.setDataFabricacao((String) valor);
            case "caminhaoMarca" -> caminhaoModel.setCaminhaoMarca((String) valor);
            case "caminhaoModelo" -> caminhaoModel.setCaminhaoModelo((String) valor);
            case "caminhaoEstaDisponivel" -> caminhaoModel.setCaminhaoEstaDisponivel((boolean) valor);
            default -> throw new IllegalStateException("Atributo incorreto: " + valor);
        }
    }

    public void validateCaminhaoEstaDisponivel(String condition) {
        switch(condition) {
            case "False" -> Assert.assertFalse(
                    gson.fromJson(
                                    response.jsonPath().prettify(), CaminhaoModel.class)
                            .isCaminhaoEstaDisponivel()
            );
            case "True" -> Assert.assertTrue(
                    gson.fromJson(
                                    response.jsonPath().prettify(), CaminhaoModel.class)
                            .isCaminhaoEstaDisponivel()
            );
            default -> throw new IllegalStateException("Condição incorreta: " + condition);
        }
    }

    public void setCaminhaoIdInvalido(int id) {
        caminhaoId = String.valueOf(id);
    }

    public void createCaminhao(String endpoint) {
        String url = baseUrl + endpoint;
        String bodyToSend = gson.toJson(caminhaoModel);

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
            String id = String.valueOf(gson.fromJson(response.jsonPath().prettify(), CaminhaoModel.class).getCaminhaoId());
            CaminhaoHook.setCaminhaoCriadoId(id);
            CadastroAgendaService.setCaminhaoId(Integer.parseInt(id));
        }
    }

    public void getCaminhao(String endpoint) {
        String url = String.format("%s%s/%s", baseUrl, endpoint, caminhaoId);
        response = given()
                .header("Authorization", "Bearer " + tokenJwt)
                .accept(ContentType.JSON)
                .when()
                .get(url)
                .then()
                .extract()
                .response();
    }

    public void setCaminhaoId() {
        if (caminhaoId == null) {
            caminhaoId = String.valueOf(gson.fromJson(response.jsonPath().prettify(), CaminhaoModel.class).getCaminhaoId());
        }
    }

    public void deleteCaminhao(String endpoint) {
        String url = String.format("%s%s/%s", baseUrl, endpoint, caminhaoId);
        response = given()
                .header("Authorization", "Bearer " + tokenJwt)
                .accept(ContentType.JSON)
                .when()
                .delete(url)
                .then()
                .extract()
                .response();
    }

    public void deleteCaminhao(String endpoint, String token, String id) {
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
            case "Cadastro de caminhao bem-sucedido" -> jsonSchema = JSONSchemaUtils.getArquivoJsonSchema(schemasPath + "cadastro-de-caminhao-bem-sucedido.json");
            default -> throw new IllegalStateException("Arquivo JSON Schema não encontrado: " + contract);
        }
    }

    public Set<ValidationMessage> validateResponseContraJsonSchema() throws IOException
    {
        return JSONSchemaUtils.validateResponseContraJsonSchema(response, jsonSchema, mapper);
    }
}
