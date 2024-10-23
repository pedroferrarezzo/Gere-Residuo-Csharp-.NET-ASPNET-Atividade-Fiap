package steps.motorista;

import com.networknt.schema.ValidationMessage;
import io.cucumber.java.pt.Dado;
import io.cucumber.java.pt.E;
import io.cucumber.java.pt.Então;
import io.cucumber.java.pt.Quando;
import model.error.ErrorModel;
import model.motorista.MotoristaModel;
import org.junit.Assert;
import services.motorista.CadastroMotoristaService;

import java.io.IOException;
import java.util.List;
import java.util.Map;
import java.util.Set;

public class CadastroMotoristaSteps {
    private final CadastroMotoristaService cadastroMotoristaService = new CadastroMotoristaService();

    @Dado("que eu tenha os seguintes dados de motorista:")
    public void queEuTenhaOsSeguintesDadosDeMotorista(List<Map<String, String>> rows) {
        for(Map<String, String> columns : rows) {
            cadastroMotoristaService.setAtributoMotorista(columns.get("atributo"),  columns.get("valor"));
        }
    }

    @Quando("uma requisição POST for enviada para a rota {string} de cadastro de motorista")
    public void umaRequisiçãoPOSTForEnviadaParaARotaDeCadastroDeMotorista(String endpoint) {
        cadastroMotoristaService.createMotorista(endpoint);
    }

    @Então("o status code que a API de cadastro de Motorista deve retornar é o {int}")
    public void oStatusCodeQueAAPIDeCadastroDeMotoristaDeveRetornarÉO(int statusCode) {
        Assert.assertEquals(statusCode, cadastroMotoristaService.getResponse().statusCode());
    }

    @E("o JSON Schema de validação a ser usado contra a resposta da API de cadastro de Motorista é o {string}")
    public void oJSONSchemaDeValidaçãoASerUsadoContraARespostaDaAPIDeCadastroDeMotoristaÉO(String contract) throws IOException {
        cadastroMotoristaService.setArquivoJsonSchema(contract);
    }

    @Então("a resposta da requisição da API de cadastro de Motorista deve estar em conformidade com o JSON Schema selecionado")
    public void aRespostaDaRequisiçãoDaAPIDeCadastroDeMotoristaDeveEstarEmConformidadeComOJSONSchemaSelecionado() throws IOException {
        Set<ValidationMessage> validateResponse = cadastroMotoristaService.validateResponseContraJsonSchema();
        Assert.assertTrue("A estrutura JSON de resposta da API não está de acordo com o JSONSChema. Erros encontrados: " + validateResponse, validateResponse.isEmpty());
    }

    @E("a API de cadastro de Motorista deve retornar um objeto JSON contendo uma mensagem de erro: {string}")
    public void aAPIDeCadastroDeMotoristaDeveRetornarUmObjetoJSONContendoUmaMensagemDeErro(String message) {
        ErrorModel errorModel = cadastroMotoristaService.getGson().fromJson(
                cadastroMotoristaService.getResponse().jsonPath().prettify(), ErrorModel.class
        );
        Assert.assertEquals(message, errorModel.getErrorMessages().getFirst());
    }

    @Dado("que eu recupere o ID do motorista criado")
    public void queEuRecupereOIDDoMotoristaCriado() {
        cadastroMotoristaService.setMotoristaId();
    }

    @Quando("uma requisição DELETE for enviada para a rota {string} passando o ID do motorista como Path Parameter")
    public void umaRequisiçãoDELETEForEnviadaParaARotaPassandoOIDDoMotoristaComoPathParameter(String endpoint) {
        cadastroMotoristaService.deleteMotorista(endpoint);
    }

    @Dado("que eu especifique um ID de motorista invalido")
    public void queEuEspecifiqueUmIDDeMotoristaInvalido() {
        cadastroMotoristaService.setMotoristaIdInvalido();
    }

    @Então("uma requisição GET deve ser enviada para {string} passando o ID do motorista da agenda como Path Parameter para obter o seu estado atual")
    public void umaRequisiçãoGETDeveSerEnviadaParaPassandoOIDDoMotoristaDaAgendaComoPathParameterParaObterOSeuEstadoAtual(String endpoint) {
        cadastroMotoristaService.getMotorista(endpoint);
    }

    @E("o atributo motoristaEstaDisponivel deve ser igual a {string}")
    public void oAtributoMotoristaEstaDisponivelDeveSerIgualA(String condition) {
        cadastroMotoristaService.validateMotoristaEstaDisponivel(condition);
    }
}