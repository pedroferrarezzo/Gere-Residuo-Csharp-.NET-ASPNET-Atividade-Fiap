package hook.morador;

import hook.bairro.BairroHook;
import io.cucumber.java.After;
import lombok.Getter;
import lombok.Setter;
import services.morador.CadastroMoradorService;
import services.usuario.CadastroUsuarioService;

public class MoradorHook {
    @Getter
    @Setter
    private static String moradorCriadoId;
    private static final CadastroMoradorService cadastroMoradorService = new CadastroMoradorService();
    private static final CadastroUsuarioService cadastroUsuarioService = new CadastroUsuarioService();
    private static final String emailAdminBaseTeste = "adminbaseteste@gereresiduo.com.br";
    private static final String senhaAdminBaseTeste = "Teste123@";

    @After(value = "@HOOK_CLEAN_MORADOR_AFTER_SCENARIO", order = 2)
    public static void afterExcluirMoradorCriado() {
        String tokenJwt = cadastroUsuarioService.authenticateUsuario("/api/v1/Usuario/Login", emailAdminBaseTeste, senhaAdminBaseTeste);
        cadastroMoradorService.deleteMorador("/api/v1/Morador", tokenJwt, moradorCriadoId);
    }
}
