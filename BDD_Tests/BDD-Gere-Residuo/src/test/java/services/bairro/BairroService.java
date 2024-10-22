package services.bairro;

import com.fasterxml.jackson.databind.ObjectMapper;
import com.google.gson.Gson;
import com.google.gson.GsonBuilder;
import deserializer.ErrorModelDeserializer;
import hook.usuario.UsuarioHook;
import io.restassured.http.ContentType;
import io.restassured.response.Response;
import model.bairro.BairroModel;
import model.enums.UsuarioRole;
import model.error.ErrorModel;
import model.usuario.UsuarioModel;

import static io.restassured.RestAssured.given;

public class BairroService {
    private static final BairroModel bairroModel = new BairroModel();
    public static final String schemasPath = "src/test/resources/schemas/bairro/";
    public static Response response;
    public final Gson gson = new GsonBuilder()
            .excludeFieldsWithoutExposeAnnotation()
            .registerTypeAdapter(ErrorModel.class, new ErrorModelDeserializer())
            .create();
    private final ObjectMapper mapper = new ObjectMapper();
    private final String baseUrl = "http://52.170.197.27:80";


    public static <T> void setAtributoBairro(String atributo, T valor) {

        switch (atributo) {
            case "bairroId" -> bairroModel.setBairroId((int) valor);
            case "percentualColetaLixoBairro" -> bairroModel.setPercentualColetaLixoBairro((int) valor);
            case "bairroEstaDisponivel" -> bairroModel.setBairroEstaDisponivel((Boolean) valor);
            case "bairroNome" -> bairroModel.setBairroNome((String) valor);
            case "quantidadeLixeiras" -> bairroModel.setQuantidadeLixeiras((int) valor);
            case "pesoMedioLixeirasKg" -> bairroModel.setPesoMedioLixeirasKg((int) valor);
            default -> throw new IllegalStateException("Atributo incorreto: " + valor);
        }
    }

    public void createBairro(String endpoint) {
        String url = baseUrl + endpoint;
        String bodyToSend = gson.toJson(bairroModel);

        response = given()
                .contentType(ContentType.JSON)
                .accept(ContentType.JSON)
                .body(bodyToSend)
                .when()
                .post(url)
                .then()
                .extract()
                .response();
    }
}



