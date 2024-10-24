package steps.morador;

import com.networknt.schema.ValidationMessage;
import io.cucumber.java.pt.Dado;
import io.cucumber.java.pt.E;
import io.cucumber.java.pt.Então;
import io.cucumber.java.pt.Quando;
import model.error.ErrorModel;
import org.junit.Assert;
import services.morador.CadastroMoradorService;

import java.io.IOException;
import java.util.List;
import java.util.Map;
import java.util.Set;

public class CadastroMoradorSteps {

    private final CadastroMoradorService cadastroMoradorService = new CadastroMoradorService();

    @Dado("que eu tenha os seguintes dados de morador:")
    public void queEuTenhaOsSeguintesDadosDeMorador(List<Map<String, String>> rows) {
        for(Map<String, String> columns : rows) {
            cadastroMoradorService.setAtributoMorador(columns.get("atributo"),  columns.get("valor"));
        }
    }

    @Quando("uma requisição POST for enviada para a rota {string} de cadastro de morador")
    public void umaRequisiçãoPOSTForEnviadaParaARotaDeCadastroDeMorador(String endpoint) {
        cadastroMoradorService.createMorador(endpoint);
    }

    @E("o status code que a API de cadastro de Morador deve retornar é o {int}")
    public void oStatusCodeQueAAPIDeCadastroDeMoradorDeveRetornarÉO(int statusCode) {
        Assert.assertEquals(statusCode, cadastroMoradorService.getResponse().statusCode());
    }

    @Então("o JSON Schema de validação a ser usado contra a resposta da API de cadastro de Morador é o {string}")
    public void oJSONSchemaDeValidaçãoASerUsadoContraARespostaDaAPIDeCadastroDeMoradorÉO(String contract) throws IOException {
        cadastroMoradorService.setArquivoJsonSchema(contract);
    }

    @E("a resposta da requisição da API de cadastro de Morador deve estar em conformidade com o JSON Schema selecionado")
    public void aRespostaDaRequisiçãoDaAPIDeCadastroDeMoradorDeveEstarEmConformidadeComOJSONSchemaSelecionado() throws IOException {
        Set<ValidationMessage> validateResponse = cadastroMoradorService.validateResponseContraJsonSchema();
        Assert.assertTrue("A estrutura JSON de resposta da API não está de acordo com o JSONSChema. Erros encontrados: " + validateResponse, validateResponse.isEmpty());
    }

    @E("a API de cadastro de Morador deve retornar um objeto JSON contendo uma mensagem de erro: {string}")
    public void aAPIDeCadastroDeMoradorDeveRetornarUmObjetoJSONContendoUmaMensagemDeErro(String message) {
        ErrorModel errorModel = cadastroMoradorService.getGson().fromJson(
                cadastroMoradorService.getResponse().jsonPath().prettify(), ErrorModel.class
        );
        Assert.assertEquals(message, errorModel.getErrorMessages().getFirst());
    }

    @Quando("eu recuperar o ID do bairro criado no contexto")
    public void euRecuperarOIDDoBairroCriadoNoContexto() {
        cadastroMoradorService.setAtributoBairroId();
    }

    @Dado("que eu recupere o ID do morador criado")
    public void queEuRecupereOIDDoMoradorCriado() {
        cadastroMoradorService.setMoradorId();
    }

    @Quando("uma requisição DELETE for enviada para a rota {string} passando o ID do morador como Path Parameter")
    public void umaRequisiçãoDELETEForEnviadaParaARotaPassandoOIDDoMoradorComoPathParameter(String endpoint) {
        cadastroMoradorService.deleteMorador(endpoint);
    }

    @Dado("que eu especifique um ID de morador invalido: {int}")
    public void queEuEspecifiqueUmIDDeMoradorInvalido(int id) {
        cadastroMoradorService.setMoradorIdInvalido(id);

    }
}
