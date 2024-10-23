package steps.notificacao;

import io.cucumber.java.pt.Dado;
import io.cucumber.java.pt.E;
import io.cucumber.java.pt.Então;
import io.cucumber.java.pt.Quando;
import model.error.ErrorModel;
import model.notificacao.NotificacaoModel;
import org.junit.Assert;
import services.notificacao.CadastroNotificacaoService;

public class CadastroNotificacaoSteps {

    private final CadastroNotificacaoService cadastroNotificacaoService = new CadastroNotificacaoService();

    @Dado("que eu recupere o ID da notificação gerada na abertura da agenda enviando uma requisição GET para {string} e filtrando")
    public void queEuRecupereOIDDaNotificaçãoGeradaNaAberturaDaAgendaEnviandoUmaRequisiçãoGETParaEFiltrando(String endpoint) {
        cadastroNotificacaoService.setNotificacaoId(endpoint);
    }

    @Quando("uma requisição DELETE for enviada para a rota {string} passando o ID da notificação como Path Parameter")
    public void umaRequisiçãoDELETEForEnviadaParaARotaPassandoOIDDaNotificaçãoComoPathParameter(String endpoint) {
        cadastroNotificacaoService.deleteNotificacao(endpoint);
    }

    @Então("o status code que a API de cadastro de Notificação deve retornar é o {int}")
    public void oStatusCodeQueAAPIDeCadastroDeNotificaçãoDeveRetornarÉO(int statusCode) {
        Assert.assertEquals(statusCode, cadastroNotificacaoService.getResponse().statusCode());
    }

    @Dado("que eu especifique um ID de notificação invalido")
    public void queEuEspecifiqueUmIDDeNotificaçãoInvalido() {
        cadastroNotificacaoService.setNotificacaoIdInvalido();
    }

    @E("a API de cadastro de Notificação deve retornar um objeto JSON contendo uma mensagem de erro: {string}")
    public void aAPIDeCadastroDeNotificaçãoDeveRetornarUmObjetoJSONContendoUmaMensagemDeErro(String message) {
        ErrorModel errorModel = cadastroNotificacaoService.getGson().fromJson(
                cadastroNotificacaoService.getResponse().jsonPath().prettify(), ErrorModel.class
        );
        Assert.assertEquals(message, errorModel.getErrorMessages().getFirst());
    }
}
