package hook.bairro;

import io.cucumber.java.After;
import lombok.Getter;
import lombok.Setter;
import services.bairro.CadastroBairroService;
import services.usuario.CadastroUsuarioService;

public class BairroHook {
    @Getter
    @Setter
    private static String bairroCriadoId;
    private static final CadastroBairroService cadastroBairroService = new CadastroBairroService();
    private static final CadastroUsuarioService cadastroUsuarioService = new CadastroUsuarioService();
    private static final String emailAdminBaseTeste = "adminbaseteste@gereresiduo.com.br";
    private static final String senhaAdminBaseTeste = "Teste123@";

    @After("@HOOK_CLEAN_BAIRRO_AFTER_SCENARIO")
    public static void afterExcluirUsuarioCriado() {
        String tokenJwt = cadastroUsuarioService.authenticateUsuario("/api/v1/Usuario/Login", emailAdminBaseTeste, senhaAdminBaseTeste);
        cadastroBairroService.deleteBairro("/api/v1/Bairro", tokenJwt, bairroCriadoId);
    }
}
