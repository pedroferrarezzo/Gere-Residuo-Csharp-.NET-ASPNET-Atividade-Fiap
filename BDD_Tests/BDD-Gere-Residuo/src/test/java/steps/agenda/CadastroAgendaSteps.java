package steps.agenda;

import io.cucumber.java.pt.Dado;
import io.cucumber.java.pt.E;
import io.cucumber.java.pt.Então;
import io.cucumber.java.pt.Quando;
import services.agenda.CadastroAgendaService;
import services.bairro.BairroService;
import services.usuario.CadastroUsuarioService;

import java.util.List;
import java.util.Map;

public class CadastroAgendaSteps {

    public CadastroAgendaService cadastroAgendaService = new CadastroAgendaService();

    @Dado("que eu tenha os seguintes dados de bairro:")
    public void queEuTenhaOsSeguintesDadosDeBairro(List<Map<String, String>> rows) {
        for(Map<String, String> columns : rows) {
            BairroService.setAtributoBairro(columns.get("atributo"),  columns.get("valor"));
        }
    }

    @Quando("uma requisição POST for enviada para a rota {string} de cadastro de bairro")
    public void umaRequisiçãoPOSTForEnviadaParaARotaDeCadastroDeBairro(String endpoint) {

        
    }

    @Então("o status code esperado na resposta da API é o {int}")
    public void oStatusCodeEsperadoNaRespostaDaAPIÉO(int statusCode) {
        
    }

    @E("o JSON Schema de validação a ser usado na resposta da API é o {string}")
    public void oJSONSchemaDeValidaçãoASerUsadoNaRespostaDaAPIÉO(String contract) {
        
    }

    @Então("a resposta da requisição deve estar em conformidade com o JSON Schema escolhido")
    public void aRespostaDaRequisiçãoDeveEstarEmConformidadeComOJSONSchemaEscolhido() {
    }
}
