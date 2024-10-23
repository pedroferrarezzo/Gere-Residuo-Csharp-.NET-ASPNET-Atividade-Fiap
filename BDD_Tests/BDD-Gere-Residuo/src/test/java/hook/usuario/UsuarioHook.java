package hook.usuario;

import io.cucumber.java.After;
import lombok.Getter;
import lombok.Setter;
import services.usuario.CadastroUsuarioService;

public class UsuarioHook {
    @Getter
    @Setter
    private static String usuarioCriadoId;
    private static final CadastroUsuarioService cadastroUsuarioService = new CadastroUsuarioService();
    private static final String emailAdminBaseTeste = "adminbaseteste@gereresiduo.com.br";
    private static final String senhaAdminBaseTeste = "Teste123@";

    @After("@HOOK_CLEAN_USUARIO_AFTER_SCENARIO")
    public static void afterExcluirUsuarioCriado() {
        String tokenJwt = cadastroUsuarioService.authenticateUsuario("/api/v1/Usuario/Login", emailAdminBaseTeste, senhaAdminBaseTeste);
        cadastroUsuarioService.deleteUsuario("/api/v1/Usuario", tokenJwt, usuarioCriadoId);
    }
}
