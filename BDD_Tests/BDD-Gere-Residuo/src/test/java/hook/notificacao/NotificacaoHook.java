package hook.notificacao;

import io.cucumber.java.After;
import lombok.Getter;
import lombok.Setter;
import services.agenda.CadastroAgendaService;
import services.notificacao.CadastroNotificacaoService;
import services.usuario.CadastroUsuarioService;

public class NotificacaoHook {
    private static final CadastroUsuarioService cadastroUsuarioService = new CadastroUsuarioService();
    private static final CadastroNotificacaoService cadastroNotificacaoService = new CadastroNotificacaoService();
    private static final String emailAdminBaseTeste = "adminbaseteste@gereresiduo.com.br";
    private static final String senhaAdminBaseTeste = "Teste123@";

    @After(value = "@HOOK_CLEAN_NOTIFICACAO_AFTER_SCENARIO", order = 3)
    public static void afterExcluirAgendaCriada() {
        String tokenJwt = cadastroUsuarioService.authenticateUsuario("/api/v1/Usuario/Login", emailAdminBaseTeste, senhaAdminBaseTeste);
        cadastroNotificacaoService.setNotificacaoId("/api/v1/Notificacao");
        cadastroNotificacaoService.deleteNotificacao("/api/v1/Notificacao", tokenJwt);
    }
}
