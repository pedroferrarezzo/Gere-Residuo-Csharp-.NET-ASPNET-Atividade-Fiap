package deserializer;

import com.google.gson.*;
import model.error.ErrorModel;

import java.lang.reflect.Type;
import java.util.ArrayList;
import java.util.Map;

public class ErrorModelDeserializer implements JsonDeserializer<ErrorModel> {
    @Override
    public ErrorModel deserialize(JsonElement json, Type typeOfT, JsonDeserializationContext context) throws JsonParseException {
        ErrorModel errorModel = new ErrorModel();
        errorModel.setErrorMessages(new ArrayList<>());
        JsonObject jsonObject = json.getAsJsonObject();

        for (Map.Entry<String, JsonElement> entry : jsonObject.entrySet()) {

            try {
                for (JsonElement message : entry.getValue().getAsJsonArray()) {
                    errorModel.getErrorMessages().add(message.getAsString());
                }
            }
            catch (IllegalStateException e) {
                errorModel.getErrorMessages().add(entry.getValue().getAsString());
            }
        }
        return errorModel;
    }
}
