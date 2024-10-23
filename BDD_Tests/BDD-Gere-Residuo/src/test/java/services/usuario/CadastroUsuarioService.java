package services.usuario;

import com.fasterxml.jackson.databind.ObjectMapper;
import com.google.gson.Gson;
import com.google.gson.GsonBuilder;
import com.networknt.schema.ValidationMessage;
import deserializer.ErrorModelDeserializer;
import hook.usuario.UsuarioHook;
import io.jsonwebtoken.JwtException;
import io.jsonwebtoken.Jwts;
import io.jsonwebtoken.security.Keys;
import io.restassured.http.ContentType;
import io.restassured.response.Response;
import lombok.Getter;
import model.enums.UsuarioRole;
import model.error.ErrorModel;
import model.token.TokenModel;
import model.usuario.UsuarioModel;
import org.json.JSONObject;
import services.agenda.CadastroAgendaService;
import services.bairro.CadastroBairroService;
import services.caminhao.CadastroCaminhaoService;
import services.morador.CadastroMoradorService;
import services.motorista.CadastroMotoristaService;
import utils.JSONSchemaUtils;

import java.io.IOException;
import java.security.Key;
import java.util.Set;

import static io.restassured.RestAssured.given;

public class CadastroUsuarioService {

    private final UsuarioModel usuarioModel = new UsuarioModel();
    @Getter
    private final Gson gson = new GsonBuilder()
            .excludeFieldsWithoutExposeAnnotation()
            .registerTypeAdapter(ErrorModel.class, new ErrorModelDeserializer())
            .create();

    @Getter
    private Response response;
    private String usuarioId;
    private String tokenJwt;
    private final String schemasPath = "src/test/resources/schemas/usuario/";
    private JSONObject jsonSchema;
    private final ObjectMapper mapper = new ObjectMapper();
    private final String baseUrl = "http://52.170.197.27:80";

    public <T> void setAtributoUsuario(String atributo, T valor) {

        switch(atributo) {
            case "usuarioId" -> usuarioModel.setUsuarioId(Integer.parseInt(valor.toString()));
            case "usuarioNome" -> usuarioModel.setUsuarioNome((String) valor);
            case "usuarioEmail" -> usuarioModel.setUsuarioEmail((String) valor);
            case "usuarioSenha" -> usuarioModel.setUsuarioSenha((String) valor);
            case "usuarioRole" -> usuarioModel.setUsuarioRole(UsuarioRole.valueOf((String) valor));
            default -> throw new IllegalStateException("Atributo incorreto: " + valor);
        }
    }

    public void createUsuario(String endpoint) {
        String url = baseUrl + endpoint;
        String bodyToSend = gson.toJson(usuarioModel);

        response = given()
                .contentType(ContentType.JSON)
                .accept(ContentType.JSON)
                .body(bodyToSend)
                .when()
                .post(url)
                .then()
                .extract()
                .response();

        if (response.getStatusCode() == 201) {
            String id = String.valueOf(gson.fromJson(response.jsonPath().prettify(), UsuarioModel.class).getUsuarioId());
            UsuarioHook.setUsuarioCriadoId(id);
        }
    }

    public void authenticateUsuario(String endpoint) {
        String url = baseUrl + endpoint;
        String bodyToSend = gson.toJson(usuarioModel);

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

    public String authenticateUsuario(String endpoint, String email, String password) {
        String url = baseUrl + endpoint;
        UsuarioModel usuarioLogin = new UsuarioModel();
        usuarioLogin.setUsuarioEmail(email);
        usuarioLogin.setUsuarioSenha(password);
        String bodyToSend = gson.toJson(usuarioLogin);
        response = given()
                .contentType(ContentType.JSON)
                .accept(ContentType.JSON)
                .body(bodyToSend)
                .when()
                .post(url)
                .then()
                .extract()
                .response();

        return String.valueOf(gson.fromJson(response.jsonPath().prettify(), TokenModel.class).getToken());
    }

    public void setUsuarioId() {
        if (usuarioId == null) {
            usuarioId = String.valueOf(gson.fromJson(response.jsonPath().prettify(), UsuarioModel.class).getUsuarioId());
        }
    }

    public void setUsuarioIdInvalido() {
        usuarioId = "0";
    }

    public void setTokenJwt() {
        String deserializedToken = String.valueOf(gson.fromJson(response.jsonPath().prettify(), TokenModel.class).getToken());
        tokenJwt = deserializedToken;

        CadastroBairroService.setTokenJwt(deserializedToken);
        CadastroMoradorService.setTokenJwt(deserializedToken);
        CadastroCaminhaoService.setTokenJwt(deserializedToken);
        CadastroMotoristaService.setTokenJwt(deserializedToken);
        CadastroAgendaService.setTokenJwt(deserializedToken);
    }

    public void validateTokenJwt() {
        try {
            Key key = Keys.hmacShaKeyFor("f+ujXAKHk00L5jlMXo2XhAWawsOoihNP1OiAM25lLSO57+X7uBMQgwPju6yzyePi".getBytes());
            Jwts.parserBuilder()
                    .setSigningKey(key)
                    .build()
                    .parseClaimsJws(tokenJwt);

        } catch (JwtException e) {
            throw new JwtException("Token JWT inválido: " + e.getMessage());
        }
    }

    public void deleteUsuario(String endpoint) {
        String url = String.format("%s%s/%s", baseUrl, endpoint, usuarioId);
        response = given()
                .header("Authorization", "Bearer " + tokenJwt)
                .accept(ContentType.JSON)
                .when()
                .delete(url)
                .then()
                .extract()
                .response();
    }

    public void deleteUsuario(String endpoint, String token, String id) {
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
            case "Cadastro de usuário bem-sucedido" -> jsonSchema = JSONSchemaUtils.getArquivoJsonSchema(schemasPath + "cadastro-de-usuario-bem-sucedido.json");
            case "Login de usuário bem sucedido" -> jsonSchema = JSONSchemaUtils.getArquivoJsonSchema(schemasPath + "login-de-usuario-bem-sucedido.json");
            default -> throw new IllegalStateException("Arquivo JSON Schema não encontrado: " + contract);
        }
    }

    public Set<ValidationMessage> validateResponseContraJsonSchema() throws IOException
    {
        return JSONSchemaUtils.validateResponseContraJsonSchema(response, jsonSchema, mapper);
    }
}
