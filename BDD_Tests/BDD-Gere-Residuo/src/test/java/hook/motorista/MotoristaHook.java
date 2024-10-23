package hook.motorista;

import io.cucumber.java.After;
import lombok.Getter;
import lombok.Setter;
import services.motorista.CadastroMotoristaService;
import services.usuario.CadastroUsuarioService;

public class MotoristaHook {
    @Getter
    @Setter
    private static String motoristaCriadoId;
    private static final CadastroMotoristaService cadastroMotoristaService = new CadastroMotoristaService();
    private static final CadastroUsuarioService cadastroUsuarioService = new CadastroUsuarioService();
    private static final String emailAdminBaseTeste = "adminbaseteste@gereresiduo.com.br";
    private static final String senhaAdminBaseTeste = "Teste123@";

    @After(value = "@HOOK_CLEAN_MOTORISTA_AFTER_SCENARIO", order = 1)
    public static void afterExcluirMotoristaCriado() {
        String tokenJwt = cadastroUsuarioService.authenticateUsuario("/api/v1/Usuario/Login", emailAdminBaseTeste, senhaAdminBaseTeste);
        cadastroMotoristaService.deleteMotorista("/api/v1/Motorista", tokenJwt, motoristaCriadoId);
    }
}
