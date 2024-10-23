package steps.morador;

import io.cucumber.java.pt.Dado;
import io.cucumber.java.pt.E;
import io.cucumber.java.pt.Então;
import io.cucumber.java.pt.Quando;
import services.morador.CadastroMoradorService;

public class CadastroMoradorSteps {

    private final CadastroMoradorService cadastroMoradorService = new CadastroMoradorService();


    @Quando("uma requisição POST for enviada para a rota {string} de cadastro de morador")
    public void umaRequisiçãoPOSTForEnviadaParaARotaDeCadastroDeMorador(String endpoint) {
    }

    @Dado("que eu tenha os seguintes dados de morador:")
    public void queEuTenhaOsSeguintesDadosDeMorador() {
    }

    @E("o status code que a API de cadastro de Morador deve retornar é o {int}")
    public void oStatusCodeQueAAPIDeCadastroDeMoradorDeveRetornarÉO(int statusCode) {
    }

    @Então("o JSON Schema de validação a ser usado contra a resposta da API de cadastro de Morador é o {string}")
    public void oJSONSchemaDeValidaçãoASerUsadoContraARespostaDaAPIDeCadastroDeMoradorÉO(String contract) {
    }

    @E("a resposta da requisição da API de cadastro de Morador deve estar em conformidade com o JSON Schema selecionado")
    public void aRespostaDaRequisiçãoDaAPIDeCadastroDeMoradorDeveEstarEmConformidadeComOJSONSchemaSelecionado() {
    }

    @E("a API de cadastro de Morador deve retornar um objeto JSON contendo uma mensagem de erro: {string}")
    public void aAPIDeCadastroDeMoradorDeveRetornarUmObjetoJSONContendoUmaMensagemDeErro(String arg0) {
    }

    @Quando("eu recuperar o ID do bairro criado no contexto")
    public void euRecuperarOIDDoBairroCriadoNoContexto() {
    }
}
