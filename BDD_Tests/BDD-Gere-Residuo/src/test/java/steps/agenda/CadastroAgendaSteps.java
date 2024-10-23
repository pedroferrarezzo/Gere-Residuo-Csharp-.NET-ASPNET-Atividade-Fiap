package steps.agenda;

import com.networknt.schema.ValidationMessage;
import io.cucumber.java.pt.Dado;
import io.cucumber.java.pt.E;
import io.cucumber.java.pt.Então;
import io.cucumber.java.pt.Quando;
import model.error.ErrorModel;
import org.junit.Assert;
import services.agenda.CadastroAgendaService;

import java.io.IOException;
import java.util.List;
import java.util.Map;
import java.util.Set;

public class CadastroAgendaSteps {
    private final CadastroAgendaService cadastroAgendaService = new CadastroAgendaService();

    @Dado("que eu tenha os seguintes dados de agenda:")
    public void queEuTenhaOsSeguintesDadosDeAgenda(List<Map<String, String>> rows) {
        for(Map<String, String> columns : rows) {
            cadastroAgendaService.setAtributoAgenda(columns.get("atributo"),  columns.get("valor"));
        }
    }

    @Quando("eu recuperar os IDs de caminhão, motorista e bairro criados")
    public void euRecuperarOsIDsDeCaminhãoMotoristaEBairroCriados() {
        cadastroAgendaService.setAtributoBairroId();
        cadastroAgendaService.setAtributoCaminhaoId();
        cadastroAgendaService.setAtributoMotoristaId();
    }

    @Então("uma requisição POST for enviada para a rota {string} de cadastro de agenda")
    public void umaRequisiçãoPOSTForEnviadaParaARotaDeCadastroDeAgenda(String endpoint) {
        cadastroAgendaService.createAgenda(endpoint);
    }

    @E("o status code que a API de cadastro de Agenda deve retornar é o {int}")
    public void oStatusCodeQueAAPIDeCadastroDeAgendaDeveRetornarÉO(int statusCode) {
        Assert.assertEquals(statusCode, cadastroAgendaService.getResponse().statusCode());
    }

    @Então("o JSON Schema de validação a ser usado contra a resposta da API de cadastro de Agenda é o {string}")
    public void oJSONSchemaDeValidaçãoASerUsadoContraARespostaDaAPIDeCadastroDeAgendaÉO(String contract) throws IOException {
        cadastroAgendaService.setArquivoJsonSchema(contract);
    }

    @E("a resposta da requisição da API de cadastro de Agenda deve estar em conformidade com o JSON Schema selecionado")
    public void aRespostaDaRequisiçãoDaAPIDeCadastroDeAgendaDeveEstarEmConformidadeComOJSONSchemaSelecionado() throws IOException {
        Set<ValidationMessage> validateResponse = cadastroAgendaService.validateResponseContraJsonSchema();
        Assert.assertTrue("A estrutura JSON de resposta da API não está de acordo com o JSONSChema. Erros encontrados: " + validateResponse, validateResponse.isEmpty());
    }

    @E("a API de cadastro de Agenda deve retornar um objeto JSON contendo uma mensagem de erro que começe com: {string}")
    public void aAPIDeCadastroDeAgendaDeveRetornarUmObjetoJSONContendoUmaMensagemDeErroQueComeçeCom(String message) {
        ErrorModel errorModel = cadastroAgendaService.getGson().fromJson(
                cadastroAgendaService.getResponse().jsonPath().prettify(), ErrorModel.class
        );
        Assert.assertTrue(errorModel.getErrorMessages().getFirst().startsWith(message));
    }
}
