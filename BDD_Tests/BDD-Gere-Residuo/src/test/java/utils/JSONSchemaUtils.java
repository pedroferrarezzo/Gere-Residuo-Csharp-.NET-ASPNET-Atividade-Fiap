package utils;

import com.fasterxml.jackson.databind.JsonNode;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.networknt.schema.JsonSchema;
import com.networknt.schema.JsonSchemaFactory;
import com.networknt.schema.SpecVersion;
import com.networknt.schema.ValidationMessage;
import io.restassured.response.Response;
import org.json.JSONObject;
import org.json.JSONTokener;

import java.io.IOException;
import java.io.InputStream;
import java.nio.file.Files;
import java.nio.file.Paths;
import java.util.Set;

public class JSONSchemaUtils {

    public static Set<ValidationMessage> validateResponseContraJsonSchema(Response response, JSONObject jsonSchema, ObjectMapper mapper) throws IOException
    {
        JSONObject jsonResponse = new JSONObject(response.getBody().asString());

        JsonSchemaFactory schemaFactory = JsonSchemaFactory.getInstance(SpecVersion.VersionFlag.V4);

        JsonSchema schema = schemaFactory.getSchema(jsonSchema.toString());

        JsonNode jsonResponseNode = mapper.readTree(jsonResponse.toString());

        return schema.validate(jsonResponseNode);
    }

    public static JSONObject getArquivoJsonSchema(String filePath) throws IOException {
        try (InputStream inputStream = Files.newInputStream(Paths.get(filePath))) {
            JSONTokener tokener = new JSONTokener(inputStream);
            return new JSONObject(tokener);
        }
    }
}
