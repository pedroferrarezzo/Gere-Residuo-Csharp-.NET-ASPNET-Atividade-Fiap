package services.morador;

import com.fasterxml.jackson.databind.ObjectMapper;
import com.google.gson.Gson;
import com.google.gson.GsonBuilder;
import com.networknt.schema.ValidationMessage;
import deserializer.ErrorModelDeserializer;
import hook.morador.MoradorHook;
import io.restassured.http.ContentType;
import io.restassured.response.Response;
import lombok.Getter;
import lombok.Setter;
import model.error.ErrorModel;
import model.morador.MoradorModel;
import org.json.JSONObject;
import utils.JSONSchemaUtils;

import java.io.IOException;
import java.util.Set;

import static io.restassured.RestAssured.given;

public class CadastroMoradorService {

    private final MoradorModel moradorModel = new MoradorModel();
    private final Gson gson = new GsonBuilder()
            .excludeFieldsWithoutExposeAnnotation()
            .registerTypeAdapter(ErrorModel.class, new ErrorModelDeserializer())
            .create();
    private Response response;
    @Getter
    @Setter
    private static int bairroId;
    private String moradorId;
    private static String tokenJwt;
    private final String schemasPath = "src/test/resources/schemas/morador/";
    private JSONObject jsonSchema;
    private final ObjectMapper mapper = new ObjectMapper();
    private final String baseUrl = "http://52.170.197.27:80";

    public <T> void setAtributoMorador(String atributo, T valor) {

        switch(atributo) {
            case "moradorId" -> moradorModel.setMoradorId(Integer.parseInt(valor.toString()));
            case "bairroId" -> moradorModel.setBairroId(Integer.parseInt(valor.toString()));
            case "moradorNome" -> moradorModel.setMoradorNome((String) valor);
            case "moradorEmail" -> moradorModel.setMoradorEmail((String) valor);
            default -> throw new IllegalStateException("Atributo incorreto: " + valor);
        }
    }

    public void createMorador(String endpoint) {
        String url = baseUrl + endpoint;
        String bodyToSend = gson.toJson(moradorModel);

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
            String id = String.valueOf(gson.fromJson(response.jsonPath().prettify(), MoradorModel.class).getMoradorId());
            MoradorHook.setMoradorCriadoId(id);
        }
    }

    public void setMoradorId() {
        if (moradorId == null) {
            moradorId = String.valueOf(gson.fromJson(response.jsonPath().prettify(), MoradorModel.class).getMoradorId());
        }
    }

    public void setMoradorIdInvalido() {
        moradorId = "0";
    }

    public void deleteMorador(String endpoint) {
        String url = String.format("%s%s/%s", baseUrl, endpoint, moradorId);
        response = given()
                .header("Authorization", "Bearer " + tokenJwt)
                .accept(ContentType.JSON)
                .when()
                .delete(url)
                .then()
                .extract()
                .response();
    }

    public void deleteMorador(String endpoint, String token, String id) {
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
            case "Cadastro de morador bem-sucedido" -> jsonSchema = JSONSchemaUtils.getArquivoJsonSchema(schemasPath + "cadastro-de-morador-bem-sucedido.json");
            default -> throw new IllegalStateException("Arquivo JSON Schema não encontrado: " + contract);
        }
    }

    public Set<ValidationMessage> validateResponseContraJsonSchema() throws IOException
    {
        return JSONSchemaUtils.validateResponseContraJsonSchema(response, jsonSchema, mapper);
    }
}
