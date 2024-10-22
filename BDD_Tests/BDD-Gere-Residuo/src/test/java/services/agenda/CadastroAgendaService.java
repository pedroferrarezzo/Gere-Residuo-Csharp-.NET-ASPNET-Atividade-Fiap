package services.agenda;

import com.fasterxml.jackson.databind.ObjectMapper;
import com.google.gson.Gson;
import com.google.gson.GsonBuilder;
import deserializer.ErrorModelDeserializer;
import io.restassured.response.Response;
import model.error.ErrorModel;
import services.bairro.BairroService;

public class CadastroAgendaService {

    public Response response;
    public final Gson gson = new GsonBuilder()
            .excludeFieldsWithoutExposeAnnotation()
            .registerTypeAdapter(ErrorModel.class, new ErrorModelDeserializer())
            .create();
    private final ObjectMapper mapper = new ObjectMapper();
    private final String baseUrl = "http://52.170.197.27:80";



}
