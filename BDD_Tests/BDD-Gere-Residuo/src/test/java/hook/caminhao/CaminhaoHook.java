package hook.caminhao;

import io.cucumber.java.After;
import lombok.Getter;
import lombok.Setter;
import services.caminhao.CadastroCaminhaoService;
import services.usuario.CadastroUsuarioService;

public class CaminhaoHook {
    @Getter
    @Setter
    private static String caminhaoCriadoId;
    private static final CadastroCaminhaoService cadastroCaminhaoService = new CadastroCaminhaoService();
    private static final CadastroUsuarioService cadastroUsuarioService = new CadastroUsuarioService();
    private static final String emailAdminBaseTeste = "adminbaseteste@gereresiduo.com.br";
    private static final String senhaAdminBaseTeste = "Teste123@";

    @After(value = "@HOOK_CLEAN_CAMINHAO_AFTER_SCENARIO", order = 1)
    public static void afterExcluirCaminhaoCriado() {
        String tokenJwt = cadastroUsuarioService.authenticateUsuario("/api/v1/Usuario/Login", emailAdminBaseTeste, senhaAdminBaseTeste);
        cadastroCaminhaoService.deleteCaminhao("/api/v1/Caminhao", tokenJwt, caminhaoCriadoId);
    }
}
