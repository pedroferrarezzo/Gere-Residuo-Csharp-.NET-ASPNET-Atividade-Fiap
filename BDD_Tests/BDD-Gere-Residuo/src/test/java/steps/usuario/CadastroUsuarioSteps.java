package steps.usuario;

import com.networknt.schema.ValidationMessage;
import io.cucumber.java.pt.Dado;
import io.cucumber.java.pt.E;
import io.cucumber.java.pt.Então;
import io.cucumber.java.pt.Quando;
import model.error.ErrorModel;
import org.junit.Assert;
import services.usuario.CadastroUsuarioService;

import java.io.IOException;
import java.util.List;
import java.util.Map;
import java.util.Set;

public class CadastroUsuarioSteps {

    CadastroUsuarioService cadastroUsuarioService = new CadastroUsuarioService();

    @Dado("que eu tenha os seguintes dados de usuário:")
    public void queEuTenhaOsSeguintesDadosDeUsuário(List<Map<String, String>> rows) {
        for(Map<String, String> columns : rows) {
            cadastroUsuarioService.setAtributoUsuario(columns.get("atributo"),  columns.get("valor"));
        }
    }

    @Quando("uma requisição POST for enviada para a rota {string} de cadastro de usuário")
    public void umaRequisiçãoPOSTForEnviadaParaARotaDeCadastroDeUsuário(String endpoint) {
        cadastroUsuarioService.createUsuario(endpoint);
    }

    @Quando("uma requisição POST for enviada para a rota {string} de Login")
    public void umaRequisiçãoPOSTForEnviadaParaARotaDeLogin(String endpoint) {
        cadastroUsuarioService.authenticateUsuario(endpoint);
    }

    @Então("o status code esperado é o {int}")
    public void oStatusCodeEsperadoÉO(int statusCode) {
        Assert.assertEquals(statusCode, cadastroUsuarioService.response.statusCode());
    }

    @E("o JSON Schema de validação a ser usado é o {string}")
    public void oJSONSchemaDeValidaçãoASerUsadoÉO(String path) throws IOException {
        cadastroUsuarioService.setArquivoJsonSchema(path);
    }

    @Então("a resposta da requisição deve estar em conformidade com o JSON Schema selecionado")
    public void aRespostaDaRequisiçãoDeveEstarEmConformidadeComOJSONSchemaSelecionado() throws IOException {
        Set<ValidationMessage> validateResponse = cadastroUsuarioService.validateResponseContraJsonSchema();
        Assert.assertTrue("A estrutura JSON de resposta da API não está de acordo com o JSONSChema. Erros encontrados: " + validateResponse, validateResponse.isEmpty());
    }


    @E("a API deve retornar um objeto JSON contendo uma mensagem de erro para o atributo faltante: {string}")
    public void aAPIDeveRetornarUmObjetoJSONContendoUmaMensagemDeErroParaOAtributoFaltante(String message) {
        ErrorModel errorModel = cadastroUsuarioService.gson.fromJson(
                cadastroUsuarioService.response.jsonPath().prettify(), ErrorModel.class
        );

        Assert.assertEquals(message, errorModel.getErrorMessages().getFirst());
    }

    @Dado("que eu recupere o ID do usuário criado durante a execução do contexto")
    public void queEuRecupereOIDDoUsuárioCriadoDuranteAExecuçãoDoContexto() {
        cadastroUsuarioService.setUsuarioId();
    }

    @Quando("uma requisição DELETE for enviada para a rota {string} passando o ID do usuário como Path Parameter")
    public void umaRequisiçãoDELETEForEnviadaParaARotaPassandoOIDDoUsuárioComoPathParameter(String endpoint) {
        cadastroUsuarioService.deleteUsuario(endpoint);
    }

    @E("o Token JWT seja recuperado da resposta da API")
    public void oTokenJWTSejaRecuperadoDaRespostaDaAPI() {
        cadastroUsuarioService.setTokenJwt();

    }

    @Então("o Token JWT retornado deve ser valido com a Secret Key")
    public void oTokenJWTRetornadoDeveSerValidoComASecretKey() {
        cadastroUsuarioService.validateTokenJwt();
    }
}