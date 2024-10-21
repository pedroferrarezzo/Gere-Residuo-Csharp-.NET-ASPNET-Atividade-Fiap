package services.usuario;

import com.fasterxml.jackson.databind.JsonNode;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.google.gson.*;
import com.networknt.schema.JsonSchema;
import com.networknt.schema.JsonSchemaFactory;
import com.networknt.schema.SpecVersion;
import com.networknt.schema.ValidationMessage;
import deserializer.ErrorModelDeserializer;
import dto.usuario.UsuarioLoginDto;
import io.cucumber.java.AfterAll;
import io.jsonwebtoken.JwtException;
import io.jsonwebtoken.Jwts;
import io.jsonwebtoken.security.Keys;
import io.restassured.http.ContentType;
import io.restassured.response.Response;
import model.error.ErrorModel;
import model.token.TokenModel;
import model.usuario.UsuarioModel;
import model.usuario.UsuarioRole;
import org.json.JSONObject;
import org.json.JSONTokener;

import java.io.IOException;
import java.io.InputStream;
import java.lang.reflect.Type;
import java.nio.file.Files;
import java.nio.file.Paths;
import java.security.Key;
import java.util.Map;
import java.util.Set;

import static io.restassured.RestAssured.given;

public class CadastroUsuarioService {


    private final UsuarioModel usuarioModel = new UsuarioModel();
    private final TokenModel tokenModel = new TokenModel();

    public final Gson gson = new GsonBuilder()
            .excludeFieldsWithoutExposeAnnotation()
            .registerTypeAdapter(ErrorModel.class, new ErrorModelDeserializer())
            .create();

    public Response response;
    public String usuarioId;
    public String tokenJwt;
    String schemasPath = "src/test/resources/schemas/";
    JSONObject jsonSchema;

    private final ObjectMapper mapper = new ObjectMapper();
    private final String baseUrl = "http://52.170.197.27:80";


    public <T> void setAtributoUsuario(String atributo, T valor) {

        switch(atributo) {
            case "usuarioId" -> usuarioModel.setUsuarioId((int) valor);
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
    }

    public void authenticateUsuario(String endpoint) {
        String url = baseUrl + endpoint;
        UsuarioLoginDto usuarioLoginDto = new UsuarioLoginDto(usuarioModel.getUsuarioEmail(), usuarioModel.getUsuarioSenha());
        String bodyToSend = gson.toJson(usuarioLoginDto);

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

    public void setUsuarioId() {
        usuarioId = String.valueOf(gson.fromJson(response.jsonPath().prettify(), usuarioModel.getClass()).getUsuarioId());
    }

    public void setTokenJwt() {
        tokenJwt = String.valueOf(gson.fromJson(response.jsonPath().prettify(), tokenModel.getClass()).getToken());
    }


    public void validateTokenJwt() {
        try {
            Key key = Keys.hmacShaKeyFor("f+ujXAKHk00L5jlMXo2XhAWawsOoihNP1OiAM25lLSO57+X7uBMQgwPju6yzyePi".getBytes());
            Jwts.parserBuilder()
                    .setSigningKey(key)
                    .build()
                    .parseClaimsJws(tokenJwt);

        } catch (JwtException e) {
            System.out.println("Token JWT inválido: " + e.getMessage());
        }
    }

    @AfterAll
    public void deleteUsuario(String endpoint) {
        String url = String.format("%s%s/%s", baseUrl, endpoint, usuarioId);
        response = given()
                .accept(ContentType.JSON)
                .when()
                .delete(url)
                .then()
                .extract()
                .response();
    }

    // JSON Schema Validator
    private JSONObject getArquivoJsonSchema(String filePath) throws IOException {
        try (InputStream inputStream = Files.newInputStream(Paths.get(filePath))) {
            JSONTokener tokener = new JSONTokener(inputStream);

            return new JSONObject(tokener);
        }
    }

    public void setArquivoJsonSchema(String contract) throws IOException {
        switch (contract) {
            case "Cadastro de usuário bem-sucedido" -> jsonSchema = getArquivoJsonSchema(schemasPath + "cadastro-de-usuario-bem-sucedido.json");
            case "Login de usuário bem sucedido" -> jsonSchema = getArquivoJsonSchema(schemasPath + "login-de-usuario-bem-sucedido.json");
            default -> throw new IllegalStateException("Arquivo JSON Schema não encontrado: " + contract);
        }
    }

    public Set<ValidationMessage> validateResponseContraJsonSchema() throws IOException
    {
        JSONObject jsonResponse = new JSONObject(response.getBody().asString());

        JsonSchemaFactory schemaFactory = JsonSchemaFactory.getInstance(SpecVersion.VersionFlag.V4);

        JsonSchema schema = schemaFactory.getSchema(jsonSchema.toString());

        JsonNode jsonResponseNode = mapper.readTree(jsonResponse.toString());

        return schema.validate(jsonResponseNode);
    }
}
