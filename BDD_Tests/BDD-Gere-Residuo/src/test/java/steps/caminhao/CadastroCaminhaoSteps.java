package steps.caminhao;

import com.networknt.schema.ValidationMessage;
import io.cucumber.java.pt.Dado;
import io.cucumber.java.pt.E;
import io.cucumber.java.pt.Então;
import io.cucumber.java.pt.Quando;
import model.caminhao.CaminhaoModel;
import model.error.ErrorModel;
import org.junit.Assert;
import services.caminhao.CadastroCaminhaoService;

import java.io.IOException;
import java.util.List;
import java.util.Map;
import java.util.Set;

public class CadastroCaminhaoSteps {

    private final CadastroCaminhaoService cadastroCaminhaoService = new CadastroCaminhaoService();

    @Dado("que eu tenha os seguintes dados de caminhao:")
    public void queEuTenhaOsSeguintesDadosDeCaminhao(List<Map<String, String>> rows) {
        for(Map<String, String> columns : rows) {
            cadastroCaminhaoService.setAtributoCaminhao(columns.get("atributo"),  columns.get("valor"));
        }
    }

    @Quando("uma requisição POST for enviada para a rota {string} de cadastro de caminhao")
    public void umaRequisiçãoPOSTForEnviadaParaARotaDeCadastroDeCaminhao(String endpoint) {
        cadastroCaminhaoService.createCaminhao(endpoint);
    }

    @Então("o status code que a API de cadastro de Caminhao deve retornar é o {int}")
    public void oStatusCodeQueAAPIDeCadastroDeCaminhaoDeveRetornarÉO(int statusCode) {
        Assert.assertEquals(statusCode, cadastroCaminhaoService.getResponse().statusCode());
    }

    @E("o JSON Schema de validação a ser usado contra a resposta da API de cadastro de Caminhao é o {string}")
    public void oJSONSchemaDeValidaçãoASerUsadoContraARespostaDaAPIDeCadastroDeCaminhaoÉO(String contract) throws IOException {
        cadastroCaminhaoService.setArquivoJsonSchema(contract);
    }

    @Então("a resposta da requisição da API de cadastro de Caminhao deve estar em conformidade com o JSON Schema selecionado")
    public void aRespostaDaRequisiçãoDaAPIDeCadastroDeCaminhaoDeveEstarEmConformidadeComOJSONSchemaSelecionado() throws IOException {
        Set<ValidationMessage> validateResponse = cadastroCaminhaoService.validateResponseContraJsonSchema();
        Assert.assertTrue("A estrutura JSON de resposta da API não está de acordo com o JSONSChema. Erros encontrados: " + validateResponse, validateResponse.isEmpty());
    }

    @E("a API de cadastro de Caminhao deve retornar um objeto JSON contendo uma mensagem de erro: {string}")
    public void aAPIDeCadastroDeCaminhaoDeveRetornarUmObjetoJSONContendoUmaMensagemDeErro(String message) {
        ErrorModel errorModel = cadastroCaminhaoService.getGson().fromJson(
                cadastroCaminhaoService.getResponse().jsonPath().prettify(), ErrorModel.class
        );
        Assert.assertEquals(message, errorModel.getErrorMessages().getFirst());
    }

    @Dado("que eu recupere o ID do caminhao criado")
    public void queEuRecupereOIDDoCaminhaoCriado() {
        cadastroCaminhaoService.setCaminhaoId();
    }

    @Quando("uma requisição DELETE for enviada para a rota {string} passando o ID do caminhao como Path Parameter")
    public void umaRequisiçãoDELETEForEnviadaParaARotaPassandoOIDDoCaminhaoComoPathParameter(String endpoint) {
        cadastroCaminhaoService.deleteCaminhao(endpoint);
    }

    @Dado("que eu especifique um ID de caminhao invalido")
    public void queEuEspecifiqueUmIDDeCaminhaoInvalido() {
        cadastroCaminhaoService.setCaminhaoIdInvalido();
    }

    @Então("uma requisição GET deve ser enviada para {string} passando o ID do caminhao da agenda como Path Parameter para obter o seu estado atual")
    public void umaRequisiçãoGETDeveSerEnviadaParaPassandoOIDDoCaminhaoDaAgendaComoPathParameterParaObterOSeuEstadoAtual(String endpoint) {
        cadastroCaminhaoService.getCaminhao(endpoint);
    }

    @E("o atributo caminhaoEstaDisponivel deve ser igual a {string}")
    public void oAtributoCaminhaoEstaDisponivelDeveSerIgualA(String condition) {
        cadastroCaminhaoService.validateCaminhaoEstaDisponivel(condition);
    }
}