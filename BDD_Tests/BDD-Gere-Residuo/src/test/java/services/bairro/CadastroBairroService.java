package services.bairro;

import com.fasterxml.jackson.databind.ObjectMapper;
import com.google.gson.Gson;
import com.google.gson.GsonBuilder;
import com.networknt.schema.ValidationMessage;
import deserializer.ErrorModelDeserializer;
import hook.bairro.BairroHook;
import io.restassured.http.ContentType;
import io.restassured.response.Response;
import lombok.Getter;
import lombok.Setter;
import model.bairro.BairroModel;
import model.error.ErrorModel;
import org.json.JSONObject;
import org.junit.Assert;
import services.agenda.CadastroAgendaService;
import services.morador.CadastroMoradorService;
import utils.JSONSchemaUtils;

import java.io.IOException;
import java.util.Set;

import static io.restassured.RestAssured.given;

public class CadastroBairroService {

    private final BairroModel bairroModel = new BairroModel();
    @Getter
    private final Gson gson = new GsonBuilder()
            .excludeFieldsWithoutExposeAnnotation()
            .registerTypeAdapter(ErrorModel.class, new ErrorModelDeserializer())
            .create();
    @Getter
    @Setter
    private Response response;
    private String bairroId;
    @Getter
    @Setter
    private static String tokenJwt;
    private final String schemasPath = "src/test/resources/schemas/bairro/";
    private JSONObject jsonSchema;
    private final ObjectMapper mapper = new ObjectMapper();
    private final String baseUrl = String.format("http://%s:%s", System.getenv("SERVER_IP"), System.getenv("SERVER_PORT"));

    public <T> void setAtributoBairro(String atributo, T valor) {

        switch(atributo) {
            case "bairroId" -> bairroModel.setBairroId(Integer.parseInt(valor.toString()));
            case "percentualColetaLixoBairro" -> bairroModel.setPercentualColetaLixoBairro(Integer.parseInt(valor.toString()));
            case "bairroEstaDisponivel" -> bairroModel.setBairroEstaDisponivel((boolean) valor);
            case "bairroNome" -> bairroModel.setBairroNome((String) valor);
            case "quantidadeLixeiras" -> bairroModel.setQuantidadeLixeiras(Integer.parseInt(valor.toString()));
            case "pesoMedioLixeirasKg" -> bairroModel.setPesoMedioLixeirasKg(Integer.parseInt(valor.toString()));
            default -> throw new IllegalStateException("Atributo incorreto: " + valor);
        }
    }

    public void validateBairroEstaDisponivel(String condition) {
        switch(condition) {
            case "False" -> Assert.assertFalse(
                    gson.fromJson(
                                    response.jsonPath().prettify(), BairroModel.class)
                            .isBairroEstaDisponivel()
            );
            case "True" -> Assert.assertTrue(
                    gson.fromJson(
                                    response.jsonPath().prettify(), BairroModel.class)
                            .isBairroEstaDisponivel()
            );
            default -> throw new IllegalStateException("Condição incorreta: " + condition);
        }
    }

    public void createBairro(String endpoint) {
        String url = baseUrl + endpoint;
        String bodyToSend = gson.toJson(bairroModel);

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
            String id = String.valueOf(gson.fromJson(response.jsonPath().prettify(), BairroModel.class).getBairroId());
            BairroHook.setBairroCriadoId(id);
            CadastroMoradorService.setBairroId(Integer.parseInt(id));
            CadastroAgendaService.setBairroId(Integer.parseInt(id));
        }
    }

    public void setBairroId() {
        if (bairroId == null) {
            bairroId = String.valueOf(gson.fromJson(response.jsonPath().prettify(), BairroModel.class).getBairroId());
        }
    }

    public void setBairroIdInvalido(int id) {
        bairroId = String.valueOf(id);
    }

    public void deleteBairro(String endpoint) {
        String url = String.format("%s%s/%s", baseUrl, endpoint, bairroId);
        response = given()
                .header("Authorization", "Bearer " + tokenJwt)
                .accept(ContentType.JSON)
                .when()
                .delete(url)
                .then()
                .extract()
                .response();
    }

    public void getBairro(String endpoint) {
        String url = String.format("%s%s/%s", baseUrl, endpoint, bairroId);
        response = given()
                .header("Authorization", "Bearer " + tokenJwt)
                .accept(ContentType.JSON)
                .when()
                .get(url)
                .then()
                .extract()
                .response();
    }

    public void deleteBairro(String endpoint, String token, String id) {
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
            case "Cadastro de bairro bem-sucedido" -> jsonSchema = JSONSchemaUtils.getArquivoJsonSchema(schemasPath + "cadastro-de-bairro-bem-sucedido.json");
            default -> throw new IllegalStateException("Arquivo JSON Schema não encontrado: " + contract);
        }
    }

    public Set<ValidationMessage> validateResponseContraJsonSchema() throws IOException
    {
        return JSONSchemaUtils.validateResponseContraJsonSchema(response, jsonSchema, mapper);
    }
}
