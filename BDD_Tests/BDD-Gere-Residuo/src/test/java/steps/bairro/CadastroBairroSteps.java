package steps.bairro;

import com.networknt.schema.ValidationMessage;
import io.cucumber.java.pt.Dado;
import io.cucumber.java.pt.E;
import io.cucumber.java.pt.Então;
import io.cucumber.java.pt.Quando;
import model.bairro.BairroModel;
import model.error.ErrorModel;
import org.junit.Assert;
import services.bairro.CadastroBairroService;

import java.io.IOException;
import java.util.List;
import java.util.Map;
import java.util.Set;

public class CadastroBairroSteps {

    private final CadastroBairroService cadastroBairroService = new CadastroBairroService();

    @Dado("que eu tenha os seguintes dados de bairro:")
    public void queEuTenhaOsSeguintesDadosDeBairro(List<Map<String, String>> rows) {
        for(Map<String, String> columns : rows) {
            cadastroBairroService.setAtributoBairro(columns.get("atributo"),  columns.get("valor"));
        }
    }

    @Quando("uma requisição POST for enviada para a rota {string} de cadastro de bairro")
    public void umaRequisiçãoPOSTForEnviadaParaARotaDeCadastroDeBairro(String endpoint) {
        cadastroBairroService.createBairro(endpoint);
    }

    @Então("o status code que a API de cadastro de Bairro deve retornar é o {int}")
    public void oStatusCodeQueAAPIDeCadastroDeBairroDeveRetornarÉO(int statusCode) {
        Assert.assertEquals(statusCode, cadastroBairroService.getResponse().statusCode());
    }

    @E("o JSON Schema de validação a ser usado contra a resposta da API de cadastro de Bairro é o {string}")
    public void oJSONSchemaDeValidaçãoASerUsadoContraARespostaDaAPIDeCadastroDeBairroÉO(String contract) throws IOException {
        cadastroBairroService.setArquivoJsonSchema(contract);
    }

    @Então("a resposta da requisição da API de cadastro de Bairro deve estar em conformidade com o JSON Schema selecionado")
    public void aRespostaDaRequisiçãoDaAPIDeCadastroDeBairroDeveEstarEmConformidadeComOJSONSchemaSelecionado() throws IOException {
        Set<ValidationMessage> validateResponse = cadastroBairroService.validateResponseContraJsonSchema();
        Assert.assertTrue("A estrutura JSON de resposta da API não está de acordo com o JSONSChema. Erros encontrados: " + validateResponse, validateResponse.isEmpty());
    }

    @E("a API de cadastro de Bairro deve retornar um objeto JSON contendo uma mensagem de erro: {string}")
    public void aAPIDeCadastroDeBairroDeveRetornarUmObjetoJSONContendoUmaMensagemDeErro(String message) {
        ErrorModel errorModel = cadastroBairroService.getGson().fromJson(
                cadastroBairroService.getResponse().jsonPath().prettify(), ErrorModel.class
        );
        Assert.assertEquals(message, errorModel.getErrorMessages().getFirst());
    }

    @Dado("que eu recupere o ID do bairro criado")
    public void queEuRecupereOIDDoBairroCriado() {
        cadastroBairroService.setBairroId();
    }

    @Quando("uma requisição DELETE for enviada para a rota {string} passando o ID do bairro como Path Parameter")
    public void umaRequisiçãoDELETEForEnviadaParaARotaPassandoOIDDoBairroComoPathParameter(String endpoint) {
        cadastroBairroService.deleteBairro(endpoint);
    }

    @Dado("que eu especifique um ID de bairro invalido")
    public void queEuEspecifiqueUmIDDeBairroInvalido() {
        cadastroBairroService.setBairroIdInvalido();
    }

    @Então("uma requisição GET deve ser enviada para {string} passando o ID do bairro da agenda como Path Parameter para obter o seu estado atual")
    public void umaRequisiçãoGETDeveSerEnviadaParaPassandoOIDDoBairroDaAgendaComoPathParameterParaObterOSeuEstadoAtual(String endpoint) {
        cadastroBairroService.getBairro(endpoint);
    }

    @E("o atributo bairroEstaDisponivel deve ser igual a {string}")
    public void oAtributoBairroEstaDisponivelDeveSerIgualA(String codition) {
        cadastroBairroService.validateBairroEstaDisponivel(codition);
    }
}